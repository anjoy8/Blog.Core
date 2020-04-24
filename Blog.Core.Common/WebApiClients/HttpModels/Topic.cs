using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebApiClient.DataAnnotations;
namespace Blog.Core.Common.WebApiClients
{
    /// <summary>Tibug 类别</summary>
    public class Topic
    {
        [AliasAs("tLogo")]
        public string TLogo { get; set; }

        [AliasAs("tName")]
        public string TName { get; set; }

        [AliasAs("tDetail")]
        public string TDetail { get; set; }

        [AliasAs("tAuthor")]
        public string TAuthor { get; set; }

        [AliasAs("tSectendDetail")]
        public string TSectendDetail { get; set; }

        [AliasAs("tIsDelete")]
        public bool TIsDelete { get; set; }

        [AliasAs("tRead")]
        public int TRead { get; set; }

        [AliasAs("tCommend")]
        public int TCommend { get; set; }

        [AliasAs("tGood")]
        public int TGood { get; set; }

        [AliasAs("tCreatetime")]
        public System.DateTimeOffset TCreatetime { get; set; }

        [AliasAs("tUpdatetime")]
        public System.DateTimeOffset TUpdatetime { get; set; }

        [AliasAs("topicDetail")]
        public List<TopicDetail> TopicDetail { get; set; }

        /// <summary>ID</summary>
        [AliasAs("id")]
        public int Id { get; set; }

    }
}