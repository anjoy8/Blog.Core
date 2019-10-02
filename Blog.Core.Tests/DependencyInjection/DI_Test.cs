using Autofac;
using Autofac.Extensions.DependencyInjection;
using Autofac.Extras.DynamicProxy;
using AutoMapper;
using Blog.Core.Common;
using Blog.Core.IServices;
using Blog.Core.Model.Models;
using Blog.Core.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Xunit;

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

            services.AddSingleton<IHostingEnvironment, HostingEnvironment>();

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

            services.AddScoped<IHostingEnvironment, HostingEnvironment>();
            services.AddSingleton(new Appsettings(basePath));
            services.AddSingleton<IRedisCacheManager, RedisCacheManager>();
            services.AddScoped<Blog.Core.Model.Models.DBSeed>();
            services.AddScoped<Blog.Core.Model.Models.MyContext>();


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
