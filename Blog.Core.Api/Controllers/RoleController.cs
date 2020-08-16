using System.Collections.Generic;
using System.Threading.Tasks;
using Blog.Core.Common.HttpContextUser;
using Blog.Core.IServices;
using Blog.Core.Model;
using Blog.Core.Model.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Blog.Core.Common.Helper;

namespace Blog.Core.Controllers
{
    /// <summary>
    /// 角色管理
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize(Permissions.Name)]
    public class RoleController : ControllerBase
    {
        readonly IRoleServices _roleServices;
        readonly IUser _user;

     
        public RoleController(IRoleServices roleServices, IUser user)
        {
            _roleServices = roleServices;
            _user = user;
        }

        /// <summary>
        /// 获取全部角色
        /// </summary>
        /// <param name="page"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        // GET: api/User
        [HttpGet]
        public async Task<MessageModel<PageModel<Role>>> Get(int page = 1, int f = 0, string key = "")
        {
            if (string.IsNullOrEmpty(key) || string.IsNullOrWhiteSpace(key))
            {
                key = "";
            }

            int intPageSize = 50;

            //var roleList = await _roleServices.QueryPage(a => a.IsDeleted != true && (a.Name != null && a.Name.Contains(key)), page, intPageSize, " Id desc ");
            PageModel<Role> roles;
            if (key == "")
            {
                roles = await _roleServices.QueryPage(a => a.IsDeleted != true 
                  && a.Pid == f, page, intPageSize,
                    " Id desc ");
            }
            else
            {
                roles = await _roleServices.QueryPage(a => a.IsDeleted != true 
                    && (a.Name != null && a.Name.Contains(key)), page, intPageSize,
                    " Id desc ");
            }

            foreach (var item in roles.data)
            {
                List<int> pidarr = new List<int> { };
                var parent = await _roleServices.QueryById(item.Pid);

                while (parent != null)
                {
                    pidarr.Add(parent.Id);
                    parent = await _roleServices.QueryById(parent.Pid);
                }

                pidarr.Reverse();
                pidarr.Insert(0, 0);
                item.PidArr = pidarr;
                item.hasChildren = await _roleServices.ExistsChild(item.Id);
            }

            return new MessageModel<PageModel<Role>>()
            {
                msg = "获取成功",
                success = roles.dataCount >= 0,
                response = roles
            };
        }

        // GET: api/User/5
        [HttpGet("{id}")]
        public string Get(string id)
        {
            return "value";
        }

        /// <summary>
        /// 添加角色
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        // POST: api/User
        [HttpPost]
        public async Task<MessageModel<string>> Post([FromBody] Role role)
        {
            var data = new MessageModel<string>();

            role.CreateId = _user.ID;
            role.CreateBy = _user.Name;

            var id = (await _roleServices.Add(role));
            data.success = id > 0;
            if (data.success)
            {
                data.response = id.ObjToString();
                data.msg = "添加成功";
            }

            return data;
        }

        /// <summary>
        /// 更新角色
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        // PUT: api/User/5
        [HttpPut]
        public async Task<MessageModel<string>> Put([FromBody] Role role)
        {
            var data = new MessageModel<string>();
            if (role != null && role.Id > 0)
            {
                data.success = await _roleServices.Update(role);
                if (data.success)
                {
                    data.msg = "更新成功";
                    data.response = role?.Id.ObjToString();
                }
            }

            return data;
        }

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // DELETE: api/ApiWithActions/5
        [HttpDelete]
        public async Task<MessageModel<string>> Delete(int id)
        {
            var data = new MessageModel<string>();
            if (id > 0)
            {
                var userDetail = await _roleServices.QueryById(id);
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
