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

本系统 v2.0 版本（目前的系统已经集成 `ids4` 和 `jwt`，并且可以自由切换），已经支持了统一授权认证，和 `blog` 项目、`Admin` 项目、`DDD` 项目等一起，使用一个统一的认证中心。  
  
具体的代码参考：`.\Blog.Core\Extensions` 文件夹下的 `Authorization_Ids4Setup.cs` ，注意需要引用指定的 `nuget` 包，核心代码如下：   

```
    //【认证】
  services.AddAuthentication(o =>
  {
      o.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
      o.DefaultChallengeScheme = nameof(ApiResponseHandler);
      o.DefaultForbidScheme = nameof(ApiResponseHandler);
  })
  // 2.添加Identityserver4认证
  .AddIdentityServerAuthentication(options =>
  {
      options.Authority = Appsettings.app(new string[] { "Startup", "IdentityServer4", "AuthorizationUrl" });
      options.RequireHttpsMetadata = false;
      options.ApiName = Appsettings.app(new string[] { "Startup", "IdentityServer4", "ApiName" });
      options.SupportedTokens = IdentityServer4.AccessTokenValidation.SupportedTokens.Jwt;
      options.ApiSecret = "api_secret";

  })


```
  
### 如何在Swagger中配置Ids4？
很简单，直接在 `SwaggerSetup.cs` 中直接接入 `oauth、Implicit` 即可：  

```
 //接入identityserver4
 c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
 {
     Type = SecuritySchemeType.OAuth2,
     Flows = new OpenApiOAuthFlows
     {
         Implicit = new OpenApiOAuthFlow
         {
             AuthorizationUrl = new Uri($"{Appsettings.app(new string[] { "Startup", "IdentityServer4", "AuthorizationUrl" })}/connect/authorize"),
             Scopes = new Dictionary<string, string> {
             {
                 "blog.core.api","ApiResource id"
             }
         }
         }
     }
 });

```
  
然后在 `IdentityServer4`  项目中，做指定的修改，配置 `8081` 的回调地址：  

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

然后再 `Swagger` 中，配置登录授权：  

<img src="http://apk.neters.club/images/20200507213830.png" alt="swagger" width="600" >


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

在线项目使用的是 `nginx` 跨域代理，但是同时也是支持 `CORS` 代理：    
1、注入服务 `services.AddCorsSetup();` 具体代码 `Blog.Core\Blog.Core\Extensions` 文件夹下的 `CorsSetup.cs` 扩展类；  
2、配置中间件 `app.UseCors("LimitRequests");` ,要注意中间件顺序；  
3、配置自己项目的前端端口，通过在 `appsettings.json` 文件中配置自己的前端项目 `ip:端口` ，来实现跨域：  

```
  "Startup": {
    "Cors": {
      "IPs": "http://127.0.0.1:2364,http://localhost:2364,http://localhost:8080,http://localhost:8021,http://localhost:1818"
    }
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

项目中一共有四个过滤器
```
1、GlobalAuthorizeFilter.cs —— 全局授权配置，添加后，就可以不用在每一个控制器上添加 [Authorize] 特性，但是3.1版本好像有些问题，【暂时放弃使用】；
2、GlobalExceptionFilter.cs —— 全局异常处理，实现 actionContext 级别的异常日志收集；
3、GlobalRoutePrefixFilter.cs —— 全局路由前缀公约，统计在路由上加上前缀；
4、UseServiceDIAttribute.cs —— 测试注入，【暂时无用】；
```
文件地址在 `.\Blog.Core\Filter` 文件夹下，其中核心的是 `2` 个，重点使用的是 `1` 个 —— 全局异常错误日志 `GlobalExceptionsFilter`:
通过注册在 `MVC` 服务 `services.AddControllers()` 中，实现全局异常过滤：
```
 services.AddControllers(o =>
 {
     // 全局异常过滤
     o.Filters.Add(typeof(GlobalExceptionsFilter));
     // 全局路由权限公约
     //o.Conventions.Insert(0, new GlobalRouteAuthorizeConvention());
     // 全局路由前缀，统一修改路由
     o.Conventions.Insert(0, new GlobalRoutePrefixFilter(new RouteAttribute(RoutePrefix.Name)));
 })
