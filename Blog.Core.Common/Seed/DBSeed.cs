using Blog.Core.Common.DB;
using Blog.Core.Common.Extensions;
using Blog.Core.Common.Helper;
using Blog.Core.Model.Models;
using Magicodes.ExporterAndImporter.Excel;
using Newtonsoft.Json;
using SqlSugar;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using Blog.Core.Common.Const;
using Blog.Core.Migrate.Core;
using Blog.Core.Model.Base.Tenants;
using Microsoft.Data.SqlClient;
using Serilog;

namespace Blog.Core.Common.Seed
{
    public class DBSeed
    {
        private static string _seedDataFolder = "BlogCore.Data.json/{0}.tsv";


        /// <summary>
        /// 异步添加种子数据
        /// </summary>
        /// <param name="myContext"></param>
        /// <param name="webRootPath"></param>
        /// <returns></returns>
        public static async Task SeedAsync(MyContext myContext, string webRootPath)
        {
            var db = myContext.Db;
            try
            {
                if (string.IsNullOrEmpty(webRootPath))
                {
                    throw new Exception("获取wwwroot路径时，异常！");
                }

                _seedDataFolder = Path.Combine(webRootPath, _seedDataFolder);

                Log.Information("===============Blog.Core DataBase Set==================");
                Log.Information("Master DB ConId: {ConfigId}", myContext.Db.CurrentConnectionConfig.ConfigId);
                Log.Information("Master DB Type: {DbType}", myContext.Db.CurrentConnectionConfig.DbType);
                Log.Information("Master DB ConnectString: {ConnectionString}", myContext.Db.CurrentConnectionConfig.ConnectionString);

                if (BaseDBConfig.MainConfig.SlaveConnectionConfigs.AnyNoException())
                {
                    var index = 0;
                    BaseDBConfig.MainConfig.SlaveConnectionConfigs.ForEach(m =>
                    {
                        index++;
                        Log.Information("Slave{Index} DB HitRate: {ObjHitRate}", index, m.HitRate);
                        Log.Information("Slave{Index} DB ConnectString: {ObjConnectionString}", index, m.ConnectionString);
                        Log.Information($"--------------------------------------");
                    });
                }
                else if (BaseDBConfig.ReuseConfigs.AnyNoException())
                {
                    var index = 0;
                    BaseDBConfig.ReuseConfigs.ForEach(m =>
                    {
                        index++;
                        Log.Information("Reuse{Index} DB ID: {ObjConfigId}", index, m.ConfigId);
                        Log.Information("Reuse{Index} DB Type: {ObjDbType}", index, m.DbType);
                        Log.Information("Reuse{Index} DB ConnectString: {ObjConnectionString}", index, m.ConnectionString);
                        Log.Information($"--------------------------------------");
                    });
                }

                // 创建数据库
                Log.Information("Create Database(The Db Id:{ConnId})...", MyContext.ConnId);

                if (MyContext.DbType != DbType.Oracle && MyContext.DbType != DbType.Dm)
                {
                    myContext.Db.DbMaintenance.CreateDatabase();
                    SqlConnection.ClearAllPools();
                    Log.Information($"Database created successfully!");
                }
                else
                {
                    //Oracle 数据库不支持该操作
                    Log.Warning($"Oracle 数据库不支持该操作，可手动创建Oracle/Dm数据库!");
                }

                // 创建数据库表，遍历指定命名空间下的class，
                // 注意不要把其他命名空间下的也添加进来。
                Log.Information("Create Tables...");

                var path = AppDomain.CurrentDomain.RelativeSearchPath ?? AppDomain.CurrentDomain.BaseDirectory;
                var referencedAssemblies = Directory.GetFiles(path, "Blog.Core.Model.dll")
                   .Select(Assembly.LoadFrom).ToArray();
                var modelTypes = referencedAssemblies
                   .SelectMany(a => a.DefinedTypes)
                   .Select(type => type.AsType())
                   .Where(x => x.IsClass && x.Namespace != null && x.Namespace.StartsWith("Blog.Core.Model.Models"))
                   .Where(s => !s.IsDefined(typeof(MultiTenantAttribute), false))
                   .ToList();
                await MigrateCore.MigrateAsync(db, modelTypes.ToArray());
                Log.Information($"Tables created successfully!");

                if (AppSettings.app("AppSettings", "SeedDBDataEnabled").ObjToBool())
                {
                    JsonSerializerSettings setting = new JsonSerializerSettings();
                    JsonConvert.DefaultSettings = () =>
                    {
                        //日期类型默认格式化处理
                        setting.DateFormatHandling = DateFormatHandling.MicrosoftDateFormat;
                        setting.DateFormatString = "yyyy-MM-dd HH:mm:ss";

                        //空值处理
                        setting.NullValueHandling = NullValueHandling.Ignore;

                        //高级用法九中的Bool类型转换 设置
                        //setting.Converters.Add(new BoolConvert("是,否"));

                        return setting;
                    };

                    Log.Information("Seeding database data (The Db Id:{ConnId})...", MyContext.ConnId);

                    var importer = new ExcelImporter();

                    #region BlogArticle

                    if (!await myContext.Db.Queryable<BlogArticle>().AnyAsync())
                    {
                        myContext.GetEntityDB<BlogArticle>().InsertRange(
                            JsonHelper.ParseFormByJson<List<BlogArticle>>(
                                FileHelper.ReadFile(string.Format(_seedDataFolder, "BlogArticle"), Encoding.UTF8)));
                        Log.Information("Table:BlogArticle created success!");
                    }
                    else
                    {
                        Log.Information("Table:BlogArticle already exists...");
                    }

                    #endregion


                    #region Modules

                    if (!await myContext.Db.Queryable<Modules>().AnyAsync())
                    {
                        var data = JsonConvert.DeserializeObject<List<Modules>>(
                            FileHelper.ReadFile(string.Format(_seedDataFolder, "Modules"), Encoding.UTF8), setting);

                        myContext.GetEntityDB<Modules>().InsertRange(data);
                        Log.Information("Table:Modules created success!");
                    }
                    else
                    {
                        Log.Information("Table:Modules already exists...");
                    }

                    #endregion


                    #region Permission

                    if (!await myContext.Db.Queryable<Permission>().AnyAsync())
                    {
                        var data = JsonConvert.DeserializeObject<List<Permission>>(
                            FileHelper.ReadFile(string.Format(_seedDataFolder, "Permission"), Encoding.UTF8), setting);

                        myContext.GetEntityDB<Permission>().InsertRange(data);
                        Log.Information("Table:Permission created success!");
                    }
                    else
                    {
                        Log.Information("Table:Permission already exists...");
                    }

                    #endregion


                    #region Role

                    if (!await myContext.Db.Queryable<Role>().AnyAsync())
                    {
                        var data = JsonConvert.DeserializeObject<List<Role>>(
                            FileHelper.ReadFile(string.Format(_seedDataFolder, "Role"), Encoding.UTF8), setting);
                        //using var stream = new FileStream(Path.Combine(WebRootPath, "BlogCore.Data.excel", "Role.xlsx"), FileMode.Open);
                        //var result = await importer.Import<Role>(stream);
                        //var data = result.Data.ToList();

                        myContext.GetEntityDB<Role>().InsertRange(data);
                        Log.Information("Table:Role created success!");
                    }
                    else
                    {
                        Log.Information("Table:Role already exists...");
                    }

                    #endregion


                    #region RoleModulePermission

                    if (!await myContext.Db.Queryable<RoleModulePermission>().AnyAsync())
                    {
                        var data = JsonConvert.DeserializeObject<List<RoleModulePermission>>(
                            FileHelper.ReadFile(string.Format(_seedDataFolder, "RoleModulePermission"), Encoding.UTF8),
                            setting);

                        myContext.GetEntityDB<RoleModulePermission>().InsertRange(data);
                        Log.Information("Table:RoleModulePermission created success!");
                    }
                    else
                    {
                        Log.Information("Table:RoleModulePermission already exists...");
                    }

                    #endregion


                    #region Topic

                    if (!await myContext.Db.Queryable<Topic>().AnyAsync())
                    {
                        var data = JsonConvert.DeserializeObject<List<Topic>>(
                            FileHelper.ReadFile(string.Format(_seedDataFolder, "Topic"), Encoding.UTF8), setting);

                        myContext.GetEntityDB<Topic>().InsertRange(data);
                        Log.Information("Table:Topic created success!");
                    }
                    else
                    {
                        Log.Information("Table:Topic already exists...");
                    }

                    #endregion


                    #region TopicDetail

                    if (!await myContext.Db.Queryable<TopicDetail>().AnyAsync())
                    {
                        var data = JsonConvert.DeserializeObject<List<TopicDetail>>(
                            FileHelper.ReadFile(string.Format(_seedDataFolder, "TopicDetail"), Encoding.UTF8), setting);

                        myContext.GetEntityDB<TopicDetail>().InsertRange(data);
                        Log.Information("Table:TopicDetail created success!");
                    }
                    else
                    {
                        Log.Information("Table:TopicDetail already exists...");
                    }

                    #endregion


                    #region UserRole

                    if (!await myContext.Db.Queryable<UserRole>().AnyAsync())
                    {
                        var data = JsonConvert.DeserializeObject<List<UserRole>>(
                            FileHelper.ReadFile(string.Format(_seedDataFolder, "UserRole"), Encoding.UTF8), setting);

                        myContext.GetEntityDB<UserRole>().InsertRange(data);
                        Log.Information("Table:UserRole created success!");
                    }
                    else
                    {
                        Log.Information("Table:UserRole already exists...");
                    }

                    #endregion


                    #region sysUserInfo

                    if (!await myContext.Db.Queryable<SysUserInfo>().AnyAsync())
                    {
                        var data = JsonConvert.DeserializeObject<List<SysUserInfo>>(
                            FileHelper.ReadFile(string.Format(_seedDataFolder, "sysUserInfo"), Encoding.UTF8), setting);

                        myContext.GetEntityDB<SysUserInfo>().InsertRange(data);
                        Log.Information("Table:sysUserInfo created success!");
                    }
                    else
                    {
                        Log.Information("Table:sysUserInfo already exists...");
                    }

                    #endregion


                    #region TasksQz

                    if (!await myContext.Db.Queryable<TasksQz>().AnyAsync())
                    {
                        var data = JsonConvert.DeserializeObject<List<TasksQz>>(
                            FileHelper.ReadFile(string.Format(_seedDataFolder, "TasksQz"), Encoding.UTF8), setting);

                        myContext.GetEntityDB<TasksQz>().InsertRange(data);
                        Log.Information("Table:TasksQz created success!");
                    }
                    else
                    {
                        Log.Information("Table:TasksQz already exists...");
                    }

                    #endregion

                    #region TasksLog

                    if (!await myContext.Db.Queryable<TasksLog>().AnyAsync())
                    {
                        Log.Information("Table:TasksLog created success!");
                    }
                    else
                    {
                        Log.Information("Table:TasksLog already exists...");
                    }

                    #endregion

                    #region Department

                    if (!await myContext.Db.Queryable<Department>().AnyAsync())
                    {
                        var data = JsonConvert.DeserializeObject<List<Department>>(
                            FileHelper.ReadFile(string.Format(_seedDataFolder, "Department"), Encoding.UTF8), setting);

                        myContext.GetEntityDB<Department>().InsertRange(data);
                        Log.Information("Table:Department created success!");
                    }
                    else
                    {
                        Log.Information("Table:Department already exists...");
                    }

                    #endregion

                    //种子初始化
                    await SeedDataAsync(myContext.Db);

                    Log.Information($"Done seeding database!");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(
                    $"1、若是Mysql,查看常见问题:https://github.com/anjoy8/Blog.Core/issues/148#issue-776281770 \n" +
                    $"2、若是Oracle,查看常见问题:https://github.com/anjoy8/Blog.Core/issues/148#issuecomment-752340231 \n" +
                    "3、其他错误：" + ex.Message);
            }
        }

        /// <summary>
        /// 种子初始化数据
        /// </summary>
        /// <param name="db"></param>
        /// <returns></returns>
        private static async Task SeedDataAsync(ISqlSugarClient db)
        {
            // 获取所有种子配置-初始化数据
            var seedDataTypes = AssemblysExtensions.GetAllAssemblies().SelectMany(s => s.DefinedTypes)
               .Where(u => !u.IsInterface && !u.IsAbstract && u.IsClass)
               .Where(u =>
                {
                    var esd = u.GetInterfaces()
                       .FirstOrDefault(i => i.HasImplementedRawGeneric(typeof(IEntitySeedData<>)));
                    if (esd is null)
                    {
                        return false;
                    }

                    var eType = esd.GenericTypeArguments[0];
                    if (eType.GetCustomAttribute<MultiTenantAttribute>() is null)
                    {
                        return true;
                    }

                    return false;
                }).ToList();

            if (!seedDataTypes.Any()) return;
            foreach (var seedType in seedDataTypes)
            {
                dynamic instance = Activator.CreateInstance(seedType);
                if (instance is null) continue;
                //初始化数据
                {
                    var seedData = instance.InitSeedData();
                    if (seedData != null && Enumerable.Any(seedData))
                    {
                        var entityType = seedType.GetInterfaces().First().GetGenericArguments().First();
                        var entity = db.EntityMaintenance.GetEntityInfo(entityType);

                        if (!await db.Queryable(entity.DbTableName, "").AnyAsync())
                        {
                            await db.Insertable(Enumerable.ToList(seedData)).ExecuteCommandAsync();
                            Log.Information("Table:{EntityDbTableName} init success!", entity.DbTableName);
                        }
                    }
                }

                //种子数据
                {
                    var seedData = instance.SeedData();
                    if (seedData != null && Enumerable.Any(seedData))
                    {
                        var entityType = seedType.GetInterfaces().First().GetGenericArguments().First();
                        var entity = db.EntityMaintenance.GetEntityInfo(entityType);

                        await db.Storageable(Enumerable.ToList(seedData)).ExecuteCommandAsync();
                        Log.Information("Table:{EntityDbTableName} seedData success!", entity.DbTableName);
                    }
                }

                //自定义处理
                {
                    await instance.CustomizeSeedData(db);
                }
            }
        }

        /// <summary>
        /// 迁移日志数据库
        /// </summary>
        /// <returns></returns>
        public static async Task MigrationLogsAsync(MyContext myContext)
        {
            // 创建数据库表，遍历指定命名空间下的class，
            // 注意不要把其他命名空间下的也添加进来。
            Log.Information("Create Log Tables...");
            if (!myContext.Db.IsAnyConnection(SqlSugarConst.LogConfigId.ToLower()))
            {
                throw new ApplicationException("未配置日志数据库，请在appsettings.json中DBS节点中配置");
            }

            var logDb = myContext.Db.GetConnection(SqlSugarConst.LogConfigId.ToLower());
            Log.Information("Create log Database(The Db Id:{Lower})...", SqlSugarConst.LogConfigId.ToLower());
            logDb.DbMaintenance.CreateDatabase();
            Log.Information($"Log Database created successfully!");
            var path = AppDomain.CurrentDomain.RelativeSearchPath ?? AppDomain.CurrentDomain.BaseDirectory;
            var referencedAssemblies = Directory.GetFiles(path, "Blog.Core.Model.dll")
               .Select(Assembly.LoadFrom).ToArray();
            var modelTypes = referencedAssemblies
               .SelectMany(a => a.DefinedTypes)
               .Select(type => type.AsType())
               .Where(x => x.IsClass && x.Namespace != null && x.Namespace.StartsWith("Blog.Core.Model.Logs"))
               .ToList();
            var sw = Stopwatch.StartNew();
            await MigrateCore.MigrateAsync(logDb, modelTypes.ToArray());
            sw.Stop();

            Log.Information("Log Tables created successfully! {SwElapsedMilliseconds}ms", sw.ElapsedMilliseconds);
        }

        /// <summary>
        /// 初始化 多租户
        /// </summary>
        /// <param name="myContext"></param>
        /// <returns></returns>
        public static async Task TenantSeedAsync(MyContext myContext)
        {
            var tenants = await myContext.Db.Queryable<SysTenant>().Where(s => s.TenantType == TenantTypeEnum.Db)
               .ToListAsync();
            if (tenants.Any())
            {
                Log.Information($@"Init Multi Tenant Db");
                foreach (var tenant in tenants)
                {
                    Log.Information("Init Multi Tenant Db : {TenantConfigId}/{TenantName}", tenant.ConfigId, tenant.Name);
                    await InitTenantSeedAsync(myContext.Db.AsTenant(), tenant.GetConnectionConfig());
                }
            }

            tenants = await myContext.Db.Queryable<SysTenant>().Where(s => s.TenantType == TenantTypeEnum.Tables)
               .ToListAsync();
            if (tenants.Any())
            {
                await InitTenantSeedAsync(myContext, tenants);
            }
        }

        #region 多租户 多表 初始化

        private static async Task InitTenantSeedAsync(MyContext myContext, List<SysTenant> tenants)
        {
            Log.Information("Init Multi Tenant Tables : {ConfigId}", myContext.Db.CurrentConnectionConfig.ConfigId);

            // 获取所有实体表-初始化租户业务表
            var entityTypes = TenantUtil.GetTenantEntityTypes(TenantTypeEnum.Tables);
            if (!entityTypes.Any()) return;

            foreach (var sysTenant in tenants)
            {
                myContext.Db.SetTenantTable(sysTenant.Id.ToString());
                await MigrateCore.MigrateAsync(myContext.Db, entityTypes.ToArray());
               
                //多租户初始化种子数据
                await TenantSeedDataAsync(myContext.Db, TenantTypeEnum.Tables);
            }

            Log.Information("Init Multi Tenant Tables : {ConfigId} created successfully!", myContext.Db.CurrentConnectionConfig.ConfigId);
        }

        #endregion

        #region 多租户 多库 初始化

        /// <summary>
        /// 初始化多库
        /// </summary>
        /// <param name="itenant"></param>
        /// <param name="config"></param>
        /// <returns></returns>
        public static async Task InitTenantSeedAsync(ITenant itenant, ConnectionConfig config)
        {
            itenant.RemoveConnection(config.ConfigId);
            itenant.AddConnection(config);

            var db = itenant.GetConnectionScope(config.ConfigId);

            db.DbMaintenance.CreateDatabase();
            Log.Information("Init Multi Tenant Db : {ConfigConfigId} Database created successfully!", config.ConfigId);

            Log.Information("Init Multi Tenant Db : {ConfigConfigId}  Create Tables", config.ConfigId);
            // 获取所有实体表-初始化租户业务表
            var entityTypes = TenantUtil.GetTenantEntityTypes(TenantTypeEnum.Db);
            if (!entityTypes.Any()) return;

            await MigrateCore.MigrateAsync(db, entityTypes.ToArray());

            //多租户初始化种子数据
            await TenantSeedDataAsync(db, TenantTypeEnum.Db);
        }

        #endregion

        #region 多租户 种子数据 初始化

        private static async Task TenantSeedDataAsync(ISqlSugarClient db, TenantTypeEnum tenantType)
        {
            // 获取所有种子配置-初始化数据
            var seedDataTypes = AssemblysExtensions.GetAllAssemblies().SelectMany(s => s.DefinedTypes)
               .Where(u => !u.IsInterface && !u.IsAbstract && u.IsClass)
               .Where(u =>
                {
                    var esd = u.GetInterfaces()
                       .FirstOrDefault(i => i.HasImplementedRawGeneric(typeof(IEntitySeedData<>)));
                    if (esd is null)
                    {
                        return false;
                    }

                    var eType = esd.GenericTypeArguments[0];
                    return eType.IsTenantEntity(tenantType);
                }).ToList();
            if (!seedDataTypes.Any()) return;
            foreach (var seedType in seedDataTypes)
            {
                dynamic instance = Activator.CreateInstance(seedType);
                if (instance is null) continue;
                //初始化数据
                {
                    var seedData = instance.InitSeedData();
                    if (seedData != null && Enumerable.Any(seedData))
                    {
                        var entityType = seedType.GetInterfaces().First().GetGenericArguments().First();
                        var entity = db.EntityMaintenance.GetEntityInfo(entityType);

                        if (!await db.Queryable(entity.DbTableName, "").AnyAsync())
                        {
                            await db.Insertable(Enumerable.ToList(seedData)).ExecuteCommandAsync();
                            Log.Information("Table:{EntityDbTableName} init success!", entity.DbTableName);
                        }
                    }
                }

                //种子数据
                {
                    var seedData = instance.SeedData();
                    if (seedData != null && Enumerable.Any(seedData))
                    {
                        var entityType = seedType.GetInterfaces().First().GetGenericArguments().First();
                        var entity = db.EntityMaintenance.GetEntityInfo(entityType);

                        await db.Storageable(Enumerable.ToList(seedData)).ExecuteCommandAsync();
                        Log.Information("Table:{EntityDbTableName} seedData success!", entity.DbTableName);
                    }
                }

                //自定义处理
                {
                    await instance.CustomizeSeedData(db);
                }
            }
        }

        #endregion
    }
}