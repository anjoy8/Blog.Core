using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebApiClient.DataAnnotations;
namespace Blog.Core.Common.WebApiClients
{
    public class ApiDate
    {
        [AliasAs("date")]
        public string Date { get; set; }

        [AliasAs("count")]
        public int Count { get; set; }

    }
}