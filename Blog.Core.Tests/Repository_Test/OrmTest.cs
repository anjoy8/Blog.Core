using System;
using Autofac;
using Blog.Core.Common.Extensions;
using Blog.Core.IRepository.Base;
using Blog.Core.Model.Models;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using SqlSugar;
using Xunit;
using Xunit.Abstractions;

namespace Blog.Core.Tests;

public class OrmTest
{
    private readonly ITestOutputHelper _testOutputHelper;
    private readonly IBaseRepository<BlogArticle> _baseRepository;
    DI_Test dI_Test = new DI_Test();

    public OrmTest(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;

        var container = dI_Test.DICollections();

        _baseRepository = container.Resolve<IBaseRepository<BlogArticle>>();
        _baseRepository.Db.Aop.OnLogExecuting = (sql, p) =>
        {
            _testOutputHelper.WriteLine("");
            _testOutputHelper.WriteLine("==================FullSql=====================", "", new string[] { sql.GetType().ToString(), GetParas(p), "【SQL语句】：" + sql });
            _testOutputHelper.WriteLine("【SQL语句】：" + sql);
            _testOutputHelper.WriteLine(GetParas(p));
            _testOutputHelper.WriteLine("==============================================");
            _testOutputHelper.WriteLine("");
        };
    }

    private static string GetParas(SugarParameter[] pars)
    {
        string key = "【SQL参数】：";
        foreach (var param in pars)
        {
            key += $"{param.ParameterName}:{param.Value}\n";
        }

        return key;
    }

    [Fact]
    public void MultiTables()
    {
        var sql = _baseRepository.Db.Queryable<BlogArticle>()
            .AS($@"{nameof(BlogArticle)}_TenantA")
            .ToSqlString();
        //_testOutputHelper.WriteLine(sql);

        _baseRepository.Db.MappingTables.Add(nameof(BlogArticle), $@"{nameof(BlogArticle)}_TenantA");

        var query = _baseRepository.Db.Queryable<BlogArticle>()
            .LeftJoin<BlogArticleComment>((a, c) => a.bID == c.bID);
        // query.QueryBuilder.AsTables.AddOrModify(nameof(BlogArticle), $@"{nameof(BlogArticle)}_TenantA");
        //query.QueryBuilder.AsTables.AddOrModify(nameof(BlogArticleComment), $@"{nameof(BlogArticleComment)}_TenantA");
        // query.QueryBuilder.AsTables.AddOrModify(nameof(BlogArticleComment), $@"{nameof(BlogArticleComment)}_TenantA");
        // query.QueryBuilder.AsTables.AddOrModify(nameof(SysUserInfo), $@"{nameof(SysUserInfo)}_TenantA");


        sql = query.ToSqlString();

        _testOutputHelper.WriteLine(sql);

        sql = _baseRepository.Db.Deleteable<BlogArticle>().ToSqlString();
        _testOutputHelper.WriteLine(sql);
    }
}