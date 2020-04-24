using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebApiClient.DataAnnotations;
namespace Blog.Core.Common.WebApiClients
{
    public class LogInfo
    {
        [AliasAs("datetime")]
        public System.DateTimeOffset Datetime { get; set; }

        [AliasAs("content")]
        public string Content { get; set; }

        [AliasAs("ip")]
        public string Ip { get; set; }

        [AliasAs("logColor")]
        public string LogColor { get; set; }

        [AliasAs("import")]
        public int Import { get; set; }

    }
}