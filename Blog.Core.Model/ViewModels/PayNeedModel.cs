namespace Blog.Core.Model.ViewModels
{
    /// <summary>
    /// 退款参数
    /// </summary>
    public class PayNeedModel
    {
        
        /// <summary>
        /// 订单ID
        /// </summary>
        public string ORDERID { get; set; }
        /// <summary>
        /// 商品名称
        /// </summary>
        public string PROINFO { get; set; }
        /// <summary>
        /// 支付金额(小数点最多两位)
        /// </summary>
        public string AMOUNT { get; set; }
        /// <summary>
        /// 二维码/条码信息
        /// </summary>
        public string QRCODE { get; set; }
        /// <summary>
        /// 备注信息1
        /// </summary>
        public string REMARK1 { get; set; }
        /// <summary>
        /// 备注信息2
        /// </summary>
        public string REMARK2 { get; set; }
    }
}
