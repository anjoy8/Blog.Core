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
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize("Permission")]
    public class RoleController : ControllerBase
    {
        IRoleServices _roleServices;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="roleServices"></param>
        public RoleController(IRoleServices roleServices )
        {
            _roleServices = roleServices;
        }

        // GET: api/User
        [HttpGet]
        public async Task<MessageModel<PageModel<Role>>> Get(int page = 1, string key = "")
        {
            var data = new MessageModel<PageModel<Role>>();
            int intTotalCount = 100;
            int TotalCount = 0;
            int PageCount = 1;
            List<Role> Roles = new List<Role>();

            Roles = await _roleServices.Query(a => a.IsDeleted != true );

            if (!string.IsNullOrEmpty(key))
            {
                Roles = Roles.Where(t => (t.Name != null && t.Name.Contains(key))).ToList();
            }


            //筛选后的数据总数
            TotalCount = Roles.Count;
            //筛选后的总页数
            PageCount = (Math.Ceiling(TotalCount.ObjToDecimal() / intTotalCount.ObjToDecimal())).ObjToInt();

            Roles = Roles.OrderByDescending(d => d.Id).Skip((page - 1) * intTotalCount).Take(intTotalCount).ToList();

            return new MessageModel<PageModel<Role>>()
            {
                msg = "获取成功",
                success = TotalCount >= 0,
                response = new PageModel<Role>()
                {
                    page = page,
                    pageCount = PageCount,
                    dataCount = TotalCount,
                    data = Roles,
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
        public async Task<MessageModel<string>> Post([FromBody] Role Role)
        {
            var data = new MessageModel<string>();

            var id = (await _roleServices.Add(Role));
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
        public async Task<MessageModel<string>> Put([FromBody] Role Role)
        {
            var data = new MessageModel<string>();
            if (Role != null && Role.Id > 0)
            {
                data.success = await _roleServices.Update(Role);
                if (data.success)
                {
                    data.msg = "更新成功";
                    data.response = Role?.Id.ObjToString();
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
                var userDetail = await _roleServices.QueryByID(id);
                userDetail.IsDeleted = true;
                data.success = await _roleServices.Update(userDetail);
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
