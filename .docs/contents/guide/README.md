# 介绍

从零开始搭建自己的前后端分离【 .NET Core3.1 Api + Vue 2.x 】框架。
ASP.NET Core 2.2/3.1 教程，前后端分离的后端接口，vue教程的姊妹篇。


## 它是如何工作的？

这是一个基于 netcore 2.x 的 webapi 项目，配合搭建前后端分离工程。

**************************************************************
系统环境

windows 10、SQL server 2012、Visual Studio 2017、Windows Server 2008 R2

后端技术：

  * .Net Core 2.0 API（因为想单纯搭建前后端分离，因此就选用的API，如果想了解.Net Core MVC，也可以交流）
  
  * Swagger 前后端文档说明，基于RESTful风格编写接口

  * Repository + Service 仓储模式编程

  * Async和Await 异步编程

  * Cors 简单的跨域解决方案

  * AOP基于切面编程技术

  * Autofac 轻量级IoC和DI依赖注入

  * Vue 本地代理跨域方案，Nginx跨域代理

  * JWT权限验证



数据库技术

  * SqlSugar 轻量级ORM框架，CodeFirst

  * T4 模板生成

  * AutoMapper 自动对象映射



分布式缓存技术

  * Redis 轻量级分布式缓存



前端技术

  * Vue 2.0 框架全家桶 Vue2 + VueRouter2 + Webpack + Axios + vue-cli + vuex

  * ElementUI 基于Vue 2.0的组件库

  * Nuxt.js服务端渲染SSR


## 它能做什么？

它能帮助您快速搭建基于asp.net core的api项目，无缝对接微服务。
提供丰富的文档和视频讲解，快速入门。


## 功能与进度

- [√] 采用仓储+服务+接口的形式封装框架；
- [√] 使用Swagger做api文档；
- [√] 使用MiniProfiler做接口性能分析；
- [√] 使用Automapper做Dto处理；
- [√] 接入SqlSugar ORM，封装数据库操作；
- [√] 项目启动，自动生成seed种子数据；
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
- [ ] IdentityServer4 认证（更新中...);
- [ ] API 限速;
- [ ] Redis 队列;
- [ ] 作业调度 Quartz.net;
- [ ] Sqlsugar 读写分离;
- [ ] 支付;
- [ ] 数据部门权限;

