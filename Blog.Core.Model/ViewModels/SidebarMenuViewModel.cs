using System.Collections.Generic;

namespace Blog.Core.Model.ViewModels
{
    /// <summary>
    /// 菜单展示model
    /// </summary>
    public class SidebarMenuViewModel
    {
        public SidebarMenuViewModel()
        {
            this.ChildMenuList = new List<SidebarMenuViewModel>();
        }

        public int Id { get; set; }
        public int? ParentId { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
        public string Code { get; set; }
        public string LinkUrl { get; set; }
        public string Area { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }

        public List<SidebarMenuViewModel> ChildMenuList { get; set; }
    }
}
