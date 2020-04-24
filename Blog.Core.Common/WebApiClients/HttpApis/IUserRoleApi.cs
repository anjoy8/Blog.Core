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
    /// 用户角色关系
    /// </summary>
    [TraceFilter]
    public interface IUserRoleApi : IHttpApi
    {
        /// <summary>
        /// 新建Role (Auth policies: Permission)
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns>Success</returns>
        [HttpGet("api/UserRole/AddRole")]
        ITask<RoleMessageModel> AddRoleAsync(string roleName);

        /// <summary>
        /// 新建用户 (Auth policies: Permission)
        /// </summary>
        /// <param name="loginName"></param>
        /// <param name="loginPwd"></param>
        /// <returns>Success</returns>
        [HttpGet("api/UserRole/AddUser")]
        ITask<SysUserInfoMessageModel> AddUserAsync(string loginName, string loginPwd);

        /// <summary>
        /// 新建用户角色关系 (Auth policies: Permission)
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="rid"></param>
        /// <returns>Success</returns>
        [HttpGet("api/UserRole/AddUserRole")]
        ITask<UserRoleMessageModel> AddUserRoleAsync(int? uid, int? rid);

    }
}