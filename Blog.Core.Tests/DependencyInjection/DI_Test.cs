using Autofac;
using Autofac.Extensions.DependencyInjection;
using Autofac.Extras.DynamicProxy;
using AutoMapper;
using Blog.Core.AuthHelper;
using Blog.Core.Common;
using Blog.Core.Common.AppConfig;
using Blog.Core.Common.DB;
using Blog.Core.Common.LogHelper;
using Blog.Core.Common.Seed;
using Blog.Core.Extensions;
using Blog.Core.IRepository.Base;
using Blog.Core.Repository.Base;
using Blog.Core.Repository.MongoRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Logging;

namespace Blog.Core.Tests
{
    public class DI_Test
    {
        /// <summary>
        /// 连接字符串 
        /// Blog.Core
        /// </summary>
        public static MutiDBOperate GetMainConnectionDb()
        {
            var mainConnetctDb = BaseDBConfig.MutiConnectionString.allDbs.Find(x => x.ConnId == MainDb.CurrentDbConnId);
            if (BaseDBConfig.MutiConnectionString.allDbs.Count > 0)
            {
                if (mainConnetctDb == null)
                {
                    mainConnetctDb = BaseDBConfig.MutiConnectionString.allDbs[0];
                }
            }
            else
            {
                throw new Exception("请确保appsettigns.json中配置连接字符串,并设置Enabled为true;");
            }

            return mainConnetctDb;
        }

        public IContainer DICollections()
        {
            var basePath = AppContext.BaseDirectory;
            AppSettings.Init(basePath);

            IServiceCollection services = new ServiceCollection();
            services.AddAutoMapper(typeof(Startup));

            services.AddSingleton(new LogLock(basePath));
            services.AddScoped<DBSeed>();
            services.AddScoped<MyContext>();

            //读取配置文件
            var symmetricKeyAsBase64 = AppSecretConfig.Audience_Secret_String;
            var keyByteArray = Encoding.ASCII.GetBytes(symmetricKeyAsBase64);
            var signingKey = new SymmetricSecurityKey(keyByteArray);


            var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

            var permission = new List<PermissionItem>();

            var permissionRequirement = new PermissionRequirement(
                "/api/denied",
                permission,
                ClaimTypes.Role,
                AppSettings.app(new string[] { "Audience", "Issuer" }),
                AppSettings.app(new string[] { "Audience", "Audience" }),
                signingCredentials,                       //签名凭据
                expiration: TimeSpan.FromSeconds(60 * 60) //接口的过期时间
            );
            services.AddSingleton(permissionRequirement);

            //【授权】
            services.AddAuthorization(options =>
            {
                options.AddPolicy(Permissions.Name,
                    policy => policy.Requirements.Add(permissionRequirement));
            });

            services.AddScoped<SqlSugar.ISqlSugarClient>(o =>
            {
                return new SqlSugar.SqlSugarScope(new SqlSugar.ConnectionConfig()
                {
                    ConnectionString = GetMainConnectionDb().Connection,    //必填, 数据库连接字符串
                    DbType = (SqlSugar.DbType)GetMainConnectionDb().DbType, //必填, 数据库类型
                    IsAutoCloseConnection = true,                           //默认false, 时候知道关闭数据库连接, 设置为true无需使用using或者Close操作
                });
            });

            //实例化 AutoFac  容器   
            var builder = new ContainerBuilder();
            //builder.RegisterType<AdvertisementServices>().As<IAdvertisementServices>();
            builder.RegisterInstance(new LoggerFactory())
                .As<ILoggerFactory>();

            builder.RegisterGeneric(typeof(Logger<>))
                .As(typeof(ILogger<>))
                .SingleInstance();
            //指定已扫描程序集中的类型注册为提供所有其实现的接口。

            builder.RegisterGeneric(typeof(BaseRepository<>)).As(typeof(IBaseRepository<>)).InstancePerDependency();           //注册仓储
            builder.RegisterGeneric(typeof(MongoBaseRepository<>)).As(typeof(IMongoBaseRepository<>)).InstancePerDependency(); //注册仓储

            // 属性注入
            var controllerBaseType = typeof(ControllerBase);
            builder.RegisterAssemblyTypes(typeof(Startup).Assembly)
                .Where(t => controllerBaseType.IsAssignableFrom(t) && t != controllerBaseType)
                .PropertiesAutowired();

            var servicesDllFile = Path.Combine(basePath, "Blog.Core.Services.dll");
            var assemblysServices = Assembly.LoadFrom(servicesDllFile);
            builder.RegisterAssemblyTypes(assemblysServices)
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope()
                .PropertiesAutowired()
                .EnableInterfaceInterceptors();

            var repositoryDllFile = Path.Combine(basePath, "Blog.Core.Repository.dll");
            var assemblysRepository = Assembly.LoadFrom(repositoryDllFile);
            builder.RegisterAssemblyTypes(assemblysRepository)
                .PropertiesAutowired().AsImplementedInterfaces();

            services.Replace(ServiceDescriptor.Transient<IControllerActivator, ServiceBasedControllerActivator>());

            services.AddAutoMapperSetup();

            //将services填充到Autofac容器生成器中
            builder.Populate(services);

            //使用已进行的组件登记创建新容器
            var ApplicationContainer = builder.Build();

            return ApplicationContainer;
        }
    }
}