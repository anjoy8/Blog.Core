using Blog.Core.Controllers;
using Blog.Core.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;

namespace Blog.Core.Api.Controllers.Systems;

/// <summary>
/// 缓存管理
/// </summary>
[Route("api/Systems/[controller]/[action]")]
[ApiController]
[Authorize(Permissions.Name)]
public class DynamicCodeFirstController : BaseApiController
{
    private readonly ISqlSugarClient _db;

    public DynamicCodeFirstController(ISqlSugarClient db)
    {
        _db = db;
    }


    /// <summary>
    /// 测试建表
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    public MessageModel TestCreateTable()
    {
        _db.DynamicBuilder();
        return Success();
    }
}