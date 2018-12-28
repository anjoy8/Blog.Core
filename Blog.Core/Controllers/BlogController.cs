using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog.Core.Common;
using Blog.Core.Common.Helper;
using Blog.Core.IServices;
using Blog.Core.Model.Models;
using Blog.Core.Model.ViewModels;
using Blog.Core.SwaggerHelper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static Blog.Core.SwaggerHelper.CustomApiVersion;

namespace Blog.Core.Controllers
{
    /// <summary>
    /// Blog控制器所有接口
    /// </summary>
    [Authorize(Policy = "Admin")]
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
        [AllowAnonymous]
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
                    item.bRemark = (HtmlHelper.ReplaceHtmlTag(item.bcontent)).Length >= 200 ? (HtmlHelper.ReplaceHtmlTag(item.bcontent)).Substring(0, 200) : (HtmlHelper.ReplaceHtmlTag(item.bcontent));
                    int totalLength = 500;
                    if (item.bcontent.Length > totalLength)
                    {
                        item.bcontent = item.bcontent.Substring(0, totalLength);
                    }
                }
            }

            return Ok(new
            {
                success = true,
                page = page,
                pageCount = TotalCount,
                data = blogArticleList
            });
        }


        // GET: api/Blog/5
        /// <summary>
        /// 获取详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<object> Get(int id)
        {
            var model = await blogArticleServices.getBlogDetails(id);
            return Ok(new
            {
                success = true,
                data = model
            });
        }


        /// <summary>
        /// 获取博客测试信息 v2版本
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        ////MVC自带特性 对 api 进行组管理
        //[ApiExplorerSettings(GroupName = "v2")]
        ////路径 如果以 / 开头，表示绝对路径，反之相对 controller 的想u地路径
        //[Route("/api/v2/blog/Blogtest")]

        //和上边的版本控制以及路由地址都是一样的
        [CustomRoute(ApiVersions.v2, "Blogtest")]
        public async Task<object> V2_Blogtest()
        {
            return Ok(new { status = 220, data = "我是第二版的博客信息" });
        }



    }
}
