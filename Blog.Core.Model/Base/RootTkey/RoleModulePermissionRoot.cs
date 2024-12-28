namespace Blog.Core.Model.Base.RootTkey
{
    /// <summary>
    /// 按钮跟权限关联表
    /// 父类
    /// </summary>
    public class RoleModulePermissionRoot<Tkey> : RootEntityTkey<Tkey> where Tkey : IEquatable<Tkey>
    {
       
        /// <summary>
        /// 角色ID
        /// </summary>
        public Tkey RoleId { get; set; }
        /// <summary>
        /// 菜单ID
        /// </summary>
        public Tkey ModuleId { get; set; }
        /// <summary>
        /// api ID
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public Tkey PermissionId { get; set; }
       
    }
}
