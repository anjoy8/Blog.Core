using SqlSugar;
using System;

namespace Blog.Core.Model.Models
{
    /// <summary>
    /// 菜单与按钮关系表
    /// </summary>
    public class ModulePermission : RootEntity
    {

        /// <summary>
        ///获取或设置是否禁用，逻辑上的删除，非物理删除
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public bool? IsDeleted { get; set; }
        /// <summary>
        /// 菜单ID
        /// </summary>
        public int ModuleId { get; set; }
        /// <summary>
        /// 按钮ID
        /// </summary>
        public int PermissionId { get; set; }
        /// <summary>
        /// 创建ID
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public int? CreateId { get; set; }
        /// <summary>
        /// 创建者
        /// </summary>
        [SugarColumn(ColumnDataType = "nvarchar", Length = 50, IsNullable = true)]
        public string CreateBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public DateTime? CreateTime { get; set; }
        /// <summary>
        /// 修改ID
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public int? ModifyId { get; set; }
        /// <summary>
        /// 修改者
        /// </summary>
        [SugarColumn(ColumnDataType = "nvarchar", Length = 50, IsNullable = true)]
        public string ModifyBy { get; set; }
        /// <summary>
        ///修改时间
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public DateTime? ModifyTime { get; set; }

        //public virtual Module Module { get; set; }
        //public virtual Permission Permission { get; set; }
    }
}
