using Blog.Core.Common;
using Blog.Core.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;

namespace Blog.Core.Api.Controllers.Test;

[Route("api/[Controller]/[Action]")]
[AllowAnonymous]
public class SqlsugarTestController : BaseApiController
{
    private readonly SqlSugarScope _db;

    public SqlsugarTestController(SqlSugarScope db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        Console.WriteLine(App.HttpContext.Request.Path);
        Console.WriteLine(App.HttpContext.RequestServices.ToString());
        Console.WriteLine(App.User?.ID);
        await Task.CompletedTask;
        return Ok();
    }
}