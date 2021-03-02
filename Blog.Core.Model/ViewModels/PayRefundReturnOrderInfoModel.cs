namespace Blog.Core.Model.ViewModels
{
    /// <summary>
    /// 订单信息
    /// </summary>
    public class PayRefundReturnOrderInfoModel
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
        /// 备注1
        /// </summary>
        public string REM1 { get; set; }
        /// <summary>
        /// 备注2
        /// </summary>
        public string REM2 { get; set; }

    }
}
