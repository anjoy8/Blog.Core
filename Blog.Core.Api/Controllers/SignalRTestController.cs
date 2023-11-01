using Blog.Core.Controllers;
using Blog.Core.Hubs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Blog.Core.Api.Controllers;

/// <summary>
/// SignalR测试
/// </summary>
[Route("api/[controller]/[action]")]
[ApiController]
[Authorize]
public class SignalRTestController : BaseApiController
{
    private readonly IHubContext<ChatHub, IChatClient> _hubContext;

    public SignalRTestController(IHubContext<ChatHub, IChatClient> hubContext)
    {
        _hubContext = hubContext;
    }

    /// <summary>
    /// 向指定用户发送消息
    /// </summary>
    /// <param name="user"></param>
    /// <param name="message"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> SendMessageToUser(string user, string message)
    {
        await _hubContext.Clients.Group(user).ReceiveMessage(user, message);
        return Ok();
    }

    /// <summary>
    /// 向指定角色发送消息
    /// </summary>
    /// <param name="role"></param>
    /// <param name="message"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> SendMessageToRole(string role, string message)
    {
        await _hubContext.Clients.Group(role).ReceiveMessage(role, message);
        return Ok();
    }
}