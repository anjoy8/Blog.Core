using Blog.Core;
using Blog.Core.Common.Caches;
using Blog.Core.Common.Extensions;
using Blog.Core.Common.Helper;
using Blog.Core.Controllers;
using Blog.Core.IServices;
using Blog.Core.IServices.BASE;
using Blog.Core.Model;
using Blog.Core.Model.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MyDotnet.Controllers.System
{
    /// <summary>
    /// 字典管理
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class DicController : BaseApiController
    {
        IDicService _dicService;
        IBaseServices<DicType> _dicType;
        IBaseServices<DicData> _dicData;
        ICaching _caching;

        public DicController(IDicService dicService, IBaseServices<DicType> dicType, IBaseServices<DicData> dicData, ICaching caching)
        {
            _dicService = dicService;
            _dicType = dicType;
            _dicData = dicData;
            _caching = caching;
        }
        /// <summary>
        /// 获取字典类型
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<MessageModel<DicType>> GetDic(string code)
        {
            var data = await _dicService.GetDic(code);
            return Success(data);
        }
        /// <summary>
        /// 获取字典类型列表
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<MessageModel<List<DicData>>> GetDicData(string code)
        {
            var data = await _dicService.GetDicData(code);
            return Success(data);
        }











        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Permissions.Name)]
        public async Task<MessageModel<PageModel<DicType>>> Get(int page = 1, int size = 10, string key = "")
        {
            var whereFind = LinqHelper.True<DicType>();

            if (!string.IsNullOrEmpty(key))
            {
                whereFind = whereFind.And(t => t.code.Contains(key) 
                || t.name.Contains(key) 
                || t.content.Contains(key)
                || t.content2.Contains(key)
                || t.content3.Contains(key)
                || t.description.Contains(key));
            }
            var data = await _dicType.QueryPage(whereFind, page, size, " Id desc ");
            return Success(data, "获取成功");
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Permissions.Name)]
        public async Task<MessageModel<string>> Post([FromBody] DicType data)
        {
            var id = await _dicType.Add(data);
            await _caching.RemoveAsync(data.code);
            return id > 0 ? Success(id.ObjToString(), "添加成功") : Failed();

        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPut]
        [Authorize(Permissions.Name)]
        public async Task<MessageModel<string>> Put([FromBody] DicType data)
        {
            if (data == null || data.Id <= 0)
                return Failed("缺少参数");
            await _caching.RemoveAsync(data.code);
            return await _dicType.Update(data) ? Success(data.Id.ObjToString(), "更新成功") : Failed();
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Authorize(Permissions.Name)]
        public async Task<MessageModel<string>> Delete(long id)
        {
            if (id <= 0)
                return Failed("缺少参数");
            var data = await _dicType.QueryById(id);
            if(data == null)
                return Failed("数据不存在");
            var isOk = await _dicType.DeleteById(id);
            await _caching.RemoveAsync(data.code);
            if (isOk)
                return Success("","删除成功");
            return Failed();
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Permissions.Name)]
        public async Task<MessageModel<string>> Deletes([FromBody] object[] ids)
        {

            var ls = await _dicType.QueryByIDs(ids);
            var isOk = await _dicType.DeleteByIds(ids);
            if (isOk)
                return Success("", "删除成功");
            foreach (var data in ls)
            {
                await _caching.RemoveAsync(data.code);
            }
            return Failed();
        }






        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <param name="key"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Permissions.Name)]
        public async Task<MessageModel<PageModel<DicData>>> DicDataGet(string code,int page = 1, int size = 10, string key = "")
        {
            var whereFind = LinqHelper.True<DicData>();
            if (string.IsNullOrEmpty(code))
            {
                return MessageModel<PageModel<DicData>>.Fail("请选择一个要查询的字典");
            }
            else
            {
                whereFind = whereFind.And(t => t.pCode.Equals(code));
            }

            if (!string.IsNullOrEmpty(key))
            {
                whereFind = whereFind.And(t => t.name.Contains(key)
                || t.content.Contains(key)
                || t.content2.Contains(key)
                || t.content3.Contains(key)
                || t.description.Contains(key));
            }
            var data = await _dicData.QueryPage(whereFind, page, size, "codeOrder asc");
            return Success(data, "获取成功");
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Permissions.Name)]
        public async Task<MessageModel<string>> DicDataPost([FromBody] DicData data)
        {
            var id = await _dicData.Add(data);
            await _caching.RemoveAsync($"{data.pCode}_list");
            return id > 0 ? Success(id.ObjToString(), "添加成功") : Failed();

        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPut]
        [Authorize(Permissions.Name)]
        public async Task<MessageModel<string>> DicDataPut([FromBody] DicData data)
        {
            if (data == null || data.Id <= 0)
                return Failed("缺少参数");
            await _caching.RemoveAsync($"{data.pCode}_list");
            return await _dicData.Update(data) ? Success(data.Id.ObjToString(), "更新成功") : Failed();
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Authorize(Permissions.Name)]
        public async Task<MessageModel<string>> DicDataDelete(long id)
        {
            if (id <= 0)
                return Failed("缺少参数");
            var data = await _dicData.QueryById(id);
            await _caching.RemoveAsync($"{data.pCode}_list");
            var isOk = await _dicData.DeleteById(id);
            if (isOk)
                return Success("", "删除成功");
            return Failed();
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Permissions.Name)]
        public async Task<MessageModel<string>> DicDataDeletes([FromBody] object[] ids)
        {
            var isOk = await _dicData.DeleteByIds(ids);
            var data = await _dicData.QueryById(ids[0]);
            await _caching.RemoveAsync($"{data.pCode}_list");
            if (isOk)
                return Success("", "删除成功");
            return Failed();
        }



    }
}
