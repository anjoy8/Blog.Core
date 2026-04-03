using System.Globalization;
using Blog.Core.Common.Utility;
using JetBrains.Annotations;
using SqlSugar;
using SqlSugar.DistributedSystem.Snowflake;
using Xunit;
using Xunit.Abstractions;

namespace Blog.Core.Tests.Utility;

[TestSubject(typeof(SqlSugarSnowflakeHelper))]
public class SqlSugarSnowflakeHelperTest(ITestOutputHelper testOutputHelper)
{
    [Fact]
    public void Test_Id_To_Datetime()
    {
        var id = SnowFlakeSingle.Instance.NextId();

        testOutputHelper.WriteLine(SqlSugarSnowflakeHelper.GetDateTime(id).ToString(CultureInfo.InvariantCulture));
    }

    [Fact]
    public void Test_Id()
    {
        var id = SnowFlakeSingle.Instance.NextId();

        testOutputHelper.WriteLine(SqlSugarSnowflakeHelper.Decode(id).ToJson());
    }
}