using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebApiClient.DataAnnotations;
namespace Blog.Core.Common.WebApiClients
{
    /// <summary>接口API地址信息表</summary>
    public class Module
    {
        /// <summary>获取或设置是否禁用，逻辑上的删除，非物理删除</summary>
        [AliasAs("isDeleted")]
        public bool? IsDeleted { get; set; }

        /// <summary>父ID</summary>
        [AliasAs("parentId")]
        public int? ParentId { get; set; }

        /// <summary>名称</summary>
        [AliasAs("name")]
        public string Name { get; set; }

        /// <summary>菜单链接地址</summary>
        [AliasAs("linkUrl")]
        public string LinkUrl { get; set; }

        /// <summary>区域名称</summary>
        [AliasAs("area")]
        public string Area { get; set; }

        /// <summary>控制器名称</summary>
        [AliasAs("controller")]
        public string Controller { get; set; }

        /// <summary>Action名称</summary>
        [AliasAs("action")]
        public string Action { get; set; }

        /// <summary>图标</summary>
        [AliasAs("icon")]
        public string Icon { get; set; }

        /// <summary>菜单编号</summary>
        [AliasAs("code")]
        public string Code { get; set; }

        /// <summary>排序</summary>
        [AliasAs("orderSort")]
        public int OrderSort { get; set; }

        /// <summary>/描述</summary>
        [AliasAs("description")]
        public string Description { get; set; }

        /// <summary>是否是右侧菜单</summary>
        [AliasAs("isMenu")]
        public bool IsMenu { get; set; }

        /// <summary>是否激活</summary>
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
        public System.DateTimeOffset CreateTime { get; set; }

        /// <summary>修改ID</summary>
        [AliasAs("modifyId")]
        public int? ModifyId { get; set; }

        /// <summary>修改者</summary>
        [AliasAs("modifyBy")]
        public string ModifyBy { get; set; }

        /// <summary>修改时间</summary>
        [AliasAs("modifyTime")]
        public System.DateTimeOffset? ModifyTime { get; set; }

        /// <summary>ID</summary>
        [AliasAs("id")]
        public int Id { get; set; }

    }
}