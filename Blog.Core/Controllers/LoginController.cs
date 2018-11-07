using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog.Core.AuthHelper;
using Blog.Core.IServices;
using Microsoft.AspNetCore.Cors;
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

        IsysUserInfoServices sysUserInfoServices;
        IUserRoleServices userRoleServices;
        IRoleServices roleServices;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="sysUserInfoServices"></param>
        /// <param name="userRoleServices"></param>
        /// <param name="roleServices"></param>
        public LoginController(IsysUserInfoServices sysUserInfoServices, IUserRoleServices userRoleServices, IRoleServices roleServices)
        {
            this.sysUserInfoServices = sysUserInfoServices;
            this.userRoleServices = userRoleServices;
            this.roleServices = roleServices;
        }


        #region 获取token的第二种方法
        /// <summary>
        /// 获取JWT的方法
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="sub">角色</param>
        /// <returns></returns>
        [HttpGet]
        [Route("Token")]
        public async Task<object> GetJWTStr(string name, string pass)
        {
            string jwtStr = string.Empty;
            bool suc = false;
            //这里就是用户登陆以后，通过数据库去调取数据，分配权限的操作
            //这里直接写死了

            var user = await sysUserInfoServices.GetUserRoleNameStr(name, pass);
            if (user != null)
            {

                TokenModelJWT tokenModel = new TokenModelJWT();
                tokenModel.Uid = 1;
                tokenModel.Role = user;

                jwtStr = JwtHelper.IssueJWT(tokenModel);
                suc = true;
            }
            else
            {
                jwtStr = "login fail!!!";
            }

            return Ok(new
            {
                success = suc,
                token = jwtStr
            });
        }



        [HttpGet]
        [Route("GetTokenNuxt")]
        public async Task<object> GetJWTStrForNuxt(string name, string pass)
        {
            string jwtStr = string.Empty;
            bool suc = false;
            //这里就是用户登陆以后，通过数据库去调取数据，分配权限的操作
            //这里直接写死了
            if (name == "admins" && pass == "admins")
            {
                TokenModelJWT tokenModel = new TokenModelJWT();
                tokenModel.Uid = 1;
                tokenModel.Role = "Admin";

                jwtStr = JwtHelper.IssueJWT(tokenModel);
                suc = true;
            }
            else
            {
                jwtStr = "login fail!!!";
            }
            var result = new
            {
                data = new { success = suc, token = jwtStr }
            };

            return Ok(new
            {
                success = suc,
                data = new { success = suc, token = jwtStr }
            });
        }
        #endregion


        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="sub"></param>
        /// <param name="expiresSliding"></param>
        /// <param name="expiresAbsoulute"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("jsonp")]
        public void Getjsonp(string callBack, long id = 1, string sub = "Admin", int expiresSliding = 30, int expiresAbsoulute = 30)
        {
            TokenModelJWT tokenModel = new TokenModelJWT();
            tokenModel.Uid = id;
            tokenModel.Role = sub;

            string jwtStr = JwtHelper.IssueJWT(tokenModel);

            string response = string.Format("\"value\":\"{0}\"", jwtStr);
            string call = callBack + "({" + response + "})";
            Response.WriteAsync(call);
        }
    }
}