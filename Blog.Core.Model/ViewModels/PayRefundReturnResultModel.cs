namespace Blog.Core.Model.ViewModels
{
    /// <summary>
    /// 退款返回结果消息
    /// </summary>
    public class PayRefundReturnResultModel
    {
        /// <summary>
        /// 订单号
        /// </summary>
        public string ORDER_NUM { get; set; }
        /// <summary>
        /// 支付金额
        /// </summary>
        public string PAY_AMOUNT { get; set; }
        /// <summary>
        /// 退款金额
        /// </summary>
        public string AMOUNT { get; set; }
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
    }
}
