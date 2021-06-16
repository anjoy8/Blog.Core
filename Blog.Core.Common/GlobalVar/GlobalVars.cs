namespace Blog.Core
{
    /// <summary>
    /// 权限变量配置
    /// </summary>
    public static class Permissions
    {
        public const string Name = "Permission";

        /// <summary>
        /// 测试网关授权
        /// 可以使用Blog.Core项目中的test用户
        /// 账号：test
        /// 密码：test
        /// </summary>
        public const string GWName = "GW";

        /// <summary>
        /// 当前项目是否启用IDS4权限方案
        /// true：表示启动IDS4
        /// false：表示使用JWT
        public static bool IsUseIds4 = false;
    }

    /// <summary>
    /// 路由变量前缀配置
    /// </summary>
    public static class RoutePrefix
    {
        /// <summary>
        /// 前缀名
        /// 如果不需要，尽量留空，不要修改
        /// 除非一定要在所有的 api 前统一加上特定前缀
        /// </summary>
        public const string Name = "";
    }

    /// <summary>
    /// RedisMqKey
    /// </summary>
    public static class RedisMqKey
    {
        public const string Loging = "Loging";
    }
}
