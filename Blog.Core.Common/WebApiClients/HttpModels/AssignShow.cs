using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebApiClient.DataAnnotations;
namespace Blog.Core.Common.WebApiClients
{
    public class AssignShow
    {
        [AliasAs("permissionids")]
        public List<int> Permissionids { get; set; }

        [AliasAs("assignbtns")]
        public List<string> Assignbtns { get; set; }

    }
}