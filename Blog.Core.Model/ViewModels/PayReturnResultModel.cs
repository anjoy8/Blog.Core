using System.Linq;
using System.Threading.Tasks;

namespace Blog.Core.Model.ViewModels
{
    /// <summary>
    /// 返回支付结果
    /// </summary>
    public class PayReturnResultModel
    {
        /// <summary>
        /// 发起的订单ID
        /// </summary>
        public string ORDERID { get; set; } 
        /// <summary>
        /// 返回支付的金额
        /// </summary>
        public string AMOUNT { get; set; }
        /// <summary>
        /// 返回支付的类型 1：龙支付 2：微信 3：支付宝 4：银联
        /// </summary>
        public string QRCODETYPE { get; set; }
        /// <summary>
        /// 返回建行的流水号
        /// </summary>
        public string TRACEID { get; set; }
        /// <summary>
        /// 错误代码
        /// </summary>
        public string ERRCODE { get; set; }
        /// <summary>
        /// 错误信息
        /// </summary>
        public string ERRMSG { get; set; }
    }
}
