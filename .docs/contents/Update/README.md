
## 更新日志

### 2020-05-07
> 重大内容更新：更新项目模板 `Update Blog.Core.Webapi.Template.2.1.0.nupkg`  [7f64fde](https://github.com/anjoy8/Blog.Core/commit/7f64fde5507f7a8572372dcadb6af5110bd37d68) 


###  2020-05-06  
> 重大内容更新：优化Log4Net使用方案,完美配合 `NetCore` 官方的 `ILogger<T>`， [ecaffb6](https://github.com/anjoy8/Blog.Core/commit/ecaffb66bdf10a90c087d01e6e817e54f23a97d4)  


### 2020-05-01

> 重要内容更新：配合Admin全部完成按钮级别权限，更新初始化种子数据

### 2020-04-27

增加功能：配合前端Admin，增加页面 `KeepAlive` 功能；  
增加功能：增加 `Sql` 语句查询Demo，支持返回 `DataTable`；


### 2020-04-25

增加功能：`Http api` 接口调用，满足微服务需求
> 重要内容更新：优化 `Appsettings.app()` 方法，通过官方 `IConfiguration` 接口来获取DBS连接字符串；  
> 优化 `BlogLogAOP.cs`


### 2020-04-15

> 重大内容更新：更新项目模板 `Update Blog.Core.Webapi.Template.1.11.30.nupkg`

  
###  2020-04-14  
> 重大内容更新：主分支，可以通过配置，一键切换JWT和Ids4认证授权模式    


###  2020-03-30  
> 重大内容更新：统一所有接口返回格式  
  

###  2020-03-25  
增加功能：支持读写分离（目前是三种模式：单库、多库、读写分离）   
> 重大BUG更新：系统登录接口，未对用户软删除进行判断，现已修复  
> API:  /api/login/GetJwtToken3  
> Code: await _sysUserInfoServices.Query(d => d.uLoginName == name && d.uLoginPWD == pass && d.tdIsDelete == false);  

  

###  2020-03-18  
增加功能：创建 Quartz.net 任务调度服务  
  

###  2020-01-09  
增加功能：项目迁移到IdentityServer4，统一授权认证中心   


###  2020-01-05  
增加功能：设计一个简单的中间件，可以查看所有已经注入的服务  
  

###  2020-01-04  
增加功能：Ip限流，防止过多刷数据  
