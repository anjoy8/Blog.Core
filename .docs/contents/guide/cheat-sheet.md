# Z  主要知识点



## AOP 

本项目多处采用面向切面编程思想——AOP，除了广义上的过滤器和中间件以外，主要通过动态代理的形式来实现AOP编程思想，主要的案例共有四个，分别是：  
1、服务日志AOP；  
2、服务InMemory缓存AOP；  
3、服务Redis缓存AOP；  
4、服务事务AOP；  
  
   
具体的代码可以在 `Blog.Core\Blog.Core\AOP` 文件夹下查看。  
  
与此同时，多个AOP也设置了阀门来控制是否开启，具体的可以查看 `appsettings.json` 中的：  

```
  "AppSettings": {
    "RedisCachingAOP": {
      "Enabled": false,
      "ConnectionString": "127.0.0.1:6319"
    },
    "MemoryCachingAOP": {
      "Enabled": true
    },
    "LogAOP": {
      "Enabled": false
    },
    "TranAOP": {
      "Enabled": false
    },
    "SqlAOP": {
      "Enabled": false
    }
  },

```

## Appsettings 

整个系统通过一个封装的操作类 `Appsettings.cs` 来控制配置文件 `appsettings.json` 文件，  
操作类地址在：`\Blog.Core.Common\Helper` 文件夹下。  
具体的使用方法是：  

```
Appsettings.app(new string[] { "AppSettings", "RedisCachingAOP", "Enabled" })

// 里边的参数，按照 appsettings.json 中设置的层级顺序来写，可以获取到指定的任意内容。

```



## AspNetCoreRateLimit

系统使用 `AspNetCoreRateLimit` 组件来实现ip限流：
1、添加 `nuget` 包：
```
<PackageReference Include="AspNetCoreRateLimit" Version="3.0.5" />
```

2、注入服务 `IpPolicyRateLimitSetup.cs`
```
services.AddIpPolicyRateLimitSetup(Configuration);
```

3、配置中间件
```
 // Ip限流,尽量放管道外层
 app.UseIpRateLimiting();
```

4、配置数据  

具体的内容，自行百度即可
```
  "IpRateLimiting": {
    "EnableEndpointRateLimiting": true,
    "StackBlockedRequests": false,
    "RealIpHeader": "X-Real-IP",
    "ClientIdHeader": "X-ClientId",
    "HttpStatusCode": 429,//返回状态码
    "GeneralRules": [//规则,结尾一定要带*
      {
        "Endpoint": "*",
        "Period": "1m",
        "Limit": 120
      },
      {
        "Endpoint": "*:/api/blog*",
        "Period": "1m",
        "Limit": 30
      }
    ]

  }
```



## Async-Await 

整个系统采用 async/await 异步编程，符合主流的开发模式，   
特别是对多线程开发很友好。



## Authorization-Ids4

本系统 v1.0 版本（目前的 is4 分支，如果没有该分支，表示已经迁移到主分支）已经实现了对 `IdentityServer4` 的迁移，已经支持了统一授权认证，和 `blog` 项目、`Admin` 项目、`DDD` 项目等一起，使用一个统一的认证中心。  
  
具体的代码参考：`Blog.Core\Blog.Core\Extensions` 文件夹下的 `AuthorizationSetup.cs` 中 `Ids4` 认证的部分，注意需要引用指定的 `nuget` 包：   

```
  // 2.添加Identityserver4认证
  .AddIdentityServerAuthentication(options =>
  {
      options.Authority = "https://ids.neters.club";
      options.RequireHttpsMetadata = false;
      options.ApiName = "blog.core.api";
      options.SupportedTokens = IdentityServer4.AccessTokenValidation.SupportedTokens.Jwt;
      options.ApiSecret = "api_secret";

  })

```
  
### 如何在Swagger中配置？
很简单，直接在 `Swagger` 中直接接入 `oauth、Implicit` 即可：  

```
 //接入identityserver4
 c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
 {
     Type = SecuritySchemeType.OAuth2,
     Flows = new OpenApiOAuthFlows
     {
         Implicit = new OpenApiOAuthFlow
         {
             AuthorizationUrl = new Uri($"http://localhost:5004/connect/authorize"),
             Scopes = new Dictionary<string, string> {
                 {
                     "blog.core.api","ApiResource id" // 资源服务 id
                 }
             }
         }
     }
 });

```
  
