using Blog.Core.Common.Caches;
using Blog.Core.Common.Caches.Interface;
using Blog.Core.Controllers;
using Blog.Core.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Core.Api.Controllers.Systems;

/// <summary>
/// 缓存管理
/// </summary>
[Route("api/Systems/[controller]")]
[ApiController]
[Authorize(Permissions.Name)]
public class CacheManageController(ICaching caching) : BaseApiController
{
    /// <summary>
    /// 获取全部缓存
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public MessageModel<List<string>> Get()
    {
        return Success(caching.GetAllCacheKeys());
    }

    /// <summary>
    /// 获取缓存
    /// </summary>
    /// <returns></returns>
    [HttpGet("{key}")]
    public async Task<MessageModel<string>> Get(string key)
    {
        return Success<string>(await caching.GetStringAsync(key));
    }

    /// <summary>
    /// 新增
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    public async Task<MessageModel> Post([FromQuery] string key, [FromQuery] string value, [FromQuery] int? expire)
    {
        if (expire.HasValue)
            await caching.SetStringAsync(key, value, TimeSpan.FromMilliseconds(expire.Value));
        else
            await caching.SetStringAsync(key, value);

        return Success();
    }

    /// <summary>
    /// 删除全部缓存
    /// </summary>
    /// <returns></returns>
    [HttpDelete]
    public MessageModel Delete()
    {
        caching.RemoveAll();
        return Success();
    }

    /// <summary>
    /// 删除缓存
    /// </summary>
    /// <returns></returns>
    [Route("{key}")]
    [HttpDelete]
    public async Task<MessageModel> Delete(string key)
    {
        await caching.RemoveAsync(key);
        return Success();
    }
}