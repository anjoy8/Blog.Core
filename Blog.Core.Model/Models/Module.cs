using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Core.Model.Models
{
    /// <summary>
    /// 菜单表
    /// </summary>
    public class Module
    {
        public Module()
        {
            this.ChildModule = new List<Module>();
            this.ModulePermission = new List<ModulePermission>();
            this.RoleModulePermission = new List<RoleModulePermission>();
        }

        public int Id { get; set; }

        /// <summary>
        ///获取或设置是否禁用，逻辑上的删除，非物理删除
        /// </summary>
        public bool? IsDeleted { get; set; }
        /// <summary>
        /// 父ID
        /// </summary>
        public int? ParentId { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 菜单链接地址
        /// </summary>
        public string LinkUrl { get; set; }
        /// <summary>
        /// 区域名称
        /// </summary>
        public string Area { get; set; }
        /// <summary>
        /// 控制器名称
        /// </summary>
        public string Controller { get; set; }
        /// <summary>
        /// Action名称
        /// </summary>
        public string Action { get; set; }
        /// <summary>
        /// 图标
        /// </summary>
        public string Icon { get; set; }
        /// <summary>
        /// 菜单编号
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int OrderSort { get; set; }
        /// <summary>
        /// /描述
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 是否是右侧菜单
        /// </summary>
        public bool IsMenu { get; set; }
        /// <summary>
        /// 是否激活
        /// </summary>
        public bool Enabled { get; set; }
        /// <summary>
        /// 创建ID
        /// </summary>
        public int? CreateId { get; set; }
        /// <summary>
        /// 创建者
        /// </summary>
        public string CreateBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreateTime { get; set; }
        /// <summary>
        /// 修改ID
        /// </summary>
        public int? ModifyId { get; set; }
        /// <summary>
        /// 修改者
        /// </summary>
        public string ModifyBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? ModifyTime { get; set; }

        public virtual Module ParentModule { get; set; }
        public virtual ICollection<Module> ChildModule { get; set; }
        public virtual ICollection<ModulePermission> ModulePermission { get; set; }
        public virtual ICollection<RoleModulePermission> RoleModulePermission { get; set; }
    }
}
