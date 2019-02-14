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
    [Authorize("Permission")]
    public class UserController : ControllerBase
    {
        IsysUserInfoServices _sysUserInfoServices;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="sysUserInfoServices"></param>
        public UserController(IsysUserInfoServices sysUserInfoServices)
        {
            _sysUserInfoServices = sysUserInfoServices;
        }

        // GET: api/User
        [HttpGet]
        public async Task<MessageModel<PageModel<sysUserInfo>>> Get(int page = 1, string key = "")
        {
            var data = new MessageModel<PageModel<sysUserInfo>>();
            int intTotalCount = 100;
            int TotalCount = 0;
            int PageCount = 1;
            List<sysUserInfo> sysUserInfos = new List<sysUserInfo>();

            sysUserInfos = await _sysUserInfoServices.Query(a => a.tdIsDelete != true && a.uStatus >= 0);

            if (!string.IsNullOrEmpty(key))
            {
                sysUserInfos = sysUserInfos.Where(t => (t.uLoginName != null && t.uLoginName.Contains(key)) || (t.uRealName != null && t.uRealName.Contains(key))).ToList();
            }


            //筛选后的数据总数
            TotalCount = sysUserInfos.Count;
            //筛选后的总页数
            PageCount = (Math.Ceiling(TotalCount.ObjToDecimal() / intTotalCount.ObjToDecimal())).ObjToInt();

            sysUserInfos = sysUserInfos.OrderByDescending(d => d.uID).Skip((page - 1) * intTotalCount).Take(intTotalCount).ToList();

            return new MessageModel<PageModel<sysUserInfo>>()
            {
                Msg = "获取成功",
                Success = TotalCount >= 0,
                Response = new PageModel<sysUserInfo>()
                {
                    page = page,
                    pageCount = PageCount,
                    dataCount = TotalCount,
                    data = sysUserInfos,
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
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/User/5
        [HttpPut]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete]
        public async Task<MessageModel<string>> Delete(int id)
        {
            var data = new MessageModel<string>();
            if (id > 0)
            {
                var userDetail = await _sysUserInfoServices.QueryByID(id);
                userDetail.tdIsDelete = true;
                data.Success = await _sysUserInfoServices.Update(userDetail);
                if (data.Success)
                {
                    data.Msg = "删除成功";
                    data.Response = userDetail?.uID.ObjToString();
                }
            }

            return data;
        }
    }
}
