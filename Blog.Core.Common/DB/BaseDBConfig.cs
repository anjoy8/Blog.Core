using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SqlSugar;

namespace Blog.Core.Common.DB
{
    public class BaseDBConfig
    {
        /// <summary>
        /// 所有库配置
        /// </summary>
        public static readonly List<ConnectionConfig> AllConfigs = new();

        /// <summary>
        /// 主库的备用连接配置
        /// </summary>
        public static readonly List<ConnectionConfig> ReuseConfigs = new();

        /// <summary>
        /// 有效的库连接(除去Log库)
        /// </summary>
        public static List<ConnectionConfig> ValidConfig = new();

        public static ConnectionConfig MainConfig;
        public static ConnectionConfig LogConfig; //日志库

        public static bool IsMulti => ValidConfig.Count > 1;

        /* 之前的单库操作已经删除，如果想要之前的代码，可以查看我的GitHub的历史记录
         * 目前是多库操作，默认加载的是appsettings.json设置为true的第一个db连接。
         *
         * 优化配置连接
         * 老的配置方式,再多库和从库中有些冲突
         * 直接在单个配置中可以配置从库
         *
         * 新增故障转移方案
         * 增加主库备用连接,配置方式为ConfigId为主库的ConfigId+随便数字 只要不重复就好
         *
         * 主库在无法连接后会自动切换到备用链接
         */
        public static (List<MutiDBOperate> allDbs, List<MutiDBOperate> slaveDbs) MutiConnectionString => MutiInitConn();

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
                catch (System.Exception)
                {
                }
            }

            return conn[conn.Length - 1];
        }


        public static (List<MutiDBOperate>, List<MutiDBOperate>) MutiInitConn()
        {
            List<MutiDBOperate> listdatabase = AppSettings.app<MutiDBOperate>("DBS")
                .Where(i => i.Enabled).ToList();
            var mainDbId = AppSettings.app(new string[] {"MainDB"}).ObjToString();
            var mainDbModel = listdatabase.Single(d => d.ConnId == mainDbId);
            listdatabase.Remove(mainDbModel);
            listdatabase.Insert(0, mainDbModel);

            foreach (var i in listdatabase) SpecialDbString(i);
            return (listdatabase, mainDbModel.Slaves);
        }

        /// <summary>
        /// 定制Db字符串
        /// 目的是保证安全：优先从本地txt文件获取，若没有文件则从appsettings.json中获取
        /// </summary>
        /// <param name="mutiDBOperate"></param>
        /// <returns></returns>
        private static MutiDBOperate SpecialDbString(MutiDBOperate mutiDBOperate)
        {
            if (mutiDBOperate.DbType == DataBaseType.Sqlite)
            {
                mutiDBOperate.Connection =
                    $"DataSource=" + Path.Combine(Environment.CurrentDirectory, mutiDBOperate.Connection);
            }
            else if (mutiDBOperate.DbType == DataBaseType.SqlServer)
            {
                mutiDBOperate.Connection = DifDBConnOfSecurity(@"D:\my-file\dbCountPsw1_SqlserverConn.txt",
                    mutiDBOperate.Connection);
            }
            else if (mutiDBOperate.DbType == DataBaseType.MySql)
            {
                mutiDBOperate.Connection =
                    DifDBConnOfSecurity(@"D:\my-file\dbCountPsw1_MySqlConn.txt", mutiDBOperate.Connection);
            }
            else if (mutiDBOperate.DbType == DataBaseType.Oracle)
            {
                mutiDBOperate.Connection =
                    DifDBConnOfSecurity(@"D:\my-file\dbCountPsw1_OracleConn.txt", mutiDBOperate.Connection);
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
        PostgreSQL = 4,
        Dm = 5,
        Kdbndp = 6,
    }

    public class MutiDBOperate
    {
        /// <summary>
        /// 连接启用开关
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// 连接ID
        /// </summary>
        public string ConnId { get; set; }

        /// <summary>
        /// 从库执行级别，越大越先执行
        /// </summary>
        public int HitRate { get; set; }

        /// <summary>
        /// 连接字符串
        /// </summary>
        public string Connection { get; set; }

        /// <summary>
        /// 数据库类型
        /// </summary>
        public DataBaseType DbType { get; set; }

        /// <summary>
        /// 从库
        /// </summary>
        public List<MutiDBOperate> Slaves { get; set; }
    }
}