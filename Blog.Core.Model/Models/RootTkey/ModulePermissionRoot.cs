using System;

namespace Blog.Core.Model
{
    /// <summary>
    /// 菜单与按钮关系表
    /// 父类
    /// </summary>
    public class ModulePermissionRoot<Tkey> : RootEntityTkey<Tkey> where Tkey : IEquatable<Tkey>
    {
        /// <summary>
        /// 菜单ID
        /// </summary>
        public Tkey ModuleId { get; set; }
        /// <summary>
        /// 按钮ID
        /// </summary>
        public Tkey PermissionId { get; set; }

    }
}
