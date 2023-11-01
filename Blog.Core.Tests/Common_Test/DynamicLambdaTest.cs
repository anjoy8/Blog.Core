using System;
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
            _testOutputHelper.WriteLine(UtilMethods.GetNativeSql(sql, p));
        };

        Init();
    }
    
    private void Init()
    {
        _baseRepository.Db.CodeFirst.InitTables<BlogArticle>();
        _baseRepository.Db.CodeFirst.InitTables<BlogArticleComment>();
        _baseRepository.Db.CodeFirst.InitTables<SysUserInfo>();
    }

    /// <summary>
    /// 普通查询 例子<br/>
    /// 没有复杂链表 主要使用导航属性<br/>
    /// 推荐将条件拼接交给前端 后端只定义个接口就很方便 维护也很简单<br/>
    /// </summary>
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
        await TestConditions("btitle like \" 测试数据\"");
        await TestConditions("btitle like \"测试数据\" && bId>0");
        await TestConditions("btitle like \"测试!@#$%^&*()_+|}{\":<>?LP\"数据\" && bId>0");
        await TestConditions(
            "btitle like \"测试!@+)(*()_&%^&^$^%$IUYWIQOJVLXKZM>?Z<>?<L:\"SQLitePCL{|\"CM<:\"KJLEGRTOWEJT\"#$%^&*()_+|}{\":<>?LP\"数据\" && bId>0");
        await TestConditions("IsDeleted == false");
        await TestConditions("IsDeleted == true");
        await TestConditions("IsDeleted == true && ( btitle like \"张三\" || btitle like \"李四\" )");
        await TestConditions(
            "IsDeleted == true && ( btitle like \"张三\" || btitle like \"李四\" || ( btitle StartsLike \"王五\" && btitle EndLike \"赵六\" ) )");

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

    /// <summary>
    /// 复杂链表 也能使用动态条件<br/>
    /// 存在复杂的链表 left join等
    /// </summary>
    [Fact]
    public async void Get_Blogs_DynamicJoinTest()
    {
        //方便前端自定义条件查询 
        //语法更舒服
        var data = await _baseRepository.Query();
        _testOutputHelper.WriteLine(data.ToJson());

        await TestJoinConditions("");
        await TestJoinConditions("bId=1");
        await TestJoinConditions("bId=2");
        await TestJoinConditions("bId in (1,2,3,4,5)");
        await TestJoinConditions("bId in (1,2,3,4,5)|| bUpdateTime>=\"2019-01-01 01:01:01\"");
        await TestJoinConditions("btitle like \" 测试数据\"");
        await TestJoinConditions("btitle like \"测试数据\" && bId>0");
        await TestJoinConditions("btitle like \"测试!@#$%^&*()_+|}{\":<>?LP\"数据\" && bId>0");
        await TestJoinConditions(
            "btitle like \"测试!@+)(*()_&%^&^$^%$IUYWIQOJVLXKZM>?Z<>?<L:\"SQLitePCL{|\"CM<:\"KJLEGRTOWEJT\"#$%^&*()_+|}{\":<>?LP\"数据\" && bId>0");
        await TestJoinConditions("IsDeleted == false");
        await TestJoinConditions("IsDeleted == true");
        await TestJoinConditions("IsDeleted == true && ( btitle like \"张三\" || btitle like \"李四\" )");
        await TestJoinConditions(
            "IsDeleted == true && ( btitle like \"张三\" || btitle like \"李四\" || ( btitle StartsLike \"王五\" && btitle EndLike \"赵六\" ) )");

        //导航属性

        //一对一

        //查询 老张的文章
        await TestJoinConditions("User.RealName like \"老张\"");
        //查询 2019年后的老张文章
        await TestJoinConditions("User.RealName like \"老张\" &&  bUpdateTime>=\"2019-01-01 01:01:01\"");

        //一对多

        //查询 评论中有"写的不错"的文章
        await TestJoinConditions("Comments.Comment like \"写的不错\"");
        //查询 2019后的 评论中有"写的不错"的文章
        await TestJoinConditions("Comments.Comment like \"写的不错\" &&  bUpdateTime>=\"2019-01-01 01:01:01\"");
        //查询 有老张评论的文章
        await TestJoinConditions("Comments.User.LoginName like \"老张\"");
    }


    private async Task TestConditions(string conditions)
    {
        var express = DynamicLinqFactory.CreateLambda<BlogArticle>(conditions);
        _testOutputHelper.WriteLine(new string('=', 100));
        var product = await _baseRepository.Query(express);
        _testOutputHelper.WriteLine($"条件:{DynamicLinqFactory.FormatString(conditions)}\r\nLambda:{express}\r\n结果:{product.Count}");
        _testOutputHelper.WriteLine(new string('=', 100));
    }

    private async Task TestJoinConditions(string conditions)
    {
        var express = DynamicLinqFactory.CreateLambda<BlogArticle>(conditions);
        _testOutputHelper.WriteLine(new string('=', 100));
        var product = await _baseRepository.Db.Queryable<BlogArticle>()
            .LeftJoin<SysUserInfo>((b, u) => Convert.ToInt64(b.bsubmitter) == u.Id)
            .MergeTable()
            .Where(express)
            .ToListAsync();
        _testOutputHelper.WriteLine($"条件:{DynamicLinqFactory.FormatString(conditions)}\r\nLambda:{express}\r\n结果:{product.Count}");
        _testOutputHelper.WriteLine(new string('=', 100));
    }
}