using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebApiClient.DataAnnotations;
namespace Blog.Core.Common.WebApiClients
{
    /// <summary>通用返回信息类</summary>
    public class AccessApiDateViewMessageModel
    {
        /// <summary>状态码</summary>
        [AliasAs("status")]
        public int Status { get; set; }

        /// <summary>操作是否成功</summary>
        [AliasAs("success")]
        public bool Success { get; set; }

        /// <summary>返回信息</summary>
        [AliasAs("msg")]
        public string Msg { get; set; }

        /// <summary>返回数据集合</summary>
        [AliasAs("response")]
        public AccessApiDateView Response { get; set; }

    }
}