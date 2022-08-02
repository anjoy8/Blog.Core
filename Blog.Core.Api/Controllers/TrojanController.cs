using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blog.Core.Common;
using Blog.Core.Common.Extensions;
using Blog.Core.Common.Helper;
using Blog.Core.Common.HttpContextUser;
using Blog.Core.IServices;
using Blog.Core.IServices.BASE;
using Blog.Core.Model;
using Blog.Core.Model.Models;
using Blog.Core.Model.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Core.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize(Permissions.Name)]
    public class TrojanController : ControllerBase
    {
        private ITrojanUsersServices _trojanUsersServices;
        public IBaseServices<TrojanServers> _baseServicesServers;
        public IBaseServices<TrojanDetails> _baseServicesDetails;
        public IBaseServices<TrojanCusServers> _baseServicesCusServers;
        public IBaseServices<TrojanUrlServers> _baseServicesUrlServers;
        private IUser _user;
        public TrojanController(ITrojanUsersServices trojanUsersServices,IUser user
            , IBaseServices<TrojanServers> baseServicesServers
            , IBaseServices<TrojanDetails> baseServicesDetails
            , IBaseServices<TrojanCusServers> baseServicesCusServers
            , IBaseServices<TrojanUrlServers> baseServicesUrlServers)
        {
            _baseServicesDetails = baseServicesDetails;
            _baseServicesServers = baseServicesServers;
            _trojanUsersServices = trojanUsersServices;
            _baseServicesCusServers = baseServicesCusServers;
            _baseServicesUrlServers = baseServicesUrlServers;
            _user = user;
        }
        /// <summary>
        /// 获取Trojan用户
        /// </summary>
        /// <param name="pagination"></param>
        /// <returns></returns>
        [HttpGet] 
        public async Task<MessageModel<PageModel<TrojanUsers>>> GetUser([FromQuery]PaginationModel pagination)
        {
            return await GetAllUser(pagination);
        }
        /// <summary>
        /// 获取Trojan用户
        /// </summary>
        /// <param name="pagination"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<MessageModel<PageModel<TrojanUsers>>> GetAllUser([FromQuery]PaginationModel pagination)
        {
            var data = await _trojanUsersServices.QueryPage(pagination);
            if (data.data.Count > 0)
            {
                var ids = data.data.Select(t => t.id).ToList();
                var where = LinqHelper.True<TrojanDetails>();
                where = where.And(t => ids.Contains(t.userId)).And(t => t.calDate < DateTime.Now).And(t => t.calDate > DateTime.Now.AddMonths(-12));
                var userDetails = await _baseServicesDetails.Query(where);
                foreach (var trojanUser in data.data)
                {

                    var ls = from t in userDetails
                             where t.userId == trojanUser.id
                             group t by new { moth = t.calDate.ToString("yyyy-MM"), id = t.userId } into g
                             orderby g.Key.moth descending
                             select new TrojanUseDetailDto { userId = g.Key.id, moth = g.Key.moth, up = g.Sum(t => Convert.ToDecimal(t.upload)), down = g.Sum(t => Convert.ToDecimal(t.download)) };
                    var lsData = ls.ToList();
                    trojanUser.useList = lsData;
                }
            }
            return MessageModel<PageModel<TrojanUsers>>.Success("获取成功", data);
        }

        /// <summary>
        /// 获取Trojan用户-下拉列表用
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<MessageModel<DataTable>> GetAllTrojanUser()
        {
            var data = await _trojanUsersServices.QueryTable("select id,username from users");
            return MessageModel<DataTable>.Success("获取成功", data);
        }
        /// <summary>
        /// 添加Trojan用户
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost] 
        public async Task<MessageModel<object>> AddUser([FromBody]TrojanUsers user)
        {
            var find = await _trojanUsersServices.Query(t => t.username == user.username);
            if(find!=null && find.Count>0) return MessageModel<object>.Fail("用户名已存在");
            var pass = StringHelper.GetGUID();
            var passEcrypt = ShaHelper.Sha224(pass);
            //user.quota = 0;
            user.upload = 0;
            user.download = 0;
            user.password = passEcrypt;
            user.passwordshow = pass;
            var data = await _trojanUsersServices.Add(user);
            return MessageModel<object>.Success("添加成功", data);
        }
        /// <summary>
        /// 更新Trojan用户
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<MessageModel<object>> UpdateUser([FromBody]TrojanUsers user)
        {
            var find = await _trojanUsersServices.QueryById(user.id);
            if (find == null) return MessageModel<object>.Fail("用户名不存在");
            find.username = user.username;
            var data = await _trojanUsersServices.Update(find, new List<string> { "username" });
            return MessageModel<object>.Success("更新成功", data);
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="users"></param>
        /// <returns></returns>
        [HttpPut] 
        public async Task<MessageModel<string>> DelUser([FromBody]int[] users)
        { 
            var data = await _trojanUsersServices.Query(t => users.Contains(t.id));
            var list = data.Select(t => t.id.ToString()).ToArray();
            await _trojanUsersServices.DeleteByIds(list);
            return MessageModel<string>.Success("删除成功");
        }
        /// <summary>
        /// 重置流量
        /// </summary>
        /// <param name="users"></param>
        /// <returns></returns>
        [HttpPut] 
        public async Task<MessageModel<string>> ResetFlow([FromBody]int[] users)
        { 
            var data = await _trojanUsersServices.Query(t => users.Contains(t.id)); 
            foreach (var item in data)
            { 
                item.upload = 0;
                item.download = 0;
                await _trojanUsersServices.Update(item, new List<string> { "upload", "download" });
            }
            return MessageModel<string>.Success("重置流量成功");
        }
        /// <summary>
        /// 限制流量
        /// </summary>
        /// <param name="limit"></param> 
        /// <returns></returns>
        [HttpPut] 
        public async Task<MessageModel<string>> LimitFlow([FromBody] TrojanLimitFlowDto limit)
        { 
            var data = await _trojanUsersServices.Query(t => limit.users.Contains(t.id));
            foreach (var item in data)
            {
                item.quota = limit.quota;
                await _trojanUsersServices.Update(item, new List<string> { "quota" });
            }
            return MessageModel<string>.Success("限制流量成功");
        }
        /// <summary>
        /// 重置链接密码
        /// </summary>
        /// <param name="users"></param> 
        /// <returns></returns>
        [HttpPut] 
        public async Task<MessageModel<string>> ResetPass([FromBody]int[] users)
        { 
            var data = await _trojanUsersServices.Query(t => users.Contains(t.id));
            var pass = StringHelper.GetGUID();
            var passEcrypt = ShaHelper.Sha224(pass);
            foreach (var item in data)
            {
                item.password = passEcrypt;
                item.passwordshow = pass;
                await _trojanUsersServices.Update(item, new List<string> { "password" , "passwordshow" });
            }
            return MessageModel<string>.Success("重置链接密码成功");
        }
        /// <summary>
        /// 获取Trojan服务器
        /// </summary>
        /// <returns></returns> 
        [HttpGet]
        public async Task<MessageModel<List<TrojanServers>>> GetServers()
        {
            var data = await _baseServicesServers.Query();
            data = data.OrderBy(t => t.servername).ToList();
            return MessageModel<List<TrojanServers>>.Success("获取成功", data);
        }
        /// <summary>
        /// 获取拼接后的Trojan服务器
        /// </summary>
        /// <param name="id">passwordshow</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<MessageModel<TrojanServerSpliceDto>> GetSpliceServers(string id)
        {
            var data = await _baseServicesServers.Query();
            data = data.OrderBy(t => t.servername).ToList();
            var res = new TrojanServerSpliceDto();
            res.normalApi = Appsettings.app(new string[] { "trojan", "normalApi" }).ObjToString();
            res.clashApi = Appsettings.app(new string[] { "trojan", "clashApi" }).ObjToString();
            res.clashApiBackup = Appsettings.app(new string[] { "trojan", "clashApiBackup" }).ObjToString();
            foreach (var item in data)
            {
                var serverSplice = GetSplice(item, id);
                res.list.Add(new TrojanServerDto { name = item.servername, value = serverSplice });
            }
            return MessageModel<TrojanServerSpliceDto>.Success("获取成功", res); ;

        }
        /// <summary>
        /// 删除Trojan服务器
        /// </summary>
        /// <param name="servers"></param>
        /// <returns></returns> 
        [HttpPut]
        public async Task<MessageModel<List<TrojanServers>>> DelServers([FromBody]int[] servers)
        {
            var data = await _baseServicesServers.DeleteByIds(servers.Select(t=>t.ToString()).ToArray());
            if (data) 
                return MessageModel<List<TrojanServers>>.Success("删除成功");
            else
                return MessageModel<List<TrojanServers>>.Fail("删除失败");
        }
        /// <summary>
        /// 更新Trojan服务器
        /// </summary>
        /// <param name="server"></param>
        /// <returns></returns> 
        [HttpPut]
        public async Task<MessageModel<List<TrojanServers>>> UpdateServers(TrojanServers server)
        {
            var data = await _baseServicesServers.Update(server);
            return MessageModel<List<TrojanServers>>.Success("更新成功");
        }
        /// <summary>
        /// 添加Trojan服务器
        /// </summary>
        /// <param name="server"></param>
        /// <returns></returns> 
        [HttpPost]
        public async Task<MessageModel<List<TrojanServers>>> AddServers(TrojanServers server)
        {
            var data = await _baseServicesServers.Add(server);
            return MessageModel<List<TrojanServers>>.Success("添加成功");
        }

        /// <summary>
        /// 获取Cus服务器
        /// </summary>
        /// <returns></returns> 
        [HttpGet]
        public async Task<MessageModel<List<TrojanCusServers>>> GetCusServers()
        {
            var data = await _baseServicesCusServers.Query();
            data = data.OrderBy(t => t.servername).ToList();
            return MessageModel<List<TrojanCusServers>>.Success("获取成功", data);
        }
        /// <summary>
        /// 删除Cus服务器
        /// </summary>
        /// <param name="servers"></param>
        /// <returns></returns> 
        [HttpPut]
        public async Task<MessageModel<List<TrojanCusServers>>> DelCusServers([FromBody] int[] servers)
        {
            var data = await _baseServicesCusServers.DeleteByIds(servers.Select(t => t.ToString()).ToArray());
            if (data)
                return MessageModel<List<TrojanCusServers>>.Success("删除成功");
            else
                return MessageModel<List<TrojanCusServers>>.Fail("删除失败");
        }
        /// <summary>
        /// 更新Cus服务器
        /// </summary>
        /// <param name="server"></param>
        /// <returns></returns> 
        [HttpPut]
        public async Task<MessageModel<List<TrojanCusServers>>> UpdateCusServers(TrojanCusServers server)
        {
            var data = await _baseServicesCusServers.Update(server);
            return MessageModel<List<TrojanCusServers>>.Success("更新成功");
        }
        /// <summary>
        /// 添加Cus服务器
        /// </summary>
        /// <param name="server"></param>
        /// <returns></returns> 
        [HttpPost]
        public async Task<MessageModel<List<TrojanCusServers>>> AddCusServers(TrojanCusServers server)
        {
            var data = await _baseServicesCusServers.Add(server);
            return MessageModel<List<TrojanCusServers>>.Success("添加成功");
        }


        /// <summary>
        /// 获取Url服务器
        /// </summary>
        /// <returns></returns> 
        [HttpGet]
        public async Task<MessageModel<List<TrojanUrlServers>>> GetUrlServers()
        {
            var data = await _baseServicesUrlServers.Query();
            data = data.OrderBy(t => t.servername).ToList();
            return MessageModel<List<TrojanUrlServers>>.Success("获取成功", data);
        }
        /// <summary>
        /// 删除Url服务器
        /// </summary>
        /// <param name="servers"></param>
        /// <returns></returns> 
        [HttpPut]
        public async Task<MessageModel<List<TrojanUrlServers>>> DelUrlServers([FromBody] int[] servers)
        {
            var data = await _baseServicesUrlServers.DeleteByIds(servers.Select(t => t.ToString()).ToArray());
            if (data)
                return MessageModel<List<TrojanUrlServers>>.Success("删除成功");
            else
                return MessageModel<List<TrojanUrlServers>>.Fail("删除失败");
        }
        /// <summary>
        /// 更新Url服务器
        /// </summary>
        /// <param name="server"></param>
        /// <returns></returns> 
        [HttpPut]
        public async Task<MessageModel<List<TrojanUrlServers>>> UpdateUrlServers(TrojanUrlServers server)
        {
            var data = await _baseServicesUrlServers.Update(server);
            return MessageModel<List<TrojanUrlServers>>.Success("更新成功");
        }
        /// <summary>
        /// 添加Url服务器
        /// </summary>
        /// <param name="server"></param>
        /// <returns></returns> 
        [HttpPost]
        public async Task<MessageModel<List<TrojanUrlServers>>> AddUrlServers(TrojanUrlServers server)
        {
            var data = await _baseServicesUrlServers.Add(server);
            return MessageModel<List<TrojanUrlServers>>.Success("添加成功");
        }
        private string GetSplice(TrojanServers item,string passwordshow)
        {
            if ("0".Equals(item.servertype))
                return $"trojan://{passwordshow}@{item.serveraddress}:{item.serverport}?allowinsecure=0&tfo=0&peer={(string.IsNullOrEmpty(item.serverpeer) ? item.serverpeer : item.serveraddress)}#{item.servername}";
            else if ("1".Equals(item.servertype))
                return $"trojan://{passwordshow}@{item.serveraddress}:{item.serverport}?wspath={item.serverpath}&ws=1&peer={(string.IsNullOrEmpty(item.serverpeer) ? item.serverpeer : item.serveraddress)}#{item.servername}";
            else
                return $"servertype:({item.servertype})错误";
        }
        private List<string> GetSplice(List<TrojanServers> items, string passwordshow)
        {
            List<string> ls = new List<string>();
            foreach (var item in items)
            {
                ls.Add(GetSplice(item, passwordshow));
            }
            return ls;
        }
        /// <summary>
        /// 获取订阅数据
        /// </summary>
        /// <param name="id">链接密码</param>
        /// <param name="isUseBase64">是否使用base64加密</param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<string> RSS(string id,bool isUseBase64=true)
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                var user = (await _trojanUsersServices.Query(t => t.passwordshow == id)).FirstOrDefault();
                if (user == null) throw new Exception("用户不存在");
                var data = await _baseServicesServers.Query(t => (t.userid == user.id || t.userid <= 0) && t.serverenable);
                if (data != null)
                {
                    data = data.OrderBy(t => t.servername).ToList();
                    foreach (var item in data)
                    {
                        sb.AppendLine(GetSplice(item, user.passwordshow));
                    }
                }
                var cusData = await _baseServicesCusServers.Query(t=> (t.userid == user.id || t.userid <=0) && t.serverenable);
                if (cusData != null)
                {
                    cusData = cusData.OrderBy(t => t.servername).ToList();
                    foreach (var item in cusData)
                    {
                        sb.AppendLine(item.serveraddress);
                    }
                }
                var urlData = await _baseServicesUrlServers.Query(t => (t.userid == user.id || t.userid <= 0) && t.serverenable);
                if (urlData != null)
                {
                    urlData = urlData.OrderBy(t => t.servername).ToList();
                    foreach (var item in urlData)
                    {
                        try
                        {
                            var urlStrObj =  await HttpHelper.GetAsync(item.serveraddress);
                            var lines = "";
                            try
                            {
                                lines = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(urlStrObj));
                            }
                            catch (Exception)
                            {
                                lines = urlStrObj;
                            }
                            finally
                            {
                                sb.AppendLine(lines);
                            }
                        }
                        catch (Exception ex)
                        {
                            sb.AppendLine($"trojan://xxxxxx@xxxxxx.xx:443?allowinsecure=0&tfo=0#{ex.Message}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                sb.AppendLine($"trojan://xxxxxx@xxxxxx.xx:443?allowinsecure=0&tfo=0#{ex.Message}");
            }
            if (isUseBase64)
            {
                return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(sb.ToString()));
            }
            else{
                return sb.ToString();
            }
        }
    }
}