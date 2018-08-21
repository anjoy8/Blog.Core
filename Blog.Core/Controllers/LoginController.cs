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


        #region Token
        /// <summary>
        /// 获取JWT，并存入缓存
        /// </summary>
        /// <param name="id">用户Id</param>
        /// <param name="sub">身份</param>
        /// <param name="expiresSliding">相对过期时间，单位为分</param>
        /// <param name="expiresAbsoulute">绝对过期时间，单位为天</param>
        /// <returns></returns>
        [HttpGet]
        [Route("Token")]
        public JsonResult GetJWTStr(long id=1, string sub="Admin", int expiresSliding = 30, int expiresAbsoulute = 30)
        {
           TokenModel tokenModel = new TokenModel();
            tokenModel.Uid = id;
            tokenModel.Sub = sub;

            DateTime d1 = DateTime.Now;
            DateTime d2 = d1.AddMinutes(expiresSliding);
            DateTime d3 = d1.AddDays(expiresAbsoulute);
            TimeSpan sliding = d2 - d1;
            TimeSpan absoulute = d3 - d1;

            string jwtStr = BlogCoreToken.IssueJWT(tokenModel, sliding, absoulute);
            return Json(jwtStr);
        }
        #endregion

    }
}