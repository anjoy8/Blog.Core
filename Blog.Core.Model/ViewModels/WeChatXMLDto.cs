using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Blog.Core.Model.ViewModels
{
    /// <summary>
    /// 微信XmlDto
    /// 作者:胡丁文
    /// 时间:2020-4-3 20:31:26
    /// </summary> 
    [XmlRoot(ElementName ="xml")]
    public class WeChatXMLDto
    { 
        /// <summary>
        /// 微信公众号唯一表示
        /// </summary>
        public string publicAccount { get; set; }
        /// <summary>
        /// 微信开发者
        /// </summary> 
        public string ToUserName { get; set; }
        /// <summary>
        /// 来自谁
        /// </summary> 
        public string FromUserName { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public string CreateTime { get; set; }
        /// <summary>
        /// 消息类型
        /// </summary> 
        public string MsgType { get; set; }
        /// <summary>
        /// 文字内容
        /// </summary> 
        public string Content { get; set; }
        /// <summary>
        /// 消息ID
        /// </summary>
        public string MsgId { get; set; }
        /// <summary>
        /// 消息事件
        /// </summary> 
        public string Event { get; set; }
        /// <summary>
        /// 事件key值
        /// </summary> 
        public string EventKey { get; set; }
        /// <summary>
        /// 图片地址
        /// </summary>
        public string PicUrl { get; set; }
        /// <summary>
        /// 多媒体ID
        /// </summary>
        public string MediaId { get; set; }
        /// <summary>
        /// 格式
        /// </summary>
        public string Format { get; set; }
        /// <summary>
        /// 语音失败
        /// </summary>
        public string Recognition { get; set; }
        /// <summary>
        /// 缩略媒体ID
        /// </summary>
        public string ThumbMediaId { get; set; }
        /// <summary>
        /// 地理位置维度
        /// </summary>
        public string Location_X { get; set; }
        /// <summary>
        /// 地理位置经度
        /// </summary>
        public string Location_Y { get; set; }
        /// <summary>
        /// 地图缩放大小
        /// </summary>
        public string Scale { get; set; }
        /// <summary>
        /// 地理位置信息
        /// </summary>
        public string Label { get; set; }
        /// <summary>
        /// 消息标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 消息描述
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 消息链接
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// 二维码的ticket，可用来换取二维码图片
        /// </summary>
        public string Ticket { get; set; }
        /// <summary>
        /// 地理位置纬度
        /// </summary>
        public string Latitude { get; set; }
        /// <summary>
        /// 地理位置经度
        /// </summary>
        public string Longitude { get; set; }
        /// <summary>
        /// 地理位置精度
        /// </summary>
        public string Precision { get; set; }
    }
}
