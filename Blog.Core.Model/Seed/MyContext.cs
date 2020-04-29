using Blog.Core.Common;
using Blog.Core.Common.DB;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.IO;

namespace Blog.Core.Model.Models
{
    public class MyContext
    {

        private static MutiDBOperate connectObject => GetMainConnectionDb();
        private static string _connectionString = connectObject.Connection;
        private static DbType _dbType = (DbType)connectObject.DbType;
        private SqlSugarClient _db;

        /// <summary>
        /// 连接字符串 
        /// Blog.Core
        /// </summary>
        public static MutiDBOperate GetMainConnectionDb()
        {
            var mainConnetctDb = BaseDBConfig.MutiConnectionString.Item1.Find(x => x.ConnId == MainDb.CurrentDbConnId);
            if (BaseDBConfig.MutiConnectionString.Item1.Count > 0)
            {
                if (mainConnetctDb == null)
                {
                    mainConnetctDb = BaseDBConfig.MutiConnectionString.Item1[0];
                }
            }
            else
            {
                throw new Exception("请确保appsettigns.json中配置连接字符串,并设置Enabled为true;");
            }

            return mainConnetctDb;
        }
        /// <summary>
        /// 连接字符串 
        /// Blog.Core
        /// </summary>
        public static string ConnectionString
        {
            get { return _connectionString; }
            set { _connectionString = value; }
        }
        /// <summary>
        /// 数据库类型 
        /// Blog.Core 
        /// </summary>
        public static DbType DbType
        {
            get { return _dbType; }
            set { _dbType = value; }
        }
        /// <summary>
        /// 数据连接对象 
        /// Blog.Core 
        /// </summary>
        public SqlSugarClient Db
        {
            get { return _db; }
            private set { _db = value; }
        }

        /// <summary>
        /// 数据库上下文实例（自动关闭连接）
        /// Blog.Core 
        /// </summary>
        public static MyContext Context
        {
            get
            {
                return new MyContext();
            }

        }


        /// <summary>
        /// 功能描述:构造函数
        /// 作　　者:Blog.Core
        /// </summary>
        public MyContext()
        {
            if (string.IsNullOrEmpty(_connectionString))
                throw new ArgumentNullException("数据库连接字符串为空");
            _db = new SqlSugarClient(new ConnectionConfig()
            {
                ConnectionString = _connectionString,
                DbType = _dbType,
                IsAutoCloseConnection = true,
                InitKeyType = InitKeyType.Attribute,//mark
                ConfigureExternalServices = new ConfigureExternalServices()
                {
                    //DataInfoCacheService = new HttpRuntimeCache()
                },
                MoreSettings = new ConnMoreSettings()
                {
                    //IsWithNoLockQuery = true,
                    IsAutoRemoveDataCache = true
                }
            });
        }


        #region 根据数据库表生产Model层