然后在 `IdentityServer4`  项目中，做指定的修改：  

```
 new Client {
     ClientId = "blogadminjs",
     ClientName = "Blog.Admin JavaScript Client",
     AllowedGrantTypes = GrantTypes.Implicit,
     AllowAccessTokensViaBrowser = true,

     RedirectUris =
     {
         "http://vueadmin.neters.club/callback",
         // 这里要配置回调地址
         "http://localhost:8081/oauth2-redirect.html" 
     },
     PostLogoutRedirectUris = { "http://vueadmin.neters.club" },
     AllowedCorsOrigins =     { "http://vueadmin.neters.club" },

     AllowedScopes = {
         IdentityServerConstants.StandardScopes.OpenId,
         IdentityServerConstants.StandardScopes.Profile,
         "roles",
         "blog.core.api"
     }
 },

```



## Authorization-JWT 

如果你不想使用 `IdentityServer4` 的话，也可以使用 `JWT` 认证，同样是是`Blog.Core\Blog.Core\Extensions` 文件夹下的 `AuthorizationSetup.cs` 中有关认证的部分：  

```
 1.添加JwtBearer认证服务
.AddJwtBearer(o =>
{
    o.TokenValidationParameters = tokenValidationParameters;
    o.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = context =>
        {
            // 如果过期，则把<是否过期>添加到，返回头信息中
            if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
            {
                context.Response.Headers.Add("Token-Expired", "true");
            }
            return Task.CompletedTask;
        }
    };
})

```


## AutoMapper

使用 `AutoMapper` 组件来实现 `Dto` 模型的传输转换，具体的用法，可以查看：   
`Blog.Core\Blog.Core\Extensions` 文件夹下的 `AutoMapperSetup.cs` 扩展类，  
通过引用 `AutoMapper` 和 `AutoMapper.Extensions.Microsoft.DependencyInjection` 两个 `nuget` 包，并设置指定的 `profile` 文件，来实现模型转换控制。

```
// 比如如何定义：
 public class CustomProfile : Profile
 {
     /// <summary>
     /// 配置构造函数，用来创建关系映射
     /// </summary>
     public CustomProfile()
     {
         CreateMap<BlogArticle, BlogViewModels>();
         CreateMap<BlogViewModels, BlogArticle>();
     }
 }


// 比如如何使用
models = _mapper.Map<BlogViewModels>(blogArticle);

```
  
具体的查看项目中代码即可。  




## CORS

本项目使用的是 `nginx` 跨域代理，但是同时也是支持 `CORS` 代理的，  
具体的代码可以查看：  
`Blog.Core\Blog.Core\Extensions` 文件夹下的 `CorsSetup.cs` 扩展类，  
通过在 `appsettings.json` 文件中配置指定的前端项目 `ip:端口` ，来实现跨域：  

```

  "Startup": {
    "Cors": {
      "IPs": "http://127.0.0.1:2364,http://localhost:2364,http://localhost:8080,http://localhost:8021,http://localhost:1818"
    },
    "ApiName": "Blog.Core"
  },

```


## DI-AutoFac

项目使用了依赖注入，除了原生的依赖注入以外，更多的使用的是第三方组件 `Autofac` ：  
1、引用依赖包：
```
    <PackageReference Include="Autofac.Extensions.DependencyInjection" Version="5.0.1" />
    <PackageReference Include="Autofac.Extras.DynamicProxy" Version="4.5.0" />

```
主要是第一个 `nuget` 包，下边的是为了实现动态代理 `AOP` 操作；  

2、项目之间采用引用解耦的方式，通过反射来注入服务层和仓储层的程序集 `dll` 来实现批量注入，更方便，以后每次新增和修改 `Service` 层和 `Repository` 层，只需要 `F6` 编译一下即可，具体代码查看 `Startup.cs`：  

