using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    //[Authorize(Policy ="Admin")]
    public class BlogController : Controller
    {
        IAdvertisementServices advertisementServices;
        IBlogArticleServices blogArticleServices;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="advertisementServices"></param>
        public BlogController(IAdvertisementServices advertisementServices, IBlogArticleServices blogArticleServices)
        {
            this.advertisementServices = advertisementServices;
            this.blogArticleServices = blogArticleServices;
        }
        // GET: api/Blog/5
        /// <summary>
        /// 根据id获取数据
        /// </summary>
        /// <param name="id">参数id</param>
        /// <returns></returns>
        [HttpGet("{id}", Name = "Get")]
        public async Task<List<Advertisement>> Get(int id)
        {
            //IAdvertisementServices advertisementServices = new AdvertisementServices();//需要引用两个命名空间Blog.Core.IServices;Blog.Core.Services;
            var testBlogDI =await blogArticleServices.Query(d => d.bID == id);

            return await advertisementServices.Query(d => d.Id == id);
        }


    }
}
