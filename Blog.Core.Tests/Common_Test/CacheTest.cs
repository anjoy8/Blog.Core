using Autofac;
using Blog.Core.Common;
using Blog.Core.Common.Caches.Interface;
using Xunit;
using Xunit.Abstractions;

namespace Blog.Core.Tests.Common_Test;

public class CacheTest
{
    private readonly ITestOutputHelper _testOutputHelper;
    DI_Test dI_Test = new DI_Test();
    private readonly ICaching _cache;

    public CacheTest(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
        dI_Test.Build();
        _cache = App.GetService<ICaching>();
    }

    [Fact]
    public void TestCaching()
    {
        _cache.Set("test", "test", new TimeSpan(0, 10, 0));

        var result = _cache.Get<string>("test");
        Assert.Equal("test", result);

        var caches = _cache.GetAllCacheKeys();
        _testOutputHelper.WriteLine(caches.ToJson());
        Assert.NotNull(caches);

        var count = _cache.GetAllCacheKeys().Count;
        Assert.Equal(1, count);
    }
}