        /// <summary>
        /// 功能描述:根据数据库表生产Model层
        /// 作　　者:Blog.Core
        /// </summary>
        /// <param name="strPath">实体类存放路径</param>
        /// <param name="strNameSpace">命名空间</param>
        /// <param name="lstTableNames">生产指定的表</param>
        /// <param name="strInterface">实现接口</param>
        /// <param name="blnSerializable">是否序列化</param>
        public void Create_Model_ClassFileByDBTalbe(
          string strPath,
          string strNameSpace,
          string[] lstTableNames,
          string strInterface,
          bool blnSerializable = false)
        {
            var IDbFirst = _db.DbFirst;
            if (lstTableNames != null && lstTableNames.Length > 0)
            {
                IDbFirst = IDbFirst.Where(lstTableNames);
            }
            var ls = IDbFirst.IsCreateDefaultValue().IsCreateAttribute()

                  .SettingClassTemplate(p => p = @"
{using}

namespace " + strNameSpace + @"
{
    {ClassDescription}{SugarTable}" + (blnSerializable ? "[Serializable]" : "") + @"
    public class {ClassName}" + (string.IsNullOrEmpty(strInterface) ? "" : (" : " + strInterface)) + @"
    {
        public {ClassName}()
        {
        }
        {PropertyName}
    }
}
                    ")

                  .SettingPropertyTemplate(p => p = @"
        {SugarColumn}
        public {PropertyType} {PropertyName} { get; set; }

            ")

                   //.SettingPropertyDescriptionTemplate(p => p = "          private {PropertyType} _{PropertyName};\r\n" + p)
                   //.SettingConstructorTemplate(p => p = "              this._{PropertyName} ={DefaultValue};")

                   .ToClassStringList(strNameSpace);
            CreateFilesByClassStringList(ls, strPath, "{0}");

        }
        #endregion


        #region 根据数据库表生产IRepository层

        /// <summary>
        /// 功能描述:根据数据库表生产IRepository层
        /// 作　　者:Blog.Core
        /// </summary>
        /// <param name="strPath">实体类存放路径</param>
        /// <param name="strNameSpace">命名空间</param>
        /// <param name="lstTableNames">生产指定的表</param>
        /// <param name="strInterface">实现接口</param>
        public void Create_IRepository_ClassFileByDBTalbe(
          string strPath,
          string strNameSpace,
          string[] lstTableNames,
          string strInterface)
        {
            var IDbFirst = _db.DbFirst;
            if (lstTableNames != null && lstTableNames.Length > 0)
            {
                IDbFirst = IDbFirst.Where(lstTableNames);
            }
            var ls = IDbFirst.IsCreateDefaultValue().IsCreateAttribute()

                 .SettingClassTemplate(p => p = @"
using Blog.Core.IRepository.Base;
using Blog.Core.Model.Models;

namespace " + strNameSpace + @"
{
	/// <summary>
	/// I{ClassName}Repository
	/// </summary>	
    public interface I{ClassName}Repository : IBaseRepository<{ClassName}>" + (string.IsNullOrEmpty(strInterface) ? "" : (" , " + strInterface)) + @"
    {
    }
}
                    ")

                  .ToClassStringList(strNameSpace);
            CreateFilesByClassStringList(ls, strPath, "I{0}Repository");
        }
        #endregion


        #region 根据数据库表生产IServices层

        /// <summary>
        /// 功能描述:根据数据库表生产IServices层
        /// 作　　者:Blog.Core
        /// </summary>
        /// <param name="strPath">实体类存放路径</param>
        /// <param name="strNameSpace">命名空间</param>
        /// <param name="lstTableNames">生产指定的表</param>
        /// <param name="strInterface">实现接口</param>
        public void Create_IServices_ClassFileByDBTalbe(
          string strPath,
          string strNameSpace,
          string[] lstTableNames,
          string strInterface)
        {
            var IDbFirst = _db.DbFirst;
            if (lstTableNames != null && lstTableNames.Length > 0)
            {
                IDbFirst = IDbFirst.Where(lstTableNames);
            }
            var ls = IDbFirst.IsCreateDefaultValue().IsCreateAttribute()

                  .SettingClassTemplate(p => p = @"
using Blog.Core.IServices.BASE;
using Blog.Core.Model.Models;

namespace " + strNameSpace + @"
{	
	/// <summary>
	/// I{ClassName}Services
	/// </summary>	
    public interface I{ClassName}Services :IBaseServices<{ClassName}>" + (string.IsNullOrEmpty(strInterface) ? "" : (" , " + strInterface)) + @"
	{

       
    }
}
                    ")

                   .ToClassStringList(strNameSpace);
            CreateFilesByClassStringList(ls, strPath, "I{0}Services");

        }
        #endregion



        #region 根据数据库表生产 Repository 层

        /// <summary>
        /// 功能描述:根据数据库表生产 Repository 层
        /// 作　　者:Blog.Core
        /// </summary>
        /// <param name="strPath">实体类存放路径</param>
        /// <param name="strNameSpace">命名空间</param>
        /// <param name="lstTableNames">生产指定的表</param>
        /// <param name="strInterface">实现接口</param>
        public void Create_Repository_ClassFileByDBTalbe(
          string strPath,
          string strNameSpace,
          string[] lstTableNames,
          string strInterface)
        {
            var IDbFirst = _db.DbFirst;
            if (lstTableNames != null && lstTableNames.Length > 0)
            {
                IDbFirst = IDbFirst.Where(lstTableNames);
            }
            var ls = IDbFirst.IsCreateDefaultValue().IsCreateAttribute()

                  .SettingClassTemplate(p => p = @"
using Blog.Core.IRepository;
using Blog.Core.IRepository.UnitOfWork;
using Blog.Core.Model.Models;
using Blog.Core.Repository.Base;

namespace " + strNameSpace + @"
{
	/// <summary>
	/// {ClassName}Repository
	/// </summary>
    public class {ClassName}Repository : BaseRepository<{ClassName}>, I{ClassName}Repository" + (string.IsNullOrEmpty(strInterface) ? "" : (" , " + strInterface)) + @"
    {
        public {ClassName}Repository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
                    ")

                   .ToClassStringList(strNameSpace);


            CreateFilesByClassStringList(ls, strPath, "{0}Repository");
        }
        #endregion


        #region 根据数据库表生产 Services 层

        /// <summary>
        /// 功能描述:根据数据库表生产 Services 层
        /// 作　　者:Blog.Core
        /// </summary>
        /// <param name="strPath">实体类存放路径</param>
        /// <param name="strNameSpace">命名空间</param>
        /// <param name="lstTableNames">生产指定的表</param>
        /// <param name="strInterface">实现接口</param>
        public void Create_Services_ClassFileByDBTalbe(
          string strPath,
          string strNameSpace,
          string[] lstTableNames,
          string strInterface)
        {
            var IDbFirst = _db.DbFirst;
            if (lstTableNames != null && lstTableNames.Length > 0)
            {
                IDbFirst = IDbFirst.Where(lstTableNames);
            }
            var ls = IDbFirst.IsCreateDefaultValue().IsCreateAttribute()

                  .SettingClassTemplate(p => p = @"
using Blog.Core.IRepository;
using Blog.Core.IServices;
using Blog.Core.Model.Models;
using Blog.Core.Services.BASE;

namespace " + strNameSpace + @"
{
    public partial class {ClassName}Services : BaseServices<{ClassName}>, I{ClassName}Services" + (string.IsNullOrEmpty(strInterface) ? "" : (" , " + strInterface)) + @"
    {
        I{ClassName}Repository _dal;
        public {ClassName}Services(I{ClassName}Repository dal)
        {
            this._dal = dal;
            base.BaseDal = dal;
        }

    }
}
                    ")

                   .ToClassStringList(strNameSpace);

            CreateFilesByClassStringList(ls, strPath, "{0}Services");
        }
        #endregion


        #region 根据模板内容批量生成文件
        /// <summary>
        /// 根据模板内容批量生成文件
        /// </summary>
        /// <param name="ls">类文件字符串list</param>
        /// <param name="strPath">生成路径</param>
        /// <param name="fileNameTp">文件名格式模板</param>
        public void CreateFilesByClassStringList(Dictionary<string, string> ls, string strPath, string fileNameTp)
        {

            foreach (var item in ls)
            {
                var fileName = $"{string.Format(fileNameTp, item.Key)}.cs";
                var fileFullPath = Path.Combine(strPath, fileName);
                if (!Directory.Exists(strPath))
                {
                    Directory.CreateDirectory(strPath);
                }
                File.WriteAllText(fileFullPath, item.Value);
            }
        }
        #endregion




        #region 实例方法
        /// <summary>
        /// 功能描述:获取数据库处理对象
        /// 作　　者:Blog.Core
        /// </summary>
        /// <returns>返回值</returns>
        public SimpleClient<T> GetEntityDB<T>() where T : class, new()
        {
            return new SimpleClient<T>(_db);
        }
        /// <summary>
        /// 功能描述:获取数据库处理对象
        /// 作　　者:Blog.Core
        /// </summary>
        /// <param name="db">db</param>
        /// <returns>返回值</returns>
        public SimpleClient<T> GetEntityDB<T>(SqlSugarClient db) where T : class, new()
        {
            return new SimpleClient<T>(db);
        }



        #endregion


        #region 根据实体类生成数据库表
        /// <summary>
        /// 功能描述:根据实体类生成数据库表
        /// 作　　者:Blog.Core
        /// </summary>
        /// <param name="blnBackupTable">是否备份表</param>
        /// <param name="lstEntitys">指定的实体</param>
        public void CreateTableByEntity<T>(bool blnBackupTable, params T[] lstEntitys) where T : class, new()
        {
            Type[] lstTypes = null;
            if (lstEntitys != null)
            {
                lstTypes = new Type[lstEntitys.Length];
                for (int i = 0; i < lstEntitys.Length; i++)
                {
                    T t = lstEntitys[i];
                    lstTypes[i] = typeof(T);
                }
            }
            CreateTableByEntity(blnBackupTable, lstTypes);
        }

        /// <summary>
        /// 功能描述:根据实体类生成数据库表
        /// 作　　者:Blog.Core
        /// </summary>
        /// <param name="blnBackupTable">是否备份表</param>
        /// <param name="lstEntitys">指定的实体</param>
        public void CreateTableByEntity(bool blnBackupTable, params Type[] lstEntitys)
        {
            if (blnBackupTable)
            {
                _db.CodeFirst.BackupTable().InitTables(lstEntitys); //change entity backupTable            
            }
            else
            {
                _db.CodeFirst.InitTables(lstEntitys);
            }
        }
        #endregion


        #region 静态方法

        /// <summary>
        /// 功能描述:获得一个DbContext
        /// 作　　者:Blog.Core
        /// </summary>
        /// <returns></returns>
        public static MyContext GetDbContext()
        {
            return new MyContext();
        }

        /// <summary>
        /// 功能描述:设置初始化参数
        /// 作　　者:Blog.Core
        /// </summary>
        /// <param name="strConnectionString">连接字符串</param>
        /// <param name="enmDbType">数据库类型</param>
        public static void Init(string strConnectionString, DbType enmDbType = SqlSugar.DbType.SqlServer)
        {
            _connectionString = strConnectionString;
            _dbType = enmDbType;
        }

        /// <summary>
        /// 功能描述:创建一个链接配置
        /// 作　　者:Blog.Core
        /// </summary>
        /// <param name="blnIsAutoCloseConnection">是否自动关闭连接</param>
        /// <param name="blnIsShardSameThread">是否夸类事务</param>
        /// <returns>ConnectionConfig</returns>
        public static ConnectionConfig GetConnectionConfig(bool blnIsAutoCloseConnection = true, bool blnIsShardSameThread = false)
        {
            ConnectionConfig config = new ConnectionConfig()
            {
                ConnectionString = _connectionString,
                DbType = _dbType,
                IsAutoCloseConnection = blnIsAutoCloseConnection,
                ConfigureExternalServices = new ConfigureExternalServices()
                {
                    //DataInfoCacheService = new HttpRuntimeCache()
                },
                IsShardSameThread = blnIsShardSameThread
            };
            return config;
        }

        /// <summary>
        /// 功能描述:获取一个自定义的DB
        /// 作　　者:Blog.Core
        /// </summary>
        /// <param name="config">config</param>
        /// <returns>返回值</returns>
        public static SqlSugarClient GetCustomDB(ConnectionConfig config)
        {
            return new SqlSugarClient(config);
        }
        /// <summary>
        /// 功能描述:获取一个自定义的数据库处理对象
        /// 作　　者:Blog.Core
        /// </summary>
        /// <param name="sugarClient">sugarClient</param>
        /// <returns>返回值</returns>
        public static SimpleClient<T> GetCustomEntityDB<T>(SqlSugarClient sugarClient) where T : class, new()
        {
            return new SimpleClient<T>(sugarClient);
        }
        /// <summary>
        /// 功能描述:获取一个自定义的数据库处理对象
        /// 作　　者:Blog.Core
        /// </summary>
        /// <param name="config">config</param>
        /// <returns>返回值</returns>
        public static SimpleClient<T> GetCustomEntityDB<T>(ConnectionConfig config) where T : class, new()
        {
            SqlSugarClient sugarClient = GetCustomDB(config);
            return GetCustomEntityDB<T>(sugarClient);
        }
        #endregion
    }
}
