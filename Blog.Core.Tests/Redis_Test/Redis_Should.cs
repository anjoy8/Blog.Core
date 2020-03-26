using Blog.Core.Common;
using Blog.Core.Controllers;
using Blog.Core.IRepository;
using Blog.Core.IServices;
using Blog.Core.Model.Models;
using Moq;
using Xunit;
using System;
using Autofac;

namespace Blog.Core.Tests
{
    public class Redis_Should
    {
        DI_Test dI_Test = new DI_Test();

        public Redis_Should()
        {
            //var container = dI_Test.DICollections();
            //_redisCacheManager = container.Resolve<IRedisCacheManager>();

        }

        [Fact]
        public void Connect_Redis_Test()
        {

            //var redisBlogCache = _redisCacheManager.Get<object>("Redis.Blog");

            //Assert.Null(redisBlogCache);
        }

    }
}
