namespace Blog.Core.Model.ViewModels
{
    /// <summary>
    /// 订单参数
    /// </summary>
    public class PayRefundNeedModel
    {
        /// <summary>
        /// 订单号
        /// </summary>
        public string ORDER { get; set; }
        /// <summary>
        /// 退款金额
        /// </summary>
        public string MONEY { get; set; }
        /// <summary>
        /// 退款流水号(可选)
        /// </summary>
        public string REFUND_CODE { get; set; }
    }
}
