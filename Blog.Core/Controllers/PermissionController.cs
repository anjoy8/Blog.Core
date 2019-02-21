using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog.Core.Common.Helper;
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
    public class PermissionController : ControllerBase
    {
        IPermissionServices _PermissionServices;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="PermissionServices"></param>
        public PermissionController(IPermissionServices PermissionServices)
        {
            _PermissionServices = PermissionServices;
        }

        // GET: api/User
        [HttpGet]
        public async Task<MessageModel<PageModel<Permission>>> Get(int page = 1, string key = "")
        {
            var data = new MessageModel<PageModel<Permission>>();
            int intTotalCount = 100;
            int TotalCount = 0;
            int PageCount = 1;
            List<Permission> Permissions = new List<Permission>();

            Permissions = await _PermissionServices.Query(a => a.IsDeleted != true);

            if (!string.IsNullOrEmpty(key))
            {
                Permissions = Permissions.Where(t => (t.Name != null && t.Name.Contains(key))).ToList();
            }


            //筛选后的数据总数
            TotalCount = Permissions.Count;
            //筛选后的总页数
            PageCount = (Math.Ceiling(TotalCount.ObjToDecimal() / intTotalCount.ObjToDecimal())).ObjToInt();

            Permissions = Permissions.OrderByDescending(d => d.Id).Skip((page - 1) * intTotalCount).Take(intTotalCount).ToList();

            foreach (var item in Permissions)
            {
                List<int> pidarr = new List<int>();
                pidarr.Add(item.Pid);
                if (item.Pid > 0)
                {
                    pidarr.Add(0);
                }
                var parent = Permissions.Where(d => d.Id == item.Pid).FirstOrDefault();

                while (parent != null)
                {
                    pidarr.Add(parent.Id);
                    parent = Permissions.Where(d => d.Id == parent.Pid).FirstOrDefault();
                }


                item.PidArr = pidarr.OrderBy(d => d).Distinct().ToList();
                foreach (var pid in item.PidArr)
                {
                    var per = Permissions.Where(d => d.Id == pid).FirstOrDefault();
                    var par = Permissions.Where(d => d.Pid == item.Id ).ToList();
                    item.PnameArr.Add((per != null ? per.Name : "根节点") + "/");
                    item.PCodeArr.Add((per != null ? $"/{per.Code}/{item.Code}" : ""));
                    if (par.Count == 0 && item.Pid == 0)
                    {
                        item.PCodeArr.Add($"/{item.Code}");
                    }
                }
            }

            return new MessageModel<PageModel<Permission>>()
            {
                msg = "获取成功",
                success = TotalCount >= 0,
                response = new PageModel<Permission>()
                {
                    page = page,
                    pageCount = PageCount,
                    dataCount = TotalCount,
                    data = Permissions,
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
        public async Task<MessageModel<string>> Post([FromBody] Permission Permission)
        {
            var data = new MessageModel<string>();

            var id = (await _PermissionServices.Add(Permission));
            data.success = id > 0;
            if (data.success)
            {
                data.response = id.ObjToString();
                data.msg = "添加成功";
            }

            return data;
        }

        // POST: api/User
        [HttpGet]
        [Route("GetPermissionTree")]
        public async Task<MessageModel<PermissionTree>> GetPermissionTree(int pid = 0)
        {
            var data = new MessageModel<PermissionTree>();

            var permissions = await _PermissionServices.Query(d => d.IsDeleted == false);
            var permissionTrees = (from child in permissions
                                   where child.IsDeleted == false
                                   orderby child.Id
                                   select new PermissionTree
                                   {
                                       value = child.Id,
                                       label = child.Name,
                                       Pid = child.Pid,
                                   }).ToList();
            PermissionTree rootRoot = new PermissionTree();
            rootRoot.value = 0;
            rootRoot.Pid = 0;
            rootRoot.label = "根节点";

            RecursionHelper.LoopToAppendChildren(permissionTrees, rootRoot, pid);

            data.success = true;
            if (data.success)
            {
                data.response = rootRoot;
                data.msg = "获取成功";
            }

            return data;
        }


        // PUT: api/User/5
        [HttpPut]
        public async Task<MessageModel<string>> Put([FromBody] Permission Permission)
        {
            var data = new MessageModel<string>();
            if (Permission != null && Permission.Id > 0)
            {
                data.success = await _PermissionServices.Update(Permission);
                if (data.success)
                {
                    data.msg = "更新成功";
                    data.response = Permission?.Id.ObjToString();
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
                var userDetail = await _PermissionServices.QueryByID(id);
                userDetail.IsDeleted = true;
                data.success = await _PermissionServices.Update(userDetail);
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
