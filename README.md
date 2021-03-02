<img align="right" height="50px" src="http://apk.neters.club/logocore.png">

# Blog.Core

[English](readme-en.md) | 简体中文

[![sdk](https://img.shields.io/badge/sdk-5.0.1-d.svg)](#)  [![Build status](https://github.com/anjoy8/blog.core/workflows/.NET%20Core/badge.svg)](https://github.com/anjoy8/Blog.Core/actions)  [![Build Status](https://dev.azure.com/laozhangisphi/anjoy8/_apis/build/status/anjoy8.Blog.Core?branchName=master)](https://dev.azure.com/laozhangisphi/anjoy8/_build?definitionId=1)  [![codecov](https://codecov.io/gh/anjoy8/Blog.Core/branch/master/graph/badge.svg)](https://codecov.io/gh/anjoy8/Blog.Core)  [![License MIT](https://img.shields.io/badge/license-Apache-blue.svg?style=flat-square)](https://github.com/anjoy8/Blog.Core/blob/master/LICENSE) [![star this repo](http://githubbadges.com/star.svg?user=anjoy8&repo=blog.core&style=flat)](https://github.com/boennemann/badges)  [![fork this repo](http://githubbadges.com/fork.svg?user=anjoy8&repo=blog.core&style=flat)](https://github.com/boennemann/badges/fork)  [![博客园](https://img.shields.io/badge/博客园-老张的哲学-brightgreen.svg)](https://www.cnblogs.com/laozhang-is-phi/)


&nbsp;
&nbsp;

 
<div style="text-align: center;">
<a href="https://mvp.microsoft.com/zh-cn/PublicProfile/5003704?fullName=anson%20zhang" >
  <img src="http://apk.neters.club/MVP_Logo_Horizontal_Preferred_Cyan300_CMYK_72ppi.png" alt="MVP"   >
</a>

<a href="https://dotnetfoundation.org/member/Profile" >
  <img src="https://vueadmin.neters.club/images/1125120255netfoundation.png" alt=".netfoundation" width="220" >
</a>
</div>


Blog.Core 开箱即用的企业级前后端分离【 .NET Core5.0 Api + Vue 2.x + RBAC】权限框架。    
官网：http://apk.neters.club/.doc/   
已被多家公司所使用：[点击查看列表](https://github.com/anjoy8/Blog.Core/issues/75)    

&nbsp;

### 功能与进度

框架模块：  
- [x] 采用`仓储+服务+接口`的形式封装框架；
- [x] 异步 async/await 开发；
- [x] 接入国产数据库ORM组件 —— SqlSugar，封装数据库操作；
- [x] 支持自由切换多种数据库，MySql/SqlServer/Sqlite/Oracle/Postgresql/达梦/人大金仓；
- [x] 实现项目启动，自动生成种子数据 ✨； 
- [x] 五种日志记录，审计/异常/请求响应/服务操作/Sql记录等； 
- [x] 支持项目事务处理（若要分布式，用cap即可）✨；
- [x] 设计4种 AOP 切面编程，功能涵盖：日志、缓存、审计、事务 ✨；
- [x] 支持 T4 代码模板，自动生成每层代码；
- [x] 或使用 DbFirst 一键创建自己项目的四层文件（支持多库）；
- [x] 封装`Blog.Core.Webapi.Template`项目模板，一键重建自己的项目 ✨；
- [x] 搭配多个前端案例供参考和借鉴：Blog.Vue、Blog.Admin、Nuxt.tbug、Blog.Mvp.Blazor ✨；
- [x] 统一集成 IdentityServer4 认证 ✨;

组件模块：
- [x] 提供 Redis 做缓存处理；
- [x] 使用 Swagger 做api文档；
- [x] 使用 MiniProfiler 做接口性能分析 ✨；
- [x] 使用 Automapper 处理对象映射；  
- [x] 使用 AutoFac 做依赖注入容器，并提供批量服务注入 ✨；
- [x] 支持 CORS 跨域；
- [x] 封装 JWT 自定义策略授权；
- [x] 使用 Log4Net 日志框架，集成原生 ILogger 接口做日志记录；
- [x] 使用 SignalR 双工通讯 ✨；
- [x] 添加 IpRateLimiting 做 API 限流处理;
- [x] 使用 Quartz.net 做任务调度（目前单机多任务，集群调度暂不支持）;
- [x] 支持 数据库`读写分离`和多库操作 ✨;
- [x] 新增 Redis 消息队列 ✨;
- [x] 新增 RabbitMQ 消息队列 ✨;
- [x] 新增 EventBus 事件总线 ✨;
- [x] 调试中 - 统一聚合支付;
- [ ] 计划 - 数据部门权限;
- [ ] 计划 - ES 搜索;

微服务模块：
- [x] 可配合 Docker 实现容器化；
- [x] 可配合 Jenkins 实现CI / CD；
- [x] 可配合 Consul 实现服务发现；
- [x] 可配合 Ocelot 实现网关处理；
- [x] 可配合 Nginx  实现负载均衡；
- [x] 可配合 Ids4   实现认证中心；


&nbsp;

## 给个星星! ⭐️
如果你喜欢这个项目或者它帮助你, 请给 Star~   
如果你的项目中借鉴了本项目，请稍微说明下[https://github.com/anjoy8/Blog.Core/issues/75](https://github.com/anjoy8/Blog.Core/issues/75)，开源不易✨。  



&nbsp;

## 官方文档 📕

还在陆续整理中，不过基本操作都在,包括如何新手入门，配置数据，连接DB等等    

[官方文档](http://apk.neters.club/.doc/)    
[公众号重要文章+视频地址](https://mvp.neters.club/)    




&nbsp;

### 系统架构图


![系统架构图](https://img.neters.club/github/20201228135550.png)

&nbsp;

&nbsp;
### 系统压测结果报告


<div align=center><img width="500" src="http://apk.neters.club/JMeterTest.png" /></div>

本项目是 .netCore 后端部分，前端部分请看我的另三个Vue工程项目
 
&nbsp;
&nbsp;
&nbsp;
&nbsp;

|个人博客Vue版本|tBug项目Nuxt版本|VueAdmin权限管理后台|
|-|-|-|
|[https://github.com/anjoy8/Blog.Vue](https://github.com/anjoy8/Blog.Vue)|[https://github.com/anjoy8/Nuxt.tBug](https://github.com/anjoy8/Nuxt.tBug)|[https://github.com/anjoy8/Blog.Admin](https://github.com/anjoy8/Blog.Admin)|
|[http://vueblog.neters.club](http://vueblog.neters.club)|[http://tibug.neters.club](http://tibug.neters.club)|[http://vueadmin.neters.club](http://vueadmin.neters.club)|



&nbsp;

### 初始化项目
 

下载项目后，编译如果没问题，直接运行即可，会自动生成种子数据，数据库是`Sqlite`，接口文档是`swagger`。    

更多操作，点击这里：http://apk.neters.club/.doc/guide/getting-started.html


&nbsp;

## Nuget Packages

| Package | NuGet Stable |  Downloads |
| ------- | -------- | ------- |
| [Blog.Core.Webapi.Template](https://www.nuget.org/packages/Blog.Core.Webapi.Template/) | [![Blog.Core.Webapi.Template](https://img.shields.io/nuget/v/Blog.Core.Webapi.Template.svg)](https://www.nuget.org/packages/Blog.Core.Webapi.Template/)  | [![Blog.Core.Webapi.Template](https://img.shields.io/nuget/dt/Blog.Core.Webapi.Template.svg)](https://www.nuget.org/packages/Blog.Core.Webapi.Template/) |


关于如何使用，点击这里：https://www.cnblogs.com/laozhang-is-phi/p/10205495.html

&nbsp;
&nbsp;

## 其他后端框架
目前一共开源四个框架项目，感兴趣的可以看看

|单层项目|简单仓储框架|仓储+服务+接口|DDD框架|
|-|-|-|-|
|CURD+Seed|CURD+Seed+DI|CURD+Seed+DI+AOP等|DDD+EFCore+DI+EventBus等|
|[NetCore-Sugar-Demo](https://github.com/anjoy8/NetCore-Sugar-Demo)|[Blog.SplRepository.Demo](https://github.com/anjoy8/Blog.SplRepository.Demo)|[Blog.Core](https://github.com/anjoy8/Blog.Core)|[ChristDDD](https://github.com/anjoy8/ChristDDD)|
| -|[Blog-EFCore-Sqlite](https://github.com/anjoy8/Blog-EFCore-Sqlite)|- | -|


&nbsp;



&nbsp;

## 售后服务与支持  

鼓励作者，简单打赏，入微信群，随时随地解答我框架中（NetCore、Vue、DDD、IdentityServer4等）的疑难杂症。     
注意主要是帮忙解决bug和思路，不会远程授课，但是可以适当发我代码，我帮忙调试，       
打赏的时候，备注自己的微信号，我拉你进群，两天内没回应，QQ私聊我（3143422472）；   

[赞赏列表](http://apk.neters.club/.doc/Contribution/)  

 
<img src="http://apk.neters.club/laozhangisphigood.jpg" alt="赞赏码" width="300" >
[图片若加载不出来，点这里](http://apk.neters.club/laozhangisphigood.jpg)



*****************************************************
### 文章+视频+直播

博客园：https://www.cnblogs.com/laozhang-is-phi/

 Bilibili：https://space.bilibili.com/387802716  
 
 直播间：https://live.bilibili.com/21507364

```
```


&nbsp;

如果你感觉看着这整个项目比较费劲，我单抽出来了几个子Demo，方便学习，项目地址 ：[https://github.com/anjoy8/BlogArti](https://github.com/anjoy8/BlogArti)



