using Autofac;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Core.Extensions
{
    public class AutofacPropertityModuleReg : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // 记得要启动服务注册
            // builder.Services.Replace(ServiceDescriptor.Transient<IControllerActivator, ServiceBasedControllerActivator>());
            var controllerBaseType = typeof(ControllerBase);
            builder.RegisterAssemblyTypes(typeof(Program).Assembly)
                .Where(t => controllerBaseType.IsAssignableFrom(t) && t != controllerBaseType)
                .PropertiesAutowired();

        }
    }
}