```


        // 注意在CreateDefaultBuilder中，添加Autofac服务工厂
        public void ConfigureContainer(ContainerBuilder builder)
        {
            var basePath = Microsoft.DotNet.PlatformAbstractions.ApplicationEnvironment.ApplicationBasePath;
            //builder.RegisterType<AdvertisementServices>().As<IAdvertisementServices>();


            #region 带有接口层的服务注入


            var servicesDllFile = Path.Combine(basePath, "Blog.Core.Services.dll");
            var repositoryDllFile = Path.Combine(basePath, "Blog.Core.Repository.dll");

            if (!(File.Exists(servicesDllFile) && File.Exists(repositoryDllFile)))
            {
                throw new Exception("Repository.dll和service.dll 丢失，因为项目解耦了，所以需要先F6编译，再F5运行，请检查 bin 文件夹，并拷贝。");
            }



            // AOP 开关，如果想要打开指定的功能，只需要在 appsettigns.json 对应对应 true 就行。
            var cacheType = new List<Type>();
            if (Appsettings.app(new string[] { "AppSettings", "RedisCachingAOP", "Enabled" }).ObjToBool())
            {
                builder.RegisterType<BlogRedisCacheAOP>();
                cacheType.Add(typeof(BlogRedisCacheAOP));
            }
            if (Appsettings.app(new string[] { "AppSettings", "MemoryCachingAOP", "Enabled" }).ObjToBool())
            {
                builder.RegisterType<BlogCacheAOP>();
                cacheType.Add(typeof(BlogCacheAOP));
            }
            if (Appsettings.app(new string[] { "AppSettings", "TranAOP", "Enabled" }).ObjToBool())
            {
                builder.RegisterType<BlogTranAOP>();
                cacheType.Add(typeof(BlogTranAOP));
            }
            if (Appsettings.app(new string[] { "AppSettings", "LogAOP", "Enabled" }).ObjToBool())
            {
                builder.RegisterType<BlogLogAOP>();
                cacheType.Add(typeof(BlogLogAOP));
            }

            // 获取 Service.dll 程序集服务，并注册
            var assemblysServices = Assembly.LoadFrom(servicesDllFile);
            builder.RegisterAssemblyTypes(assemblysServices)
                      .AsImplementedInterfaces()
                      .InstancePerDependency()
                      .EnableInterfaceInterceptors()//引用Autofac.Extras.DynamicProxy;
                      .InterceptedBy(cacheType.ToArray());//允许将拦截器服务的列表分配给注册。

            // 获取 Repository.dll 程序集服务，并注册
            var assemblysRepository = Assembly.LoadFrom(repositoryDllFile);
            builder.RegisterAssemblyTypes(assemblysRepository)
                   .AsImplementedInterfaces()
                   .InstancePerDependency();

            #endregion

            #region 没有接口层的服务层注入

            //因为没有接口层，所以不能实现解耦，只能用 Load 方法。
            //注意如果使用没有接口的服务，并想对其使用 AOP 拦截，就必须设置为虚方法
            //var assemblysServicesNoInterfaces = Assembly.Load("Blog.Core.Services");
            //builder.RegisterAssemblyTypes(assemblysServicesNoInterfaces);

            #endregion

            #region 没有接口的单独类 class 注入

            //只能注入该类中的虚方法
            builder.RegisterAssemblyTypes(Assembly.GetAssembly(typeof(Love)))
                .EnableClassInterceptors()
                .InterceptedBy(cacheType.ToArray());

            #endregion


            // 这里和注入没关系，只是获取注册列表，请忽略
            tsDIAutofac.AddRange(assemblysServices.GetTypes().ToList());
            tsDIAutofac.AddRange(assemblysRepository.GetTypes().ToList());
        }

```

3、然后 `Program.cs` 中也要加一句话：` .UseServiceProviderFactory(new AutofacServiceProviderFactory()) //<--NOTE THIS `  



## DI-NetCore

除了主要的 `Autofac` 依赖注入以外，也减少的使用了原生的依赖注入方式，很简单，比如这样的：  
```

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            // 注入权限处理器
            services.AddScoped<IAuthorizationHandler, PermissionHandler>();
            services.AddSingleton(permissionRequirement);
```


