using Blog.Core.Common.Helper;
using Blog.Core.Model.Models;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Blog.Core.Model.Models
{
    public class DBSeed
    {
        private static string GitJsonFileFormat = "https://github.com/anjoy8/Blog.Data.Share/raw/master/Blog.Core.Data.json/{0}.tsv";
        /// <summary>
        /// 异步添加种子数据
        /// </summary>
        /// <param name="myContext"></param>
        /// <returns></returns>
        public static async Task SeedAsync(MyContext myContext)
        {
            try
            {
                // 注意！一定要先手动创建一个【空的数据库】
                // 如果生成过了，第二次，就不用再执行一遍了,注释掉该方法即可
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
                    typeof(UserRole));

                // 后期单独处理某些表
                //myContext.Db.CodeFirst.InitTables(typeof(sysUserInfo));
                //myContext.Db.CodeFirst.InitTables(typeof(Permission)); 
                //myContext.Db.CodeFirst.InitTables(typeof(Advertisement));

                Console.WriteLine("Database:WMBlog created success!");
                Console.WriteLine();

                Console.WriteLine("Seeding database...");

                #region BlogArticle
                if (!await myContext.Db.Queryable<BlogArticle>().AnyAsync())
                {
                    myContext.GetEntityDB<BlogArticle>().InsertRange(JsonHelper.ParseFormByJson<List<BlogArticle>>(GetNetData.Get(string.Format(GitJsonFileFormat, "BlogArticle"))));
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
                    myContext.GetEntityDB<Module>().InsertRange(JsonHelper.ParseFormByJson<List<Module>>(GetNetData.Get(string.Format(GitJsonFileFormat, "Module"))));
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
                    myContext.GetEntityDB<Permission>().InsertRange(JsonHelper.ParseFormByJson<List<Permission>>(GetNetData.Get(string.Format(GitJsonFileFormat, "Permission"))));
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
                    myContext.GetEntityDB<Role>().InsertRange(JsonHelper.ParseFormByJson<List<Role>>(GetNetData.Get(string.Format(GitJsonFileFormat, "Role"))));
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
                    myContext.GetEntityDB<RoleModulePermission>().InsertRange(JsonHelper.ParseFormByJson<List<RoleModulePermission>>(GetNetData.Get(string.Format(GitJsonFileFormat, "RoleModulePermission"))));
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
                    myContext.GetEntityDB<Topic>().InsertRange(JsonHelper.ParseFormByJson<List<Topic>>(GetNetData.Get(string.Format(GitJsonFileFormat, "Topic"))));
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
                    myContext.GetEntityDB<TopicDetail>().InsertRange(JsonHelper.ParseFormByJson<List<TopicDetail>>(GetNetData.Get(string.Format(GitJsonFileFormat, "TopicDetail"))));
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
                    myContext.GetEntityDB<UserRole>().InsertRange(JsonHelper.ParseFormByJson<List<UserRole>>(GetNetData.Get(string.Format(GitJsonFileFormat, "UserRole"))));
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
                    myContext.GetEntityDB<sysUserInfo>().InsertRange(JsonHelper.ParseFormByJson<List<sysUserInfo>>(GetNetData.Get(string.Format(GitJsonFileFormat, "sysUserInfo"))));
                    Console.WriteLine("Table:sysUserInfo created success!");
                }
                else
                {
                    Console.WriteLine("Table:sysUserInfo already exists...");
                }
                #endregion


                Console.WriteLine("Done seeding database.");
                Console.WriteLine();

            }
            catch (Exception ex)
            {
                throw new Exception("1、注意要先创建空的数据库\n2、" + ex.Message);
            }
        }
    }
}
