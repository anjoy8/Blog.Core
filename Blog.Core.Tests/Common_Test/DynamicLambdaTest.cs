using System.Threading.Tasks;
using Autofac;
using Blog.Core.Common.Helper;
using Blog.Core.IRepository.Base;
using Blog.Core.Model.Models;
using SqlSugar;
using Xunit;
using Xunit.Abstractions;

namespace Blog.Core.Tests.Common_Test;

public class DynamicLambdaTest
{
    private readonly ITestOutputHelper _testOutputHelper;
    private readonly IBaseRepository<BlogArticle> _baseRepository;
    DI_Test dI_Test = new DI_Test();

    public DynamicLambdaTest(ITestOutputHelper testOutputHelper)
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
        //DbContext.Init(BaseDBConfig.ConnectionString,(DbType)BaseDBConfig.DbType);

        Init();
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

    private void Init()
    {
        _baseRepository.Db.CodeFirst.InitTables<BlogArticle>();
        _baseRepository.Db.CodeFirst.InitTables<BlogArticleComment>();
    }

    [Fact]
    public async void Get_Blogs_DynamicTest()
    {
        //方便前端自定义条件查询 
        //语法更舒服
        var data = await _baseRepository.Query();
        _testOutputHelper.WriteLine(data.ToJson());

        await TestConditions("");
        await TestConditions("bId=1");
        await TestConditions("bId=2");
        await TestConditions("bId in (1,2,3,4,5)");
        await TestConditions("bId in (1,2,3,4,5)|| bUpdateTime>=\"2019-01-01 01:01:01\"");
        await TestConditions("btitle like \"测试数据\" && bId>0");
        await TestConditions("btitle like \"测试!@#$%^&*()_+|}{\":<>?LP\"数据\" && bId>0");
        await TestConditions("btitle like \"测试!@+)(*()_&%^&^$^%$IUYWIQOJVLXKZM>?Z<>?<L:\"SQLitePCL{|\"CM<:\"KJLEGRTOWEJT\"#$%^&*()_+|}{\":<>?LP\"数据\" && bId>0");
        await TestConditions("IsDeleted == false");
        await TestConditions("IsDeleted == true");

        //导航属性

        //一对一

        //查询 老张的文章
        await TestConditions("User.RealName like \"老张\""); 
        //查询 2019年后的老张文章
        await TestConditions("User.RealName like \"老张\" &&  bUpdateTime>=\"2019-01-01 01:01:01\""); 

        //一对多

        //查询 评论中有"写的不错"的文章
        await TestConditions("Comments.Comment like \"写的不错\"");      
        //查询 2019后的 评论中有"写的不错"的文章
        await TestConditions("Comments.Comment like \"写的不错\" &&  bUpdateTime>=\"2019-01-01 01:01:01\"");  
        //查询 有老张评论的文章
        await TestConditions("Comments.User.LoginName like \"老张\"");
    }

    private async Task TestConditions(string conditions)
    {
        var express = DynamicLinqFactory.CreateLambda<BlogArticle>(conditions);
        var product = await _baseRepository.Query(express);
        _testOutputHelper.WriteLine(new string('=', 100));
        _testOutputHelper.WriteLine($"原条件:{conditions}\r\nLambda:{express}\r\n结果:{product.Count}");
    }
}