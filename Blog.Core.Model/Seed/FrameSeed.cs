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
    public class FrameSeed
    {

        /// <summary>
        /// 生成Model层
        /// </summary>
        /// <param name="myContext"></param>
        /// <returns></returns>
        public static bool CreateModels(MyContext myContext)
        {

            try
            {
                myContext.Create_Model_ClassFileByDBTalbe($@"C:\my-file\Blog.Core.Model", "Blog.Core.Model.Models", new string[] {  }, "");
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

        /// <summary>
        /// 生成IRepository层
        /// </summary>
        /// <param name="myContext"></param>
        /// <returns></returns>
        public static bool CreateIRepositorys(MyContext myContext)
        {

            try
            {
                myContext.Create_IRepository_ClassFileByDBTalbe($@"C:\my-file\Blog.Core.IRepository", "Blog.Core.IRepository", new string[] {  }, "");
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }



        /// <summary>
        /// 生成 IService 层
        /// </summary>
        /// <param name="myContext"></param>
        /// <returns></returns>
        public static bool CreateIServices(MyContext myContext)
        {

            try
            {
                myContext.Create_IServices_ClassFileByDBTalbe($@"C:\my-file\Blog.Core.IServices", "Blog.Core.IServices", new string[] { "Module" }, "");
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }



        /// <summary>
        /// 生成 Repository 层
        /// </summary>
        /// <param name="myContext"></param>
        /// <returns></returns>
        public static bool CreateRepository(MyContext myContext)
        {

            try
            {
                myContext.Create_Repository_ClassFileByDBTalbe($@"C:\my-file\Blog.Core.Repository", "Blog.Core.Repository", new string[] { "Module" }, "");
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }



        /// <summary>
        /// 生成 Repository 层
        /// </summary>
        /// <param name="myContext"></param>
        /// <returns></returns>
        public static bool CreateServices(MyContext myContext)
        {

            try
            {
                myContext.Create_Repository_ClassFileByDBTalbe($@"C:\my-file\Blog.Core.Services", "Blog.Core.Services", new string[] { "Module" }, "");
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }
    }
}
