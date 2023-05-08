using Blog.Core.Common.DB;
using Blog.Core.Common.Extensions;
using Blog.Core.Common.Helper;
using Blog.Core.Model.Models;
using Blog.Core.Model.Tenants;
using Magicodes.ExporterAndImporter.Excel;
using Newtonsoft.Json;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Blog.Core.Common.Const;

namespace Blog.Core.Common.Seed
{
    public class DBSeed
    {
        private static string SeedDataFolder = "BlogCore.Data.json/{0}.tsv";


        /// <summary>
        /// 异步添加种子数据
        /// </summary>
        /// <param name="myContext"></param>
        /// <param name="WebRootPath"></param>
        /// <returns></returns>
        public static async Task SeedAsync(MyContext myContext, string WebRootPath)
        {
            try
            {
                if (string.IsNullOrEmpty(WebRootPath))
                {
                    throw new Exception("获取wwwroot路径时，异常！");
                }

                SeedDataFolder = Path.Combine(WebRootPath, SeedDataFolder);

                Console.WriteLine("************ Blog.Core DataBase Set *****************");
                Console.WriteLine($"Is multi-DataBase: {AppSettings.app(new string[] { "MutiDBEnabled" })}");
                Console.WriteLine($"Is CQRS: {AppSettings.app(new string[] { "CQRSEnabled" })}");
                Console.WriteLine();
                Console.WriteLine($"Master DB ConId: {myContext.Db.CurrentConnectionConfig.ConfigId}");
                Console.WriteLine($"Master DB Type: {myContext.Db.CurrentConnectionConfig.DbType}");
                Console.WriteLine($"Master DB ConnectString: {myContext.Db.CurrentConnectionConfig.ConnectionString}");
                Console.WriteLine();
                if (AppSettings.app(new string[] { "MutiDBEnabled" }).ObjToBool())
                {
                    var slaveIndex = 0;
                    BaseDBConfig.MutiConnectionString.allDbs.Where(x => x.ConnId != MainDb.CurrentDbConnId).ToList().ForEach(m =>
                    {
                        slaveIndex++;
                        Console.WriteLine($"Slave{slaveIndex} DB ID: {m.ConnId}");
                        Console.WriteLine($"Slave{slaveIndex} DB Type: {m.DbType}");
                        Console.WriteLine($"Slave{slaveIndex} DB ConnectString: {m.Connection}");
                        Console.WriteLine($"--------------------------------------");
                    });
                }
                else if (AppSettings.app(new string[] { "CQRSEnabled" }).ObjToBool())
                {
                    var slaveIndex = 0;
                    BaseDBConfig.MutiConnectionString.slaveDbs.Where(x => x.ConnId != MainDb.CurrentDbConnId).ToList().ForEach(m =>
                    {
                        slaveIndex++;
                        Console.WriteLine($"Slave{slaveIndex} DB ID: {m.ConnId}");
                        Console.WriteLine($"Slave{slaveIndex} DB Type: {m.DbType}");
                        Console.WriteLine($"Slave{slaveIndex} DB ConnectString: {m.Connection}");
                        Console.WriteLine($"--------------------------------------");
                    });
                }
                else
                {
                }

                Console.WriteLine();

                // 创建数据库
                Console.WriteLine($"Create Database(The Db Id:{MyContext.ConnId})...");

                if (MyContext.DbType != SqlSugar.DbType.Oracle)
                {
                    myContext.Db.DbMaintenance.CreateDatabase();
                    ConsoleHelper.WriteSuccessLine($"Database created successfully!");
                }
                else
                {
                    //Oracle 数据库不支持该操作
                    ConsoleHelper.WriteSuccessLine($"Oracle 数据库不支持该操作，可手动创建Oracle数据库!");
                }

                // 创建数据库表，遍历指定命名空间下的class，
                // 注意不要把其他命名空间下的也添加进来。
                Console.WriteLine("Create Tables...");

                var path = AppDomain.CurrentDomain.RelativeSearchPath ?? AppDomain.CurrentDomain.BaseDirectory;
                var referencedAssemblies = System.IO.Directory.GetFiles(path, "Blog.Core.Model.dll").Select(Assembly.LoadFrom).ToArray();
                var modelTypes = referencedAssemblies
                    .SelectMany(a => a.DefinedTypes)
                    .Select(type => type.AsType())
                    .Where(x => x.IsClass && x.Namespace is "Blog.Core.Model.Models")
                    .Where(s => !s.IsDefined(typeof(MultiTenantAttribute), false))
                    .ToList();
                modelTypes.ForEach(t =>
                {
                    // 这里只支持添加表，不支持删除
                    // 如果想要删除，数据库直接右键删除，或者联系SqlSugar作者；
                    if (!myContext.Db.DbMaintenance.IsAnyTable(t.Name))
                    {
                        Console.WriteLine(t.Name);
                        myContext.Db.CodeFirst.SplitTables().InitTables(t);
                    }
                });
                ConsoleHelper.WriteSuccessLine($"Tables created successfully!");
                Console.WriteLine();

                if (AppSettings.app(new string[] { "AppSettings", "SeedDBDataEnabled" }).ObjToBool())
                {
                    JsonSerializerSettings setting = new JsonSerializerSettings();
                    JsonConvert.DefaultSettings = new Func<JsonSerializerSettings>(() =>
                    {
                        //日期类型默认格式化处理
                        setting.DateFormatHandling = DateFormatHandling.MicrosoftDateFormat;
                        setting.DateFormatString = "yyyy-MM-dd HH:mm:ss";

                        //空值处理
                        setting.NullValueHandling = NullValueHandling.Ignore;

                        //高级用法九中的Bool类型转换 设置
                        //setting.Converters.Add(new BoolConvert("是,否"));

                        return setting;
                    });

                    Console.WriteLine($"Seeding database data (The Db Id:{MyContext.ConnId})...");

                    var importer = new ExcelImporter();

                    #region BlogArticle

                    if (!await myContext.Db.Queryable<BlogArticle>().AnyAsync())
                    {
                        myContext.GetEntityDB<BlogArticle>().InsertRange(JsonHelper.ParseFormByJson<List<BlogArticle>>(FileHelper.ReadFile(string.Format(SeedDataFolder, "BlogArticle"), Encoding.UTF8)));
                        Console.WriteLine("Table:BlogArticle created success!");
                    }
                    else
                    {
                        Console.WriteLine("Table:BlogArticle already exists...");
                    }

                    #endregion


                    #region Modules

                    if (!await myContext.Db.Queryable<Modules>().AnyAsync())
                    {
                        var data = JsonConvert.DeserializeObject<List<Modules>>(FileHelper.ReadFile(string.Format(SeedDataFolder, "Modules"), Encoding.UTF8), setting);

                        myContext.GetEntityDB<Modules>().InsertRange(data);
                        Console.WriteLine("Table:Modules created success!");
                    }
                    else
                    {
                        Console.WriteLine("Table:Modules already exists...");
                    }

                    #endregion


                    #region Permission

                    if (!await myContext.Db.Queryable<Permission>().AnyAsync())
                    {
                        var data = JsonConvert.DeserializeObject<List<Permission>>(FileHelper.ReadFile(string.Format(SeedDataFolder, "Permission"), Encoding.UTF8), setting);

                        foreach (var item in data)
                        {
                            Console.WriteLine($"{item.Name}:{item.Id}");
                            myContext.GetEntityDB<Permission>().Insert(item);
                        }
                        Console.WriteLine("Table:Permission created success!");
                    }
                    else
                    {
                        Console.WriteLine("Table:Permission already exists...");
                    }

                    #endregion


                    #region Role

                    if (!await myContext.Db.Queryable<Role>().AnyAsync())
                    {
                        var data = JsonConvert.DeserializeObject<List<Role>>(FileHelper.ReadFile(string.Format(SeedDataFolder, "Role"), Encoding.UTF8), setting);
                        //using var stream = new FileStream(Path.Combine(WebRootPath, "BlogCore.Data.excel", "Role.xlsx"), FileMode.Open);
                        //var result = await importer.Import<Role>(stream);
                        //var data = result.Data.ToList();

                        myContext.GetEntityDB<Role>().InsertRange(data);
                        Console.WriteLine("Table:Role created success!");
                    }
                    else
                    {
                        Console.WriteLine("Table:Role already exists...");
                    }

                    #endregion


                    #region RoleModulePermission

                    if (!await myContext.Db.Queryable<RoleModulePermission>().AnyAsync())
                    {
                        var data = JsonConvert.DeserializeObject<List<RoleModulePermission>>(FileHelper.ReadFile(string.Format(SeedDataFolder, "RoleModulePermission"), Encoding.UTF8), setting);

                        foreach (var item in data)
                        {
                            Console.WriteLine($"{item.Id}");
                            myContext.GetEntityDB<RoleModulePermission>().Insert(item);
                        }
                        Console.WriteLine("Table:RoleModulePermission created success!");
                    }
                    else
                    {
                        Console.WriteLine("Table:RoleModulePermission already exists...");
                    }

                    #endregion


                    #region Topic

                    if (!await myContext.Db.Queryable<Topic>().AnyAsync())
                    {
                        var data = JsonConvert.DeserializeObject<List<Topic>>(FileHelper.ReadFile(string.Format(SeedDataFolder, "Topic"), Encoding.UTF8), setting);

                        myContext.GetEntityDB<Topic>().InsertRange(data);
                        Console.WriteLine("Table:Topic created success!");
                    }
                    else
                    {
                        Console.WriteLine("Table:Topic already exists...");
                    }

                    #endregion


                    #region TopicDetail

                    if (!await myContext.Db.Queryable<TopicDetail>().AnyAsync())
                    {
                        var data = JsonConvert.DeserializeObject<List<TopicDetail>>(FileHelper.ReadFile(string.Format(SeedDataFolder, "TopicDetail"), Encoding.UTF8), setting);

                        myContext.GetEntityDB<TopicDetail>().InsertRange(data);
                        Console.WriteLine("Table:TopicDetail created success!");
                    }
                    else
                    {
                        Console.WriteLine("Table:TopicDetail already exists...");
                    }

                    #endregion


                    #region UserRole

                    if (!await myContext.Db.Queryable<UserRole>().AnyAsync())
                    {
                        var data = JsonConvert.DeserializeObject<List<UserRole>>(FileHelper.ReadFile(string.Format(SeedDataFolder, "UserRole"), Encoding.UTF8), setting);

                        myContext.GetEntityDB<UserRole>().InsertRange(data);
                        Console.WriteLine("Table:UserRole created success!");
                    }
                    else
                    {
                        Console.WriteLine("Table:UserRole already exists...");
                    }

                    #endregion


                    #region sysUserInfo

                    if (!await myContext.Db.Queryable<SysUserInfo>().AnyAsync())
                    {
                        var data = JsonConvert.DeserializeObject<List<SysUserInfo>>(FileHelper.ReadFile(string.Format(SeedDataFolder, "sysUserInfo"), Encoding.UTF8), setting);

                        myContext.GetEntityDB<SysUserInfo>().InsertRange(data);
                        Console.WriteLine("Table:sysUserInfo created success!");
                    }
                    else
                    {
                        Console.WriteLine("Table:sysUserInfo already exists...");
                    }

                    #endregion


                    #region TasksQz

                    if (!await myContext.Db.Queryable<TasksQz>().AnyAsync())
                    {
                        var data = JsonConvert.DeserializeObject<List<TasksQz>>(FileHelper.ReadFile(string.Format(SeedDataFolder, "TasksQz"), Encoding.UTF8), setting);

                        myContext.GetEntityDB<TasksQz>().InsertRange(data);
                        Console.WriteLine("Table:TasksQz created success!");
                    }
                    else
                    {
                        Console.WriteLine("Table:TasksQz already exists...");
                    }

                    #endregion

                    #region TasksLog

                    if (!await myContext.Db.Queryable<TasksLog>().AnyAsync())
                    {
                        Console.WriteLine("Table:TasksLog created success!");
                    }
                    else
                    {
                        Console.WriteLine("Table:TasksLog already exists...");
                    }

                    #endregion

                    #region Department

                    if (!await myContext.Db.Queryable<Department>().AnyAsync())
                    {
                        var data = JsonConvert.DeserializeObject<List<Department>>(FileHelper.ReadFile(string.Format(SeedDataFolder, "Department"), Encoding.UTF8), setting);

                        myContext.GetEntityDB<Department>().InsertRange(data);
                        Console.WriteLine("Table:Department created success!");
                    }
                    else
                    {
                        Console.WriteLine("Table:Department already exists...");
                    }

                    #endregion

                    //种子初始化
                    await SeedDataAsync(myContext.Db);

                    ConsoleHelper.WriteSuccessLine($"Done seeding database!");
                }

                Console.WriteLine();
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
        /// <param name="myContext"></param>
        /// <returns></returns>
        private static async Task SeedDataAsync(ISqlSugarClient db)
        {
            // 获取所有种子配置-初始化数据
            var seedDataTypes = AssemblysExtensions.GetAllAssemblies().SelectMany(s => s.DefinedTypes)
                .Where(u => !u.IsInterface && !u.IsAbstract && u.IsClass)
                .Where(u =>
                {
                    var esd = u.GetInterfaces().FirstOrDefault(i => i.HasImplementedRawGeneric(typeof(IEntitySeedData<>)));
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
                });

            if (!seedDataTypes.Any()) return;
            foreach (var seedType in seedDataTypes)
            {
                dynamic instance = Activator.CreateInstance(seedType);
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
                            Console.WriteLine($"Table:{entity.DbTableName} init success!");
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
                        Console.WriteLine($"Table:{entity.DbTableName} seedData success!");
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
        public static void MigrationLogs(MyContext myContext)
        {
            // 创建数据库表，遍历指定命名空间下的class，
            // 注意不要把其他命名空间下的也添加进来。
            Console.WriteLine("Create Log Tables...");
            if (!myContext.Db.IsAnyConnection(SqlSugarConst.LogConfigId.ToLower()))
            {
                throw new ApplicationException("未配置日志数据库，请在appsettings.json中DBS节点中配置");
            }

            var logDb = myContext.Db.GetConnection(SqlSugarConst.LogConfigId.ToLower());
            Console.WriteLine($"Create log Database(The Db Id:{SqlSugarConst.LogConfigId.ToLower()})...");
            logDb.DbMaintenance.CreateDatabase();
            ConsoleHelper.WriteSuccessLine($"Log Database created successfully!");
            var path = AppDomain.CurrentDomain.RelativeSearchPath ?? AppDomain.CurrentDomain.BaseDirectory;
            var referencedAssemblies = System.IO.Directory.GetFiles(path, "Blog.Core.Model.dll").Select(Assembly.LoadFrom).ToArray();
            var modelTypes = referencedAssemblies
                .SelectMany(a => a.DefinedTypes)
                .Select(type => type.AsType())
                .Where(x => x.IsClass && x.Namespace != null && x.Namespace.StartsWith("Blog.Core.Model.Logs")).ToList();
            Stopwatch sw = Stopwatch.StartNew();

            var tables = logDb.DbMaintenance.GetTableInfoList();

            modelTypes.ForEach(t =>
            {
                // 这里只支持添加修改表，不支持删除
                // 如果想要删除，数据库直接右键删除，或者联系SqlSugar作者；
                if (!tables.Any(s => s.Name.Contains(t.Name)))
                {
                    Console.WriteLine(t.Name);
                    if (t.GetCustomAttribute<SplitTableAttribute>() != null)
                    {
                        logDb.CodeFirst.SplitTables().InitTables(t);
                    }
                    else
                    {
                        logDb.CodeFirst.InitTables(t);
                    }
                }
            });

            sw.Stop();

            $"Log Tables created successfully! {sw.ElapsedMilliseconds}ms".WriteSuccessLine();
            Console.WriteLine();
        }


        /// <summary>
        /// 初始化 多租户
        /// </summary>
        /// <param name="myContext"></param>
        /// <returns></returns>
        public static async Task TenantSeedAsync(MyContext myContext)
        {
            var tenants = await myContext.Db.Queryable<SysTenant>().Where(s => s.TenantType == TenantTypeEnum.Db).ToListAsync();
            if (tenants.Any())
            {
                Console.WriteLine($@"Init Multi Tenant Db");
                foreach (var tenant in tenants)
                {
                    Console.WriteLine($@"Init Multi Tenant Db : {tenant.ConfigId}/{tenant.Name}");
                    await InitTenantSeedAsync(myContext.Db.AsTenant(), tenant.GetConnectionConfig());
                }
            }

            tenants = await myContext.Db.Queryable<SysTenant>().Where(s => s.TenantType == TenantTypeEnum.Tables).ToListAsync();
            if (tenants.Any())
            {
                await InitTenantSeedAsync(myContext, tenants);
            }
        }

        #region 多租户 多表 初始化

        private static async Task InitTenantSeedAsync(MyContext myContext, List<SysTenant> tenants)
        {
            ConsoleHelper.WriteInfoLine($"Init Multi Tenant Tables : {myContext.Db.CurrentConnectionConfig.ConfigId}");

            // 获取所有实体表-初始化租户业务表
            var entityTypes = TenantUtil.GetTenantEntityTypes(TenantTypeEnum.Tables);
            if (!entityTypes.Any()) return;

            foreach (var sysTenant in tenants)
            {
                foreach (var entityType in entityTypes)
                {
                    myContext.Db.CodeFirst
                        .As(entityType, entityType.GetTenantTableName(myContext.Db, sysTenant))
                        .InitTables(entityType);

                    Console.WriteLine($@"Init Tables:{entityType.GetTenantTableName(myContext.Db, sysTenant)}");
                }

                myContext.Db.SetTenantTable(sysTenant.Id.ToString());
                //多租户初始化种子数据
                await TenantSeedDataAsync(myContext.Db, TenantTypeEnum.Tables);
            }

            ConsoleHelper.WriteSuccessLine($"Init Multi Tenant Tables : {myContext.Db.CurrentConnectionConfig.ConfigId} created successfully!");
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
            ConsoleHelper.WriteSuccessLine($"Init Multi Tenant Db : {config.ConfigId} Database created successfully!");

            Console.WriteLine($@"Init Multi Tenant Db : {config.ConfigId}  Create Tables");

            // 获取所有实体表-初始化租户业务表
            var entityTypes = TenantUtil.GetTenantEntityTypes(TenantTypeEnum.Db);
            if (!entityTypes.Any()) return;
            foreach (var entityType in entityTypes)
            {
                var splitTable = entityType.GetCustomAttribute<SplitTableAttribute>();
                if (splitTable == null)
                    db.CodeFirst.InitTables(entityType);
                else
                    db.CodeFirst.SplitTables().InitTables(entityType);

                Console.WriteLine(entityType.Name);
            }

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
                    var esd = u.GetInterfaces().FirstOrDefault(i => i.HasImplementedRawGeneric(typeof(IEntitySeedData<>)));
                    if (esd is null)
                    {
                        return false;
                    }

                    var eType = esd.GenericTypeArguments[0];
                    return eType.IsTenantEntity(tenantType);
                });
            if (!seedDataTypes.Any()) return;
            foreach (var seedType in seedDataTypes)
            {
                dynamic instance = Activator.CreateInstance(seedType);
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
                            Console.WriteLine($"Table:{entity.DbTableName} init success!");
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
                        Console.WriteLine($"Table:{entity.DbTableName} seedData success!");
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