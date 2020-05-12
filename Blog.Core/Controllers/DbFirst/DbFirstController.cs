using Blog.Core.Common.DB;
using Blog.Core.Model;
using Blog.Core.Model.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

        /// <summary>
        /// 构造函数
        /// </summary>
        public DbFirstController(ISqlSugarClient sqlSugarClient)
        {
            _sqlSugarClient = sqlSugarClient as SqlSugarClient;
        }

        /// <summary>
        /// 获取 整体框架 文件
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public MessageModel<string> GetFrameFiles()
        {
            var data = new MessageModel<string>() { success = true, msg = "" };
            BaseDBConfig.MutiConnectionString.Item1.ToList().ForEach(m =>
            {
                _sqlSugarClient.ChangeDatabase(m.ConnId.ToLower());
                data.response += $"库{m.ConnId}-Model层生成：{FrameSeed.CreateModels(_sqlSugarClient)} || ";
                data.response += $"库{m.ConnId}-IRepositorys层生成：{FrameSeed.CreateIRepositorys(_sqlSugarClient)} || ";
                data.response += $"库{m.ConnId}-IServices层生成：{FrameSeed.CreateIServices(_sqlSugarClient)} || ";
                data.response += $"库{m.ConnId}-Repository层生成：{FrameSeed.CreateRepository(_sqlSugarClient)} || ";
                data.response += $"库{m.ConnId}-Services层生成：{FrameSeed.CreateServices(_sqlSugarClient)} || ";
            });

            // 切回主库
            _sqlSugarClient.ChangeDatabase(MainDb.CurrentDbConnId.ToLower());

            return data;
        }
    }
}