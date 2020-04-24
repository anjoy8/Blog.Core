using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebApiClient.DataAnnotations;
namespace Blog.Core.Common.WebApiClients
{
    /// <summary>路由菜单表</summary>
    public class Permission
    {
        /// <summary>菜单执行Action名</summary>
        [AliasAs("code")]
        public string Code { get; set; }

        /// <summary>菜单显示名（如用户页、编辑(按钮)、删除(按钮)）</summary>
        [AliasAs("name")]
        public string Name { get; set; }

        /// <summary>是否是按钮</summary>
        [AliasAs("isButton")]
        public bool IsButton { get; set; }

        /// <summary>是否是隐藏菜单</summary>
        [AliasAs("isHide")]
        public bool? IsHide { get; set; }

        /// <summary>按钮事件</summary>
        [AliasAs("func")]
        public string Func { get; set; }

        /// <summary>上一级菜单（0表示上一级无菜单）</summary>
        [AliasAs("pid")]
        public int Pid { get; set; }

        /// <summary>接口api</summary>
        [AliasAs("mid")]
        public int Mid { get; set; }

        /// <summary>排序</summary>
        [AliasAs("orderSort")]
        public int OrderSort { get; set; }

        /// <summary>菜单图标</summary>
        [AliasAs("icon")]
        public string Icon { get; set; }

        /// <summary>菜单描述</summary>
        [AliasAs("description")]
        public string Description { get; set; }

        /// <summary>激活状态</summary>
        [AliasAs("enabled")]
        public bool Enabled { get; set; }

        /// <summary>创建ID</summary>
        [AliasAs("createId")]
        public int? CreateId { get; set; }

        /// <summary>创建者</summary>
        [AliasAs("createBy")]
        public string CreateBy { get; set; }

        /// <summary>创建时间</summary>
        [AliasAs("createTime")]
        public System.DateTimeOffset? CreateTime { get; set; }

        /// <summary>修改ID</summary>
        [AliasAs("modifyId")]
        public int? ModifyId { get; set; }

        /// <summary>修改者</summary>
        [AliasAs("modifyBy")]
        public string ModifyBy { get; set; }

        /// <summary>修改时间</summary>
        [AliasAs("modifyTime")]
        public System.DateTimeOffset? ModifyTime { get; set; }

        /// <summary>获取或设置是否禁用，逻辑上的删除，非物理删除</summary>
        [AliasAs("isDeleted")]
        public bool? IsDeleted { get; set; }

        [AliasAs("pidArr")]
        public List<int> PidArr { get; set; }

        [AliasAs("pnameArr")]
        public List<string> PnameArr { get; set; }

        [AliasAs("pCodeArr")]
        public List<string> PCodeArr { get; set; }

        [AliasAs("mName")]
        public string MName { get; set; }

        [AliasAs("hasChildren")]
        public bool HasChildren { get; set; }

        /// <summary>ID</summary>
        [AliasAs("id")]
        public int Id { get; set; }

    }
}