using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebApiClient.DataAnnotations;
namespace Blog.Core.Common.WebApiClients
{
    public class PermissionTree
    {
        [AliasAs("value")]
        public int Value { get; set; }

        [AliasAs("pid")]
        public int Pid { get; set; }

        [AliasAs("label")]
        public string Label { get; set; }

        [AliasAs("order")]
        public int Order { get; set; }

        [AliasAs("isbtn")]
        public bool Isbtn { get; set; }

        [AliasAs("disabled")]
        public bool Disabled { get; set; }

        [AliasAs("children")]
        public List<PermissionTree> Children { get; set; }

        [AliasAs("btns")]
        public List<PermissionTree> Btns { get; set; }

    }
}