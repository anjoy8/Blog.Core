using Blog.Core.Common;
using Blog.Core.Common.DB;
using Blog.Core.Model;
using Blog.Core.Model.Seed;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using SqlSugar;
using System.Linq;

namespace Blog.Core.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    //[Authorize(Permissions.Name)]
    public class DbFirstController : ControllerBase
    {
        private readonly SqlSugarClient _sqlSugarClient;
        private readonly IWebHostEnvironment Env;

        /// <summary>
        /// 构造函数
        /// </summary>
        public DbFirstController(ISqlSugarClient sqlSugarClient, IWebHostEnvironment env)
        {
            _sqlSugarClient = sqlSugarClient as SqlSugarClient;
            Env = env;
        }

        /// <summary>
        /// 获取 整体框架 文件
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public MessageModel<string> GetFrameFiles()
        {
            var data = new MessageModel<string>() { success = true, msg = "" };
            data.response += @"file path is:C:\my-file\}";
            var isMuti = Appsettings.app(new string[] { "MutiDBEnabled" }).ObjToBool();
            if (Env.IsDevelopment())
            {
                data.response += $"Controller层生成：{FrameSeed.CreateControllers(_sqlSugarClient)} || ";

                BaseDBConfig.MutiConnectionString.Item1.ToList().ForEach(m =>
                {
                    _sqlSugarClient.ChangeDatabase(m.ConnId.ToLower());
                    data.response += $"库{m.ConnId}-Model层生成：{FrameSeed.CreateModels(_sqlSugarClient, m.ConnId, isMuti)} || ";
                    data.response += $"库{m.ConnId}-IRepositorys层生成：{FrameSeed.CreateIRepositorys(_sqlSugarClient, m.ConnId, isMuti)} || ";
                    data.response += $"库{m.ConnId}-IServices层生成：{FrameSeed.CreateIServices(_sqlSugarClient, m.ConnId, isMuti)} || ";
                    data.response += $"库{m.ConnId}-Repository层生成：{FrameSeed.CreateRepository(_sqlSugarClient, m.ConnId, isMuti)} || ";
                    data.response += $"库{m.ConnId}-Services层生成：{FrameSeed.CreateServices(_sqlSugarClient, m.ConnId, isMuti)} || ";
                });

                // 切回主库
                _sqlSugarClient.ChangeDatabase(MainDb.CurrentDbConnId.ToLower());
            }
            else
            {
                data.success = false;
                data.msg = "当前不处于开发模式，代码生成不可用！";
            }

            return data;
        }

        /// <summary>
        /// 根据数据库表名 生成整体框架
        /// 仅针对通过CodeFirst生成表的情况
        /// </summary>
        /// <param name="ConnID">数据库链接名称</param>
        /// <param name="tableNames">需要生成的表名</param>
        /// <returns></returns>
        [HttpPost]
        public MessageModel<string> GetFrameFilesByTableNames([FromBody]string[] tableNames, [FromQuery]string ConnID = null)
        {
            ConnID = ConnID == null ? MainDb.CurrentDbConnId.ToLower() : ConnID;

            var isMuti = Appsettings.app(new string[] { "MutiDBEnabled" }).ObjToBool();
            var data = new MessageModel<string>() { success = true, msg = "" };
            if (Env.IsDevelopment())
            {

                data.response += $"库{ConnID}-IRepositorys层生成：{FrameSeed.CreateIRepositorys(_sqlSugarClient, ConnID, isMuti, tableNames)} || ";
                data.response += $"库{ConnID}-IServices层生成：{FrameSeed.CreateIServices(_sqlSugarClient, ConnID, isMuti, tableNames)} || ";
                data.response += $"库{ConnID}-Repository层生成：{FrameSeed.CreateRepository(_sqlSugarClient, ConnID, isMuti, tableNames)} || ";
                data.response += $"库{ConnID}-Services层生成：{FrameSeed.CreateServices(_sqlSugarClient, ConnID, isMuti, tableNames)} || ";
            }
            else
            {
                data.success = false;
                data.msg = "当前不处于开发模式，代码生成不可用！";
            }

            return data;
        }

        /// <summary>
        /// DbFrist 根据数据库表名 生成整体框架,包含Model层
        /// </summary>
        /// <param name="ConnID">数据库链接名称</param>
        /// <param name="tableNames">需要生成的表名</param>
        /// <returns></returns>
        [HttpPost]
        public MessageModel<string> GetAllFrameFilesByTableNames([FromBody]string[] tableNames, [FromQuery]string ConnID = null)
        {
            ConnID = ConnID == null ? MainDb.CurrentDbConnId.ToLower() : ConnID;

            var isMuti = Appsettings.app(new string[] { "MutiDBEnabled" }).ObjToBool();
            var data = new MessageModel<string>() { success = true, msg = "" };
            if (Env.IsDevelopment())
            {
                _sqlSugarClient.ChangeDatabase(ConnID.ToLower());
                data.response += $"Controller层生成：{FrameSeed.CreateControllers(_sqlSugarClient, ConnID, isMuti, tableNames)} || ";
                data.response += $"库{ConnID}-Model层生成：{FrameSeed.CreateModels(_sqlSugarClient, ConnID, isMuti, tableNames)} || ";
                data.response += $"库{ConnID}-IRepositorys层生成：{FrameSeed.CreateIRepositorys(_sqlSugarClient, ConnID, isMuti, tableNames)} || ";
                data.response += $"库{ConnID}-IServices层生成：{FrameSeed.CreateIServices(_sqlSugarClient, ConnID, isMuti, tableNames)} || ";
                data.response += $"库{ConnID}-Repository层生成：{FrameSeed.CreateRepository(_sqlSugarClient, ConnID, isMuti, tableNames)} || ";
                data.response += $"库{ConnID}-Services层生成：{FrameSeed.CreateServices(_sqlSugarClient, ConnID, isMuti, tableNames)} || ";
                // 切回主库
                _sqlSugarClient.ChangeDatabase(MainDb.CurrentDbConnId.ToLower());
            }
            else
            {
                data.success = false;
                data.msg = "当前不处于开发模式，代码生成不可用！";
            }

            return data;
        }


    }
}