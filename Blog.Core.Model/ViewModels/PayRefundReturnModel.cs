namespace Blog.Core.Model.ViewModels
{
    /// <summary>
    /// 退款返回消息
    /// </summary>
    public class PayRefundReturnModel
    {
        /// <summary>
        /// 序列号
        /// </summary>
        public string REQUEST_SN { get; set; }
        /// <summary>
        /// 商户号
        /// </summary>
        public string CUST_ID { get; set; }
        /// <summary>
        /// 交易码
        /// </summary>
        public string TX_CODE { get; set; }
        /// <summary>
        /// 返回码
        /// </summary>
        public string RETURN_CODE { get; set; }
        /// <summary>
        /// 返回码说明
        /// </summary>
        public string RETURN_MSG { get; set; }
        /// <summary>
        /// 语言
        /// </summary>
        public string LANGUAGE { get; set; }
        /// <summary>
        /// 订单信息
        /// </summary>
        public PayRefundReturnOrderInfoModel TX_INFO { get; set; }
    }
}
