using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Core.Model.Models
{
    /// <summary>
    /// 路由菜单表
    /// </summary>
    public class Permission : RootEntity
    {
        public Permission()
        {
            //this.ModulePermission = new List<ModulePermission>();
            //this.RoleModulePermission = new List<RoleModulePermission>();
        }

        /// <summary>
        /// 菜单执行Action名
        /// </summary>
        [SugarColumn(Length = 50, IsNullable = true)]
        public string Code { get; set; }
        /// <summary>
        /// 菜单显示名（如用户页、编辑(按钮)、删除(按钮)）
        /// </summary>
        [SugarColumn(Length = 50, IsNullable = true)]
        public string Name { get; set; }
        /// <summary>
        /// 是否是按钮
        /// </summary>
        public bool IsButton { get; set; } = false;
        /// <summary>
        /// 是否是隐藏菜单
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public bool? IsHide { get; set; } = false;


        /// <summary>
        /// 按钮事件
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string Func { get; set; }



        /// <summary>
        /// 上一级菜单（0表示上一级无菜单）
        /// </summary>
        public int Pid { get; set; }


        /// <summary>
        /// 接口api
        /// </summary>
        public int Mid { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int OrderSort { get; set; }
        /// <summary>
        /// 菜单图标
        /// </summary>
        [SugarColumn(Length = 100, IsNullable = true)]
        public string Icon { get; set; }
        /// <summary>
        /// 菜单描述    
        /// </summary>
        [SugarColumn(Length = 100, IsNullable = true)]
        public string Description { get; set; }
        /// <summary>
        /// 激活状态
        /// </summary>
        public bool Enabled { get; set; }
        /// <summary>
        /// 创建ID
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public int? CreateId { get; set; }
        /// <summary>
        /// 创建者
        /// </summary>
        [SugarColumn(Length = 50, IsNullable = true)]
        public string CreateBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public DateTime? CreateTime { get; set; } = DateTime.Now;
        /// <summary>
        /// 修改ID
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public int? ModifyId { get; set; }
        /// <summary>
        /// 修改者
        /// </summary>
        [SugarColumn(Length = 50, IsNullable = true)]
        public string ModifyBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public DateTime? ModifyTime { get; set; } = DateTime.Now;

        /// <summary>
        ///获取或设置是否禁用，逻辑上的删除，非物理删除
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public bool? IsDeleted { get; set; }




        [SugarColumn(IsIgnore = true)]
        public List<int> PidArr { get; set; }
        [SugarColumn(IsIgnore = true)]
        public List<string> PnameArr { get; set; } = new List<string>();
        [SugarColumn(IsIgnore = true)]
        public List<string> PCodeArr { get; set; } = new List<string>();
        [SugarColumn(IsIgnore = true)]
        public string MName { get; set; }



        //public virtual ICollection<ModulePermission> ModulePermission { get; set; }
        //public virtual ICollection<RoleModulePermission> RoleModulePermission { get; set; }
    }
}
