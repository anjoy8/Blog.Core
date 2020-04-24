using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using WebApiClient;
using WebApiClient.Attributes;
using WebApiClient.DataAnnotations;
using WebApiClient.Parameterables;
namespace Blog.Core.Common.WebApiClients
{
    /// <summary>
    /// 菜单管理
    /// </summary>
    [TraceFilter]
    public interface IPermissionApi : IHttpApi
    {
        /// <summary>
        /// 保存菜单权限分配 (Auth policies: Permission)
        /// </summary>
        /// <param name="body"></param>
        /// <returns>Success</returns>
        [HttpPost("api/Permission/Assign")]
        ITask<StringMessageModel> AssignAsync([JsonContent] AssignView body);

        /// <summary>
        /// 删除菜单 (Auth policies: Permission)
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Success</returns>
        [HttpDelete("api/Permission/Delete")]
        ITask<StringMessageModel> Delete3Async(int? id);

        /// <summary>
        /// 获取菜单 (Auth policies: Permission)
        /// </summary>
        /// <param name="page"></param>
        /// <param name="key"></param>
        /// <returns>Success</returns>
        [HttpGet("api/Permission/Get")]
        ITask<PermissionPageModelMessageModel> Get4Async(int page, string key);

        /// <summary>
        /// (Auth policies: Permission)
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Success</returns>
        [HttpGet("api/Permission/Get/{id}")]
        ITask<string> Get5Async([Required] string id);

        /// <summary>
        /// 获取路由树 (Auth policies: Permission)
        /// </summary>
        /// <param name="uid"></param>
        /// <returns>Success</returns>
        [HttpGet("api/Permission/GetNavigationBar")]
        ITask<NavigationBarMessageModel> GetNavigationBarAsync(int? uid);

        /// <summary>
        /// 通过角色获取菜单【无权限】
        /// </summary>
        /// <param name="rid"></param>
        /// <returns>Success</returns>
        [HttpGet("api/Permission/GetPermissionIdByRoleId")]
        ITask<AssignShowMessageModel> GetPermissionIdByRoleIdAsync(int rid);

        /// <summary>
        /// 获取菜单树 (Auth policies: Permission)
        /// </summary>
        /// <param name="pid"></param>
        /// <param name="needbtn"></param>
        /// <returns>Success</returns>
        [HttpGet("api/Permission/GetPermissionTree")]
        ITask<PermissionTreeMessageModel> GetPermissionTreeAsync(int pid, bool needbtn);

        /// <summary>
        /// 查询树形 Table
        /// </summary>
        /// <param name="f">父节点</param>
        /// <param name="key">关键字</param>
        /// <returns>Success</returns>
        [HttpGet("api/Permission/GetTreeTable")]
        ITask<PermissionListMessageModel> GetTreeTableAsync(int f, string key);

        /// <summary>
        /// 添加一个菜单 (Auth policies: Permission)
        /// </summary>
        /// <param name="body"></param>
        /// <returns>Success</returns>
        [HttpPost("api/Permission/Post")]
        ITask<StringMessageModel> Post2Async([JsonContent] Permission body);

        /// <summary>
        /// 更新菜单 (Auth policies: Permission)
        /// </summary>
        /// <param name="body"></param>
        /// <returns>Success</returns>
        [HttpPut("api/Permission/Put")]
        ITask<StringMessageModel> Put2Async([JsonContent] Permission body);

    }
}