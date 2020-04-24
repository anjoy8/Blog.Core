using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebApiClient.DataAnnotations;
namespace Blog.Core.Common.WebApiClients
{
    /// <summary>博客信息展示类</summary>
    public class BlogViewModels
    {
        [AliasAs("bID")]
        public int BID { get; set; }

        /// <summary>创建人</summary>
        [AliasAs("bsubmitter")]
        public string Bsubmitter { get; set; }

        /// <summary>博客标题</summary>
        [AliasAs("btitle")]
        public string Btitle { get; set; }

        /// <summary>摘要</summary>
        [AliasAs("digest")]
        public string Digest { get; set; }

        /// <summary>上一篇</summary>
        [AliasAs("previous")]
        public string Previous { get; set; }

        /// <summary>上一篇id</summary>
        [AliasAs("previousID")]
        public int PreviousID { get; set; }

        /// <summary>下一篇</summary>
        [AliasAs("next")]
        public string Next { get; set; }

        /// <summary>下一篇id</summary>
        [AliasAs("nextID")]
        public int NextID { get; set; }

        /// <summary>类别</summary>
        [AliasAs("bcategory")]
        public string Bcategory { get; set; }

        /// <summary>内容</summary>
        [AliasAs("bcontent")]
        public string Bcontent { get; set; }

        /// <summary>访问量</summary>
        [AliasAs("btraffic")]
        public int Btraffic { get; set; }

        /// <summary>评论数量</summary>
        [AliasAs("bcommentNum")]
        public int BcommentNum { get; set; }

        /// <summary>修改时间</summary>
        [AliasAs("bUpdateTime")]
        public System.DateTimeOffset BUpdateTime { get; set; }

        /// <summary>创建时间</summary>
        [AliasAs("bCreateTime")]
        public System.DateTimeOffset BCreateTime { get; set; }

        /// <summary>备注</summary>
        [AliasAs("bRemark")]
        public string BRemark { get; set; }

    }
}