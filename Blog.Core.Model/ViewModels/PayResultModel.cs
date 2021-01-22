namespace Blog.Core.Model.ViewModels
{
    /// <summary>
    /// 支付结果dto
    /// </summary>
    public class PayResultModel
    {
        /// <summary>
        /// 支付结果
        /// Y：成功
        /// N：失败
        /// U：不确定
        /// Q：待轮询 
        /// </summary>
        public string RESULT { get; set; }
        /// <summary>
        /// 订单ID
        /// </summary>
        public string ORDERID { get; set; }
        /// <summary>
        /// 支付金额
        /// </summary>
        public string AMOUNT { get; set; }
        /// <summary>
        /// 二维码类型
        /// 1：龙支付
        /// 2：微信
        /// 3：支付宝
        /// 4：银联 
        /// </summary>
        public string QRCODETYPE { get; set; }
        /// <summary>
        /// 等待时间-轮询等待时间
        /// </summary>
        public string WAITTIME { get; set; }
        /// <summary>
        /// 全局事件跟踪号-建行交易流水号
        /// </summary>
        public string TRACEID { get; set; }
        /// <summary>
        /// 错误码
        /// </summary>
        public string ERRCODE { get; set; }
        /// <summary>
        /// 错误信息
        /// </summary>
        public string ERRMSG { get; set; }
        /// <summary>
        /// 验证签名-防止伪造攻击
        /// </summary>
        public string SIGN { get; set; }
    }
}
