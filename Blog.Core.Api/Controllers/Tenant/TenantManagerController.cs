using Blog.Core.Controllers;
using Blog.Core.IServices;
using Blog.Core.Model;
using Blog.Core.Model.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Core.Api.Controllers.Tenant;

/// <summary>
/// 租户管理
/// </summary>
[Produces("application/json")]
[Route("api/TenantManager")]
[Authorize]
public class TenantManagerController : BaseApiController
{
    private readonly ITenantService _services;

    public TenantManagerController(ITenantService services)
    {
        _services = services;
    }


    /// <summary>
    /// 获取全部租户
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<MessageModel<List<SysTenant>>> GetAll()
    {
        var data = await _services.Query();
        return Success(data);
    }


    /// <summary>
    /// 获取租户信息
    /// </summary>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<MessageModel<SysTenant>> GetInfo(long id)
    {
        var data = await _services.QueryById(id);
        return Success(data);
    }

    /// <summary>
    /// 新增租户信息 <br/>
    /// 此处只做演示，具体要以实际业务为准
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    public async Task<MessageModel> Post(SysTenant tenant)
    {
        await _services.SaveTenant(tenant);
        return Success();
    }

    /// <summary>
    /// 修改租户信息 <br/>
    /// 此处只做演示，具体要以实际业务为准
    /// </summary>
    /// <returns></returns>
    [HttpPut]
    public async Task<MessageModel> Put(SysTenant tenant)
    {
        await _services.SaveTenant(tenant);
        return Success();
    }

    /// <summary>
    /// 删除租户 <br/>
    /// 此处只做演示，具体要以实际业务为准
    /// </summary>
    /// <returns></returns>
    [HttpDelete]
    public async Task<MessageModel> Delete(long id)
    {
        //是否删除租户库?
        //要根据实际情况而定
        //例如直接删除租户库、备份租户库到xx
        await _services.DeleteById(id);
        return Success();
    }
}