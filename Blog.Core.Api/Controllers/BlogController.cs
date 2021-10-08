using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Blog.Core.Common.Helper;
using Blog.Core.IServices;
using Blog.Core.Model;
using Blog.Core.Model.Models;
using Blog.Core.Model.ViewModels;
using Blog.Core.SwaggerHelper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StackExchange.Profiling;
using static Blog.Core.Extensions.CustomApiVersion;

namespace Blog.Core.Controllers
{
    /// <summary>
    /// 博客管理
    /// </summary>
    [Produces("application/json")]
    [Route("api/Blog")]
    public class BlogController : BaseApiCpntroller
    {
        public IBlogArticleServices _blogArticleServices { get; set; }
        private readonly ILogger<BlogController> _logger;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger"></param>
        /// 
        public BlogController(ILogger<BlogController> logger)
        {
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
        public async Task<MessageModel<PageModel<BlogArticle>>> Get(int id, int page = 1, string bcategory = "技术博文", string key = "")
        {
            int intPageSize = 6;
            if (string.IsNullOrEmpty(key) || string.IsNullOrWhiteSpace(key))
            {
                key = "";
            }

            Expression<Func<BlogArticle, bool>> whereExpression = a => (a.bcategory == bcategory && a.IsDeleted == false) && ((a.btitle != null && a.btitle.Contains(key)) || (a.bcontent != null && a.bcontent.Contains(key)));

            var pageModelBlog = await _blogArticleServices.QueryPage(whereExpression, page, intPageSize, " bID desc ");

            using (MiniProfiler.Current.Step("获取成功后，开始处理最终数据"))
            {
                foreach (var item in pageModelBlog.data)
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

            return SuccessPage(pageModelBlog);
        }


        /// <summary>
        /// 获取博客详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        //[Authorize(Policy = "Scope_BlogModule_Policy")]
        [Authorize]
        public async Task<MessageModel<BlogViewModels>> Get(int id)
        {
            return Success(await _blogArticleServices.GetBlogDetails(id));
        }


        /// <summary>
        /// 获取详情【无权限】
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("DetailNuxtNoPer")]
        public async Task<MessageModel<BlogViewModels>> DetailNuxtNoPer(int id)
        {
            _logger.LogInformation("xxxxxxxxxxxxxxxxxxx");
            return Success(await _blogArticleServices.GetBlogDetails(id));
        }

        [HttpGet]
        [Route("GoUrl")]
        public async Task<IActionResult> GoUrl(int id = 0)
        {
            var response = await _blogArticleServices.QueryById(id);
            if (response != null && response.bsubmitter.IsNotEmptyOrNull())
            {
                string Url = @"^http(s)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?$";
                if (Regex.IsMatch(response.bsubmitter, Url))
                {
                    response.btraffic += 1;
                    await _blogArticleServices.Update(response);
                    return Redirect(response.bsubmitter);
                }

            }

            return Ok();
        }

        [HttpGet]
        [Route("GetBlogsByTypesForMVP")]
        public async Task<MessageModel<List<BlogArticle>>> GetBlogsByTypesForMVP(string types = "", int id = 0)
        {
            if (types.IsNotEmptyOrNull())
            {
                var blogs = await _blogArticleServices.Query(d => d.bcategory != null && types.Contains(d.bcategory) && d.IsDeleted == false, d => d.bID, false);
                return Success(blogs);
            }
            return Success(new List<BlogArticle>() { });
        }

        [HttpGet]
        [Route("GetBlogByIdForMVP")]
        public async Task<MessageModel<BlogArticle>> GetBlogByIdForMVP(int id = 0)
        {
            if (id > 0)
            {
                return Success(await _blogArticleServices.QueryById(id));
            }
            return Success(new BlogArticle());
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
        public MessageModel<string> V2_Blogtest()
        {
            return Success<string>("我是第二版的博客信息");
        }

        /// <summary>
        /// 添加博客【无权限】
        /// </summary>
        /// <param name="blogArticle"></param>
        /// <returns></returns>
        [HttpPost]
        //[Authorize(Policy = "Scope_BlogModule_Policy")]
        [Authorize]
        public async Task<MessageModel<string>> Post([FromBody] BlogArticle blogArticle)
        {
            if (blogArticle.btitle.Length > 5 && blogArticle.bcontent.Length > 50)
            {

                blogArticle.bCreateTime = DateTime.Now;
                blogArticle.bUpdateTime = DateTime.Now;
                blogArticle.IsDeleted = false;
                blogArticle.bcategory = "技术博文";
                var id = (await _blogArticleServices.Add(blogArticle));
                return id > 0 ? Success<string>(id.ObjToString()) : Failed("添加失败");
            }
            else
            {
                return Failed("文章标题不能少于5个字符，内容不能少于50个字符！");
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="blogArticle"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("AddForMVP")]
        [Authorize(Permissions.Name)]
        public async Task<MessageModel<string>> AddForMVP([FromBody] BlogArticle blogArticle)
        {
            blogArticle.bCreateTime = DateTime.Now;
            blogArticle.bUpdateTime = DateTime.Now;
            blogArticle.IsDeleted = false;
            var id = (await _blogArticleServices.Add(blogArticle));
            return id > 0 ? Success<string>(id.ObjToString()) : Failed("添加失败");
        }
        /// <summary>
        /// 更新博客信息
        /// </summary>
        /// <param name="BlogArticle"></param>
        /// <returns></returns>
        // PUT: api/User/5
        [HttpPut]
        [Route("Update")]
        [Authorize(Permissions.Name)]
        public async Task<MessageModel<string>> Put([FromBody] BlogArticle BlogArticle)
        {
            if (BlogArticle != null && BlogArticle.bID > 0)
            {
                var model = await _blogArticleServices.QueryById(BlogArticle.bID);

                if (model != null)
                {
                    model.btitle = BlogArticle.btitle;
                    model.bcategory = BlogArticle.bcategory;
                    model.bsubmitter = BlogArticle.bsubmitter;
                    model.bcontent = BlogArticle.bcontent;
                    model.btraffic = BlogArticle.btraffic;

                    if (await _blogArticleServices.Update(model))
                        Success<string>(BlogArticle?.bID.ObjToString());
                }
            }
            return Failed("更新失败");
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
            if (id > 0)
            {
                var blogArticle = await _blogArticleServices.QueryById(id);
                blogArticle.IsDeleted = true;
                return await _blogArticleServices.Update(blogArticle) ? Success(blogArticle?.bID.ObjToString(), "删除成功") : Failed("删除失败");
            }
            return Failed("入参无效");
        }
        /// <summary>
        /// apache jemeter 压力测试
        /// 更新接口
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("ApacheTestUpdate")]
        public async Task<MessageModel<bool>> ApacheTestUpdate()
        {
            return Success(await _blogArticleServices.Update(new { bsubmitter = $"laozhang{DateTime.Now.Millisecond}", bID = 1 }), "更新成功");
        }
    }
}