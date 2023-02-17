using Blog.Core.Common.HttpContextUser;
using Blog.Core.Controllers;
using Blog.Core.IServices.BASE;
using Blog.Core.Model;
using Blog.Core.Model.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Core.Api.Controllers.Tenant;

/// <summary>
/// 多租户测试
/// </summary>
[Produces("application/json")]
[Route("api/Tenant/ByDb")]
[Authorize]
public class TenantByDbController : BaseApiController
{
    private readonly IBaseServices<SubLibraryBusinessTable> _services;
    private readonly IUser _user;

    public TenantByDbController(IUser user, IBaseServices<SubLibraryBusinessTable> services)
    {
        _user = user;
        _services = services;
    }

    /// <summary>
    /// 获取租户下全部业务数据 <br/>
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<MessageModel<List<SubLibraryBusinessTable>>> GetAll()
    {
        var data = await _services.Query();
        return Success(data);
    }
}