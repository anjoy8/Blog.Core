using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog.Core.Common;
using Blog.Core.IServices;
using Blog.Core.Model.Models;
using Blog.Core.Model.VeiwModels;
using Blog.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Core.Controllers
{
    /// <summary>
    /// Blog控制器所有接口
    /// </summary>
    [Produces("application/json")]
    [Route("api/Blog")]
    [Authorize(Roles = "Admin")]
    public class BlogController : Controller
    {
        IAdvertisementServices advertisementServices;
        IBlogArticleServices blogArticleServices;
        IRedisCacheManager redisCacheManager;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="advertisementServices"></param>
        /// <param name="blogArticleServices"></param>
        /// <param name="redisCacheManager"></param>
        public BlogController(IAdvertisementServices advertisementServices, IBlogArticleServices blogArticleServices, IRedisCacheManager redisCacheManager)
        {
            this.advertisementServices = advertisementServices;
            this.blogArticleServices = blogArticleServices;
            this.redisCacheManager = redisCacheManager;
        }
        /// <summary>
        /// 获取博客列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetBlogs")]
        public async Task<List<BlogArticle>> GetBlogs()
        {
            //var connect=Appsettings.app(new string[] { "AppSettings", "RedisCaching" , "ConnectionString" });//按照层级的顺序，依次写出来
            List<BlogArticle> blogArticleList = new List<BlogArticle>();

            if (redisCacheManager.Get<object>("Redis.Blog") != null)
            {
                blogArticleList = redisCacheManager.Get<List<BlogArticle>>("Redis.Blog");
            }
            else
            {
                blogArticleList = await blogArticleServices.Query(d => d.bID > 5);
                redisCacheManager.Set("Redis.Blog", blogArticleList, TimeSpan.FromHours(2));
            }

            return blogArticleList;
        }

        // GET: api/Blog/5
        /// <summary>
        /// 获取详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}", Name = "Get")]
        public async Task<object> Get(int id)
        {
            var model = await blogArticleServices.getBlogDetails(id);
            var data = new { success = true, data = model };
            return data;
        }



    }
}
