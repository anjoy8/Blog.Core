# K  快速上手
注意

请确保你的 `Visual Studio 2019` 版本 >= `16.4`。


## 下载
Github（国际） 下载 [https://github.com/anjoy8/Blog.Core](https://github.com/anjoy8/Blog.Core)  
    
Gitee（国内） 下载 [https://gitee.com/laozhangIsPhi/Blog.Core](https://gitee.com/laozhangIsPhi/Blog.Core)  


## 编译与运行
1、拿到项目后，双击 `Blog.Core.sln` 解决方案；  
2、首先 `F6` 编译，看是否有错误；  
3、然后 `F5` 运行，调起 `8081` 端口，浏览器查看效果；  
4、因为系统默认的是 `sqlite` 数据库，如果你想换其他数据库，请看下边；    
5、注意：本系统是直接自动生成数据库和数据的，不用手动创建数据库；  



## 如何配置数据库连接字符串

1、打开 `Blog.Core` 项目下的 `appsettings.json` 文件；  
2、修改 `DBS` 字节内容，配置对应的连接字符串，注意`DBType`对应不同的数据库类型；  
3、把你想要运行的数据库 `Enabled` 为 `true` 即可，其他都要设置 `false`；  
4、然后 `MainDB` 设置为下边你使用的指定 `ConnId`:  

```
  "MainDB": "WMBLOG_MSSQL", //当前项目的主库，所对应的连接字符串的Enabled必须为true
  "MutiDBEnabled": false, //是否开启多库
  "DBS": [
    {
      "ConnId": "WMBLOG_SQLITE",
      "DBType": 2,// sqlite数据库
      "Enabled": true,// 设置为true，启用1
      "Connection": "WMBlog.db" //只写数据库名就行
    },
    {
      "ConnId": "WMBLOG_MSSQL",
      "DBType": 1,// sqlserver数据库
      "Enabled": true,// 设置为true，启用2
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
  a：MainDB 设置为主库的 ConnId；
  b：MutiDBEnabled设置为true，
  c：把下边想要连接的多个连接字符串都设置为true
```

## 如何配置项目端口号
1、在 `Blog.Core` 层下的 `Program.cs` 文件中，将
` .UseUrls("http://localhost:8081") `  中的`8081`端口，修改为自己想要的端口号；  
2、或者直接删掉上边的配置，在 `launchSettings.json` 中设置；

## 如何项目重命名
1、双击项目根目录下的 `CreateYourProject.bat` 批处理文件；  
2、根据提示，输入自己想要的项目名称即可；  
3、在根目录会有一个 `.1YourProject` 文件夹，里边即你的项目；  


## 新增实体模块后如何迁移到数据库
1、在 `Blog.Core.Model` 项目目录下的 `Seed` 文件夹下，找到 `DBSeed` 类；  
2、根据提示，找到生成table的地方 `myContext.CreateTableByEntity`；  
3、添加进去你新增的实体类，当然也可以用下边的单独写法；  
4、编译项目，没错后，运行,则数据库更新完毕；  


## 新增实体，如何进行增删改查CURD操作
1、随便找一个含有业务逻辑的 `controller` 参考一下即可；  
2、主要 `api` 是通过 `Service` 服务层提供业务逻辑；  
3、然后服务层通过 `Repository` 仓储层封装持久化操作；  
4、每一个表基本上对应一个仓储类，基本的操作都封装到了 `BaseRepository.cs` 基类仓储中；  
5、添加完业务逻辑，记得要 `F6` 重新编译一下，因为项目间引用解耦了；  
6、项目已经自动注入了，直接在控制器使用对应的服务层接口就行： `IxxxxService` ;  


## 新增数据库表，如何反向生成四层文件
1、可以通过 `T4` 模板来生成，是 `Blog.Core.FrameWork` 层的使用；  
2、也可以通过 `Sqlsugar` 所带的方法来实现，具体查看 `Controller` 层下的 `DbFirstController.cs`；  
3、总体操作过程，可以参考我的视频：https://www.bilibili.com/video/av77612407?p=2；   


## 发布与部署
1、双击项目根目录下的 `Blog.Core.Publish.bat`批处理文件；  
2、执行完成后，根目录会有一个`.PublishFiles` 文件夹，就是发布后的项目；

