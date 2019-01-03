[ENGLISH](https://github.com/anjoy8/Blog.Core/blob/master/README-en.md) | [中文版](https://github.com/anjoy8/Blog.Core/blob/master/README.md)


![Logo](https://github.com/anjoy8/Blog.Core/blob/master/Blog.Core/wwwroot/logocore.png)


Build your own front and rear end separation from scratch ". NET Core2.1 Api + Vue 2.0" framework, currently version 2.2, each version see branch.
It's just. Netcore back-end section, Front end section, see my other engineering vue

Https://github.com/anjoy8/Blog.Vue 

# Give a star!⭐️

If you like this project or it helps you, please give star~ (hard Star)

*********************************************************
# Tips: 
1. Blog.Core.FrameWork project is a simple implementation of generating files using T4 templates.
If there is an error, you can contact me,
QQ Group: 867095512
If you don't want to deal with this error, you can uninstall the project first without affecting the overall operation.

2. when the project is executed after downloading, the Redis server needs to be installed, installation and use of the description address:
https://www.cnblogs.com/laozhang-is-phi/p/9554210.html#autoid-5-0-0

3. the system new automated Generation database, and the ability to generate seed data, In the Progrm.cs in the Blog.core layer, cancel the Dbseed.seedasync (mycontext). Wait ();
The comment can be.

4. If you do not want to use Codefirst and seed data, you can use the database table structure SQL file to execute in the database,

In the Wwwroot folder under the Blog.core project.
*********************************************************

### Modify Database connection string
1, in the Blog.Core.Repository layer under the Sugar folder under the BaseDBConfig.cs, configure their own strings
``` 
public static string connectionstring = File.exists (@ "D:my-filedbCountPsw1.txt")? 
File.readalltext (@ "D:my-filedbCountPsw1.txt"). Trim ():  "server=.;
Uid=sa;pwd=sa;database=blogdb ";

```
2, in the Blog.Core.FrameWork layer of the dbhelper.ttinclude, configure their own strings
``` 
public static readonly String connectionstring = File.exists (@ "D:my-filedbCountPsw2.txt")? 
File.readalltext (@ "D:my-filedbCountPsw2.txt"). Trim ():  "server=.;
Uid=sa;pwd=sa;database=blogdb ";
```

*****************************************************

### three platforms synchronized live

Jianshu: https://www.jianshu.com/notebooks/28621653

Blog Park: https://www.cnblogs.com/laozhang-is-phi/
 
 csdn:https://blog.csdn.net/baidu_35726140 
 Code Cloud: Https://gitee.com/laozhangIsPhi/Blog.Core
 
 <div class=""allindex"">
<h1 id=""allindex"">Directory</h1>
<h2 id=""abp框架学习目录如下"">Lao Zhang. The Netcore and Vue Framework Learning catalogue is as follows</h2>
<ul>
<li>
<h3 id=""autoid-2-1-0"">Back-end. NET Core overview</h3>
<ul>
<li><a id=""post_title_link_9495620"" href=""https://www.cnblogs.com/laozhang-is-phi/p/9495620.html"">Framework bis | | Back-end Project construction<br></a></li>
<li><a id=""post_title_link_9495624"" href=""https://www.cnblogs.com/laozhang-is-phi/p/9495624.html"">Use of Swagger 3.1</a></li>
<li><a id=""post_title_link_9507387"" href=""https://www.cnblogs.com/laozhang-is-phi/p/9507387.html"">Use of Swagger 3.2</a></li>
<li><a id=""post_title_link_9511869"" href=""https://www.cnblogs.com/laozhang-is-phi/p/9511869.html"">Swagger use 3.3 JWT permission to verify "modify"</a></li>
<li><a id=""post_title_link_9896431"" href=""https://www.cnblogs.com/laozhang-is-phi/p/9896431.html"">[. Netcore occasional] 36║ resolve JWT permission validation expiration issues</a></li>
<li><a id=""post_title_link_9516890"" href=""https://www.cnblogs.com/laozhang-is-phi/p/9516890.html"">API Project Overall build 6.1 warehousing mode</a></li>
<li><a id=""post_title_link_9523148"" href=""https://www.cnblogs.com/laozhang-is-phi/p/9523148.html"">6.2 Lightweight ORM built as a whole for API projects</a></li>
<li><a id=""post_title_link_9529480"" href=""https://www.cnblogs.com/laozhang-is-phi/p/9529480.html"">API Project Overall build 6.3 asynchronous generic warehousing + Dependency Injection</a></li>
<li><a id=""post_title_link_9541414"" href=""https://www.cnblogs.com/laozhang-is-phi/p/9541414.html"">Discussion on Dependency Injection IOC learning + AOP Tangent programming</a></li>
<li><a id=""post_title_link_9547574"" href=""https://www.cnblogs.com/laozhang-is-phi/p/9547574.html"">AOP oriented tangent programming shallow parsing: Simple logging + service facet caching</a></li>
<li><a id=""post_title_link_9554210"" href=""https://www.cnblogs.com/laozhang-is-phi/p/9554210.html"">AOP custom filtering, Redis Getting Started 11.1</a></li>
<li><a id=""post_title_link_9560949"" href=""https://www.cnblogs.com/laozhang-is-phi/p/9560949.html"">Comparison of three cross-domain modes, DTOs (data transmission object)</a></li>
<li><a id=""post_title_link_9565227"" href=""https://www.cnblogs.com/laozhang-is-phi/p/9565227.html"">DTOs Object mapping usage, project deployment Windows+linux full version</a></li>
<li><a id=""post_title_link_9757999"" href=""https://www.cnblogs.com/laozhang-is-phi/p/9757999.html"">32 ║ Four methods to quickly realize the semi-automatic construction of the project</a></li>
<li><a id=""post_title_link_9767400"" href=""https://www.cnblogs.com/laozhang-is-phi/p/9767400.html"">33 ║⅖ ways to achieve perfect cross-domain</a></li>
<li><a id=""post_title_link_9795689"" href=""https://www.cnblogs.com/laozhang-is-phi/p/9795689.html"">34 ║swagger Processing Multi-version control, the thoughts brought about</a></li>
<li><a id=""post_title_link_9855836"" href=""https://www.cnblogs.com/laozhang-is-phi/p/9855836.html"">35 ║ Perfect Global exception logging</a></li>
<li><a id=""post_title_link_10139204"" href=""https://www.cnblogs.com/laozhang-is-phi/p/10139204.html"">37║JWT Perfect implementation of the dynamic allocation of permissions and interfaces</a></li>
 <li><a id=""link_post_title"" class=""link-post-title"" href=""https://www.cnblogs.com/laozhang-is-phi/p/10173536.html"">38║ automatically initializes the database</a></li>
<li><a id=""post_title_link_10205495"" href=""https://www.cnblogs.com/laozhang-is-phi/p/10205495.html"">39 | | Want to create your own dotnet template? Look here</a></li>

</ul></li>

<li>
<h3 id=""autoid-2-2-0"">Front End Vue Overview</h3>
<ul>
<li><a id=""post_title_link_9577805"" href=""https://www.cnblogs.com/laozhang-is-phi/p/9577805.html"">14 ║vue Plan, a brief history of my front and back development</a></li>
<li><a id=""post_title_link_9580807"" href=""https://www.cnblogs.com/laozhang-is-phi/p/9580807.html"">Xv. ║vue Basics: JS Object-oriented, the literal amount of the</a></li>
<li><a id=""post_title_link_9585766"" href=""https://www.cnblogs.com/laozhang-is-phi/p/9585766.html"">16 ║vue Basics: ES6 Initial experience of the</a></li>
<li><a id=""post_title_link_9593740"" href=""https://www.cnblogs.com/laozhang-is-phi/p/9593740.html"">17 ║vue Basics: Using Vue.js to draw blog Home + instructions (i)</a></li>
<li><a id=""post_title_link_9602077"" href=""https://www.cnblogs.com/laozhang-is-phi/p/9602077.html"">18 ║vue Basics: directive (bottom) + Calculated properties +watch</a></li>
<li><a id=""post_title_link_9611632"" href=""https://www.cnblogs.com/laozhang-is-phi/p/9611632.html"">19 ║vue Basics: Style dynamic binding + lifecycle</a></li>
<li><a id=""post_title_link_9622031"" href=""https://www.cnblogs.com/laozhang-is-phi/p/9622031.html"">20 ║vue Base Audrey: Component Detail + Project description<br><br></a></li>
<li>👆 above these basics can not look at if you just want to quickly get started with Vue<br><br></li>
<li><a id=""post_title_link_9629026"" href=""https://www.cnblogs.com/laozhang-is-phi/p/9629026.html"">21 ║vue Actual Combat: development environment to build a "detailed version"</a></li>
<li><a id=""post_title_link_9640974"" href=""https://www.cnblogs.com/laozhang-is-phi/p/9640974.html"">22 ║vue Actual Combat: The first edition of the personal blog (axios+router)</a></li>
<li><a id=""post_title_link_9647008"" href=""https://www.cnblogs.com/laozhang-is-phi/p/9647008.html"">23 ║vue Actual Combat: Vuex is actually very simple</a></li>
<li><a id=""post_title_link_9658019"" href=""https://www.cnblogs.com/laozhang-is-phi/p/9658019.html"">24 ║vuex + JWT Implementation Authorization Authentication Login</a></li>
<li><a id=""post_title_link_9670342"" href=""https://www.cnblogs.com/laozhang-is-phi/p/9670342.html"">25 ║ Preliminary study on SSR server rendering (personal blog II)</a></li>
<li><a id=""post_title_link_9675822"" href=""https://www.cnblogs.com/laozhang-is-phi/p/9675822.html"">26 ║client rendering, server rendering know how much {supplemental}</a></li>
<li><a id=""post_title_link_9682289"" href=""https://www.cnblogs.com/laozhang-is-phi/p/9682289.html"">27 ║nuxt Foundation: A preliminary study of the framework</a></li>
<li><a id=""post_title_link_9687504"" href=""https://www.cnblogs.com/laozhang-is-phi/p/9687504.html"">28 ║nuxt Basics: Source-oriented research nuxt.js</a></li>
<li><a id=""post_title_link_9697450"" href=""https://www.cnblogs.com/laozhang-is-phi/p/9697450.html"">29 ║nuxt Actual Combat: Asynchronous implementation of data dual-end rendering</a></li>
<li><a id=""post_title_link_9702677"" href=""https://www.cnblogs.com/laozhang-is-phi/p/9702677.html"">30 ║NUXT actual Combat: Dynamic routing + isomorphism</a></li>
<li><a id=""post_title_link_9713219"" href=""https://www.cnblogs.com/laozhang-is-phi/p/9713219.html"">31 ║NUXT Audrey: A probe into permission validation based on Vuex</a></li>
<li></li>

</ul>

</li>


</ul>


</div>


**************************************************************

System environment

Windows 10, SQL Server 2012, Visual Studio 2017, Windows Server R2

Back-end Technology:
      
      *. NET core 2.0 API (because you want to simply build front and rear separation, so choose the API, 
      if you want to understand. Net Core MVC, you can also communicate)

* Swagger front and rear end document description, writing interface based on restful style

* Repository + Service Warehousing mode programming

* Async and await asynchronous programming

* Cors Simple cross-domain solution

* AOP based on tangent programming technology

* AUTOFAC lightweight IOC and di dependency injection

* Vue Local Agent cross-domain scenario, Nginx cross-domain proxy

* JWT Permission Authentication

Database technology

* Sqlsugar Lightweight ORM Framework, Codefirst

* T4 Template generation

* AutoMapper Automatic Object mapping

Distributed caching technology

* Redis Lightweight Distributed cache

Front End Technology

* Vue 2.0 Framework Family Barrel Vue2 + VueRouter2 + Webpack + Axios + vue-cli + Vuex

* Elementui component library based on Vue 2.0
* Nuxt.js Server Render SSR
