using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebApiClient.DataAnnotations;
namespace Blog.Core.Common.WebApiClients
{
    public class NavigationBar
    {
        [AliasAs("id")]
        public int Id { get; set; }

        [AliasAs("pid")]
        public int Pid { get; set; }

        [AliasAs("order")]
        public int Order { get; set; }

        [AliasAs("name")]
        public string Name { get; set; }

        [AliasAs("isHide")]
        public bool IsHide { get; set; }

        [AliasAs("isButton")]
        public bool IsButton { get; set; }

        [AliasAs("path")]
        public string Path { get; set; }

        [AliasAs("func")]
        public string Func { get; set; }

        [AliasAs("iconCls")]
        public string IconCls { get; set; }

        [AliasAs("meta")]
        public NavigationBarMeta Meta { get; set; }

        [AliasAs("children")]
        public List<NavigationBar> Children { get; set; }

    }
}