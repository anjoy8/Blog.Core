# W 文档指南
## 亮点与优势

Blog.Core 是一个开箱即用的企业级权限管理应用框架。  
采用最新的前后端完全分离技术【 ASP.NET Core Api 3.x + Vue 2.x 】。  
并结合 `IdentityServer4` ，可快速解决多客户端和多资源服务的统一认证与鉴权的问题。   
  

### 为什么选择 ASPNET.Core
1、【开源】`ASPNET.NET Core` 是由 `Microsoft` 和 `.NET` 社区在 `GitHub` 上开源并维护的一个跨平台（支持 Windows、macOS 和 Linux）的新一代高性能框架，
拥有十分广泛的社区与支持者，可用于构建web应用、物联网IOT应用和移动端应用。  
2、【高效】Asp.net core(.net core)来源于.net，很容易迁移，而且也很容易上手，
但是又是不同的一个框架，除了上述对.net开发者十分友好以外，相对于之前的.net项目，速度上有巨大的改进，
相比与原来的`Web（.net framework 4.6）`程序性能提升了`2300%`。跟`python`、`java`等相同环境比较，性能都要优越，
参考[www.techempower.com](https://www.techempower.com/benchmarks/)。  
3、【跨平台】可以在`Windows`、`Mac`和`Linux`构建和运行跨平台的`Asp.Net Core`应用。  
4、【云原生】在云原生领域拥有天然的优势，搭配Azure云服务，配合K8s，更好的实现分布式应用，以及微服务应用。   
5、【微服务】`ASP.NET Core`尤其适用于微服务架构，也就是说ASP.NET Core不仅适合于中小型项目而且还特别适合于大型，超大型项目。  
6、【大公司】目前国内采用`ASP.NET Core`的大公司比如腾讯、网易，国际的有Bing，GoDaddy，Stackoverflow，Adobe，Microsoft
7、【总结来说】，`java`支持的，`ASPNET.Core`都支持，而且更轻量级、更高效跨，并且对.net开发者十分友好，微服务案例成熟。



### 框架功能点
1、丰富完整的接口文档，在查看的基础上，可以模拟前端调用，更方便。  
2、采用多层开发，隔离性更好，封装更完善。  
3、基于项目模板，可以一键创建自己的项目。  
4、搭配代码生成器，实现快速开发，节省成本。  
5、项目集成多库模式以及读写分离模式，可以同时处理多个数据库的不同模块，更快更安全。  
6、集成统一认证平台 `IdentityServer4` ，实现多个项目的统一认证管理，解决了之前一个项目，
一套用户的弊端，更适用微服务的开发。  
7、丰富的审计日志处理，方便线上项目快速定位异常点。  
8、支持自由切换多种数据库，Sqlite/SqlServer/MySql/PostgreSQL/Oracle；  
9、支持 `Docker` 容器化开发，可以搭配 k8s 更好的实现微服务。  


### 应用领域
1、【对接第三方api】项目通过`webapi`，可以快速对接第三方`api`服务，实现业务逻辑。  
2、【前后端分离】 采用的是`API`+前端的完全分离的开发模式，满足平时开发的所有需求，
你可以对接任何的自定义前端项目：无论是微信小程序，还是授权APP，无论是PC网页，
还是手机H5。  
3、【多项目】同时框架还集成了一套鉴权平台，采用IdentityServer4，可以快速的实现多个客户端的认证与授权服务，
从而大大的减少了平时的工作量，可以快速的进行产品迭代。  
4、【微服务】当然，因为采用的是API模式，所以同样适用于微服务项目，实现高并发的产品需求。  



### 市场前景
1、前后端分离模式已经是目前的主流开发模式，框架已经是一套可行的方案，开箱即用。 
2、拥有几十篇技术文档和3000人的技术社区，方便快捷的解决问题。  
3、目前已经有超过20多家公司在生产环境中使用，当然实际中更多，具体查看 [点击查看使用的情况](https://github.com/anjoy8/Blog.Core/issues/75)。  
4、同时可以搭配自己的业务，实现微服务的开发，在大数据高并发中，占有更好的优势。  
5、本项目直接作者由微软MVP“老张的哲学”出品，并长久维护，不会断更，有保障。



## 功能与进度

- [√] 采用仓储+服务+接口的形式封装框架；
- [√] 使用Swagger做api文档；
- [√] 使用MiniProfiler做接口性能分析；
- [√] 使用Automapper做Dto处理；
- [√] 接入SqlSugar ORM，封装数据库操作；
- [√] 项目启动，自动生成seed种子数据；
- [√] 提供五种日志输出；
- [√] 支持自由切换多种数据库，Sqlite/SqlServer/MySql/PostgreSQL/Oracle；
- [√] 异步async/await开发；
- [√] 支持事务；
- [√] AutoFac接入做依赖注入；
- [√] 支持AOP切面编程；
- [√] 支持CORS跨域；
- [√] 支持T4代码模板，自动生成每层代码；
- [√] 支持一键创建自己项目；
- [√] 封装 JWT 自定义策略授权；
- [√] 使用Log4Net日志框架+自定义日志输出；
- [√] 使用SingleR推送日志信息到管理后台；
- [√] 搭配前端Blog项目，vue开发；
- [√] 搭配一个Admin管理后台，用vue+ele开发；
- [√] IdentityServer4 认证;
- [√] API 限速;
- [√] 作业调度 Quartz.net;
- [√] Sqlsugar 读写分离;
- [ ] Redis 队列;
- [ ] 支付;
- [ ] 数据部门权限;



## 它是如何工作的？

这是一个基于 ASP.NET Core 3.1 的 api 项目，配合搭建 VUE 实现前后端分离工程。

**************************************************************
系统环境

> windows 10、SQL server 2012、Visual Studio 2017、Windows Server 2008 R2

后端技术：

> 1、ASP.NET Core 3.1 API 
 2、Swagger 前后端文档说明，基于RESTful风格编写接口  
 3、Repository + Service 仓储模式编程  
 4、Async和Await 异步编程  
 5、CORS 简单的跨域解决方案  
 6、AOP基于切面编程技术  
 7、Autofac 轻量级IoC和DI依赖注入  
 8、Vue 本地代理跨域方案，Nginx跨域代理  
 9、JWT权限验证  
10、Filter 过滤器  
11、Middleware 中间件  
12、AutoMapper 自动对象映射
13、Redis  


数据库技术

> SqlSugar 轻量级ORM框架，CodeFirst  
 T4 模板生成框架结构  
 支持SqlServer、Mysql、Sqlite、Oracle、Pgql数据库  
 支持多库操作




前端技术

> Vue 2.x 框架全家桶 Vue2 + VueRouter2 + Webpack + Axios + vue-cli + vuex  
ElementUI 基于Vue 2.0的组件库  
Nuxt.js服务端渲染SSR  



