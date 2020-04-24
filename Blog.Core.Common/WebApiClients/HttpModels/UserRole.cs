using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebApiClient.DataAnnotations;
namespace Blog.Core.Common.WebApiClients
{
    /// <summary>用户跟角色关联表</summary>
    public class UserRole
    {
        /// <summary>获取或设置是否禁用，逻辑上的删除，非物理删除</summary>
        [AliasAs("isDeleted")]
        public bool? IsDeleted { get; set; }

        /// <summary>用户ID</summary>
        [AliasAs("userId")]
        public int UserId { get; set; }

        /// <summary>角色ID</summary>
        [AliasAs("roleId")]
        public int RoleId { get; set; }

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

        /// <summary>ID</summary>
        [AliasAs("id")]
        public int Id { get; set; }

    }
}