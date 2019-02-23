using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog.Core.IServices;
using Blog.Core.Model;
using Blog.Core.Model.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Core.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize("Permission")]
    public class ModuleController : ControllerBase
    {
        IModuleServices _ModuleServices;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="ModuleServices"></param>
        public ModuleController(IModuleServices ModuleServices )
        {
            _ModuleServices = ModuleServices;
        }

        // GET: api/User
        [HttpGet]
        public async Task<MessageModel<PageModel<Module>>> Get(int page = 1, string key = "")
        {
            var data = new MessageModel<PageModel<Module>>();
            int intTotalCount = 50;
            int TotalCount = 0;
            int PageCount = 1;
            List<Module> Modules = new List<Module>();

            Modules = await _ModuleServices.Query(a => a.IsDeleted != true );

            if (!string.IsNullOrEmpty(key))
            {
                Modules = Modules.Where(t => (t.Name != null && t.Name.Contains(key))).ToList();
            }


            //筛选后的数据总数
            TotalCount = Modules.Count;
            //筛选后的总页数
            PageCount = (Math.Ceiling(TotalCount.ObjToDecimal() / intTotalCount.ObjToDecimal())).ObjToInt();

            Modules = Modules.OrderByDescending(d => d.Id).Skip((page - 1) * intTotalCount).Take(intTotalCount).ToList();

            return new MessageModel<PageModel<Module>>()
            {
                msg = "获取成功",
                success = TotalCount >= 0,
                response = new PageModel<Module>()
                {
                    page = page,
                    pageCount = PageCount,
                    dataCount = TotalCount,
                    data = Modules,
                }
            };

        }

        // GET: api/User/5
        [HttpGet("{id}")]
        public string Get(string id)
        {
            return "value";
        }

        // POST: api/User
        [HttpPost]
        public async Task<MessageModel<string>> Post([FromBody] Module Module)
        {
            var data = new MessageModel<string>();

            var id = (await _ModuleServices.Add(Module));
            data.success = id > 0;
            if (data.success)
            {
                data.response = id.ObjToString();
                data.msg = "添加成功";
            }

            return data;
        }

        // PUT: api/User/5
        [HttpPut]
        public async Task<MessageModel<string>> Put([FromBody] Module Module)
        {
            var data = new MessageModel<string>();
            if (Module != null && Module.Id > 0)
            {
                data.success = await _ModuleServices.Update(Module);
                if (data.success)
                {
                    data.msg = "更新成功";
                    data.response = Module?.Id.ObjToString();
                }
            }

            return data;
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete]
        public async Task<MessageModel<string>> Delete(int id)
        {
            var data = new MessageModel<string>();
            if (id > 0)
            {
                var userDetail = await _ModuleServices.QueryByID(id);
                userDetail.IsDeleted = true;
                data.success = await _ModuleServices.Update(userDetail);
                if (data.success)
                {
                    data.msg = "删除成功";
                    data.response = userDetail?.Id.ObjToString();
                }
            }

            return data;
        }
    }
}
