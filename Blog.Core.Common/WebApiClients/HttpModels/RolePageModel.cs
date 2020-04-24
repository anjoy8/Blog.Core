using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebApiClient.DataAnnotations;
namespace Blog.Core.Common.WebApiClients
{
    /// <summary>通用分页信息类</summary>
    public class RolePageModel
    {
        /// <summary>当前页标</summary>
        [AliasAs("page")]
        public int Page { get; set; }

        /// <summary>总页数</summary>
        [AliasAs("pageCount")]
        public int PageCount { get; set; }

        /// <summary>数据总数</summary>
        [AliasAs("dataCount")]
        public int DataCount { get; set; }

        /// <summary>每页大小</summary>
        [AliasAs("pageSize")]
        public int PageSize { get; set; }

        /// <summary>返回数据</summary>
        [AliasAs("data")]
        public List<Role> Data { get; set; }

    }
}