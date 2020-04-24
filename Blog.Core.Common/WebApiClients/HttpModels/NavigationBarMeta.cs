using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebApiClient.DataAnnotations;
namespace Blog.Core.Common.WebApiClients
{
    public class NavigationBarMeta
    {
        [AliasAs("title")]
        public string Title { get; set; }

        [AliasAs("requireAuth")]
        public bool RequireAuth { get; set; }

        [AliasAs("noTabPage")]
        public bool NoTabPage { get; set; }

    }
}