using System;

namespace Blog.Core.Model
{
    /// <summary>
    /// 用户跟角色关联表
    /// 父类
    /// </summary>
    public class UserRoleRoot<Tkey> : RootEntityTkey<Tkey> where Tkey : IEquatable<Tkey>
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public Tkey UserId { get; set; }
        /// <summary>
        /// 角色ID
        /// </summary>
        public Tkey RoleId { get; set; }

    }
}
