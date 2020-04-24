using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebApiClient.DataAnnotations;
namespace Blog.Core.Common.WebApiClients
{
    public class RequestApiWeekView
    {
        [AliasAs("columns")]
        public List<string> Columns { get; set; }

        [AliasAs("rows")]
        public string Rows { get; set; }

    }
}