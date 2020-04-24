using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebApiClient.DataAnnotations;
namespace Blog.Core.Common.WebApiClients
{
    /// <summary>Tibug 博文</summary>
    public class TopicDetail
    {
        [AliasAs("topicId")]
        public int TopicId { get; set; }

        [AliasAs("tdLogo")]
        public string TdLogo { get; set; }

        [AliasAs("tdName")]
        public string TdName { get; set; }

        [AliasAs("tdContent")]
        public string TdContent { get; set; }

        [AliasAs("tdDetail")]
        public string TdDetail { get; set; }

        [AliasAs("tdSectendDetail")]
        public string TdSectendDetail { get; set; }

        [AliasAs("tdIsDelete")]
        public bool TdIsDelete { get; set; }

        [AliasAs("tdRead")]
        public int TdRead { get; set; }

        [AliasAs("tdCommend")]
        public int TdCommend { get; set; }

        [AliasAs("tdGood")]
        public int TdGood { get; set; }

        [AliasAs("tdCreatetime")]
        public System.DateTimeOffset TdCreatetime { get; set; }

        [AliasAs("tdUpdatetime")]
        public System.DateTimeOffset TdUpdatetime { get; set; }

        [AliasAs("tdTop")]
        public int TdTop { get; set; }

        [AliasAs("tdAuthor")]
        public string TdAuthor { get; set; }

        [AliasAs("topic")]
        public Topic Topic { get; set; }

        /// <summary>ID</summary>
        [AliasAs("id")]
        public int Id { get; set; }

    }
}