```



## Framework 

项目采用 `服务+仓储+接口` 的多层结构，使用依赖注入，并且通过解耦项目，较完整的实现了 `DIP` 原则：  
高层模块不应该依赖于底层模块，二者都应该依赖于抽象。  
抽象不应该依赖于细节，细节应该依赖于抽象。  

同时项目也封装了:  
`CodeFirst` 初始化数据库以及数据；  
`DbFirst` 根据数据库（支持多库），生成多层代码，算是简单代码生成器；  
其他功能，[核心功能与进度](http://apk.neters.club/.doc/guide/#%E5%8A%9F%E8%83%BD%E4%B8%8E%E8%BF%9B%E5%BA%A6)


 

## Log 

通过集成 `Log4Net` 组件，完美配合 `NetCore` 官方的 `ILogger<T>` 接口，实现对日志的管控，引用 `nuget` 包 `Microsoft.Extensions.Logging.Log4Net.AspNetCore`:
Program.cs
```
  webBuilder
  .UseStartup<Startup>()
  .ConfigureLogging((hostingContext, builder) =>
  {
      //该方法需要引入Microsoft.Extensions.Logging名称空间
      builder.AddFilter("System", LogLevel.Error); //过滤掉系统默认的一些日志
      builder.AddFilter("Microsoft", LogLevel.Error);//过滤掉系统默认的一些日志

      //添加Log4Net
      //var path = Directory.GetCurrentDirectory() + "\\log4net.config"; 
      //不带参数：表示log4net.config的配置文件就在应用程序根目录下，也可以指定配置文件的路径
      //需要添加nuget包：Microsoft.Extensions.Logging.Log4Net.AspNetCore
      builder.AddLog4Net();
  });

```

然后直接在需要的地方注入使用，比如在控制器中
` public UserController(ILogger<UserController> logger)`

然后就可以使用了。  

> 注意：日志 其实是分为两部分的：  
> netcore输出(控制台、输出窗口等) 和 `ILogger` 持久化  
> 两者对应配置也不一样，就比如上边的过滤，是针对日志持久化的，如果想要对控制台进行控制，需要配置 `appsettings.json` 中的 `Logging` 节点


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

项目集成 `T4` 模板 `.\Blog.Core.FrameWork` 层，目的是可以一键生成项目模板代码。  
1、需要在 `DbHelper.ttinclude` 中配置连接数据库连接字符串；  
2、针对每一层的代码，就去指定的 `.tt` 模板，直接 `CTRL+S` 保存即可；  

> 注意，目前的代码是 `SqlServer` 版本的，其他数据库版本的，可以去群文件查看。


## Test-xUnit

项目简单使用了单元测试，通过 `xUnit` 组件，具体的可以查看 `Blog.Core.Tests` 层相关代码。  
目前单元测试用例还比较少，大家可以自行添加。  


## Temple-Nuget 

本项目封装了 `Nuget` 自定义模板，你可以根据这个模板，一键创建自己的项目名，具体的操作，可以双击项目根目录下的 `CreateYourProject.bat` ，可以参考 [#如何项目重命名](http://apk.neters.club/.doc/guide/getting-started.html#%E5%A6%82%E4%BD%95%E9%A1%B9%E7%9B%AE%E9%87%8D%E5%91%BD%E5%90%8D)  

同时，你也可以再 `Nuget` 管理器中，搜索到：
<img src="http://apk.neters.club/images/20200507223058.png" alt="nuget" width="600" >



## UserInfo 


项目中封装了获取用户信息的代码：  
在 `.\Blog.Core.Common\HttpContextUser` 文件夹下 `AspNetUser.cs` 实现类和 `IUser.cs` 接口。  

如果使用，首先需要注册相应的服务，参见：`.\Blog.Core\Extensions` 文件夹下的 `HttpContextSetup.cs`；    
然后，就直接在控制器构造函数中，注入接口 `IUser` 即可；  

> `注意`：  
> 1、如果要想获取指定的服务，必须登录，也就是必须要在 `Header` 中传递有效 `Token` ，这是肯定的。    
> 2、如果要获取用户信息，一定要在中间件 `app.UseAuthentication()` 之后（不要问为什么），控制器肯定在它之后，所以能获取到；  
> 3、`【并不是】`一定需要添加 `[Authorize]` 特性，如果你加了这个特性，可以直接获取，但是如果不加，可以从我的 `AspNetUser.cs` 方法中，有一个直接从 `Header` 中解析的方法 `List<string> GetUserInfoFromToken(string ClaimType);`：

```
 public string GetToken()
 {
     return _accessor.HttpContext.Request.Headers["Authorization"].ObjToString().Replace("Bearer ", "");
 }

 public List<string> GetUserInfoFromToken(string ClaimType)
 {

     var jwtHandler = new JwtSecurityTokenHandler();
     if (!string.IsNullOrEmpty(GetToken()))
     {
         JwtSecurityToken jwtToken = jwtHandler.ReadJwtToken(GetToken());

         return (from item in jwtToken.Claims
                 where item.Type == ClaimType
                 select item.Value).ToList();
     }
     else
     {
         return new List<string>() { };
     }
 }

```
