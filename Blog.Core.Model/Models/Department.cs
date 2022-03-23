using System;


namespace Blog.Core.Model.Models
{
    ///<summary>
    /// 部门表
    ///</summary>
    public class Department : RootEntityTkey<long>
    {
        /// <summary>
        /// Desc:父部门
        /// Nullable:True
        /// </summary>
        public long PId { get; set; }
        /// <summary>
        /// Desc:部门关系编码
        /// Default:
        /// Nullable:True
        /// </summary>
        public string CodeRelationship { get; set; }
        /// <summary>
        /// Desc:部门名称
        /// Default:
        /// Nullable:True
        /// </summary>
        public string DepartName { get; set; }
        /// <summary>
        /// Desc:负责人
        /// Default:
        /// Nullable:True
        /// </summary>
        public string Leader { get; set; }
        /// <summary>
        /// Desc:排序
        /// Default:
        /// Nullable:True
        /// </summary>
        public int? OrderNum { get; set; } = 0;
        /// <summary>
        /// Desc:部门状态（0正常 1停用）
        /// Default:0
        /// Nullable:True
        /// </summary>
        public bool Status { get; set; }
        /// <summary>
        /// Desc:删除标志（0代表存在 2代表删除）
        /// Default:0
        /// Nullable:True
        /// </summary>
        public bool IsDeleted { get; set; } = false;
        /// <summary>
        /// Desc:创建者
        /// Default:
        /// Nullable:True
        /// </summary>
        public string CreateBy { get; set; }
        /// <summary>
        /// Desc:创建时间
        /// Default:
        /// Nullable:True
        /// </summary>
        public DateTime? CreateTime { get; set; }
        /// <summary>
        /// Desc:更新者
        /// Default:
        /// Nullable:True
        /// </summary>
        public string ModifyBy { get; set; }
        /// <summary>
        /// Desc:更新时间
        /// Default:
        /// Nullable:True
        /// </summary>
        public DateTime? ModifyTime { get; set; }
    }
}