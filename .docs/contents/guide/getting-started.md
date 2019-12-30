# 快速上手
注意

请确保你的 `Visual Studio 2019` 版本 >= `16.4`。


## 下载
Github（国际） 下载 [https://github.com/anjoy8/Blog.Core](https://github.com/anjoy8/Blog.Core)  
    
Gitee（国内） 下载 [https://gitee.com/laozhangIsPhi/Blog.Core](https://gitee.com/laozhangIsPhi/Blog.Core)  


## 编译与运行
1、拿到项目后，双击 `Blog.Core.sln` 解决方案；  
2、首先 `F6` 编译，看是否有错误；  
3、然后 `F5` 运行，调起 `8081` 端口，浏览器查看效果；


## 如何配置数据库连接字符串
1、打开 `Blog.Core` 项目下的 `appsettings.json` 文件；  
2、修改 `DBS` 字节内容，配置对应的连接字符串，注意`DBType`对应不同的数据库类型；  
3、单库操作只需要把你想要运行的数据库 `Enabled` 为 `true` 即可，其他都要设置 `false`；  
4、举例来说，比如你想使用`Sqlserver`数据库，连接字符串为 `Server=.;Database=WMBlogDB;User ID=sa;Password=123; `你可以这么配置:  

```
  "DBS": [
    {
      "ConnId": "WMBLOG_SQLITE",
      "DBType": 2,// sqlite数据库
      "Enabled": false,// 设置为false，不启用
      "Connection": "WMBlog.db" //只写数据库名就行
    },
    {
      "ConnId": "WMBLOG_MSSQL",
      "DBType": 1,// sqlserver数据库
      "Enabled": true,// 设置为true，启用
      "Connection": "Server=.;Database=WMBlogDB;User ID=sa;Password=123;",
      "ProviderName": "System.Data.SqlClient"
    },
    {
      "ConnId": "WMBLOG_MYSQL",
      "DBType": 0,// mysql
      "Enabled": false,// false 不启用
      "Connection": "Server=localhost; Port=3306;Stmt=; Database=wmblogdb; Uid=root; Pwd=456;"
    },
    {
      "ConnId": "WMBLOG_ORACLE",
      "DBType": 3,// Oracle 
      "Enabled": false,// 不启用
      "Connection": "Provider=OraOLEDB.Oracle; Data Source=WMBlogDB; User Id=sss; Password=789;"
    }
  ],
```

5、如果你想多库操作，需要配置
```
1：MainDB 设置为主库的 ConnId；
2：MutiDBEnabled设置为true，
3：把下边想要连接的多个连接字符串都设置为true
```

## 如何配置项目端口号
1、在 `Blog.Core` 层下的 `Program.cs` 文件中，将
` .UseUrls("http://localhost:8081") `  中的`8081`端口，修改为自己想要的端口号；  
2、或者直接删掉上边的配置，在 `launchSettings.json` 中设置；

## 如何项目重命名
1、双击项目根目录下的 `CreateYourProject.bat` 批处理文件；  
2、根据提示，输入自己想要的项目名称即可；  
3、在根目录会有一个 `.1YourProject` 文件夹，里边即你的项目；


## 发布与部署
1、双击项目根目录下的 `Blog.Core.Publish.bat`批处理文件；  
2、执行完成后，根目录会有一个`.PublishFiles` 文件夹，就是发布后的项目；

