namespace Blog.Core.Model.CustomEnums
{
    public enum AuthorityScopeEnum
    {
        /// <summary>
        /// 无任何权限
        /// </summary>
        NONE = -1,
        /// <summary>
        /// 自定义权限
        /// </summary>
        Custom = 1,
        /// <summary>
        /// 本部门
        /// </summary>
        MyDepart = 2,
        /// <summary>
        /// 本部门及以下
        /// </summary>
        MyDepartAndDown = 3,
        /// <summary>
        /// 仅自己
        /// </summary>
        OnlySelf = 4,
        /// <summary>
        /// 所有
        /// </summary>
        ALL = 9
    }
}
