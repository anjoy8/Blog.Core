using Autofac;
using Autofac.Extensions.DependencyInjection;
using Autofac.Extras.DynamicProxy;
using AutoMapper;
using Blog.Core.AuthHelper;
using Blog.Core.Common;
using Blog.Core.Common.DB;
using Blog.Core.Common.LogHelper;
using Blog.Core.IServices;
using Blog.Core.Model.Models;
using Blog.Core.Services;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using System.Linq;
using System.Reflection;
using Xunit;
using System;
using System.Security.Claims;
using System.Configuration;
using Blog.Core.Common.AppConfig;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;

namespace Blog.Core.Tests
{
    public class DI_Test
    {

        [Fact]
        public void DI_Connet_Test()
        {
            var basePath = Microsoft.DotNet.PlatformAbstractions.ApplicationEnvironment.ApplicationBasePath;

            IServiceCollection services = new ServiceCollection();
            services.AddAutoMapper(typeof(Startup));

            services.AddScoped<SqlSugar.ISqlSugarClient>(o =>
            {
                return new SqlSugar.SqlSugarClient(new SqlSugar.ConnectionConfig()
                {
                    ConnectionString = BaseDBConfig.ConnectionString,//必填, 数据库连接字符串
                    DbType = (SqlSugar.DbType)BaseDBConfig.DbType,//必填, 数据库类型
                    IsAutoCloseConnection = true,//默认false, 时候知道关闭数据库连接, 设置为true无需使用using或者Close操作
                    IsShardSameThread = true,//共享线程
                    InitKeyType = SqlSugar.InitKeyType.SystemTable//默认SystemTable, 字段信息读取, 如：该属性是不是主键，标识列等等信息
                });
            });

            //services.AddSingleton(new Appsettings(Env));


            //实例化 AutoFac  容器   
            var builder = new ContainerBuilder();
            builder.RegisterType<AdvertisementServices>().As<IAdvertisementServices>();

            //指定已扫描程序集中的类型注册为提供所有其实现的接口。
            //var assemblysServices = Assembly.Load("Blog.Core.Services");
            //builder.RegisterAssemblyTypes(assemblysServices).AsImplementedInterfaces();
            //var assemblysRepository = Assembly.Load("Blog.Core.Repository");
            //builder.RegisterAssemblyTypes(assemblysRepository).AsImplementedInterfaces();

            var servicesDllFile = Path.Combine(basePath, "Blog.Core.Services.dll");
            var assemblysServices = Assembly.LoadFrom(servicesDllFile);
            builder.RegisterAssemblyTypes(assemblysServices)
                         .AsImplementedInterfaces()
                         .InstancePerLifetimeScope()
                         .EnableInterfaceInterceptors();

            var repositoryDllFile = Path.Combine(basePath, "Blog.Core.Repository.dll");
            var assemblysRepository = Assembly.LoadFrom(repositoryDllFile);
            builder.RegisterAssemblyTypes(assemblysRepository).AsImplementedInterfaces();

            //将services填充到Autofac容器生成器中
            builder.Populate(services);

            //使用已进行的组件登记创建新容器
            var ApplicationContainer = builder.Build();

            var blogservice = ApplicationContainer.Resolve<IBlogArticleServices>();

            Assert.True(ApplicationContainer.ComponentRegistry.Registrations.Count() > 0);
        }


        public IContainer DICollections()
        {
            var basePath = Microsoft.DotNet.PlatformAbstractions.ApplicationEnvironment.ApplicationBasePath;

            IServiceCollection services = new ServiceCollection();
            services.AddAutoMapper(typeof(Startup));

            services.AddSingleton(new Appsettings(basePath));
            services.AddSingleton(new LogLock(basePath));
            services.AddSingleton<IRedisCacheManager, RedisCacheManager>();
            services.AddScoped<Blog.Core.Model.Models.DBSeed>();
            services.AddScoped<Blog.Core.Model.Models.MyContext>();

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
            Appsettings.app(new string[] { "Audience", "Issuer" }),
            Appsettings.app(new string[] { "Audience", "Audience" }),
            signingCredentials,//签名凭据
            expiration: TimeSpan.FromSeconds(60 * 60)//接口的过期时间
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
                return new SqlSugar.SqlSugarClient(new SqlSugar.ConnectionConfig()
                {
                    ConnectionString = BaseDBConfig.ConnectionString,//必填, 数据库连接字符串
                    DbType = (SqlSugar.DbType)BaseDBConfig.DbType,//必填, 数据库类型
                    IsAutoCloseConnection = true,//默认false, 时候知道关闭数据库连接, 设置为true无需使用using或者Close操作
                    IsShardSameThread = true,//共享线程
                    InitKeyType = SqlSugar.InitKeyType.SystemTable//默认SystemTable, 字段信息读取, 如：该属性是不是主键，标识列等等信息
                });
            });

            //实例化 AutoFac  容器   
            var builder = new ContainerBuilder();
            //builder.RegisterType<AdvertisementServices>().As<IAdvertisementServices>();

            //指定已扫描程序集中的类型注册为提供所有其实现的接口。

            var servicesDllFile = Path.Combine(basePath, "Blog.Core.Services.dll");
            var assemblysServices = Assembly.LoadFrom(servicesDllFile);
            builder.RegisterAssemblyTypes(assemblysServices)
                         .AsImplementedInterfaces()
                         .InstancePerLifetimeScope()
                         .EnableInterfaceInterceptors();

            var repositoryDllFile = Path.Combine(basePath, "Blog.Core.Repository.dll");
            var assemblysRepository = Assembly.LoadFrom(repositoryDllFile);
            builder.RegisterAssemblyTypes(assemblysRepository).AsImplementedInterfaces();

            //将services填充到Autofac容器生成器中
            builder.Populate(services);

            //使用已进行的组件登记创建新容器
            var ApplicationContainer = builder.Build();

            var blogservice = ApplicationContainer.Resolve<IBlogArticleServices>();
            var myContext = ApplicationContainer.Resolve<MyContext>();

            DBSeed.SeedAsync(myContext).Wait();


            return ApplicationContainer;
        }
    }
}
