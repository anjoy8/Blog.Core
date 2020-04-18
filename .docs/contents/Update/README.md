
## 更新日志
  
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
