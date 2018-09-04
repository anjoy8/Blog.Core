using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog.Core.AuthHelper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Core.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Produces("application/json")]
    [Route("api/Login")]
    public class LoginController : Controller
    {

      


        #region 获取token的第二种方法
        /// <summary>
        /// 获取JWT的重写方法，推荐这种，注意在文件夹OverWrite下
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="sub">角色</param>
        /// <returns></returns>
        [HttpGet]
        [Route("Token2")]
        public JsonResult GetJWTStr(long id = 1, string sub = "Admin")
        {
            //这里就是用户登陆以后，通过数据库去调取数据，分配权限的操作
            TokenModelJWT tokenModel = new TokenModelJWT();
            tokenModel.Uid = id;
            tokenModel.Role = sub;

            string jwtStr = JwtHelper.IssueJWT(tokenModel);
            return Json(jwtStr);
        }
        #endregion

    }
}