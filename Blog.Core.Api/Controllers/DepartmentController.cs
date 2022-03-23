using Blog.Core.IServices;
using Blog.Core.Model;
using Blog.Core.Model.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Blog.Core.Api.Controllers
{
	[Route("api/[controller]/[action]")]
	[ApiController]
    [Authorize(Permissions.Name)]
     public class DepartmentController : ControllerBase
    {
            private readonly IDepartmentServices _departmentServices;
    
            public DepartmentController(IDepartmentServices departmentServices)
            {
                _departmentServices = departmentServices;
            }
    
            [HttpGet]
            public async Task<MessageModel<PageModel<Department>>> Get(int page = 1, string key = "",int intPageSize = 50)
            {
                if (string.IsNullOrEmpty(key) || string.IsNullOrWhiteSpace(key))
                {
                    key = "";
                }
    
                Expression<Func<Department, bool>> whereExpression = a => true;
    
                return new MessageModel<PageModel<Department>>()
                {
                    msg = "获取成功",
                    success = true,
                    response = await _departmentServices.QueryPage(whereExpression, page, intPageSize)
                };

            }

            [HttpGet("{id}")]
            public async Task<MessageModel<Department>> Get(string id)
            {
                return new MessageModel<Department>()
                {
                    msg = "获取成功",
                    success = true,
                    response = await _departmentServices.QueryById(id)
                };
            }

            [HttpPost]
            public async Task<MessageModel<string>> Post([FromBody] Department request)
            {
                var data = new MessageModel<string>();

                var id = await _departmentServices.Add(request);
                if (data.success)
                {
                    data.response = id.ObjToString();
                    data.msg = "添加成功";
                } 

                return data;
            }

            [HttpPut]
            public async Task<MessageModel<string>> Put([FromBody] Department request)
            {
                var data = new MessageModel<string>();
                data.success = await _departmentServices.Update(request);
                if (data.success)
                {
                    data.msg = "更新成功";
                    data.response = request?.Id.ObjToString();
                }

                return data;
            }

            [HttpDelete("{id}")]
            public async Task<MessageModel<string>> Delete(string id)
            {
                var data = new MessageModel<string>();
                data.success = await _departmentServices.DeleteById(id);
                if (data.success)
                {
                    data.msg = "删除成功";
                    data.response = id;
                }

                return data;
            }
    }
}