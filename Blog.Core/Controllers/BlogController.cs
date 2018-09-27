using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog.Core.Common;
using Blog.Core.Common.Helper;
using Blog.Core.IServices;
using Blog.Core.Model.Models;
using Blog.Core.Model.VeiwModels;
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
        /// <param name="id"></param>
        /// <param name="page"></param>
        /// <param name="bcategory"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<object> Get(int id, int page = 1, string bcategory = "技术博文")
        {
            int intTotalCount = 6;
            int TotalCount = 1;
            List<BlogArticle> blogArticleList = new List<BlogArticle>();

            if (redisCacheManager.Get<object>("Redis.Blog") != null)
            {
                blogArticleList = redisCacheManager.Get<List<BlogArticle>>("Redis.Blog");
            }
            else
            {
                blogArticleList = await blogArticleServices.Query(a => a.bcategory == bcategory);
                redisCacheManager.Set("Redis.Blog", blogArticleList, TimeSpan.FromHours(2));
            }


            TotalCount = blogArticleList.Count() / intTotalCount;

            blogArticleList = blogArticleList.OrderByDescending(d => d.bID).Skip((page - 1) * intTotalCount).Take(intTotalCount).ToList();

            foreach (var item in blogArticleList)
            {
                if (!string.IsNullOrEmpty(item.bcontent))
                {
                    item.bRemark = (HtmlHelper.ReplaceHtmlTag(item.bcontent)).Substring(0, 200);
                    int totalLength = 500;
                    if (item.bcontent.Length > totalLength)
                    {
                        item.bcontent = item.bcontent.Substring(0, totalLength);
                    }
                }
            }

            var data = new { success = true, page = page, pageCount = TotalCount, data = blogArticleList };


            return data;
        }


        // GET: api/Blog/5
        /// <summary>
        /// 获取详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}", Name = "Get")]
        [Authorize(Policy = "Admin")]
        public async Task<object> Get(int id)
        {
            var model = await blogArticleServices.getBlogDetails(id);
            var data = new { success = true, data = model };
            return data;
        }


    }
}
