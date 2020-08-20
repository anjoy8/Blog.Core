using SqlSugar;
using System;

namespace Blog.Core.Model.IDS4DbModels
{
    /// <summary>
    /// 以下model 来自ids4项目，多库模式，为了调取ids4数据
    /// 角色表
    /// </summary>
    [SugarTable("ApplicationRole", "WMBLOG_MYSQL_2")]
    public class ApplicationRole
    {
        public bool IsDeleted { get; set; }
        public string Description { get; set; }
        /// <summary>
        ///排序
        /// </summary>
        public int OrderSort { get; set; }
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
        public DateTime? CreateTime { get; set; } = DateTime.Now;
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
        public DateTime? ModifyTime { get; set; } = DateTime.Now;
    }
}
