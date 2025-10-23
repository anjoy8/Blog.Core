using System.Globalization;
using Blog.Core.Common.Utility;
using JetBrains.Annotations;
using Xunit;
using Xunit.Abstractions;
using Yitter.IdGenerator;

namespace Blog.Core.Tests.Utility;

[TestSubject(typeof(YitterSnowflakeHelper))]
public class YitterSnowflakeHelperTest(ITestOutputHelper testOutputHelper)
{
    private static readonly IdGeneratorOptions _options = new IdGeneratorOptions { WorkerId = 1 };
    private readonly IIdGenerator _idGenInstance = new DefaultIdGenerator(_options);

    [Fact]
    public void Test_Id_To_Datetime()
    {
        var id = _idGenInstance.NewLong();

        var dateTime = YitterSnowflakeHelper.GetDateTime(_options, id);
        testOutputHelper.WriteLine(dateTime.ToString(CultureInfo.InvariantCulture));
    }

    [Fact]
    public void Test_Id()
    {
        var id = _idGenInstance.NewLong();

        var decoded = YitterSnowflakeHelper.Decode(_options, id);
        testOutputHelper.WriteLine(decoded.ToJson());
    }
}