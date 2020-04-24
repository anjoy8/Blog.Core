using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebApiClient.DataAnnotations;
namespace Blog.Core.Common.WebApiClients
{
    public class AssignView
    {
        [AliasAs("pids")]
        public List<int> Pids { get; set; }

        [AliasAs("rid")]
        public int Rid { get; set; }

    }
}