using SqlSugar;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Blog.Core.Model.Seed
{
    public class FrameSeed
    {

        /// <summary>
        /// 生成Controller层
        /// </summary>
        /// <param name="sqlSugarClient">sqlsugar实例</param>
        /// <param name="ConnId">数据库链接ID</param>
        /// <param name="tableNames">数据库表名数组，默认空，生成所有表</param>
        /// <param name="isMuti"></param>
        /// <returns></returns>
        public static bool CreateControllers(SqlSugarClient sqlSugarClient, string ConnId = null, bool isMuti = false, string[] tableNames = null)
        {

            try
            {
                Create_Controller_ClassFileByDBTalbe(sqlSugarClient, ConnId, $@"C:\my-file\Blog.Core.Api.Controllers", "Blog.Core.Api.Controllers", tableNames, "", isMuti);
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

        /// <summary>
        /// 生成Model层
        /// </summary>
        /// <param name="sqlSugarClient">sqlsugar实例</param>
        /// <param name="ConnId">数据库链接ID</param>
        /// <param name="tableNames">数据库表名数组，默认空，生成所有表</param>
        /// <param name="isMuti"></param>
        /// <returns></returns>
        public static bool CreateModels(SqlSugarClient sqlSugarClient, string ConnId, bool isMuti = false, string[] tableNames = null)
        {

            try
            {
                Create_Model_ClassFileByDBTalbe(sqlSugarClient, ConnId, $@"C:\my-file\Blog.Core.Model", "Blog.Core.Model.Models", tableNames, "", isMuti);
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
        /// <param name="sqlSugarClient">sqlsugar实例</param>
        /// <param name="ConnId">数据库链接ID</param>
        /// <param name="isMuti"></param>
        /// <param name="tableNames">数据库表名数组，默认空，生成所有表</param>
        /// <returns></returns>
        public static bool CreateIRepositorys(SqlSugarClient sqlSugarClient, string ConnId, bool isMuti = false, string[] tableNames = null)
        {

            try
            {
                Create_IRepository_ClassFileByDBTalbe(sqlSugarClient, ConnId, $@"C:\my-file\Blog.Core.IRepository", "Blog.Core.IRepository", tableNames, "", isMuti);
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
        /// <param name="sqlSugarClient">sqlsugar实例</param>
        /// <param name="ConnId">数据库链接ID</param>
        /// <param name="isMuti"></param>
        /// <param name="tableNames">数据库表名数组，默认空，生成所有表</param>
        /// <returns></returns>
        public static bool CreateIServices(SqlSugarClient sqlSugarClient, string ConnId, bool isMuti = false, string[] tableNames = null)
        {

            try
            {
                Create_IServices_ClassFileByDBTalbe(sqlSugarClient, ConnId, $@"C:\my-file\Blog.Core.IServices", "Blog.Core.IServices", tableNames, "", isMuti);
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
        /// <param name="sqlSugarClient">sqlsugar实例</param>
        /// <param name="ConnId">数据库链接ID</param>
        /// <param name="isMuti"></param>
        /// <param name="tableNames">数据库表名数组，默认空，生成所有表</param>
        /// <returns></returns>
        public static bool CreateRepository(SqlSugarClient sqlSugarClient, string ConnId, bool isMuti = false, string[] tableNames = null)
        {

            try
            {
                Create_Repository_ClassFileByDBTalbe(sqlSugarClient, ConnId, $@"C:\my-file\Blog.Core.Repository", "Blog.Core.Repository", tableNames, "", isMuti);
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }



        /// <summary>
        /// 生成 Service 层
        /// </summary>
        /// <param name="sqlSugarClient">sqlsugar实例</param>
        /// <param name="ConnId">数据库链接ID</param>
        /// <param name="isMuti"></param>
        /// <param name="tableNames">数据库表名数组，默认空，生成所有表</param>
        /// <returns></returns>
        public static bool CreateServices(SqlSugarClient sqlSugarClient, string ConnId, bool isMuti = false, string[] tableNames = null)
        {

            try
            {
                Create_Services_ClassFileByDBTalbe(sqlSugarClient, ConnId, $@"C:\my-file\Blog.Core.Services", "Blog.Core.Services", tableNames, "", isMuti);
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }


        #region 根据数据库表生产Controller层

        /// <summary>
        /// 功能描述:根据数据库表生产Controller层
        /// 作　　者:Blog.Core
        /// </summary>
        /// <param name="sqlSugarClient"></param>
        /// <param name="ConnId">数据库链接ID</param>
        /// <param name="strPath">实体类存放路径</param>
        /// <param name="strNameSpace">命名空间</param>
        /// <param name="lstTableNames">生产指定的表</param>
        /// <param name="strInterface">实现接口</param>
        /// <param name="isMuti"></param>
        /// <param name="blnSerializable">是否序列化</param>
        private static void Create_Controller_ClassFileByDBTalbe(
          SqlSugarClient sqlSugarClient,
          string ConnId,
          string strPath,
          string strNameSpace,
          string[] lstTableNames,
          string strInterface,
          bool isMuti = false,
          bool blnSerializable = false)
        {
            var IDbFirst = sqlSugarClient.DbFirst;
            if (lstTableNames != null && lstTableNames.Length > 0)
            {
                IDbFirst = IDbFirst.Where(lstTableNames);
            }
            var ls = IDbFirst.IsCreateDefaultValue().IsCreateAttribute()

                 .SettingClassTemplate(p => p =
@"using Blog.Core.IServices;
using Blog.Core.Model;
using Blog.Core.Model.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace " + strNameSpace + @"
{
	[Route(""api/[controller]/[action]"")]
	[ApiController]
    [Authorize(Permissions.Name)]
     public class {ClassName}Controller : ControllerBase
        {
             /// <summary>
             /// 服务器接口，因为是模板生成，所以首字母是大写的，自己可以重构下
             /// </summary>
            private readonly I{ClassName}Services _{ClassName}Services;
    
            public {ClassName}Controller(I{ClassName}Services {ClassName}Services)
            {
                _{ClassName}Services = {ClassName}Services;
            }
    
            [HttpGet]
            public async Task<MessageModel<PageModel<{ClassName}>>> Get(int page = 1, string key = """",int intPageSize = 50)
            {
                if (string.IsNullOrEmpty(key) || string.IsNullOrWhiteSpace(key))
                {
                    key = """";
                }
    
                Expression<Func<{ClassName}, bool>> whereExpression = a => a.id > 0;
    
                return new MessageModel<PageModel<{ClassName}>>()
                {
                    msg = ""获取成功"",
                    success = true,
                    response = await _{ClassName}Services.QueryPage(whereExpression, page, intPageSize)
                };

    }

    [HttpGet(""{id}"")]
    public async Task<MessageModel<{ClassName}>> Get(int id = 0)
    {
        return new MessageModel<{ClassName}>()
        {
            msg = ""获取成功"",
            success = true,
            response = await _{ClassName}Services.QueryById(id)
        };
    }

    [HttpPost]
    public async Task<MessageModel<string>> Post([FromBody] {ClassName} request)
    {
        var data = new MessageModel<string>();

        var id = await _{ClassName}Services.Add(request);
        data.success = id > 0;

        if (data.success)
        {
            data.response = id.ObjToString();
            data.msg = ""添加成功"";
        }

        return data;
    }

    [HttpPut]
    public async Task<MessageModel<string>> Put([FromBody] {ClassName} request)
    {
        var data = new MessageModel<string>();
        if (request.id > 0)
        {
            data.success = await _{ClassName}Services.Update(request);
            if (data.success)
            {
                data.msg = ""更新成功"";
                data.response = request?.id.ObjToString();
            }
        }

        return data;
    }

    [HttpDelete(""{id}"")]
    public async Task<MessageModel<string>> Delete(int id = 0)
    {
        var data = new MessageModel<string>();
        if (id > 0)
        {
            var detail = await _{ClassName}Services.QueryById(id);

            detail.IsDeleted = true;

                if (detail != null)
                {
                    data.success = await _{ClassName}Services.Update(detail);
                    if (data.success)
                    {
                        data.msg = ""删除成功"";
                        data.response = detail?.id.ObjToString();
                    }
                }
        }

        return data;
    }
}
}")

                  .ToClassStringList(strNameSpace);

            Dictionary<string, string> newdic = new Dictionary<string, string>();
            //循环处理 首字母小写 并插入新的 Dictionary
            foreach (KeyValuePair<string, string> item in ls)
            {
                string newkey = "_" + item.Key.First().ToString().ToLower() + item.Key.Substring(1);
                string newvalue = item.Value.Replace("_" + item.Key, newkey);
                newdic.Add(item.Key, newvalue);
            }
            CreateFilesByClassStringList(newdic, strPath, "{0}Controller");
        }
        #endregion


        #region 根据数据库表生产Model层

        /// <summary>
        /// 功能描述:根据数据库表生产Model层
        /// 作　　者:Blog.Core
        /// </summary>
        /// <param name="sqlSugarClient"></param>
        /// <param name="ConnId">数据库链接ID</param>
        /// <param name="strPath">实体类存放路径</param>
        /// <param name="strNameSpace">命名空间</param>
        /// <param name="lstTableNames">生产指定的表</param>
        /// <param name="strInterface">实现接口</param>
        /// <param name="isMuti"></param>
        /// <param name="blnSerializable">是否序列化</param>
        private static void Create_Model_ClassFileByDBTalbe(
          SqlSugarClient sqlSugarClient,
          string ConnId,
          string strPath,
          string strNameSpace,
          string[] lstTableNames,
          string strInterface,
          bool isMuti = false,
          bool blnSerializable = false)
        {
            //多库文件分离
            if (isMuti)
            {
                strPath = strPath + @"\Models\" + ConnId;
                strNameSpace = strNameSpace + "." + ConnId;
            }

            var IDbFirst = sqlSugarClient.DbFirst;
            if (lstTableNames != null && lstTableNames.Length > 0)
            {
                IDbFirst = IDbFirst.Where(lstTableNames);
            }
            var ls = IDbFirst.IsCreateDefaultValue().IsCreateAttribute()

                  .SettingClassTemplate(p => p =
@"{using}

namespace " + strNameSpace + @"
{
{ClassDescription}
    [SugarTable( ""{ClassName}"", """ + ConnId + @""")]" + (blnSerializable ? "\n    [Serializable]" : "") + @"
    public class {ClassName}" + (string.IsNullOrEmpty(strInterface) ? "" : (" : " + strInterface)) + @"
    {
        public {ClassName}()
        {
        }
        {PropertyName}
    }
}")
                  .SettingPropertyDescriptionTemplate(p => p = string.Empty)
                  .SettingPropertyTemplate(p => p =
@"{SugarColumn}
           public {PropertyType} {PropertyName} { get; set; }")

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
        /// <param name="sqlSugarClient"></param>
        /// <param name="ConnId">数据库链接ID</param>
        /// <param name="strPath">实体类存放路径</param>
        /// <param name="strNameSpace">命名空间</param>
        /// <param name="lstTableNames">生产指定的表</param>
        /// <param name="strInterface">实现接口</param>
        /// <param name="isMuti"></param>
        private static void Create_IRepository_ClassFileByDBTalbe(
          SqlSugarClient sqlSugarClient,
          string ConnId,
          string strPath,
          string strNameSpace,
          string[] lstTableNames,
          string strInterface,
          bool isMuti = false
            )
        {
            //多库文件分离
            if (isMuti)
            {
                strPath = strPath + @"\" + ConnId;
                strNameSpace = strNameSpace + "." + ConnId;
            }

            var IDbFirst = sqlSugarClient.DbFirst;
            if (lstTableNames != null && lstTableNames.Length > 0)
            {
                IDbFirst = IDbFirst.Where(lstTableNames);
            }
            var ls = IDbFirst.IsCreateDefaultValue().IsCreateAttribute()

                 .SettingClassTemplate(p => p =
@"using Blog.Core.IRepository.Base;
using Blog.Core.Model.Models" + (isMuti ? "." + ConnId + "" : "") + @";

namespace " + strNameSpace + @"
{
	/// <summary>
	/// I{ClassName}Repository
	/// </summary>	
    public interface I{ClassName}Repository : IBaseRepository<{ClassName}>" + (string.IsNullOrEmpty(strInterface) ? "" : (" , " + strInterface)) + @"
    {
    }
}")

                  .ToClassStringList(strNameSpace);
            CreateFilesByClassStringList(ls, strPath, "I{0}Repository");
        }
        #endregion


        #region 根据数据库表生产IServices层

        /// <summary>
        /// 功能描述:根据数据库表生产IServices层
        /// 作　　者:Blog.Core
        /// </summary>
        /// <param name="sqlSugarClient"></param>
        /// <param name="ConnId">数据库链接ID</param>
        /// <param name="strPath">实体类存放路径</param>
        /// <param name="strNameSpace">命名空间</param>
        /// <param name="lstTableNames">生产指定的表</param>
        /// <param name="strInterface">实现接口</param>
        /// <param name="isMuti"></param>
        private static void Create_IServices_ClassFileByDBTalbe(
          SqlSugarClient sqlSugarClient,
          string ConnId,
          string strPath,
          string strNameSpace,
          string[] lstTableNames,
          string strInterface,
          bool isMuti = false)
        {
            //多库文件分离
            if (isMuti)
            {
                strPath = strPath + @"\" + ConnId;
                strNameSpace = strNameSpace + "." + ConnId;
            }

            var IDbFirst = sqlSugarClient.DbFirst;
            if (lstTableNames != null && lstTableNames.Length > 0)
            {
                IDbFirst = IDbFirst.Where(lstTableNames);
            }
            var ls = IDbFirst.IsCreateDefaultValue().IsCreateAttribute()

                  .SettingClassTemplate(p => p =
@"using Blog.Core.IServices.BASE;
using Blog.Core.Model.Models" + (isMuti ? "." + ConnId + "" : "") + @";

namespace " + strNameSpace + @"
{	
	/// <summary>
	/// I{ClassName}Services
	/// </summary>	
    public interface I{ClassName}Services :IBaseServices<{ClassName}>" + (string.IsNullOrEmpty(strInterface) ? "" : (" , " + strInterface)) + @"
	{
    }
}")

                   .ToClassStringList(strNameSpace);
            CreateFilesByClassStringList(ls, strPath, "I{0}Services");
        }
        #endregion



        #region 根据数据库表生产 Repository 层

        /// <summary>
        /// 功能描述:根据数据库表生产 Repository 层
        /// 作　　者:Blog.Core
        /// </summary>
        /// <param name="sqlSugarClient"></param>
        /// <param name="ConnId">数据库链接ID</param>
        /// <param name="strPath">实体类存放路径</param>
        /// <param name="strNameSpace">命名空间</param>
        /// <param name="lstTableNames">生产指定的表</param>
        /// <param name="strInterface">实现接口</param>
        /// <param name="isMuti"></param>
        private static void Create_Repository_ClassFileByDBTalbe(
          SqlSugarClient sqlSugarClient,
          string ConnId,
          string strPath,
          string strNameSpace,
          string[] lstTableNames,
          string strInterface,
          bool isMuti = false)
        {
            //多库文件分离
            if (isMuti)
            {
                strPath = strPath + @"\" + ConnId;
                strNameSpace = strNameSpace + "." + ConnId;
            }

            var IDbFirst = sqlSugarClient.DbFirst;
            if (lstTableNames != null && lstTableNames.Length > 0)
            {
                IDbFirst = IDbFirst.Where(lstTableNames);
            }
            var ls = IDbFirst.IsCreateDefaultValue().IsCreateAttribute()

                  .SettingClassTemplate(p => p =
@"using Blog.Core.IRepository" + (isMuti ? "." + ConnId + "" : "") + @";
using Blog.Core.IRepository.UnitOfWork;
using Blog.Core.Model.Models" + (isMuti ? "." + ConnId + "" : "") + @";
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
}")
                  .ToClassStringList(strNameSpace);


            CreateFilesByClassStringList(ls, strPath, "{0}Repository");
        }
        #endregion


        #region 根据数据库表生产 Services 层

        /// <summary>
        /// 功能描述:根据数据库表生产 Services 层
        /// 作　　者:Blog.Core
        /// </summary>
        /// <param name="sqlSugarClient"></param>
        /// <param name="ConnId">数据库链接ID</param>
        /// <param name="strPath">实体类存放路径</param>
        /// <param name="strNameSpace">命名空间</param>
        /// <param name="lstTableNames">生产指定的表</param>
        /// <param name="strInterface">实现接口</param>
        /// <param name="isMuti"></param>
        private static void Create_Services_ClassFileByDBTalbe(
          SqlSugarClient sqlSugarClient,
          string ConnId,
          string strPath,
          string strNameSpace,
          string[] lstTableNames,
          string strInterface,
          bool isMuti = false)
        {
            //多库文件分离
            if (isMuti)
            {
                strPath = strPath + @"\" + ConnId;
                strNameSpace = strNameSpace + "." + ConnId;
            }

            var IDbFirst = sqlSugarClient.DbFirst;
            if (lstTableNames != null && lstTableNames.Length > 0)
            {
                IDbFirst = IDbFirst.Where(lstTableNames);
            }
            var ls = IDbFirst.IsCreateDefaultValue().IsCreateAttribute()

                  .SettingClassTemplate(p => p =
@"
using Blog.Core.IServices" + (isMuti ? "." + ConnId + "" : "") + @";
using Blog.Core.Model.Models" + (isMuti ? "." + ConnId + "" : "") + @";
using Blog.Core.Services.BASE;
using Blog.Core.IRepository.Base;

namespace " + strNameSpace + @"
{
    public class {ClassName}Services : BaseServices<{ClassName}>, I{ClassName}Services" + (string.IsNullOrEmpty(strInterface) ? "" : (" , " + strInterface)) + @"
    {
        private readonly IBaseRepository<{ClassName}> _dal;
        public {ClassName}Services(IBaseRepository<{ClassName}> dal)
        {
            this._dal = dal;
            base.BaseDal = dal;
        }
    }
}")
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
        private static void CreateFilesByClassStringList(Dictionary<string, string> ls, string strPath, string fileNameTp)
        {

            foreach (var item in ls)
            {
                var fileName = $"{string.Format(fileNameTp, item.Key)}.cs";
                var fileFullPath = Path.Combine(strPath, fileName);
                if (!Directory.Exists(strPath))
                {
                    Directory.CreateDirectory(strPath);
                }
                File.WriteAllText(fileFullPath, item.Value, Encoding.UTF8);
            }
        }
        #endregion
    }
}
