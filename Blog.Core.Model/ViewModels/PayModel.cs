namespace Blog.Core.Model.ViewModels
{
    public class PayModel
    {
        /// <summary>
        /// 商户号
        /// </summary>  
        public string MERCHANTID { get; set; }//105910100190000");// => self::MERCHANTID, // 商户号
        /// <summary>
        /// 柜台号
        /// </summary>  
        public string POSID { get; set; } //610000000");// => self::POSID, // 柜台号
        /// <summary>
        /// 分行号
        /// </summary>  
        public string BRANCHID { get; set; } //610000000");// => self::BRANCHID, // 分行号
        /// <summary>
        /// 集团商户信息
        /// </summary>  
        public string GROUPMCH { get; set; } //;// => '', // 集团商户信息
        /// <summary>
        /// 交易码
        /// </summary>  
        public string TXCODE { get; set; } //PAY100");// => 'PAY100', // 交易码
        /// <summary>
        /// 商户类型
        /// </summary>  
        public string MERFLAG { get; set; } //// => '', // 商户类型
        /// <summary>
        /// 终端编号 1
        /// </summary>  
        public string TERMNO1 { get; set; } //// => '', // 终端编号 1
        /// <summary>
        /// 终端编号 2
        /// </summary>  
        public string TERMNO2 { get; set; } //// => '', // 终端编号 2
        /// <summary>
        /// 订单号
        /// </summary>  
        public string ORDERID { get; set; }//// => '', // 订单号
        /// <summary>
        /// 码信息（一维码、二维码）
        /// </summary>  
        public string QRCODE { get; set; } //// => '', // 码信息（一维码、二维码）
        /// <summary>
        /// 订单金额，单位：元
        /// </summary>  
        public string AMOUNT { get; set; } //");// => '0.01', // 订单金额，单位：元
        /// <summary>
        /// 商品名称
        /// </summary>  
        public string PROINFO { get; set; } //// => '', // 商品名称
        /// <summary>
        /// 备注 1
        /// </summary>  
        public string REMARK1 { get; set; } //// => '', // 备注 1
        /// <summary>
        /// 备注 2
        /// </summary>  
        public string REMARK2 { get; set; }//// => '', // 备注 2
        /// <summary>
        /// 分账信息一
        /// </summary>  
        public string FZINFO1 { get; set; } //// => '', // 分账信息一
        /// <summary>
        /// 分账信息二
        /// </summary>  
        public string FZINFO2 { get; set; } //// => '', // 分账信息二
        /// <summary>
        /// 子商户公众账号 ID
        /// </summary>  
        public string SUB_APPID { get; set; } //);// => '', // 子商户公众账号 ID
        /// <summary>
        /// 返回信息位图
        /// </summary>  
        public string RETURN_FIELD { get; set; }// "");// => '', // 返回信息位图
        /// <summary>
        /// 实名支付
        /// </summary>  
        public string USERPARAM { get; set; } //);// => '', // 实名支付
        /// <summary>
        /// 商品详情
        /// </summary>  
        public string detail { get; set; }//// => '', // 商品详情
        /// <summary>
        /// 订单优惠标记
        /// </summary>                           
        public string goods_tag { get; set; } //);// => '', // 订单优惠标记
        /// <summary>
        /// 公钥
        /// </summary>
        public string pubKey { get; set; } 
        /// <summary>
        /// 请求地址
        /// </summary>
        public string url { get; set; } 
        /// <summary>
        /// 是否删除空值
        /// </summary>
        public bool deleteEmpty { get; set; }
    }
}
