Dev Build:: 

[![Gitter](https://badges.gitter.im/Blog_core/community.svg)](https://gitter.im/Blog_core/community?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge)  [![sdk](https://img.shields.io/badge/sdk-3.0-d.svg)](#)  [![Build status](https://ci.appveyor.com/api/projects/status/facdw9pinrxs1610?svg=true)](https://ci.appveyor.com/project/anjoy8/blog-core) [![codecov](https://codecov.io/gh/anjoy8/Blog.Core/branch/master/graph/badge.svg)](https://codecov.io/gh/anjoy8/Blog.Core)  [![License MIT](https://img.shields.io/badge/license-MIT-blue.svg?style=flat-square)](https://github.com/anjoy8/Blog.Core/blob/master/LICENSE) [![Language](https://img.shields.io/badge/language-csharp-d.svg)](#) 
[![star this repo](http://githubbadges.com/star.svg?user=anjoy8&repo=blog.core&style=flat)](https://github.com/anjoy8/blog.core)
[![fork this repo](http://githubbadges.com/fork.svg?user=anjoy8&repo=blog.core&style=flat)](https://github.com/anjoy8/blog.core/fork)



&nbsp;
&nbsp;

[ENGLISH](https://github.com/anjoy8/Blog.Core/blob/master/README-en.md) | [中文版](https://github.com/anjoy8/Blog.Core/blob/master/README.md)

![Logo](https://github.com/anjoy8/Blog.Core/blob/master/Blog.Core/wwwroot/logocore.png)


从零开始搭建自己的前后端分离【 .NET Core3.0 Api + Vue 2.x 】框架。（🔒目前是3.0 版本，因作者开源的项目较多，维护成本过高，所以本项目其他分支会缓慢不定时更新🔒）


&nbsp;
### 系统架构图


![系统架构图](https://github.com/anjoy8/Blog.Core/blob/master/Blog.Core.System.Architecture.png)

&nbsp;

&nbsp;
### 系统压测结果报告


&nbsp;
其他接口压测内存占用在：220~350 m 之间，具体的，自行压测即可。
&nbsp;



![系统压测结果报告](https://github.com/anjoy8/Blog.Core/blob/master/Blog.Core/wwwroot/JMeterTest.png)

&nbsp;

如果你感觉看着这整个项目比较费劲，我单抽出来了几个子Demo，方便学习，项目地址 ：[https://github.com/anjoy8/BlogArti](https://github.com/anjoy8/BlogArti)


这只是 .netCore 后端部分，前端部分请看我的另三个Vue工程项目
 
&nbsp;
&nbsp;
&nbsp;
&nbsp;

|个人博客Vue版本|tBug项目Nuxt版本|VueAdmin管理后台(更新中)|
|-|-|-|
|[https://github.com/anjoy8/Blog.Vue](https://github.com/anjoy8/Blog.Vue)|[https://github.com/anjoy8/Nuxt.tBug](https://github.com/anjoy8/Nuxt.tBug)|[https://github.com/anjoy8/Blog.Admin](https://github.com/anjoy8/Blog.Admin)|
|[http://vueblog.neters.club](http://vueblog.neters.club)|[http://vueblog.neters.club](http://vueblog.neters.club)|[http://vueadmin.neters.club](http://vueadmin.neters.club)|



&nbsp;

### 初始项目

#### 不要再使用 .sql 文件了，不更新了，用下边动图的方法，直接 seed data.

数据查看：[Blog.Core.Data.json](https://github.com/anjoy8/Blog.Data.Share/tree/master/Blog.Core.Data.json)

文章讲解：[支持多种数据库 & 快速数据库生成](https://www.cnblogs.com/laozhang-is-phi/p/10718755.html)
 
&nbsp;

 


![操作流程](https://github.com/anjoy8/Blog.Core/blob/master/Blog.Core/wwwroot/operateFlow.gif)


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

## 给个星星! ⭐️
如果你喜欢这个项目或者它帮助你, 请给 Star~（辛苦星咯）

*********************************************************

## Tips：
```

1【重要】、Blog.Core.FrameWork 项目是用T4模板生成文件的简单实现。如果有错误，可以联系我，
QQ群：867095512
如果你不想处理这个错误，你可以先把项目卸载，不影响整体运行。


2【重要】、项目中，有三个AOP的操作类，分别是Redis缓存切面，memory缓存切面、Log日志切面
你可以在自定义开关，对其进行是否启用，在 appsettings.json 中的：

    "RedisCaching": {
      "Enabled": false,
      "ConnectionString": "127.0.0.1:6319"
    },
    "MemoryCachingAOP": {
      "Enabled": true
    },
    "LogoAOP": {
      "Enabled": false
    },


3【重要】、如何你使用Redis，需要安装Redis服务端，安装和使用说明地址：
https://www.cnblogs.com/laozhang-is-phi/p/9554210.html#autoid-3-4-0
端口是 6319 ，注意！


4【重要+】、系统新增自动化生成数据库，和生成种子数据的功能，
在Blog.Core层中的 appsettings.json 中开启 SeedDBEnabled：true 即可。
具体文章请看：《[支持多种数据库 & 快速数据库生成](https://www.cnblogs.com/laozhang-is-phi/p/10718755.html)》。


5、如果你不想用CodeFirst 和种子数据，可以用数据库表结构Sql文件在数据库里执行，
在Blog.Core 项目下的 wwwroot 文件夹中Blog.Core.Table.sql（表结构）、Blog.Core.Table&Data.sql（结构和数据）。
或者来群里，群文件的是最新的。


6、如果想单独查看关于【JWT授权】的相关内容，可以访问 https://github.com/anjoy8/BlogArti/tree/master/Blog.Core_JWT，
   我单拎出来的一个demo。


7、项目后期发布的时候可以有两个办法，一种是dotnet的kestrel部署，另一种是 IIS 发布部署，但是在发布的时候，
因为解耦了，所以会导致无法把 service.dll & repository.dll 拷贝到生成目录下，大家可以采用：
Blog.Core -> 属性 -> Build Events -> Post-build event command ->>>>

Copy "$(ProjectDir)bin\Debug\netcoreapp2.2\" "$(SolutionDir)Blog.Core\bin\Debug\"

```
*********************************************************
### 修改数据库连接字符串

注意：修改完数据库连接字符串以后，一定要F6重新编译项目或者重启项目。

1、在Blog.Core层 appsettings.json 中，配置自己的字符串
```
    "SqlServer": {
      "SqlServerConnection": "Server=.;Database=WMBlogDB;User ID=sa;Password=123;",
      "ProviderName": "System.Data.SqlClient"
    },
```

2、文章中有三个地方用到了数据库连接字符串
```
A、系统中使用 Blog.Core.Repository -> BaseDBConfig.cs
B、Seed数据库 Blog.Core.Model -> MyContext.cs
C、T4 模板 Blog.Core.FrameWork -> DbHelper.ttinclude

其实针对AB两个情况，只需要配置 appsettings.json 即可

```

3、如果想使用T4模板，在Blog.Core.FrameWork层的DbHelper.ttinclude 中，配置自己的字符串
```
public static readonly string ConnectionString = File.Exists(@"D:\my-file\dbCountPsw2.txt") ? 
File.ReadAllText(@"D:\my-file\dbCountPsw2.txt").Trim(): "server=.;uid=sa;pwd=sa;database=BlogDB";
```


*****************************************************
### 三大平台同步直播

简  书：https://www.jianshu.com/notebooks/28621653

博客园：https://www.cnblogs.com/laozhang-is-phi/

 CSDN：https://blog.csdn.net/baidu_35726140
 
 码云：https://gitee.com/laozhangIsPhi/Blog.Core

```
```


<div class="allindex">
<h2 id="abp框架学习目录如下">.NetCore与Vue 框架学习目录如下</h2>
<ul>
<li>
<h3 id="autoid-2-1-0">后端 .net core 概览</h3>
<ul>
<li><a id="post_title_link_9495620" href="https://www.cnblogs.com/laozhang-is-phi/p/9495620.html">框架之二 || 后端项目搭建<br></a></li>
<li><a id="post_title_link_9495624" href="https://www.cnblogs.com/laozhang-is-phi/p/9495624.html">Swagger的使用 3.1</a></li>
<li><a id="post_title_link_9507387" href="https://www.cnblogs.com/laozhang-is-phi/p/9507387.html">Swagger的使用 3.2</a></li>
<li><a id="post_title_link_9511869" href="https://www.cnblogs.com/laozhang-is-phi/p/9511869.html">Swagger的使用 3.3 JWT权限验证【修改】</a></li>
<li><a id="post_title_link_9896431" href="https://www.cnblogs.com/laozhang-is-phi/p/9896431.html">36 ║解决JWT权限验证过期问题</a></li>
<li><a id="post_title_link_9516890" href="https://www.cnblogs.com/laozhang-is-phi/p/9516890.html">API项目整体搭建 6.1 仓储模式</a></li>
<li><a id="post_title_link_9523148" href="https://www.cnblogs.com/laozhang-is-phi/p/9523148.html">API项目整体搭建 6.2 轻量级ORM</a></li>
<li><a id="post_title_link_9529480" href="https://www.cnblogs.com/laozhang-is-phi/p/9529480.html">API项目整体搭建 6.3 异步泛型仓储+依赖注入初探</a></li>
<li><a id="post_title_link_9541414" href="https://www.cnblogs.com/laozhang-is-phi/p/9541414.html">依赖注入IoC学习 + AOP切面编程初探</a></li>
<li><a id="post_title_link_9547574" href="https://www.cnblogs.com/laozhang-is-phi/p/9547574.html">AOP面向切面编程浅解析：简单日志记录 + 服务切面缓存</a></li>
<li><a id="post_title_link_9554210" href="https://www.cnblogs.com/laozhang-is-phi/p/9554210.html">AOP自定义筛选，Redis入门 11.1</a></li>
<li><a id="post_title_link_9560949" href="https://www.cnblogs.com/laozhang-is-phi/p/9560949.html">三种跨域方式比较，DTOs(数据传输对象)初探</a></li>
<li><a id="post_title_link_9565227" href="https://www.cnblogs.com/laozhang-is-phi/p/9565227.html">DTOs 对象映射使用，项目部署Windows+Linux完整版</a></li>
<li><a id="post_title_link_9757999" href="https://www.cnblogs.com/laozhang-is-phi/p/9757999.html">三十二║ 四种方法快速实现项目的半自动化搭建</a></li>
<li><a id="post_title_link_9767400" href="https://www.cnblogs.com/laozhang-is-phi/p/9767400.html">三十三║ ⅖ 种方法实现完美跨域</a></li>
<li><a id="post_title_link_9795689" href="https://www.cnblogs.com/laozhang-is-phi/p/9795689.html">三十四║ Swagger 处理多版本控制，所带来的思考</a></li>
<li><a id="post_title_link_9855836" href="https://www.cnblogs.com/laozhang-is-phi/p/9855836.html">三十五║ 完美实现全局异常日志记录</a></li>
<li><a id="post_title_link_10139204" href="https://www.cnblogs.com/laozhang-is-phi/p/10139204.html">37 ║JWT完美实现权限与接口的动态分配</a></li>
 <li><a id="link_post_title" class="link-post-title" href="https://www.cnblogs.com/laozhang-is-phi/p/10173536.html">38 ║自动初始化数据库</a></li>
<li><a id="post_title_link_10205495" href="https://www.cnblogs.com/laozhang-is-phi/p/10205495.html">39 || 想创建自己的dotnet模板么？看这里</a></li>
<li><a id="post_title_link_10287023" href="https://www.cnblogs.com/laozhang-is-phi/p/10287023.html">40 || 完美基于AOP的接口性能分析</a></li>
 <li><a id="post_title_link_10322040" href="https://www.cnblogs.com/laozhang-is-phi/p/10322040.html">41 || Nginx+Github+PM2 快速部署项目(一)</a></li>

<li><a href="https://www.cnblogs.com/laozhang-is-phi/p/10462316.html">42&nbsp;</a><a id="post_title_link_9767400" href="https://www.cnblogs.com/laozhang-is-phi/p/9767400.html">║</a><a id="post_title_link_10462316" href="https://www.cnblogs.com/laozhang-is-phi/p/10462316.html"> 完美实现 JWT 滑动授权刷新</a></li>
<li><a id="post_title_link_10718755" href="https://www.cnblogs.com/laozhang-is-phi/p/10718755.html">43 ║ 支持多种数据库 &amp; 快速数据库生成</a></li>
<li><a id="post_title_link_10836887" href="https://www.cnblogs.com/laozhang-is-phi/p/beautifulPublish-mostBugs.html">43 ║最全的部署方案 &amp; 最丰富的错误分析【再会】</a></li>













</ul>

















</li>
<li>
<h3 id="autoid-2-2-0">前端 Vue 概览</h3>
<ul>
<li><a id="post_title_link_9577805" href="https://www.cnblogs.com/laozhang-is-phi/p/9577805.html">十四 ║ VUE 计划书 &amp; 我的前后端开发简史</a></li>
<li><a id="post_title_link_9580807" href="https://www.cnblogs.com/laozhang-is-phi/p/9580807.html">十五 ║Vue基础：JS面向对象&amp;字面量&amp; this字</a></li>
<li><a id="post_title_link_9585766" href="https://www.cnblogs.com/laozhang-is-phi/p/9585766.html">十六 ║Vue基础：ES6初体验 &amp; 模块化编程</a></li>
<li><a id="post_title_link_9593740" href="https://www.cnblogs.com/laozhang-is-phi/p/9593740.html">十七 ║Vue基础：使用Vue.js 来画博客首页+指令(一)</a></li>
<li><a id="post_title_link_9602077" href="https://www.cnblogs.com/laozhang-is-phi/p/9602077.html">十八║Vue基础: 指令(下)+计算属性+watch</a></li>
<li><a id="post_title_link_9611632" href="https://www.cnblogs.com/laozhang-is-phi/p/9611632.html">十九║Vue基础: 样式动态绑定+生命周期</a></li>
<li><a id="post_title_link_9622031" href="https://www.cnblogs.com/laozhang-is-phi/p/9622031.html">二十║Vue基础终篇：组件详解+项目说明<br><br></a></li>
<li>👆 上边的这些基础，可以不用看，如果你只想快速入门 Vue 的话<br><br></li>
<li><a id="post_title_link_9629026" href="https://www.cnblogs.com/laozhang-is-phi/p/9629026.html">二十一║Vue实战：开发环境搭建【详细版】</a></li>
<li><a id="post_title_link_9640974" href="https://www.cnblogs.com/laozhang-is-phi/p/9640974.html">二十二║Vue实战：个人博客第一版(axios+router)</a></li>
<li><a id="post_title_link_9647008" href="https://www.cnblogs.com/laozhang-is-phi/p/9647008.html">二十三║Vue实战：Vuex 其实很简单</a></li>
<li><a id="post_title_link_9658019" href="https://www.cnblogs.com/laozhang-is-phi/p/9658019.html">二十四║ Vuex + JWT 实现授权验证登陆</a></li>
<li><a id="post_title_link_9670342" href="https://www.cnblogs.com/laozhang-is-phi/p/9670342.html">二十五║初探SSR服务端渲染（个人博客二）</a></li>
<li><a id="post_title_link_9675822" href="https://www.cnblogs.com/laozhang-is-phi/p/9675822.html">二十六║Client渲染、Server渲染知多少{补充}</a></li>
<li><a id="post_title_link_9682289" href="https://www.cnblogs.com/laozhang-is-phi/p/9682289.html">二七║ Nuxt 基础：框架初探</a></li>
<li><a id="post_title_link_9687504" href="https://www.cnblogs.com/laozhang-is-phi/p/9687504.html">二八║ Nuxt 基础：面向源码研究Nuxt.js</a></li>
<li><a id="post_title_link_9697450" href="https://www.cnblogs.com/laozhang-is-phi/p/9697450.html">二九║ Nuxt实战：异步实现数据双端渲染</a></li>
<li><a id="post_title_link_9702677" href="https://www.cnblogs.com/laozhang-is-phi/p/9702677.html">三十║ Nuxt实战：动态路由+同构</a></li>
<li><a id="post_title_link_9713219" href="https://www.cnblogs.com/laozhang-is-phi/p/9713219.html">三十一║ Nuxt终篇：基于Vuex的权限验证探究</a></li>
<li></li>

















</ul>

















</li>

















</ul>


</div>
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
