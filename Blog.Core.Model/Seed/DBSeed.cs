using Blog.Core.Common;
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
        // 这是我的在线demo数据，比较多，且杂乱
        // 国内网络不好的，可以使用这个 gitee 上的地址：https://gitee.com/laozhangIsPhi/Blog.Data.Share/tree/master/Blog.Core.Data.json/{0}.tsv
        private static string GitJsonFileFormat = "https://github.com/anjoy8/Blog.Data.Share/raw/master/Blog.Core.Data.json/{0}.tsv";


        // 这里我把重要的权限数据提出来的精简版，默认一个Admin_Role + 一个管理员用户，
        // 然后就是菜单+接口+权限分配，注意没有其他博客信息了，下边seeddata 的时候，删掉即可。
        // 国内网络不好的，可以使用这个 gitee 上的地址：https://gitee.com/laozhangIsPhi/Blog.Data.Share/tree/master/Student.Achieve.json/{0}.tsv
        private static string GitJsonFileFormat2 = "https://github.com/anjoy8/Blog.Data.Share/raw/master/Student.Achieve.json/{0}.tsv";

        /// <summary>
        /// 异步添加种子数据
        /// </summary>
        /// <param name="myContext"></param>
        /// <returns></returns>
        public static async Task SeedAsync(MyContext myContext)
        {
            try
            {
                // 如果生成过了，第二次，就不用再执行一遍了,注释掉该方法即可

                #region 自动创建数据库暂停服务
                // 自动创建数据库，注意版本是 sugar 5.x 版本的

                // 注意：这里还是有些问题，比如使用mysql的话，如果通过这个方法创建空数据库，字符串不是utf8的，所以还是手动创建空的数据库吧，然后设置数据库为utf-8，我再和作者讨论一下。
                // 但是使用SqlServer 和 Sqlite 好像没有这个问题。
                //myContext.Db.DbMaintenance.CreateDatabase(); 
                #endregion


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
                    typeof(UserRole));

                // 后期单独处理某些表
                //myContext.Db.CodeFirst.InitTables(typeof(sysUserInfo));
                //myContext.Db.CodeFirst.InitTables(typeof(Permission)); 
                //myContext.Db.CodeFirst.InitTables(typeof(Advertisement));

                Console.WriteLine("Database:WMBlog created success!");
                Console.WriteLine();

                if (Appsettings.app(new string[] { "AppSettings", "SeedDBDataEnabled" }).ObjToBool())
                {
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
                }

                Console.WriteLine();

            }
            catch (Exception ex)
            {
                throw new Exception("1、注意要先创建空的数据库\n2、" + ex.Message);
            }
        }
    }
}
