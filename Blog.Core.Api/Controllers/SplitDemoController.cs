using Blog.Core.IServices;
using Blog.Core.Model;
using Blog.Core.Model.Models;
using Blog.Core.Repository.UnitOfWorks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using Blog.Core.Controllers;
using SqlSugar;

namespace Blog.Core.Api.Controllers;

/// <summary>
/// 分表demo
/// </summary>
[Route("api/[controller]/[action]")]
[ApiController]
[Authorize(Permissions.Name)]
public class SplitDemoController : BaseApiController
{
    readonly ISplitDemoServices splitDemoServices;
    readonly IUnitOfWorkManage unitOfWorkManage;
    private readonly ISqlSugarClient _db;

    public SplitDemoController(ISplitDemoServices _splitDemoServices, IUnitOfWorkManage _unitOfWorkManage, ISqlSugarClient db)
    {
        splitDemoServices = _splitDemoServices;
        unitOfWorkManage = _unitOfWorkManage;
        _db = db;
    }

    /// <summary>
    /// 分页获取数据
    /// </summary>
    /// <param name="beginTime"></param>
    /// <param name="endTime"></param>
    /// <param name="page"></param>
    /// <param name="key"></param>
    /// <param name="pageSize"></param>
    /// <returns></returns>
    [HttpGet]
    [AllowAnonymous]
    public async Task<MessageModel<PageModel<SplitDemo>>> Get(DateTime beginTime, DateTime endTime, int page = 1, string key = "",
        int pageSize = 10)
    {
        if (string.IsNullOrEmpty(key) || string.IsNullOrWhiteSpace(key))
        {
            key = "";
        }

        Expression<Func<SplitDemo, bool>> whereExpression = a => (a.Name != null && a.Name.Contains(key));
        var data = await splitDemoServices.QueryPageSplit(whereExpression, beginTime, endTime, page, pageSize, " Id desc ");
        return MessageModel<PageModel<SplitDemo>>.Message(data.dataCount >= 0, "获取成功", data);
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<MessageModel<List<SplitDemo>>> GetSpilt()
    {
        var data = await _db.Queryable<SplitDemo>().AS("SplitDemo_20241231").ToListAsync();
        return Success(data);
    }

    /// <summary>
    /// 根据ID获取信息
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet]
    [AllowAnonymous]
    public async Task<MessageModel<SplitDemo>> GetById(long id)
    {
        var data = new MessageModel<string>();
        var model = await splitDemoServices.QueryByIdSplit(id);
        if (model != null)
        {
            return MessageModel<SplitDemo>.Success("获取成功", model);
        }
        else
        {
            return MessageModel<SplitDemo>.Fail("获取失败");
        }
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<MessageModel> GenTestData()
    {
        //帮我生成一个月数据
        for (int i = 0; i < 30; i++)
        {
            await splitDemoServices.AddSplit(new SplitDemo()
            {
                Name = "测试数据" + i,
                CreateTime = DateTime.Now.AddDays(-i)
            });
        }

        return Success();
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<MessageModel> InitTable()
    {
        _db.MappingTables.Add("SplitDemo","SplitDemo_20241231");
        _db.CodeFirst.InitTables<SplitDemo>();
        await Task.Delay(1);
        return Success();
    }

    /// <summary>
    /// 添加一条测试数据
    /// </summary>
    /// <param name="splitDemo"></param>
    /// <returns></returns>
    [HttpPost]
    [AllowAnonymous]
    public async Task<MessageModel<string>> Post([FromBody] SplitDemo splitDemo)
    {
        var data = new MessageModel<string>();
        //unitOfWorkManage.BeginTran();
        var id = (await splitDemoServices.AddSplit(splitDemo));
        data.success = (id == null ? false : true);
        try
        {
            if (data.success)
            {
                data.response = id.FirstOrDefault().ToString();
                data.msg = "添加成功";
            }
            else
            {
                data.msg = "添加失败";
            }
        }
        catch (Exception)
        {
            throw;
        }
        finally
        {
            //if (data.success)
            //    unitOfWorkManage.CommitTran();
            //else
            //    unitOfWorkManage.RollbackTran();
        }

        return data;
    }

    /// <summary>
    /// 修改一条测试数据
    /// </summary>
    /// <param name="splitDemo"></param>
    /// <returns></returns>
    [HttpPut]
    [AllowAnonymous]
    public async Task<MessageModel<string>> Put([FromBody] SplitDemo splitDemo)
    {
        var data = new MessageModel<string>();
        if (splitDemo != null && splitDemo.Id > 0)
        {
            unitOfWorkManage.BeginTran();
            data.success = await splitDemoServices.UpdateSplit(splitDemo, splitDemo.CreateTime);
            try
            {
                if (data.success)
                {
                    data.msg = "修改成功";
                    data.response = splitDemo?.Id.ObjToString();
                }
                else
                {
                    data.msg = "修改失败";
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (data.success)
                    unitOfWorkManage.CommitTran();
                else
                    unitOfWorkManage.RollbackTran();
            }
        }

        return data;
    }

    /// <summary>
    /// 根据id删除数据
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete]
    [AllowAnonymous]
    public async Task<MessageModel<string>> Delete(long id)
    {
        var data = new MessageModel<string>();

        var model = await splitDemoServices.QueryByIdSplit(id);
        if (model != null)
        {
            unitOfWorkManage.BeginTran();
            data.success = await splitDemoServices.DeleteSplit(model, model.CreateTime);
            try
            {
                data.response = id.ObjToString();
                if (data.success)
                {
                    data.msg = "删除成功";
                }
                else
                {
                    data.msg = "删除失败";
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (data.success)
                    unitOfWorkManage.CommitTran();
                else
                    unitOfWorkManage.RollbackTran();
            }
        }
        else
        {
            data.msg = "不存在";
        }

        return data;
    }
}