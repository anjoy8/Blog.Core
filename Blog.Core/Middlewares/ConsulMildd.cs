using Consul;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;

namespace Blog.Core.Extensions
{
    /// <summary>
    /// Consul 注册服务
    /// </summary>
    public static class ConsulMildd
    {
        public static IApplicationBuilder UseConsulMildd(this IApplicationBuilder app, IConfiguration configuration, IHostApplicationLifetime lifetime)
        {
            if (configuration["Middleware:Consul:Enabled"].ObjToBool())
            {
                var consulClient = new ConsulClient(c =>
               {
                   //consul地址
                   c.Address = new Uri(configuration["ConsulSetting:ConsulAddress"]);
               });

                var registration = new AgentServiceRegistration()
                {
                    ID = Guid.NewGuid().ToString(),//服务实例唯一标识
                    Name = configuration["ConsulSetting:ServiceName"],//服务名
                    Address = configuration["ConsulSetting:ServiceIP"], //服务IP
                    Port = int.Parse(configuration["ConsulSetting:ServicePort"]),//服务端口
                    Check = new AgentServiceCheck()
                    {
                        DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(5),//服务启动多久后注册
                        Interval = TimeSpan.FromSeconds(10),//健康检查时间间隔
                        HTTP = $"http://{configuration["ConsulSetting:ServiceIP"]}:{configuration["ConsulSetting:ServicePort"]}{configuration["ConsulSetting:ServiceHealthCheck"]}",//健康检查地址
                        Timeout = TimeSpan.FromSeconds(5)//超时时间
                    }
                };

                //服务注册
                consulClient.Agent.ServiceRegister(registration).Wait();

                //应用程序终止时，取消注册
                lifetime.ApplicationStopping.Register(() =>
                {
                    consulClient.Agent.ServiceDeregister(registration.ID).Wait();
                });

            }
            return app;
        }
    }
}
