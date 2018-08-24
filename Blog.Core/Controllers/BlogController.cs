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
        // GET: api/Blog
        /// <summary>
        /// Sum接口
        /// </summary>
        /// <param name="i">参数i</param>
        /// <param name="j">参数j</param>
        /// <returns></returns>
        [HttpGet]
        public int Get(int i, int j)
        {

            return i + j;
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
            IAdvertisementServices advertisementServices = new AdvertisementServices();

            return await advertisementServices.Query(d => d.Id == id);
        }


    }
}
