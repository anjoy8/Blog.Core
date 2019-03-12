
using Blog.Core.Common;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Linq;

namespace Blog.Core.Repository
{
    public class BaseDBConfig
    {
        static string sqlServerConnection = Appsettings.app(new string[] { "AppSettings", "SqlServer", "SqlServerConnection" });//获取连接字符串

        public static string ConnectionString = File.Exists(@"D:\my-file\dbCountPsw1.txt") ? File.ReadAllText(@"D:\my-file\dbCountPsw1.txt").Trim() : (!string.IsNullOrEmpty(sqlServerConnection) ? sqlServerConnection : "server=.;uid=sa;pwd=sa;database=WMBlogDB");

        //正常格式是

        //public static string ConnectionString = "server=.;uid=sa;pwd=sa;database=WMBlogDB"; 

        //原谅我用配置文件的形式，因为我直接调用的是我的服务器账号和密码，安全起见

    }
}
