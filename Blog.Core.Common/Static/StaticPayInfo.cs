

namespace Blog.Core.Common.Static
{
    public static class StaticPayInfo
    {
        /// <summary>
        /// 商户号
        /// </summary>
        public readonly static string MERCHANTID = Appsettings.app(new string[] { "PayInfo", "MERCHANTID" }).ObjToString();
        /// <summary>
        /// 柜台号
        /// </summary>
        public readonly static string POSID = Appsettings.app(new string[] { "PayInfo", "POSID" }).ObjToString();
        /// <summary>
        /// 分行号
        /// </summary>
        public readonly static string BRANCHID = Appsettings.app(new string[] { "PayInfo", "BRANCHID" }).ObjToString();
        /// <summary>
        /// 公钥
        /// </summary>
        public readonly static string pubKey = Appsettings.app(new string[] { "PayInfo", "pubKey" }).ObjToString(); 
        /// <summary>
        /// 操作员号
        /// </summary>
        public readonly static string USER_ID = Appsettings.app(new string[] { "PayInfo", "USER_ID" }).ObjToString();
        /// <summary>
        /// 密码
        /// </summary>
        public readonly static string PASSWORD = Appsettings.app(new string[] { "PayInfo", "PASSWORD" }).ObjToString();
        /// <summary>
        /// 外联平台通讯地址
        /// </summary>
        public readonly static string OutAddress = Appsettings.app(new string[] { "PayInfo", "OutAddress" }).ObjToString();
        
    }
}
