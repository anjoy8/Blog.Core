using System.Diagnostics.CodeAnalysis;
using Blog.Core.Common;
using Blog.Core.Common.DB;
using Blog.Core.Controllers;
using Blog.Core.Model;
using Blog.Core.Model.Models;
using Blog.Core.Model.Systems.DataBase;
using Blog.Core.Model.Tenants;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;

namespace Blog.Core.Api.Controllers.Systems;

/// <summary>
/// 数据库管理
/// </summary>
[Route("api/Systems/[controller]/[action]")]
[ApiController]
[Authorize(Permissions.Name)]
public class DataBaseController : BaseApiController
{
	private readonly ISqlSugarClient _db;

	public DataBaseController(ISqlSugarClient db)
	{
		_db = db;
	}

	[return: NotNull]
	public ISqlSugarClient GetTenantDb(string configId)
	{
		if (!_db.AsTenant().IsAnyConnection(configId))
		{
			var tenant = _db.Queryable<SysTenant>().WithCache()
				.Where(s => s.TenantType == TenantTypeEnum.Db)
				.Where(s => s.ConfigId == configId)
				.First();
			if (tenant != null)
			{
				_db.AsTenant().AddConnection(tenant.GetConnectionConfig());
			}
		}

		var db = _db.AsTenant().GetConnectionScope(configId);
		if (db is null)
		{
			throw new ApplicationException("无效的数据库配置");
		}

		return db;
	}

	/// <summary>
	/// 获取库配置
	/// </summary>
	/// <returns></returns>
	[HttpGet]
	public async Task<MessageModel<List<DatabaseOutput>>> GetAllConfig()
	{
		//增加多租户的连接
		var allConfigs = new List<ConnectionConfig>(BaseDBConfig.AllConfigs);
		var tenants = await _db.Queryable<SysTenant>().WithCache()
			.Where(s => s.TenantType == TenantTypeEnum.Db)
			.ToListAsync();
		if (tenants.Any())
		{
			allConfigs.AddRange(tenants.Select(tenant => tenant.GetConnectionConfig()));
		}

		var configs = await Task.FromResult(allConfigs);
		return Success(configs.Adapt<List<DatabaseOutput>>());
	}

	/// <summary>
	/// 获取表信息
	/// </summary>
	/// <param name="configId">配置Id</param>
	/// <param name="readType">读取类型</param>
	/// <returns></returns>
	[HttpGet]
	public MessageModel<List<DbTableInfo>> GetTableInfoList(string configId,
		DataBaseReadType readType = DataBaseReadType.Db)
	{
		if (configId.IsNullOrEmpty())
		{
			configId = MainDb.CurrentDbConnId;
		}

		var provider = GetTenantDb(configId);
		List<DbTableInfo> data = null;
		switch (readType)
		{
			case DataBaseReadType.Db:
				data = provider.DbMaintenance.GetTableInfoList(false);
				break;
			case DataBaseReadType.Entity:
				if (EntityUtility.TenantEntitys.TryGetValue(configId, out var types))
				{
					data = types.Select(s => provider.EntityMaintenance.GetEntityInfo(s))
						.Select(s => new {Name = s.DbTableName, Description = s.TableDescription})
						.Adapt<List<DbTableInfo>>();
				}

				break;
		}


		return Success(data);
	}

	/// <summary>
	/// 获取表字段
	/// </summary>
	/// <param name="tableName">表名</param>
	/// <param name="configId">ConfigId</param>
	/// <param name="readType">读取类型</param>
	/// <returns></returns>
	[HttpGet]
	public MessageModel<List<DbColumnInfoOutput>> GetColumnInfosByTableName(string tableName, string configId = null,
		DataBaseReadType readType = DataBaseReadType.Db)
	{
		if (string.IsNullOrWhiteSpace(tableName))
			return Failed<List<DbColumnInfoOutput>>("表名不能为空");

		if (configId.IsNullOrEmpty())
		{
			configId = MainDb.CurrentDbConnId;
		}

		List<DbColumnInfoOutput> data = null;
		var provider = GetTenantDb(configId);
		switch (readType)
		{
			case DataBaseReadType.Db:
				data = provider.DbMaintenance.GetColumnInfosByTableName(tableName, false)
					.Adapt<List<DbColumnInfoOutput>>();
				break;
			case DataBaseReadType.Entity:
				if (EntityUtility.TenantEntitys.TryGetValue(configId, out var types))
				{
					var type = types.FirstOrDefault(s => s.Name == tableName);
					data = provider.EntityMaintenance.GetEntityInfo(type).Columns.Adapt<List<DbColumnInfoOutput>>();
				}

				break;
		}


		return Success(data);
	}

	/// <summary>
	/// 编辑表备注
	/// </summary>
	/// <param name="input"></param>
	[HttpPut]
	public MessageModel PutTableEditRemark([FromBody] EditTableInput input)
	{
		var provider = GetTenantDb(input.ConfigId);
		if (provider.DbMaintenance.IsAnyTableRemark(input.TableName))
		{
			provider.DbMaintenance.DeleteTableRemark(input.TableName);
		}

		provider.DbMaintenance.AddTableRemark(input.TableName, input.Description);
		return Success();
	}

	/// <summary>
	/// 编辑列备注
	/// </summary>
	/// <param name="input"></param>
	[HttpPut]
	public MessageModel PutColumnEditRemark([FromBody] EditColumnInput input)
	{
		var provider = GetTenantDb(input.ConfigId);
		if (provider.DbMaintenance.IsAnyColumnRemark(input.DbColumnName, input.TableName))
		{
			provider.DbMaintenance.DeleteColumnRemark(input.DbColumnName, input.TableName);
		}

		provider.DbMaintenance.AddColumnRemark(input.DbColumnName, input.TableName, input.ColumnDescription);

		return Success();
	}
}