using Blog.Core.Common;
using Blog.Core.Controllers;
using Blog.Core.IRepository;
using Blog.Core.IServices;
using Blog.Core.Model.Models;
using Moq;
using Xunit;
using System;

namespace Blog.Core.Tests
{
    public class Redis_Should
    {
        //IRedisCacheManager _redisCacheManager;

        //public Redis_Should(IRedisCacheManager redisCacheManager)
        //{
        //    _redisCacheManager = redisCacheManager;
        //}

        [Fact]
        public void Connect_Redis_Test()
        {
            RedisCacheManager _redisCacheManager = new RedisCacheManager();

            var redisBlogCache = _redisCacheManager.Get<object>("Redis.Blog");

            Assert.NotNull(redisBlogCache);
        }

    }
}
