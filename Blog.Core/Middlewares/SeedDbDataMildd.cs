using Blog.Core.Common;
using Blog.Core.Model.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Blog.Core.Middlewares
{
    /// <summary>
    /// 中间件
    /// 记录IP请求数据
    /// </summary>
    public class SeedDbDataMildd
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly RequestDelegate _next;
        private readonly MyContext _myContext;
        private readonly IWebHostEnvironment _env;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="next"></param>
        /// <param name="myContext"></param>
        /// <param name="env"></param>
        public SeedDbDataMildd(RequestDelegate next, MyContext myContext, IWebHostEnvironment env)
        {
            _next = next;
            _myContext = myContext;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (Appsettings.app("AppSettings", "SeedDBEnabled").ObjToBool() || Appsettings.app("AppSettings", "SeedDBDataEnabled").ObjToBool())
            {
                DBSeed.SeedAsync(_myContext, _env.WebRootPath).Wait();
                await _next(context);
            }
            else
            {
                await _next(context);
            }
        }
    }
}

