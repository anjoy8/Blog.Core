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
    public class BlogArticleShould
    {
        Mock<IBlogArticleServices> mockBlogSev = new Mock<IBlogArticleServices>();
        Mock<IRedisCacheManager> mockRedisMag = new Mock<IRedisCacheManager>();
        BlogController blogController;

        public BlogArticleShould()
        {
            mockBlogSev.Setup(r => r.Query());
            blogController = new BlogController(mockBlogSev.Object, mockRedisMag.Object);


        }

        [Fact]
        public void TestEntity()
        {
            BlogArticle blogArticle = new BlogArticle();

            Assert.True(blogArticle.bID >= 0);
        }
        [Fact]
        public void AddEntity()
        {
            BlogArticle blogArticle = new BlogArticle()
            {
                bCreateTime = DateTime.Now,
                bUpdateTime = DateTime.Now,
                btitle = "xuint",

            };
            //blogController.Post(blogArticle).Wait();

            var data = blogController.Get(1);
            Assert.Null(data);//为空包错了，证明不为空
        }
    }
}
