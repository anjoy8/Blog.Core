using Blog.Core.Common.HttpContextUser;
using Blog.Core.Controllers;
using Blog.Core.IServices.BASE;
using Blog.Core.Model;
using Blog.Core.Model.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Core.Api.Controllers.Tenant;

/// <summary>
/// 多租户-Id方案 测试
/// </summary>
[Produces("application/json")]
[Route("api/Tenant/ById")]
[Authorize]
public class TenantByIdController : BaseApiController
{
    private readonly IBaseServices<BusinessTable> _services;
    private readonly IUser _user;

    public TenantByIdController(IUser user, IBaseServices<BusinessTable> services)
    {
        _user = user;
        _services = services;
    }

    /// <summary>
    /// 获取租户下全部业务数据 <br/>
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<MessageModel<List<BusinessTable>>> GetAll()
    {
        var data = await _services.Query();
        return Success(data);
    }

    /// <summary>
    /// 新增业务数据
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    public async Task<MessageModel> Post([FromBody] BusinessTable data)
    {
        await _services.Db.Insertable(data).ExecuteReturnSnowflakeIdAsync();
        return Success();
    }
}