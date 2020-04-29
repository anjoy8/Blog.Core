using Blog.Core.Common;
using Blog.Core.Common.DB;
using Blog.Core.Common.Helper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Core.Model.Models
{
    public class DBSeed
    {
        // 这是我的在线demo数据，比较多，且杂乱
        // gitee 源数据
        private static string SeedDataFolder = "BlogCore.Data.json/{0}.tsv";


        // 这里我把重要的权限数据提出来的精简版，默认一个Admin_Role + 一个管理员用户，
        // 然后就是菜单+接口+权限分配，注意没有其他博客信息了，下边seeddata 的时候，删掉即可。

        // gitee 源数据
        private static string SeedDataFolderMini = "BlogCore.Mini.Data.json/{0}.tsv";


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
                SeedDataFolderMini = Path.Combine(WebRootPath, SeedDataFolderMini);

                Console.WriteLine("Database config data init...");
                Console.WriteLine($"Is multi-DataBase: {Appsettings.app(new string[] { "MutiDBEnabled" })}");
                Console.WriteLine($"Is CQRS: {Appsettings.app(new string[] { "CQRSEnabled" })}");
                Console.WriteLine();
                if (Appsettings.app(new string[] { "MutiDBEnabled" }).ObjToBool())
                {
                    Console.WriteLine($"Master DB Type: {MyContext.DbType}");
                    Console.WriteLine($"Master DB ConnectString: {MyContext.ConnectionString}");
                    Console.WriteLine();

                    var slaveIndex = 0;
                    BaseDBConfig.MutiConnectionString.Item1.Where(x => x.ConnId != MainDb.CurrentDbConnId).ToList().ForEach(m =>
                    {
                        slaveIndex++;
                        Console.WriteLine($"Slave{slaveIndex} DB ID: {m.ConnId}");
                        Console.WriteLine($"Slave{slaveIndex} DB Type: {m.DbType}");
                        Console.WriteLine($"Slave{slaveIndex} DB ConnectString: {m.Connection}");
                    });

                }
                else if (Appsettings.app(new string[] { "CQRSEnabled" }).ObjToBool())
                {
                    Console.WriteLine($"Master DB Type: {MyContext.DbType}");
                    Console.WriteLine($"Master DB ConnectString: {MyContext.ConnectionString}");
                    Console.WriteLine();

                    var slaveIndex = 0;
                    BaseDBConfig.MutiConnectionString.Item2.Where(x => x.ConnId != MainDb.CurrentDbConnId).ToList().ForEach(m =>
                    {
                        slaveIndex++;
                        Console.WriteLine($"Slave{slaveIndex} DB ID: {m.ConnId}");
                        Console.WriteLine($"Slave{slaveIndex} DB Type: {m.DbType}");
                        Console.WriteLine($"Slave{slaveIndex} DB ConnectString: {m.Connection}");
                    });

                }
                else
                {
                    Console.WriteLine("DB Type: " + MyContext.DbType);
                    Console.WriteLine("DB ConnectString: " + MyContext.ConnectionString);
                }

                Console.WriteLine();
                Console.WriteLine("Create Database...");
                // 创建数据库
                myContext.Db.DbMaintenance.CreateDatabase();

                ConsoleHelper.WriteSuccessLine($"Database created successfully!");

                Console.WriteLine("Create Tables...");
                // 创建表
                myContext.CreateTableByEntity(false,
                    typeof(Advertisement),
                    typeof(BlogArticle),
                    typeof(Guestbook),
                    typeof(Module),
                    typeof(ModulePermission),
                    typeof(OperateLog),
                    typeof(PasswordLib),
                    typeof(Permission),
                    typeof(Role),
                    typeof(RoleModulePermission),
                    typeof(sysUserInfo),
                    typeof(Topic),
                    typeof(TopicDetail),
                    typeof(TasksQz),
                    typeof(UserRole));

                // 后期单独处理某些表
                // myContext.Db.CodeFirst.InitTables(typeof(sysUserInfo));

                ConsoleHelper.WriteSuccessLine($"Tables created successfully!");
                Console.WriteLine();

                if (Appsettings.app(new string[] { "AppSettings", "SeedDBDataEnabled" }).ObjToBool())
                {
                    Console.WriteLine("Seeding database data...");

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


                    #region Module
                    if (!await myContext.Db.Queryable<Module>().AnyAsync())
                    {
                        myContext.GetEntityDB<Module>().InsertRange(JsonHelper.ParseFormByJson<List<Module>>(FileHelper.ReadFile(string.Format(SeedDataFolder, "Module"), Encoding.UTF8)));
                        Console.WriteLine("Table:Module created success!");
                    }
                    else
                    {
                        Console.WriteLine("Table:Module already exists...");
                    }
                    #endregion


                    #region Permission
                    if (!await myContext.Db.Queryable<Permission>().AnyAsync())
                    {
                        myContext.GetEntityDB<Permission>().InsertRange(JsonHelper.ParseFormByJson<List<Permission>>(FileHelper.ReadFile(string.Format(SeedDataFolder, "Permission"), Encoding.UTF8)));
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
                        myContext.GetEntityDB<Role>().InsertRange(JsonHelper.ParseFormByJson<List<Role>>(FileHelper.ReadFile(string.Format(SeedDataFolder, "Role"), Encoding.UTF8)));
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
                        myContext.GetEntityDB<RoleModulePermission>().InsertRange(JsonHelper.ParseFormByJson<List<RoleModulePermission>>(FileHelper.ReadFile(string.Format(SeedDataFolder, "RoleModulePermission"), Encoding.UTF8)));
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
                        myContext.GetEntityDB<Topic>().InsertRange(JsonHelper.ParseFormByJson<List<Topic>>(FileHelper.ReadFile(string.Format(SeedDataFolder, "Topic"), Encoding.UTF8)));
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
                        myContext.GetEntityDB<TopicDetail>().InsertRange(JsonHelper.ParseFormByJson<List<TopicDetail>>(FileHelper.ReadFile(string.Format(SeedDataFolder, "TopicDetail"), Encoding.UTF8)));
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
                        myContext.GetEntityDB<UserRole>().InsertRange(JsonHelper.ParseFormByJson<List<UserRole>>(FileHelper.ReadFile(string.Format(SeedDataFolder, "UserRole"), Encoding.UTF8)));
                        Console.WriteLine("Table:UserRole created success!");
                    }
                    else
                    {
                        Console.WriteLine("Table:UserRole already exists...");
                    }
                    #endregion


                    #region sysUserInfo
                    if (!await myContext.Db.Queryable<sysUserInfo>().AnyAsync())
                    {
                        myContext.GetEntityDB<sysUserInfo>().InsertRange(JsonHelper.ParseFormByJson<List<sysUserInfo>>(FileHelper.ReadFile(string.Format(SeedDataFolder, "sysUserInfo"), Encoding.UTF8)));
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
                        myContext.GetEntityDB<TasksQz>().InsertRange(JsonHelper.ParseFormByJson<List<TasksQz>>(FileHelper.ReadFile(string.Format(SeedDataFolder, "TasksQz"), Encoding.UTF8)));
                        Console.WriteLine("Table:TasksQz created success!");
                    }
                    else
                    {
                        Console.WriteLine("Table:TasksQz already exists...");
                    }
                    #endregion

                    ConsoleHelper.WriteSuccessLine($"Done seeding database!");
                }

                Console.WriteLine();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}