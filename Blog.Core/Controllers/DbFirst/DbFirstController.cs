using Blog.Core.Model.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Core.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize(Permissions.Name)]
    public class DbFirstController : ControllerBase
    {
        private readonly MyContext myContext;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="myContext"></param>
        public DbFirstController(MyContext myContext)
        {
            this.myContext = myContext;
        }

        /// <summary>
        /// 获取 整体框架 文件
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public bool GetFrameFiles()
        {
            return FrameSeed.CreateModels(myContext)
                && FrameSeed.CreateIRepositorys(myContext)
                && FrameSeed.CreateIServices(myContext)
                && FrameSeed.CreateRepository(myContext)
                && FrameSeed.CreateServices(myContext)
                ;
        }


        /// <summary>
        /// 获取 Model 层文件
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public bool GetModelFiles()
        {
            return FrameSeed.CreateModels(myContext);
        }

        /// <summary>
        /// 获取 IRepository 层文件
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public bool GetIRepositoryFiles()
        {
            return FrameSeed.CreateIRepositorys(myContext);
        }

        /// <summary>
        /// 获取 IService 层文件
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public bool GetIServiceFiles()
        {
            return FrameSeed.CreateIServices(myContext);
        }

        /// <summary>
        /// 获取 Repository 层文件
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public bool GetRepositoryFiles()
        {
            return FrameSeed.CreateRepository(myContext);
        }

        /// <summary>
        /// 获取 Services 层文件
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public bool GetServicesFiles()
        {
            return FrameSeed.CreateServices(myContext);
        }

    }
}