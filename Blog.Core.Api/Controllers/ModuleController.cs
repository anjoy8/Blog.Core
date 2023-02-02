using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Blog.Core.Common.HttpContextUser;
using Blog.Core.IServices;
using Blog.Core.Model;
using Blog.Core.Model.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Core.Controllers
{
    /// <summary>
    /// 接口管理
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize(Permissions.Name)]
    public class ModuleController : BaseApiController
    {
        readonly IModuleServices _moduleServices;
        readonly IUser _user;


        public ModuleController(IModuleServices moduleServices, IUser user)
        {
            _moduleServices = moduleServices;
            _user = user;
        }

        /// <summary>
        /// 获取全部接口api
        /// </summary>
        /// <param name="page"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        // GET: api/User
        [HttpGet]
        public async Task<MessageModel<PageModel<Modules>>> Get(int page = 1, string key = "")
        {
            if (string.IsNullOrEmpty(key) || string.IsNullOrWhiteSpace(key))
            {
                key = "";
            }
            int intPageSize = 50;

            Expression<Func<Modules, bool>> whereExpression = a => a.IsDeleted != true && (a.Name != null && a.Name.Contains(key));

            PageModel<Modules> data = new PageModel<Modules>();

            if (page == -1)
            {
                var modules = await _moduleServices.Query(whereExpression, " Id desc ");
                data.data = modules;
            }
            else
            {
                data = await _moduleServices.QueryPage(whereExpression, page, intPageSize, " Id desc ");
            }


            return Success(data, "获取成功");


        }

        // GET: api/User/5
        [HttpGet("{id}")]
        public string Get(string id)
        {
            return "value";
        }

        /// <summary>
        /// 添加一条接口信息
        /// </summary>
        /// <param name="module"></param>
        /// <returns></returns>
        // POST: api/User
        [HttpPost]
        public async Task<MessageModel<string>> Post([FromBody] Modules module)
        {
            module.CreateId = _user.ID;
            module.CreateBy = _user.Name;
            var id = (await _moduleServices.Add(module));
            return id > 0 ? Success(id.ObjToString(), "添加成功") : Failed();

        }

        /// <summary>
        /// 更新接口信息
        /// </summary>
        /// <param name="module"></param>
        /// <returns></returns>
        // PUT: api/User/5
        [HttpPut]
        public async Task<MessageModel<string>> Put([FromBody] Modules module)
        {
            //var data = new MessageModel<string>();
            //if (module != null && module.Id > 0)
            //{
            //data.success = await _moduleServices.Update(module);
            //if (data.success)
            //{
            //    data.msg = "更新成功";
            //    data.response = module?.Id.ObjToString();
            //}

            // }

            //return data;
            if (module == null || module.Id <= 0)
                return Failed("缺少参数");
            return await _moduleServices.Update(module) ? Success(module?.Id.ObjToString(), "更新成功") : Failed();
        }

        /// <summary>
        /// 删除一条接口
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // DELETE: api/ApiWithActions/5
        [HttpDelete]
        public async Task<MessageModel<string>> Delete(int id)
        {
            if (id <= 0)
                return Failed("缺少参数");
            var userDetail = await _moduleServices.QueryById(id);
            if (userDetail == null)
                return Failed("信息不存在");

            userDetail.IsDeleted = true;
            return await _moduleServices.Update(userDetail) ? Success(userDetail?.Id.ObjToString(), "删除成功") : Failed("删除失败");

            //var data = new MessageModel<string>();
            //if (id > 0)
            //{
            //    var userDetail = await _moduleServices.QueryById(id);
            //    userDetail.IsDeleted = true;
            //    data.success = await _moduleServices.Update(userDetail);
            //    if (data.success)
            //    {
            //        data.msg = "删除成功";
            //        data.response = userDetail?.Id.ObjToString();
            //    }
            //}
            //return data;
        }

        /// <summary>
        /// 导入多条接口信息
        /// </summary>
        /// <param name="modules"></param>
        /// <returns></returns>
        // POST: api/User
        [HttpPost]
        public async Task<MessageModel<string>> BatchPost([FromBody] List<Modules> modules)
        {
            string ids = string.Empty;
            int sucCount = 0;

            for (int i = 0; i < modules.Count; i++)
            {
                var module = modules[i];
                if (module != null)
                {
                    module.CreateId = _user.ID;
                    module.CreateBy = _user.Name;
                    ids += (await _moduleServices.Add(module));
                    sucCount++;
                }
            }
            return ids.IsNotEmptyOrNull() ? Success(ids, $"{sucCount}条数据添加成功") : Failed();
        }
    }
}
