# G  功能一览表



## 一、日志记录

本框架涵盖了不同领域的日志记录，共五个，分别是：  
  
1、全局异常日志  
    
    开启方式：无需操作。  
    文件路径：web目录下，Log/GlobalExcepLogs_{日期}.log。  
    功能描述：记录项目启动后出现的所有异常日志，不包括中间件中异常。  

  
2、IP 请求日志    
    
    开启方式：无需操作。  
    文件路径：web目录下，Log/RequestIpInfoLog.log。  
    功能描述：记录项目启动后客户端请求的ip和接口信息。   
    举例来说：  
    {"Ip":"xxx.xx.xx.x","Url":"/api/values","Datetime":"2020-01-06 18:02:19","Date":"2020-01-06","Week":"周一"}

  
3、全部请求与响应日志    
    
    开启方式：appsettings.json -> Middlewar -> RequestResponseLog 节点为true。  
    文件路径：web目录下，Log/RequestIpInfoLog.log。  
    功能描述：记录项目启动后客户端所有的请求和响应日志，包括url参数、body以及相应json。  

     
4、服务层请求响应AOP日志    
    
    开启方式：appsettings.json -> AppSettings -> LogAOP 节点为true。  
    文件路径：web目录下，Log/AOPLog.log。  
    功能描述：记录项目启动请求api后，所有的service层日志，包括方法名、参数、响应结果或用户（非必须）。 

     
5、数据库操作日志    
    
    开启方式：appsettings.json -> AppSettings -> SqlAOP 节点为true。  
    文件路径：web目录下，Log/SqlLog.log。  
    功能描述：记录项目启动请求api并访问service后，所有的db操作日志，包括Sql参数与Sql语句。
    举例来说：  
    --------------------------------
    1/6/2020 6:13:04 PM|
    【SQL参数】：@bID0:1
    【SQL语句】：SELECT `bID`,`bsubmitter`,`btitle`,`bcategory`,`bcontent`,`btraffic`,`bcommentNum`,`bUpdateTime`,`bCreateTime`,`bRemark`,`IsDeleted` FROM `BlogArticle`  WHERE ( `bID` = @bID0 ) 


     
6、控制器使用 ILogger< blogcontroller >    
    
    注意：这种方式并不太友好，除了会加载很多不必要的启动信息以为，目前只能生成到异常日志里，这里我建议使用Log4Net自己的方案：  
    private static readonly log4net.ILog log =
        log4net.LogManager.GetLogger(typeof(GlobalExceptionsFilter));    
    或者我自己写的日志方法:  
     LogLock.OutSql2Log("RequestResponseLog", new string[] { "Request Data:", content });  

      
    开启方式：appsettings.json -> Middleware -> RecordAllLogs 节点为true。  
    文件路径：web目录下，Log/GlobalExcepLogs_{日期}.log。  
    功能描述：记录项目中的所有日志打印，除非在手动控制日志的输出，否则会打印很多日志。
    举例来说：  
    --------------------------------
   
    2020-02-17 11:15:07,049| 
    Request finished in 564.826ms 200 application/json;charset=utf-8
    --------------------------------
    2020-02-17 11:16:19,512| 
    Request starting HTTP/1.1 GET http://localhost:8081/api/Blog/DetailNuxtNoPer?id=1  
    --------------------------------
    2020-02-17 11:16:19,882| 
    AuthenticationScheme: Bearer was not authenticated.
    --------------------------------
    2020-02-17 11:16:28,835| 
    xxxxxxxxxxxxxxxxxxx
    --------------------------------

