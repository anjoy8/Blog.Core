using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog.Core.Common;
using Blog.Core.Common.Helper;
using Blog.Core.IServices;
using Blog.Core.Model;
using Blog.Core.Model.Models;
using Blog.Core.SwaggerHelper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StackExchange.Profiling;
using static Blog.Core.SwaggerHelper.CustomApiVersion;

namespace Blog.Core.Controllers
{
    /// <summary>
    /// 博客管理
    /// </summary>
    [Produces("application/json")]
    [Route("api/Blog")]
    [Authorize]
    public class BlogController : Controller
    {
        readonly IBlogArticleServices _blogArticleServices;
        readonly IRedisCacheManager _redisCacheManager;
        private readonly ILogger<BlogController> _logger;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="blogArticleServices"></param>
        /// <param name="redisCacheManager"></param>
        public BlogController(IBlogArticleServices blogArticleServices, IRedisCacheManager redisCacheManager, ILogger<BlogController> logger)
        {
            _blogArticleServices = blogArticleServices;
            _redisCacheManager = redisCacheManager;
            _logger = logger;
        }


        /// <summary>
        /// 获取博客列表【无权限】
        /// </summary>
        /// <param name="id"></param>
        /// <param name="page"></param>
        /// <param name="bcategory"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        //[ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        //[ResponseCache(Duration = 600)]
        public async Task<object> Get(int id, int page = 1, string bcategory = "技术博文", string key = "")
        {
            int intTotalCount = 6;
            int total;
            int totalCount = 1;
            List<BlogArticle> blogArticleList = new List<BlogArticle>();
            if (string.IsNullOrEmpty(key) || string.IsNullOrWhiteSpace(key))
            {
                key = "";
            }

            using (MiniProfiler.Current.Step("开始加载数据："))
            {
                try
                {
                    if (_redisCacheManager.Get<object>("Redis.Blog") != null)
                    {
                        MiniProfiler.Current.Step("从Redis服务器中加载数据：");
                        blogArticleList = _redisCacheManager.Get<List<BlogArticle>>("Redis.Blog");
                    }
                    else
                    {
                        MiniProfiler.Current.Step("从MSSQL服务器中加载数据：");
                        blogArticleList = await _blogArticleServices.Query(a => a.bcategory == bcategory && a.IsDeleted == false);
                        _redisCacheManager.Set("Redis.Blog", blogArticleList, TimeSpan.FromHours(2));
                    }

                }
                catch (Exception e)
                {
                    MiniProfiler.Current.CustomTiming("Errors：", "Redis服务未启用，请开启该服务，并且请注意端口号，本项目使用的的6319，而且我的是没有设置密码。" + e.Message);
                    blogArticleList = await _blogArticleServices.Query(a => a.bcategory == bcategory && a.IsDeleted == false);
                }
            }

            blogArticleList = blogArticleList.Where(d => (d.btitle != null && d.btitle.Contains(key)) || (d.bcontent != null && d.bcontent.Contains(key))).ToList();

            total = blogArticleList.Count();
            totalCount = blogArticleList.Count() / intTotalCount;

            using (MiniProfiler.Current.Step("获取成功后，开始处理最终数据"))
            {
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
            }

            return Ok(new
            {
                success = true,
                page,
                total,
                pageCount = totalCount,
                data = blogArticleList
            });
        }


        /// <summary>
        /// 获取博客详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<object> Get(int id)
        {
            var model = await _blogArticleServices.GetBlogDetails(id);
            return Ok(new
            {
                success = true,
                data = model
            });
        }


        /// <summary>
        /// 获取详情【无权限】
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("DetailNuxtNoPer")]
        [AllowAnonymous]
        public async Task<object> DetailNuxtNoPer(int id)
        {
            _logger.LogInformation("xxxxxxxxxxxxxxxxxxx");
            var model = await _blogArticleServices.GetBlogDetails(id);
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

        [CustomRoute(ApiVersions.V2, "Blogtest")]
        [AllowAnonymous]
        public object V2_Blogtest()
        {
            return Ok(new { status = 220, data = "我是第二版的博客信息" });
        }

        /// <summary>
        /// 添加博客【无权限】
        /// </summary>
        /// <param name="blogArticle"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<MessageModel<string>> Post([FromBody] BlogArticle blogArticle)
        {
            var data = new MessageModel<string>();

            blogArticle.bCreateTime = DateTime.Now;
            blogArticle.bUpdateTime = DateTime.Now;
            blogArticle.IsDeleted = false;

            var id = (await _blogArticleServices.Add(blogArticle));
            data.success = id > 0;
            if (data.success)
            {
                data.response = id.ObjToString();
                data.msg = "添加成功";
            }

            return data;
        }

        /// <summary>
        /// 删除博客
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Authorize(Permissions.Name)]
        [Route("Delete")]
        public async Task<MessageModel<string>> Delete(int id)
        {
            var data = new MessageModel<string>();
            if (id > 0)
            {
                var blogArticle = await _blogArticleServices.QueryById(id);
                blogArticle.IsDeleted = true;
                data.success = await _blogArticleServices.Update(blogArticle);
                if (data.success)
                {
                    data.msg = "删除成功";
                    data.response = blogArticle?.bID.ObjToString();
                }
            }

            return data;
        }

    }
}
