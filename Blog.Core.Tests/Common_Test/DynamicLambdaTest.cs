using System.Threading.Tasks;
using Autofac;
using Blog.Core.Common.Helper;
using Blog.Core.IRepository.Base;
using Blog.Core.Model.Models;
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

        //DbContext.Init(BaseDBConfig.ConnectionString,(DbType)BaseDBConfig.DbType);
    }

    [Fact]
    public async void Get_Blogs_DynamicTest()
    {
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
        
        //比如文章下 过滤创建人 
        //await TestConditions("btitle.user.name like \"老张\"");
    }

    private async Task TestConditions(string conditions)
    {
        var express = DynamicLinqFactory.CreateLambda<BlogArticle>(conditions);
        var product = await _baseRepository.Query(express);
        _testOutputHelper.WriteLine(new string('=', 100));
        _testOutputHelper.WriteLine($"原条件:{conditions}\r\nLambda:{express}\r\n结果:{product.Count}");
    }
}