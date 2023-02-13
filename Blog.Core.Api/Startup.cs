using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Text;
using Autofac;
using Blog.Core.Common;
using Blog.Core.Common.LogHelper;
using Blog.Core.Common.Seed;
using Blog.Core.Extensions;
using Blog.Core.Extensions.Middlewares;
using Blog.Core.Filter;
using Blog.Core.Hubs;
using Blog.Core.IServices;
using Blog.Core.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace Blog.Core
{
    public class Startup
    {
        private IServiceCollection _services;

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            Env = env;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Env { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // 以下code可能与文章中不一样,对代码做了封装,具体查看右侧 Extensions 文件夹.
            services.AddSingleton(new AppSettings(Configuration));
            services.AddSingleton(new LogLock(Env.ContentRootPath));
            services.AddUiFilesZipSetup(Env);

            Permissions.IsUseIds4 = AppSettings.app(new string[] { "Startup", "IdentityServer4", "Enabled" }).ObjToBool();
            RoutePrefix.Name = AppSettings.app(new string[] { "AppSettings", "SvcName" }).ObjToString();

            // 确保从认证中心返回的ClaimType不被更改，不使用Map映射
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            services.AddMemoryCacheSetup();
            services.AddRedisCacheSetup();

            services.AddSqlsugarSetup();
            services.AddDbSetup();
            services.AddAutoMapperSetup();
            services.AddCorsSetup();
            services.AddMiniProfilerSetup();
            services.AddSwaggerSetup();
            services.AddJobSetup();
            services.AddHttpContextSetup();
            //services.AddAppConfigSetup(Env);
            services.AddAppTableConfigSetup(Env);//表格打印配置
            services.AddHttpApi();
            services.AddRedisInitMqSetup();

            services.AddRabbitMQSetup();
            services.AddKafkaSetup(Configuration);
            services.AddEventBusSetup();

            services.AddNacosSetup(Configuration);
            services.AddInitializationHostServiceSetup();
            // 授权+认证 (jwt or ids4)
            services.AddAuthorizationSetup();
            if (Permissions.IsUseIds4)
            {
                services.AddAuthentication_Ids4Setup();
            }
            else
            {
                services.AddAuthentication_JWTSetup();
            }

            services.AddIpPolicyRateLimitSetup(Configuration);

            services.AddSignalR().AddNewtonsoftJsonProtocol();

            services.AddScoped<UseServiceDIAttribute>();

            services.Configure<KestrelServerOptions>(x => x.AllowSynchronousIO = true)
                    .Configure<IISServerOptions>(x => x.AllowSynchronousIO = true);

            services.AddDistributedMemoryCache();
            services.AddSession();
            services.AddHttpPollySetup();

            services.AddControllers(o =>
            {
                // 全局异常过滤
                o.Filters.Add(typeof(GlobalExceptionsFilter));
                // 全局路由权限公约
                //o.Conventions.Insert(0, new GlobalRouteAuthorizeConvention());
                // 全局路由前缀，统一修改路由
                o.Conventions.Insert(0, new GlobalRoutePrefixFilter(new RouteAttribute(RoutePrefix.Name)));
            })
            // 这种写法也可以
            //.AddJsonOptions(options =>
            //{
            //    options.JsonSerializerOptions.PropertyNamingPolicy = null;
            //})
            //MVC全局配置Json序列化处理
            .AddNewtonsoftJson(options =>
            {
                //忽略循环引用
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                //不使用驼峰样式的key
                options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                //设置时间格式
                options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
                //忽略Model中为null的属性
                //options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                //设置本地时间而非UTC时间
                options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Local;
                //添加Enum转string
                options.SerializerSettings.Converters.Add(new StringEnumConverter());
            });

            services.Replace(ServiceDescriptor.Transient<IControllerActivator, ServiceBasedControllerActivator>());

            _services = services;
            //支持编码大全 例如:支持 System.Text.Encoding.GetEncoding("GB2312")  System.Text.Encoding.GetEncoding("GB18030")
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }

        // 注意在Program.CreateHostBuilder，添加Autofac服务工厂
        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule(new AutofacModuleRegister());
            builder.RegisterModule<AutofacPropertityModuleReg>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, MyContext myContext, ITasksQzServices tasksQzServices, ISchedulerCenter schedulerCenter, IHostApplicationLifetime lifetime)
        {
            // Ip限流,尽量放管道外层
            app.UseIpLimitMiddle();
            // 记录请求与返回数据
            app.UseRequestResponseLogMiddle();
            // 用户访问记录(必须放到外层，不然如果遇到异常，会报错，因为不能返回流)
            app.UseRecordAccessLogsMiddle();
            // signalr
            app.UseSignalRSendMiddle();
            // 记录ip请求
            app.UseIpLogMiddle();
            // 查看注入的所有服务
            app.UseAllServicesMiddle(_services);

            if (env.IsDevelopment())
            {
                // 在开发环境中，使用异常页面，这样可以暴露错误堆栈信息，所以不要放在生产环境。
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // 在非开发环境中，使用HTTP严格安全传输(or HSTS) 对于保护web安全是非常重要的。
                // 强制实施 HTTPS 在 ASP.NET Core，配合 app.UseHttpsRedirection
                //app.UseHsts();
            }

            app.UseSession();
            app.UseSwaggerAuthorized();
            // 封装Swagger展示
            app.UseSwaggerMiddle(() => GetType().GetTypeInfo().Assembly.GetManifestResourceStream("Blog.Core.Api.index.html"));

            // ↓↓↓↓↓↓ 注意下边这些中间件的顺序，很重要 ↓↓↓↓↓↓

            // CORS跨域
            app.UseCors(AppSettings.app(new string[] { "Startup", "Cors", "PolicyName" }));
            // 跳转https
            //app.UseHttpsRedirection();
            // 使用静态文件
            DefaultFilesOptions defaultFilesOptions = new DefaultFilesOptions();
            defaultFilesOptions.DefaultFileNames.Clear();
            defaultFilesOptions.DefaultFileNames.Add("index.html");
            app.UseDefaultFiles(defaultFilesOptions);
            app.UseStaticFiles();
            // 使用cookie
            app.UseCookiePolicy();
            // 返回错误码
            app.UseStatusCodePages();
            // Routing
            app.UseRouting();
            // 这种自定义授权中间件，可以尝试，但不推荐
            // app.UseJwtTokenAuth();

            // 测试用户，用来通过鉴权
            if (Configuration.GetValue<bool>("AppSettings:UseLoadTest"))
            {
                app.UseMiddleware<ByPassAuthMiddleware>();
            }
            // 先开启认证
            app.UseAuthentication();
            // 然后是授权中间件
            app.UseAuthorization();
            //开启性能分析
            app.UseMiniProfilerMiddleware();
            // 开启异常中间件，要放到最后
            //app.UseExceptionHandlerMidd();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                endpoints.MapHub<ChatHub>("/api2/chatHub");
            });

            // 生成种子数据
            //app.UseSeedDataMiddle(myContext, Env.WebRootPath);
            // 开启QuartzNetJob调度服务
            //app.UseQuartzJobMiddleware(tasksQzServices, schedulerCenter);
            // 服务注册
            //app.UseConsulMiddle(Configuration, lifetime);
            // 事件总线，订阅服务
            //app.ConfigureEventBus();
        }
    }
}