## Filter
精力有限，还是更新中...   
如果你愿意帮忙，可以直接在GitHub中，提交pull request，   
我会在后边的贡献者页面里，列出你的名字和项目地址做推广

## Framework 

精力有限，还是更新中...   
如果你愿意帮忙，可以直接在GitHub中，提交pull request，   
我会在后边的贡献者页面里，列出你的名字和项目地址做推广
## GlobalExceptionsFilter

精力有限，还是更新中...   
如果你愿意帮忙，可以直接在GitHub中，提交pull request，   
我会在后边的贡献者页面里，列出你的名字和项目地址做推广
## HttpContext

精力有限，还是更新中...   
如果你愿意帮忙，可以直接在GitHub中，提交pull request，   
我会在后边的贡献者页面里，列出你的名字和项目地址做推广

## Log4 

精力有限，还是更新中...   
如果你愿意帮忙，可以直接在GitHub中，提交pull request，   
我会在后边的贡献者页面里，列出你的名字和项目地址做推广
## MemoryCache

精力有限，还是更新中...   
如果你愿意帮忙，可以直接在GitHub中，提交pull request，   
我会在后边的贡献者页面里，列出你的名字和项目地址做推广
## Middleware

精力有限，还是更新中...   
如果你愿意帮忙，可以直接在GitHub中，提交pull request，   
我会在后边的贡献者页面里，列出你的名字和项目地址做推广
## MiniProfiler

精力有限，还是更新中...   
如果你愿意帮忙，可以直接在GitHub中，提交pull request，   
我会在后边的贡献者页面里，列出你的名字和项目地址做推广

## publish
精力有限，还是更新中...   
如果你愿意帮忙，可以直接在GitHub中，提交pull request，   
我会在后边的贡献者页面里，列出你的名字和项目地址做推广


## Redis

精力有限，还是更新中...   
如果你愿意帮忙，可以直接在GitHub中，提交pull request，   
我会在后边的贡献者页面里，列出你的名字和项目地址做推广
## Repository
精力有限，还是更新中...   
如果你愿意帮忙，可以直接在GitHub中，提交pull request，   
我会在后边的贡献者页面里，列出你的名字和项目地址做推广
## SeedData

精力有限，还是更新中...   
如果你愿意帮忙，可以直接在GitHub中，提交pull request，   
我会在后边的贡献者页面里，列出你的名字和项目地址做推广
## SignalR

精力有限，还是更新中...   
如果你愿意帮忙，可以直接在GitHub中，提交pull request，   
我会在后边的贡献者页面里，列出你的名字和项目地址做推广
## SqlSugar

精力有限，还是更新中...   
如果你愿意帮忙，可以直接在GitHub中，提交pull request，   
我会在后边的贡献者页面里，列出你的名字和项目地址做推广
## SqlSugar-Codefirst&DataSeed

精力有限，还是更新中...   
如果你愿意帮忙，可以直接在GitHub中，提交pull request，   
我会在后边的贡献者页面里，列出你的名字和项目地址做推广
## SqlSugar-SqlAOP

精力有限，还是更新中...   
如果你愿意帮忙，可以直接在GitHub中，提交pull request，   
我会在后边的贡献者页面里，列出你的名字和项目地址做推广
## Swagger

精力有限，还是更新中...   
如果你愿意帮忙，可以直接在GitHub中，提交pull request，   
我会在后边的贡献者页面里，列出你的名字和项目地址做推广
## T4

精力有限，还是更新中...   
如果你愿意帮忙，可以直接在GitHub中，提交pull request，   
我会在后边的贡献者页面里，列出你的名字和项目地址做推广
## Test-xUnit

精力有限，还是更新中...   
如果你愿意帮忙，可以直接在GitHub中，提交pull request，   
我会在后边的贡献者页面里，列出你的名字和项目地址做推广
## Temple-Nuget 
精力有限，还是更新中...   
如果你愿意帮忙，可以直接在GitHub中，提交pull request，   
我会在后边的贡献者页面里，列出你的名字和项目地址做推广
## UserInfo 

精力有限，还是更新中...   
如果你愿意帮忙，可以直接在GitHub中，提交pull request，   
我会在后边的贡献者页面里，列出你的名字和项目地址做推广
