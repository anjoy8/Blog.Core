using System.IO;

namespace Blog.Core.Common.DB
{
    public class BaseDBConfig
    {
        private static string sqlServerConnection = Appsettings.app(new string[] { "AppSettings", "SqlServer", "SqlServerConnection" });
        private static bool isSqlServerEnabled = (Appsettings.app(new string[] { "AppSettings", "SqlServer", "Enabled" })).ObjToBool();
        private static string mySqlConnection = Appsettings.app(new string[] { "AppSettings", "MySql", "MySqlConnection" });
        private static bool isMySqlEnabled = (Appsettings.app(new string[] { "AppSettings", "MySql", "Enabled" })).ObjToBool();


        public static string ConnectionString => InitConn();
        public static DataBaseType DbType = DataBaseType.SqlServer;


        private static string InitConn()
        {
            if (isSqlServerEnabled)
            {
                DbType = DataBaseType.SqlServer;
                return File.Exists(@"D:\my-file\dbCountPsw1.txt") ? File.ReadAllText(@"D:\my-file\dbCountPsw1.txt").Trim() : sqlServerConnection;
            }
            else if (isMySqlEnabled)
            {
                DbType = DataBaseType.MySql;
                return File.Exists(@"D:\my-file\dbCountPsw1_MySqlConn.txt") ? File.ReadAllText(@"D:\my-file\dbCountPsw1_MySqlConn.txt").Trim() : mySqlConnection;
            }
            else
            {
                return "server=.;uid=sa;pwd=sa;database=WMBlogDB";
            }

        }

    }

    public enum DataBaseType
    {
        MySql = 0,
        SqlServer = 1,
        Sqlite = 2,
        Oracle = 3,
        PostgreSQL = 4
    }
}
