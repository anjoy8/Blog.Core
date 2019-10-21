using Blog.Core.Common;
using Blog.Core.Controllers;
using Blog.Core.IRepository;
using Blog.Core.IServices;
using Blog.Core.Model.Models;
using Moq;
using Xunit;
using System;
using Microsoft.Extensions.Logging;
using Autofac;
using System.Linq;

namespace Blog.Core.Tests
{
    public class ClaimsController_Should
    {
        ClaimsController claimsController;



        public ClaimsController_Should()
        {
            claimsController = new ClaimsController();
        }

        [Fact]
        public void GetTest()
        {
            var data = claimsController.Get();
            Assert.True(data.Any());
        }
        [Fact]
        public void GetDetailsTest()
        {
            object blogs =claimsController.Get(1);

            Assert.NotNull(blogs);
        }

    }
}
