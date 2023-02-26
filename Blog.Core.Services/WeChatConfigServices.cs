using Blog.Core.Common;
using Blog.Core.Common.Helper;
using Blog.Core.IRepository.Base;
using Blog.Core.IServices;
using Blog.Core.Model;
using Blog.Core.Model.Models;
using Blog.Core.Model.ViewModels;
using Blog.Core.Services.BASE;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Blog.Core.Repository.UnitOfWorks;

namespace Blog.Core.Services
{
    /// <summary>
	/// WeChatConfigServices
	/// </summary>
    public class WeChatConfigServices : BaseServices<WeChatConfig>, IWeChatConfigServices
    {
        readonly IUnitOfWorkManage _unitOfWorkManage;
        readonly ILogger<WeChatConfigServices> _logger;
        public WeChatConfigServices(IUnitOfWorkManage unitOfWorkManage, ILogger<WeChatConfigServices> logger)
        {
            this._unitOfWorkManage = unitOfWorkManage;
            this._logger = logger;
        }  
        public async Task<MessageModel<WeChatApiDto>> GetToken(string publicAccount)
        { 
            var config = await this.QueryById(publicAccount);
            if (config == null) MessageModel<string>.Success($"公众号{publicAccount}未维护至系统");//还没过期,直接返回 
            if (config.tokenExpiration > DateTime.Now)
            {
                //再次判断token在微信服务器是否正确
                var wechatIP = await WeChatHelper.GetWechatIP(config.token);
                if (wechatIP.errcode == 0)
                    MessageModel<WeChatApiDto>.Success("", new WeChatApiDto { access_token = config.token });//还没过期,直接返回
            }
            //过期了,重新获取
            var data = await WeChatHelper.GetToken(config.appid, config.appsecret);
            if (data.errcode.Equals(0))
            {
                config.token = data.access_token;
                config.tokenExpiration = DateTime.Now.AddSeconds(data.expires_in);
                await this.Update(config);
                return MessageModel<WeChatApiDto>.Success("",data);
            }
            else
            {
                return MessageModel<WeChatApiDto>.Fail($"\r\n获取Token失败\r\n错误代码:{data.errcode}\r\n错误信息:{data.errmsg}"); 
            }
        }
        public async Task<MessageModel<WeChatApiDto>> RefreshToken(string publicAccount)
        {
            var config = await this.QueryById(publicAccount);
            if (config == null) MessageModel<string>.Success($"公众号{publicAccount}未维护至系统");//还没过期,直接返回  
            //过期了,重新获取
            var data = await WeChatHelper.GetToken(config.appid, config.appsecret);
            if (data.errcode.Equals(0))
            {
                config.token = data.access_token;
                config.tokenExpiration = DateTime.Now.AddSeconds(data.expires_in);
                await this.Update(config);
                return MessageModel<WeChatApiDto>.Success("", data);
            }
            else
            {
                return MessageModel<WeChatApiDto>.Fail($"\r\n获取Token失败\r\n错误代码:{data.errcode}\r\n错误信息:{data.errmsg}");
            }
        }
        public async Task<MessageModel<WeChatApiDto>> GetTemplate(string id)
        {
            var res = await GetToken(id);
            if (!res.success) return res;
            var data = await WeChatHelper.GetTemplate(res.response.access_token);
            if (data.errcode.Equals(0))
            {
               return MessageModel<WeChatApiDto>.Success("", data); 
            }
            else
            {
               return MessageModel<WeChatApiDto>.Success($"\r\n获取模板失败\r\n错误代码:{data.errcode}\r\n错误信息:{data.errmsg}", data); 
            }
        }
        /// <summary>
        /// 获取菜单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<MessageModel<WeChatApiDto>> GetMenu(string id)
        {
            var res = await GetToken(id);
            if (!res.success) return res;
            var data = await WeChatHelper.GetMenu(res.response.access_token);
            if (data.errcode.Equals(0))
            {
                return MessageModel<WeChatApiDto>.Success("", data);
            }
            else
            {
                return MessageModel<WeChatApiDto>.Success($"\r\n获取菜单失败\r\n错误代码:{data.errcode}\r\n错误信息:{data.errmsg}", data);
            }
        }
        public async Task<MessageModel<WeChatApiDto>> GetSubUsers(string id)
        {
            var res = await GetToken(id);
            if (!res.success) return res;
            var data = await WeChatHelper.GetUsers(res.response.access_token);
            if (data.errcode.Equals(0))
            {
                data.users = new List<WeChatApiDto>();
                foreach (var openid in data.data.openid)
                {
                    data.users.Add(await WeChatHelper.GetUserInfo(res.response.access_token, openid));
                }
                return MessageModel<WeChatApiDto>.Success("", data);
            }
            else
            {
                return MessageModel<WeChatApiDto>.Success($"\r\n获取订阅用户失败\r\n错误代码:{data.errcode}\r\n错误信息:{data.errmsg}", data);
            }
        }
        public async Task<MessageModel<WeChatApiDto>> GetSubUser(string id,string openid)
        {
            var res = await GetToken(id); 
            if (!res.success) return res;
            var data = await WeChatHelper.GetUserInfo(res.response.access_token,openid);
            if (data.errcode.Equals(0))
            {
                return MessageModel<WeChatApiDto>.Success("", data);
            }
            else
            {
                return MessageModel<WeChatApiDto>.Success($"\r\n获取订阅用户失败\r\n错误代码:{data.errcode}\r\n错误信息:{data.errmsg}", data);
            }
        }
        public async Task<string> Valid(WeChatValidDto validDto,string body)
        { 
            WeChatXMLDto weChatData = null;
            string objReturn = null;
            try
            {
                _logger.LogInformation("会话开始");
                if (string.IsNullOrEmpty(validDto.publicAccount)) throw new Exception("没有微信公众号唯一标识id数据");
                var config = await QueryById(validDto.publicAccount);
                if (config == null) throw new Exception($"公众号不存在=>{validDto.publicAccount}");
                _logger.LogInformation(JsonHelper.GetJSON<WeChatValidDto>(validDto));
                var token = config.interactiveToken;//验证用的token 和access_token不一样
                string[] arrTmp = { token, validDto.timestamp, validDto.nonce };
                Array.Sort(arrTmp);
                string combineString = string.Join("", arrTmp);
                string encryption = MD5Helper.Sha1(combineString).ToLower();

                _logger.LogInformation(
                    $"来自公众号:{validDto.publicAccount}\r\n" +
                    $"微信signature:{validDto.signature}\r\n" +
                    $"微信timestamp:{validDto.timestamp}\r\n" +
                    $"微信nonce:{validDto.nonce}\r\n" +
                    $"合并字符串:{combineString}\r\n" +
                    $"微信服务器signature:{validDto.signature}\r\n" +
                    $"本地服务器signature:{encryption}"
                );
                if (encryption == validDto.signature)
                {
                    //判断是首次验证还是交互?
                    if (string.IsNullOrEmpty(validDto.echoStr))
                    {
                        //非首次验证 
                        weChatData = XmlHelper.ParseFormByXml<WeChatXMLDto>(body, "xml");
                        weChatData.publicAccount = validDto.publicAccount;
                        objReturn = await HandleWeChat(weChatData);
                    }
                    else
                    {
                        //首次接口地址验证 
                        objReturn = validDto.echoStr;
                    }
                }
                else
                {
                    objReturn = "签名验证失败";
                }

            }
            catch (Exception ex)
            {
                _logger.LogInformation($"会话出错(信息)=>\r\n{ex.Message}");
                _logger.LogInformation($"会话出错(堆栈)=>\r\n{ex.StackTrace}");
                //返回错误给用户 
                objReturn = string.Format(@$"<xml><ToUserName><![CDATA[{weChatData?.FromUserName}]]></ToUserName>
                                                    <FromUserName><![CDATA[{weChatData?.ToUserName}]]></FromUserName>
                                                    <CreateTime>{DateTime.Now.Ticks.ToString()}</CreateTime>
                                                    <MsgType><![CDATA[text]]></MsgType>
                                                    <Content><![CDATA[{ex.Message}]]></Content></xml>");
            }
            finally
            {
                _logger.LogInformation($"微信get数据=>\r\n{JsonHelper.GetJSON<WeChatValidDto>(validDto)}");
                _logger.LogInformation($"微信post数据=>\r\n{body}");
                _logger.LogInformation($"返回微信数据=>\r\n{objReturn}");
                _logger.LogInformation($"会话结束");
            }
            return objReturn;
        }
        public async Task<MessageModel<WeChatResponseUserInfo>> GetQRBind(WeChatUserInfo info)
        { 
            var res = await GetToken(info?.id);
            if (!res.success) return MessageModel<WeChatResponseUserInfo>.Fail(res.msg); 
            var push = new WeChatQRDto
            {
                expire_seconds = 604800,
                action_name = "QR_STR_SCENE",
                action_info = new WeChatQRActionDto
                {
                    scene = new WeChatQRActionInfoDto
                    {
                        scene_str = $"bind_{info?.id}"
                    }
                }
            };
            WeChatResponseUserInfo reData = new WeChatResponseUserInfo();
            reData.companyCode = info.companyCode;
            reData.id = info.id;
            var pushJosn = JsonHelper.GetJSON<WeChatQRDto>(push);
            var data = await WeChatHelper.GetQRCode(res.response.access_token, pushJosn);
            WeChatQR weChatQR = new WeChatQR
            {
                QRbindCompanyID = info.companyCode,
                QRbindJobID = info.userID,
                QRbindJobNick = info.userNick,
                QRcrateTime = DateTime.Now,
                QRpublicAccount = info.id,
                QRticket = data.ticket
            }; 
            data.id = info.userID;
            await this.BaseDal.Db.Insertable<WeChatQR>(weChatQR).ExecuteCommandAsync();
            reData.usersData= data;
            return MessageModel<WeChatResponseUserInfo>.Success("获取二维码成功", reData);
        }
        public async Task<MessageModel<WeChatResponseUserInfo>> PushCardMsg(WeChatCardMsgDataDto msg,string ip)
        { 
            var bindUser = await BaseDal.Db.Queryable<WeChatSub>().Where(t => t.SubFromPublicAccount == msg.info.id && t.CompanyID == msg.info.companyCode && t.IsUnBind == false && msg.info.userID.Contains(t.SubJobID)).SingleAsync();
            if (bindUser == null)
                return MessageModel<WeChatResponseUserInfo>.Fail("用户不存在或者已经解绑!");
            var res = await GetToken(msg?.info?.id);
            if(!res.success)
                return MessageModel<WeChatResponseUserInfo>.Fail(res.msg);
            WeChatResponseUserInfo reData = new WeChatResponseUserInfo();
            reData.companyCode = msg.info.companyCode;
            reData.id = msg.info.id;  
            try
            {
                var pushData = new WeChatPushCardMsgDto
                {
                    template_id = msg.cardMsg.template_id,
                    url = msg.cardMsg.url,
                    touser = bindUser.SubUserOpenID,
                    data = new WeChatPushCardMsgDetailDto
                    {
                        first = new WeChatPushCardMsgValueColorDto
                        {
                            value = msg.cardMsg.first,
                            color = msg.cardMsg.color1
                        },
                        keyword1 = new WeChatPushCardMsgValueColorDto
                        {
                            value = msg.cardMsg.keyword1,
                            color = msg.cardMsg.color1
                        },
                        keyword2 = new WeChatPushCardMsgValueColorDto
                        {
                            value = msg.cardMsg.keyword2,
                            color = msg.cardMsg.color2
                        },
                        keyword3 = new WeChatPushCardMsgValueColorDto
                        {
                            value = msg.cardMsg.keyword3,
                            color = msg.cardMsg.color3
                        },
                        keyword4 = new WeChatPushCardMsgValueColorDto
                        {
                            value = msg.cardMsg.keyword4,
                            color = msg.cardMsg.color4
                        },
                        keyword5 = new WeChatPushCardMsgValueColorDto
                        {
                            value = msg.cardMsg.keyword5,
                            color = msg.cardMsg.color5
                        },
                        remark = new WeChatPushCardMsgValueColorDto
                        {
                            value = msg.cardMsg.remark,
                            color = msg.cardMsg.colorRemark
                        }
                    }
                };
                var pushJson = JsonHelper.GetJSON<WeChatPushCardMsgDto>(pushData);
                var data = await WeChatHelper.SendCardMsg(res.response.access_token, pushJson);
                reData.usersData = data;
                try
                {
                    var pushLog = new WeChatPushLog
                    {
                        PushLogCompanyID = msg.info.companyCode,
                        PushLogPublicAccount = msg.info.id,
                        PushLogContent = pushJson,
                        PushLogOpenid = bindUser.SubUserOpenID,
                        PushLogToUserID = bindUser.SubJobID,
                        PushLogStatus = data.errcode == 0 ? "Y" : "N",
                        PushLogRemark = data.errmsg,
                        PushLogTime = DateTime.Now,
                        PushLogTemplateID = msg.cardMsg.template_id,
                        PushLogIP = ip
                    };
                    await BaseDal.Db.Insertable<WeChatPushLog>(pushLog).ExecuteCommandAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogInformation($"记录失败\r\n{ex.Message}\r\n{ex.StackTrace}");
                }
                if (reData.usersData.errcode.Equals(0))
                {
                    return MessageModel<WeChatResponseUserInfo>.Success("卡片消息推送成功", reData);
                }
                else
                {
                    return MessageModel<WeChatResponseUserInfo>.Success("卡片消息推送失败", reData);
                }
                
            }
            catch (Exception ex)
            { 
                return MessageModel<WeChatResponseUserInfo>.Success($"卡片消息推送错误=>{ex.Message}", reData); 
            }
            
        }

        public async Task<MessageModel<WeChatApiDto>> PushTxtMsg(WeChatPushTestDto msg) {
            var res = await GetToken(msg.selectWeChat);
            if (!res.success) return res;
            var token = res.response.access_token;
            if (msg.selectBindOrSub.Equals("sub"))
            { 
                return await PushText(token, msg);
            }
            else
            {
                MessageModel<WeChatApiDto> messageModel = new MessageModel<WeChatApiDto>();
                messageModel.success = true;
                //绑定用户
                if (msg.selectOperate.Equals("one"))
                {
                    //发送单个 
                    var usrs = BaseDal.Db.Queryable<WeChatSub>().Where(t => t.SubFromPublicAccount.Equals(msg.selectWeChat) && t.CompanyID.Equals(msg.selectCompany) && t.SubJobID.Equals(msg.selectUser)).ToList();
                    foreach (var item in usrs)
                    {
                        msg.selectUser = item.SubUserOpenID;
                        var info = await PushText(token, msg);
                        if (!info.success)
                        {
                            messageModel.success = false;
                        }
                        messageModel.msg += info.msg;
                    }
                }
                else
                {
                    //发送所有
                    var usrs = BaseDal.Db.Queryable<WeChatSub>().Where(t => t.SubFromPublicAccount.Equals(msg.selectWeChat) && t.CompanyID.Equals(msg.selectCompany)).ToList();
                    foreach (var item in usrs)
                    {
                        msg.selectUser = item.SubUserOpenID;
                        var info = await PushText(token, msg);
                        if (!info.success)
                        {
                            messageModel.success = false;
                        }
                        messageModel.msg += info.msg;
                    }
                }
                return messageModel;
            }
            
        }
        public async Task<MessageModel<WeChatApiDto>> PushText(string token,WeChatPushTestDto msg) {

            object data = null; ;
            WeChatApiDto pushres = null; ;
            //订阅用户  
            switch (msg.selectMsgType)
            {
                case "text":
                    //发送文本 
                    data = new
                    {
                        filter = new
                        {
                            is_to_all = msg.selectOperate.Equals("one") ? false : true,
                            tag_id = 0,
                        },
                        touser = msg.selectUser,
                        msgtype = msg.selectMsgType,
                        text = new
                        {
                            content = msg.textContent.text
                        }
                    };

                    if (msg.selectOperate.Equals("one"))
                    {
                        pushres = await WeChatHelper.SendMsg(token, JsonHelper.ObjToJson(data));
                    }
                    else
                    {
                        pushres = await WeChatHelper.SendMsgToAll(token, JsonHelper.ObjToJson(data));
                    }
                    break;
                case "image":
                    //发送图片 
                    data = new
                    {
                        filter = new
                        {
                            is_to_all = msg.selectOperate.Equals("one") ? false : true,
                            tag_id = 0,
                        },
                        touser = msg.selectUser,
                        msgtype = msg.selectMsgType,
                        images = new
                        {
                            media_ids = new List<string> {
                                msg.pictureContent.pictureMediaID
                            },
                            recommend = "xxx",
                            need_open_comment = 1,
                            only_fans_can_comment = 0
                        }
                    };
                    if (msg.selectOperate.Equals("one"))
                    {
                        pushres = await WeChatHelper.SendMsg(token, JsonHelper.ObjToJson(data));
                    }
                    else
                    {
                        pushres = await WeChatHelper.SendMsgToAll(token, JsonHelper.ObjToJson(data));
                    }
                    break;
                case "voice":
                    //发送音频
                    data = new
                    {
                        filter = new
                        {
                            is_to_all = msg.selectOperate.Equals("one") ? false : true,
                            tag_id = 0,
                        },
                        touser = msg.selectUser,
                        msgtype = msg.selectMsgType,
                        voice = new
                        {
                            media_id = msg.voiceContent.voiceMediaID
                        }
                    };
                    if (msg.selectOperate.Equals("one"))
                    {
                        pushres = await WeChatHelper.SendMsg(token, JsonHelper.ObjToJson(data));
                    }
                    else
                    {
                        pushres = await WeChatHelper.SendMsgToAll(token, JsonHelper.ObjToJson(data));
                    }
                    break;
                case "mpvideo":
                    //发送视频
                    data = new
                    {
                        filter = new
                        {
                            is_to_all = msg.selectOperate.Equals("one") ? false : true,
                            tag_id = 0,
                        },
                        touser = msg.selectUser,
                        msgtype = msg.selectMsgType,
                        mpvideo = new
                        {
                            media_id = msg.videoContent.videoMediaID,
                        }
                    };
                    if (msg.selectOperate.Equals("one"))
                    {
                        pushres = await WeChatHelper.SendMsg(token, JsonHelper.ObjToJson(data));
                    }
                    else
                    {
                        pushres = await WeChatHelper.SendMsgToAll(token, JsonHelper.ObjToJson(data));
                    }
                    break;
                default:
                    pushres = new WeChatApiDto() { errcode = -1, errmsg = $"未找到推送类型{msg.selectMsgType}" };
                    break;
            }
            if (pushres.errcode.Equals(0))
            {
                return MessageModel<WeChatApiDto>.Success("推送成功", pushres);

            }
            else
            {
                return MessageModel<WeChatApiDto>.Fail($"\r\n推送失败\r\n错误代码:{pushres.errcode}\r\n错误信息:{pushres.errmsg}", pushres);
            }
        }
        public async Task<MessageModel<WeChatApiDto>> UpdateMenu(WeChatApiDto menu)
        {
            WeChatHelper.ConverMenuButtonForEvent(menu);
            var res = await GetToken(menu.id);
            if (!res.success) return res;
            var data = await WeChatHelper.SetMenu(res.response.access_token, JsonHelper.ObjToJson(menu.menu));
            if (data.errcode.Equals(0))
            {

                return MessageModel<WeChatApiDto>.Success("更新成功", data);
            }
            else
            { 
                return MessageModel<WeChatApiDto>.Success("更新失败", data);
            }
        }
        public async Task<MessageModel<WeChatResponseUserInfo>> GetBindUserInfo(WeChatUserInfo info)
        {
            var bindUser = await BaseDal.Db.Queryable<WeChatSub>().Where(t => t.SubFromPublicAccount == info.id && t.CompanyID == info.companyCode && info.userID.Equals(t.SubJobID) && t.IsUnBind == false ).FirstAsync();
            if (bindUser == null) return MessageModel<WeChatResponseUserInfo>.Fail("用户不存在或者已经解绑!");
            var res = await GetToken(info.id);
            if(!res.success) return MessageModel<WeChatResponseUserInfo>.Fail(res.msg);
            var token = res.response.access_token;
            WeChatResponseUserInfo reData = new WeChatResponseUserInfo();
            reData.companyCode = info.companyCode;
            reData.id = info.id;
            var data = await WeChatHelper.GetUserInfo(token, bindUser.SubUserOpenID);
            reData.usersData = data;
            if (data.errcode.Equals(0))
            {
                return MessageModel<WeChatResponseUserInfo>.Success("用户信息获取成功", reData);
            }
            else
            {
                return MessageModel<WeChatResponseUserInfo>.Fail("用户信息获取失败", reData);
            } 
        }
        public async Task<MessageModel<WeChatResponseUserInfo>> UnBind(WeChatUserInfo info)
        { 
            var bindUser = await BaseDal.Db.Queryable<WeChatSub>().Where(t => t.SubFromPublicAccount == info.id && t.CompanyID == info.companyCode && info.userID.Equals(t.SubJobID) && t.IsUnBind == false ).FirstAsync();
            if (bindUser == null) return MessageModel<WeChatResponseUserInfo>.Fail("用户不存在或者已经解绑!");
            WeChatResponseUserInfo reData = new WeChatResponseUserInfo();
            reData.companyCode = info.companyCode;
            reData.id = info.id;
            bindUser.IsUnBind = true;
            bindUser.SubUserRefTime = DateTime.Now;
            await BaseDal.Db.Updateable<WeChatSub>(bindUser).UpdateColumns(t=> new{ t.IsUnBind,t.SubUserRefTime}).ExecuteCommandAsync();
            return MessageModel<WeChatResponseUserInfo>.Success("用户解绑成功", reData);
        }

        public async Task<string> HandleWeChat(WeChatXMLDto weChat)
        {

            switch (weChat.MsgType)
            {
                case "text":
                    return await HandText(weChat);
                case "image":
                    return await HandImage(weChat);
                case "voice":
                    return await HandVoice(weChat);
                case "shortvideo":
                    return await HandShortvideo(weChat);
                case "location":
                    return await HandLocation(weChat);
                case "link":
                    return await HandLink(weChat);
                case "event":
                    return await HandEvent(weChat);
                default:
                    return await Task.Run(() =>
                    {
                        return @$"<xml><ToUserName><![CDATA[{weChat.FromUserName}]]></ToUserName>
                                <FromUserName><![CDATA[{weChat.ToUserName}]]></FromUserName>
                                <CreateTime>{DateTime.Now.Ticks.ToString()}</CreateTime>
                                <MsgType><![CDATA[text]]></MsgType>
                                <Content><![CDATA[处理失败,没有找到消息类型=>{weChat.MsgType}]]></Content></xml>";
                    });
            }

        }
        /// <summary>
        /// 处理文本
        /// </summary>
        /// <param name="weChat"></param>
        /// <returns></returns>
        private async Task<string> HandText(WeChatXMLDto weChat)
        {
            return await Task.Run(() =>
            {
                return @$"<xml><ToUserName><![CDATA[{weChat.FromUserName}]]></ToUserName>
                                <FromUserName><![CDATA[{weChat.ToUserName}]]></FromUserName>
                                <CreateTime>{DateTime.Now.Ticks.ToString()}</CreateTime>
                                <MsgType><![CDATA[text]]></MsgType>
                                <Content><![CDATA[我收到了文本=>{weChat.Content}]]></Content></xml>";
            });
        }
        /// <summary>
        /// 处理图片
        /// </summary>
        /// <param name="weChat"></param>
        /// <returns></returns>
        private async Task<string> HandImage(WeChatXMLDto weChat)
        {
            return await Task.Run(() =>
            {
                return @$"<xml><ToUserName><![CDATA[{weChat.FromUserName}]]></ToUserName>
                                <FromUserName><![CDATA[{weChat.ToUserName}]]></FromUserName>
                                <CreateTime>{DateTime.Now.Ticks.ToString()}</CreateTime>
                                <MsgType><![CDATA[text]]></MsgType>
                                <Content><![CDATA[我收到了图片=>{weChat.PicUrl}]]></Content></xml>";
            });
        }
        /// <summary>
        /// 处理声音
        /// </summary>
        /// <param name="weChat"></param>
        /// <returns></returns>
        private async Task<string> HandVoice(WeChatXMLDto weChat)
        {
            return await Task.Run(() =>
            {
                return @$"<xml><ToUserName><![CDATA[{weChat.FromUserName}]]></ToUserName>
                                <FromUserName><![CDATA[{weChat.ToUserName}]]></FromUserName>
                                <CreateTime>{DateTime.Now.Ticks.ToString()}</CreateTime>
                                <MsgType><![CDATA[text]]></MsgType>
                                <Content><![CDATA[我收到了声音=>{weChat.MediaId}]]></Content></xml>";
            });
        }
        /// <summary>
        /// 处理小视频
        /// </summary>
        /// <param name="weChat"></param>
        /// <returns></returns>
        private async Task<string> HandShortvideo(WeChatXMLDto weChat)
        {
            return await Task.Run(() =>
            {
                return @$"<xml><ToUserName><![CDATA[{weChat.FromUserName}]]></ToUserName>
                                <FromUserName><![CDATA[{weChat.ToUserName}]]></FromUserName>
                                <CreateTime>{DateTime.Now.Ticks.ToString()}</CreateTime>
                                <MsgType><![CDATA[text]]></MsgType>
                                <Content><![CDATA[我收到了小视频=>{weChat.MediaId}]]></Content></xml>";
            });
        }
        /// <summary>
        /// 处理地理位置
        /// </summary>
        /// <param name="weChat"></param>
        /// <returns></returns>
        private async Task<string> HandLocation(WeChatXMLDto weChat)
        {
            return await Task.Run(() =>
            {
                return @$"<xml><ToUserName><![CDATA[{weChat.FromUserName}]]></ToUserName>
                                <FromUserName><![CDATA[{weChat.ToUserName}]]></FromUserName>
                                <CreateTime>{DateTime.Now.Ticks.ToString()}</CreateTime>
                                <MsgType><![CDATA[text]]></MsgType>
                                <Content><![CDATA[我收到了地址位置=>{weChat.Label}]]></Content></xml>";
            });
        }
        /// <summary>
        /// 处理链接消息
        /// </summary>
        /// <param name="weChat"></param>
        /// <returns></returns>
        private async Task<string> HandLink(WeChatXMLDto weChat)
        {
            return await Task.Run(() =>
            {
                return @$"<xml><ToUserName><![CDATA[{weChat.FromUserName}]]></ToUserName>
                                <FromUserName><![CDATA[{weChat.ToUserName}]]></FromUserName>
                                <CreateTime>{DateTime.Now.Ticks.ToString()}</CreateTime>
                                <MsgType><![CDATA[text]]></MsgType>
                                <Content><![CDATA[我收到了链接消息=>{weChat.Url}]]></Content></xml>";
            });
        }
        /// <summary>
        /// 处理事件
        /// </summary>
        /// <param name="weChat"></param>
        /// <returns></returns>
        private async Task<string> HandEvent(WeChatXMLDto weChat)
        {

            switch (weChat.Event)
            {
                case "subscribe":
                    return await EventSubscribe(weChat);
                case "unsubscribe":
                    return await EventUnsubscribe(weChat);
                case "SCAN":
                    return await EventSCAN(weChat);
                case "LOCATION":
                    return await EventLOCATION(weChat);
                case "CLICK":
                    return await EventCLICK(weChat);
                case "VIEW":
                    return await EventVIEW(weChat);
                default:
                    return await Task.Run(() =>
                    {
                        return @$"<xml><ToUserName><![CDATA[{weChat.FromUserName}]]></ToUserName>
                                <FromUserName><![CDATA[{weChat.ToUserName}]]></FromUserName>
                                <CreateTime>{DateTime.Now.Ticks.ToString()}</CreateTime>
                                <MsgType><![CDATA[text]]></MsgType>
                                <Content><![CDATA[处理失败,没有找到事件类型=>{weChat.Event}]]></Content></xml>";
                    });
            }
        }
        /// <summary>
        /// 关注事件
        /// </summary>
        /// <param name="weChat"></param>
        /// <returns></returns>
        private async Task<string> EventSubscribe(WeChatXMLDto weChat)
        {
            if (weChat.EventKey != null && (weChat.EventKey.Equals("bind") || weChat.EventKey.Equals("qrscene_bind")))
            {
                return await QRBind(weChat);
            }
            else
            {
                return await Task.Run(() =>
                {
                    return @$"<xml><ToUserName><![CDATA[{weChat.FromUserName}]]></ToUserName>
                                <FromUserName><![CDATA[{weChat.ToUserName}]]></FromUserName>
                                <CreateTime>{DateTime.Now.Ticks.ToString()}</CreateTime>
                                <MsgType><![CDATA[text]]></MsgType>
                                <Content><![CDATA[我收到了已关注事件=>key:{weChat.EventKey}=>ticket:{weChat.Ticket}]]></Content></xml>";
                });
            }
        }
        /// <summary>
        /// 取消关注事件
        /// </summary>
        /// <param name="weChat"></param>
        /// <returns></returns>
        private async Task<string> EventUnsubscribe(WeChatXMLDto weChat)
        {
            return await Task.Run(() =>
            {
                return @$"<xml><ToUserName><![CDATA[{weChat.FromUserName}]]></ToUserName>
                                <FromUserName><![CDATA[{weChat.ToUserName}]]></FromUserName>
                                <CreateTime>{DateTime.Now.Ticks.ToString()}</CreateTime>
                                <MsgType><![CDATA[text]]></MsgType>
                                <Content><![CDATA[我收到了取消关注事件=>{weChat.Event}]]></Content></xml>";
            });
        }
        /// <summary>
        /// 已关注扫码事件
        /// </summary>
        /// <param name="weChat"></param>
        /// <returns></returns>
        private async Task<string> EventSCAN(WeChatXMLDto weChat)
        {
            if (weChat.EventKey != null && (weChat.EventKey.StartsWith("bind_") || weChat.EventKey.StartsWith("qrscene_bind_")))
            {

                return await QRBind(weChat);
            }
            else
            {
                return await Task.Run(() =>
                {
                    return @$"<xml><ToUserName><![CDATA[{weChat.FromUserName}]]></ToUserName>
                                <FromUserName><![CDATA[{weChat.ToUserName}]]></FromUserName>
                                <CreateTime>{DateTime.Now.Ticks.ToString()}</CreateTime>
                                <MsgType><![CDATA[text]]></MsgType>
                                <Content><![CDATA[我收到了已关注扫码事件=>key:{weChat.EventKey}=>ticket:{weChat.Ticket}]]></Content></xml>";
                });

            }

        }
        /// <summary>
        /// 扫码绑定
        /// </summary>
        /// <param name="weChat"></param>
        /// <returns></returns>

        private async Task<string> QRBind(WeChatXMLDto weChat)
        {  
            var ticket = await BaseDal.Db.Queryable<WeChatQR>().InSingleAsync(weChat.Ticket);
            if (ticket == null) throw new Exception("ticket未找到");
            if (ticket.QRisUsed) throw new Exception("ticket已被使用");
            if (!ticket.QRpublicAccount.Equals(weChat.publicAccount)) throw new Exception($"公众号错误  need:{ticket.QRpublicAccount}  but:{weChat.publicAccount}");

            var bindUser = await BaseDal.Db.Queryable<WeChatSub>().Where(t => t.SubFromPublicAccount == ticket.QRpublicAccount && t.CompanyID == ticket.QRbindCompanyID && t.SubJobID == ticket.QRbindJobID).SingleAsync();
            bool isNewBind;
            if (bindUser == null )
            {
                isNewBind = true;
                bindUser = new WeChatSub
                {
                    SubFromPublicAccount = ticket.QRpublicAccount,
                    CompanyID = ticket.QRbindCompanyID,
                    SubJobID = ticket.QRbindJobID,
                    SubUserOpenID = weChat.FromUserName,
                    SubUserRegTime = DateTime.Now,
                };
            }
            else
            {
                isNewBind = false;
                //订阅过的就更新
                if (bindUser.SubUserOpenID != weChat.FromUserName)
                {
                    //记录上一次的订阅此工号的微信号
                    bindUser.LastSubUserOpenID = bindUser.SubUserOpenID;
                }
                bindUser.SubUserOpenID = weChat.FromUserName;
                bindUser.SubUserRefTime = DateTime.Now;
                bindUser.IsUnBind = false;
            }
            ticket.QRisUsed = true;
            ticket.QRuseTime = DateTime.Now;
            ticket.QRuseOpenid = weChat.FromUserName;

            try
            {
                _unitOfWorkManage.BeginTran();
                await BaseDal.Db.Updateable<WeChatQR>(ticket).ExecuteCommandAsync();
                if (isNewBind)
                    await BaseDal.Db.Insertable<WeChatSub>(bindUser).ExecuteCommandAsync();
                else
                    await BaseDal.Db.Updateable<WeChatSub>(bindUser).ExecuteCommandAsync();
                _unitOfWorkManage.CommitTran();
            }
            catch
            {
                _unitOfWorkManage.RollbackTran();
                throw;
            }
            return @$"<xml><ToUserName><![CDATA[{weChat.FromUserName}]]></ToUserName>
                                <FromUserName><![CDATA[{weChat.ToUserName}]]></FromUserName>
                                <CreateTime>{DateTime.Now.Ticks.ToString()}</CreateTime>
                                <MsgType><![CDATA[text]]></MsgType>
                                <Content><![CDATA[恭喜您:{(string.IsNullOrEmpty(ticket.QRbindJobNick) ? ticket.QRbindJobID : ticket.QRbindJobNick)},绑定成功!]]></Content></xml>";
        }
        /// <summary>
        /// 上报位置地理事件
        /// </summary>
        /// <param name="weChat"></param>
        /// <returns></returns>
        private async Task<string> EventLOCATION(WeChatXMLDto weChat)
        {
            return await Task.Run(() =>
            {
                return @$"<xml><ToUserName><![CDATA[{weChat.FromUserName}]]></ToUserName>
                                <FromUserName><![CDATA[{weChat.ToUserName}]]></FromUserName>
                                <CreateTime>{DateTime.Now.Ticks.ToString()}</CreateTime>
                                <MsgType><![CDATA[text]]></MsgType>
                                <Content><![CDATA[我收到了地理位置事件=>维度:{weChat.Latitude}经度:{weChat.Longitude}位置精度:{weChat.Precision}]]></Content></xml>";
            });
        }
        /// <summary>
        /// 点击菜单按钮事件
        /// </summary>
        /// <param name="weChat"></param>
        /// <returns></returns>
        private async Task<string> EventCLICK(WeChatXMLDto weChat)
        {
            return await Task.Run(() =>
            {
                return @$"<xml><ToUserName><![CDATA[{weChat.FromUserName}]]></ToUserName>
                                <FromUserName><![CDATA[{weChat.ToUserName}]]></FromUserName>
                                <CreateTime>{DateTime.Now.Ticks.ToString()}</CreateTime>
                                <MsgType><![CDATA[text]]></MsgType>
                                <Content><![CDATA[我收到了菜单点击按钮事件=>{weChat.EventKey}]]></Content></xml>";
            });
        }
        /// <summary>
        /// 点击菜单网址事件
        /// </summary>
        /// <param name="weChat"></param>
        /// <returns></returns>
        private async Task<string> EventVIEW(WeChatXMLDto weChat)
        {
            return await Task.Run(() =>
            {
                return @$"<xml><ToUserName><![CDATA[{weChat.FromUserName}]]></ToUserName>
                                <FromUserName><![CDATA[{weChat.ToUserName}]]></FromUserName>
                                <CreateTime>{DateTime.Now.Ticks.ToString()}</CreateTime>
                                <MsgType><![CDATA[text]]></MsgType>
                                <Content><![CDATA[我收到了菜单点击网址事件=>{weChat.EventKey}]]></Content></xml>";
            });
        }

    }
}