using Blog.Core.Common;
using Blog.Core.Common.Const;
using Blog.Core.Common.DB;
using Blog.Core.Common.DB.Aop;
using Blog.Core.Common.LogHelper;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using SqlSugar;
using StackExchange.Profiling;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Blog.Core.Extensions
{
	/// <summary>
	/// SqlSugar 启动服务
	/// </summary>
	public static class SqlsugarSetup
	{
		private static readonly MemoryCache Cache = new MemoryCache(new MemoryCacheOptions());

		public static void AddSqlsugarSetup(this IServiceCollection services)
		{
			if (services == null) throw new ArgumentNullException(nameof(services));

			// 默认添加主数据库连接
			MainDb.CurrentDbConnId = AppSettings.app(new string[] {"MainDB"});

			BaseDBConfig.MutiConnectionString.slaveDbs.ForEach(s =>
			{
				BaseDBConfig.AllSlaveConfigs.Add(new SlaveConnectionConfig()
				{
					HitRate = s.HitRate,
					ConnectionString = s.Connection
				});
			});

			BaseDBConfig.MutiConnectionString.allDbs.ForEach(m =>
			{
				var config = new ConnectionConfig()
				{
					ConfigId = m.ConnId.ObjToString().ToLower(),
					ConnectionString = m.Connection,
					DbType = (DbType) m.DbType,
					IsAutoCloseConnection = true,
					// Check out more information: https://github.com/anjoy8/Blog.Core/issues/122
					//IsShardSameThread = false,
					MoreSettings = new ConnMoreSettings()
					{
						//IsWithNoLockQuery = true,
						IsAutoRemoveDataCache = true,
						SqlServerCodeFirstNvarchar = true,
					},
					// 从库
					SlaveConnectionConfigs = BaseDBConfig.AllSlaveConfigs,
					// 自定义特性
					ConfigureExternalServices = new ConfigureExternalServices()
					{
						EntityService = (property, column) =>
						{
							if (column.IsPrimarykey && property.PropertyType == typeof(int))
							{
								column.IsIdentity = true;
							}
						}
					},
					InitKeyType = InitKeyType.Attribute
				};
				if (SqlSugarConst.LogConfigId.ToLower().Equals(m.ConnId.ToLower()))
				{
					BaseDBConfig.LogConfig = config;
				}
				else
				{
					BaseDBConfig.ValidConfig.Add(config);
				}

				BaseDBConfig.AllConfigs.Add(config);
			});

			if (BaseDBConfig.LogConfig is null)
			{
				throw new ApplicationException("未配置Log库连接");
			}


			// SqlSugarScope是线程安全，可使用单例注入
			// 参考：https://www.donet5.com/Home/Doc?typeId=1181
			services.AddSingleton<ISqlSugarClient>(o =>
			{
				var memoryCache = o.GetRequiredService<IMemoryCache>();

				foreach (var config in BaseDBConfig.AllConfigs)
				{
					config.ConfigureExternalServices.DataInfoCacheService = new SqlSugarMemoryCacheService(memoryCache);
				}

				return new SqlSugarScope(BaseDBConfig.AllConfigs, db =>
				{
					BaseDBConfig.ValidConfig.ForEach(config =>
					{
						var dbProvider = db.GetConnectionScope((string) config.ConfigId);

						// 打印SQL语句
						dbProvider.Aop.OnLogExecuting = (s, parameters) =>
							SqlSugarAop.OnLogExecuting(dbProvider, s, parameters, config);

						// 数据审计
						dbProvider.Aop.DataExecuting = SqlSugarAop.DataExecuting;

						// 配置实体假删除过滤器
						RepositorySetting.SetDeletedEntityFilter(dbProvider);
						// 配置实体数据权限
						RepositorySetting.SetTenantEntityFilter(dbProvider);
					});
				});
			});
		}

		private static string GetWholeSql(SugarParameter[] paramArr, string sql)
		{
			foreach (var param in paramArr)
			{
				sql.Replace(param.ParameterName, param.Value.ObjToString());
			}

			return sql;
		}

		private static string GetParas(SugarParameter[] pars)
		{
			string key = "【SQL参数】：";
			foreach (var param in pars)
			{
				key += $"{param.ParameterName}:{param.Value}\n";
			}

			return key;
		}
	}
}