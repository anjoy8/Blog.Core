using Blog.Core.IServices;
using Blog.Core.Model.Models;
using Xunit;
using System;
using System.Linq;
using Autofac;

namespace Blog.Core.Tests
{
    public class BlogArticleService_Should
    {

        private IBlogArticleServices blogArticleServices;
        DI_Test dI_Test = new DI_Test();


        public BlogArticleService_Should()
        {
            //mockBlogRep.Setup(r => r.Query());

            var container = dI_Test.DICollections();

            blogArticleServices = container.Resolve<IBlogArticleServices>();

        }


        [Fact]
        public void BlogArticleServices_Test()
        {
            Assert.NotNull(blogArticleServices);
        }


        [Fact]
        public async void Get_Blogs_Test()
        {
            var data = await blogArticleServices.GetBlogs();

            Assert.True(data.Any());
        }

        [Fact]
        public async void Add_Blog_Test()
        {
            BlogArticle blogArticle = new BlogArticle()
            {
                bCreateTime = DateTime.Now,
                bUpdateTime = DateTime.Now,
                btitle = "xuint test title",
                bcontent = "xuint test content",
                bsubmitter = "xuint test submitter",
            };

            var BId = await blogArticleServices.Add(blogArticle);

            Assert.True(BId > 0);
        }

        [Fact]
        public async void Delete_Blog_Test()
        {
            var deleteModel = (await blogArticleServices.Query(d => d.btitle == "xuint test title")).FirstOrDefault();

            Assert.NotNull(deleteModel);

            var IsDel = await blogArticleServices.Delete(deleteModel);

            Assert.True(IsDel);
        }
    }
}
