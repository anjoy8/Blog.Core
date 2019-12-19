using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Blog.Core.Common.DB
{
    public class BaseDBConfig
    {
        private static string sqliteConnection = Appsettings.app(new string[] { "AppSettings", "Sqlite", "Connection" });
        private static bool isSqliteEnabled = (Appsettings.app(new string[] { "AppSettings", "Sqlite", "Enabled" })).ObjToBool();

        private static string sqlServerConnection = Appsettings.app(new string[] { "AppSettings", "SqlServer", "Connection" });
        private static bool isSqlServerEnabled = (Appsettings.app(new string[] { "AppSettings", "SqlServer", "Enabled" })).ObjToBool();

        private static string mySqlConnection = Appsettings.app(new string[] { "AppSettings", "MySql", "Connection" });
        private static bool isMySqlEnabled = (Appsettings.app(new string[] { "AppSettings", "MySql", "Enabled" })).ObjToBool();

        private static string oracleConnection = Appsettings.app(new string[] { "AppSettings", "Oracle", "Connection" });
        private static bool IsOracleEnabled = (Appsettings.app(new string[] { "AppSettings", "Oracle", "Enabled" })).ObjToBool();


        public static string ConnectionString => InitConn();
        public static DataBaseType DbType = DataBaseType.SqlServer;
        public static List<MutiDBOperate> MutiConnectionString => MutiInitConn();


        private static string InitConn()
        {
            if (isSqliteEnabled)
            {
                DbType = DataBaseType.Sqlite;
                return $"DataSource=" + Path.Combine(Environment.CurrentDirectory, sqliteConnection);
            }
            else if (isSqlServerEnabled)
            {
                DbType = DataBaseType.SqlServer;
                return DifDBConnOfSecurity(@"D:\my-file\dbCountPsw1.txt", @"c:\my-file\dbCountPsw1.txt", sqlServerConnection);
            }
            else if (isMySqlEnabled)
            {
                DbType = DataBaseType.MySql;
                return DifDBConnOfSecurity(@"D:\my-file\dbCountPsw1_MySqlConn.txt", @"c:\my-file\dbCountPsw1_MySqlConn.txt", mySqlConnection);
            }
            else if (IsOracleEnabled)
            {
                DbType = DataBaseType.Oracle;
                return DifDBConnOfSecurity(@"D:\my-file\dbCountPsw1_OracleConn.txt", @"c:\my-file\dbCountPsw1_OracleConn.txt", oracleConnection);
            }
            else
            {
                return "server=.;uid=sa;pwd=sa;database=WMBlogDB";
            }

        }
        private static string DifDBConnOfSecurity(params string[] conn)
        {
            foreach (var item in conn)
            {
                try
                {
                    if (File.Exists(item))
                    {
                        return File.ReadAllText(item).Trim();
                    }
                }
                catch (System.Exception) { }
            }

            return conn[conn.Length - 1];
        }


        public static List<MutiDBOperate> MutiInitConn()
        {
            List<MutiDBOperate> listdatabase = new List<MutiDBOperate>();
            string Path = "appsettings.json";
            using (var file = new StreamReader(Path))
            using (var reader = new JsonTextReader(file))
            {
                var jObj = (JObject)JToken.ReadFrom(reader);
                if (!string.IsNullOrWhiteSpace("DBS"))
                {
                    var secJt = jObj["DBS"];
                    if (secJt != null)
                    {
                        for (int i = 0; i < secJt.Count(); i++)
                        {
                            if (secJt[i]["Enabled"].ObjToBool())
                            {
                                listdatabase.Add(SpecialDbString(new MutiDBOperate()
                                {
                                    ConnId = secJt[i]["ConnId"].ObjToString(),
                                    Conn = secJt[i]["Connection"].ObjToString(),
                                    DbType = (DataBaseType)(secJt[i]["DBType"].ObjToInt()),
                                }));
                            }
                        }
                    }
                }
                return listdatabase;
            }
        }

        private static MutiDBOperate SpecialDbString(MutiDBOperate mutiDBOperate)
        {
            if (mutiDBOperate.DbType == DataBaseType.Sqlite)
            {
                mutiDBOperate.Conn = $"DataSource=" + Path.Combine(Environment.CurrentDirectory, mutiDBOperate.Conn);
            }
            else if (mutiDBOperate.DbType == DataBaseType.SqlServer)
            {
                mutiDBOperate.Conn = DifDBConnOfSecurity(@"D:\my-file\dbCountPsw1.txt", @"c:\my-file\dbCountPsw1.txt", mutiDBOperate.Conn);
            }
            else if (mutiDBOperate.DbType == DataBaseType.MySql)
            {
                mutiDBOperate.Conn = DifDBConnOfSecurity(@"D:\my-file\dbCountPsw1_MySqlConn.txt", @"c:\my-file\dbCountPsw1_MySqlConn.txt", mutiDBOperate.Conn);
            }
            else if (mutiDBOperate.DbType == DataBaseType.Oracle)
            {
                mutiDBOperate.Conn = DifDBConnOfSecurity(@"D:\my-file\dbCountPsw1_OracleConn.txt", @"c:\my-file\dbCountPsw1_OracleConn.txt", mutiDBOperate.Conn);
            }

            return mutiDBOperate;
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
    public class MutiDBOperate
    {
        public string ConnId { get; set; }
        public string Conn { get; set; }
        public DataBaseType DbType { get; set; }
    }
}
