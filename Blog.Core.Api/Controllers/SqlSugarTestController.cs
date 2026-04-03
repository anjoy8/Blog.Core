using System.Text;
using Blog.Core.Common.DB.Extension;
using Blog.Core.Controllers;
using Blog.Core.Model;
using Blog.Core.Model.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;

namespace Blog.Core.Api.Controllers;

/// <summary>
/// SqlSugar 相关测试
/// </summary>
[Route("api/[controller]/[action]")]
[ApiController]
[AllowAnonymous]
public class SqlSugarTestController(ISqlSugarClient db) : BaseApiController
{
    /// <summary>
    /// 测试建表后，SqlSugar缓存
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public MessageModel ClearDbTableCache()
    {
        var tableName = "BlogArticle_Test";

        //先删除表
        try
        {
            db.DbMaintenance.DropTable(tableName);
            db.ClearDbTableCache();
        }
        catch
        {
            //Ignore
        }

        StringBuilder sb = new StringBuilder();

        //提前检查表是否存在，测试缓存
        sb.AppendLine($"表{tableName} 是否存在：{db.DbMaintenance.IsAnyTable(tableName)}");

        //创建表
        db.CodeFirst.As<BlogArticle>(tableName).InitTables<BlogArticle>();
        sb.AppendLine($"表{tableName} 创建成功");

        //检查表是否存在
        sb.AppendLine($"表{tableName} 是否存在：{db.DbMaintenance.IsAnyTable(tableName)}");

        //清除缓存
        db.ClearDbTableCache();
        sb.AppendLine($"清除缓存后");

        //检查表是否存在
        sb.AppendLine($"表{tableName} 是否存在：{db.DbMaintenance.IsAnyTable(tableName)}");

        return Success(sb.ToString());
    }
}