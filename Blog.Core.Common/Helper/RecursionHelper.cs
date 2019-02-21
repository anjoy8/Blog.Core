using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Blog.Core.Common.Helper
{
    /// <summary>
    /// 泛型递归求树形结构
    /// </summary>
    public static class RecursionHelper
    {
        public static void LoopToAppendChildren(List<PermissionTree> all, PermissionTree curItem, int pid)
        {

            var subItems = all.Where(ee => ee.Pid == curItem.value).ToList();
            if (subItems.Count > 0)
            {
                curItem.children = new List<PermissionTree>();
                curItem.children.AddRange(subItems);
            }
            else
            {
                curItem.children = null;
            }
            foreach (var subItem in subItems)
            {
                if (subItem.value == pid && pid > 0)
                {
                    subItem.disabled = true;
                }
                LoopToAppendChildren(all, subItem, pid);
            }
        }

        public static void LoopToAppendChildren<T>(List<T> all, T curItem, string parentIdName = "Pid", string idName = "value", string childrenName = "children")
        {
            var subItems = all.Where(ee => ee.GetType().GetProperty(parentIdName).GetValue(ee, null).ToString() == curItem.GetType().GetProperty(idName).GetValue(curItem, null).ToString()).ToList(); //新闻1

            if (subItems.Count > 0) curItem.GetType().GetField(childrenName).SetValue(curItem, subItems);
            foreach (var subItem in subItems)
            {
                LoopToAppendChildren(all, subItem);
            }
        }
    }

    public class PermissionTree
    {
        public int value { get; set; }
        public int Pid { get; set; }
        public string label { get; set; }
        public bool disabled { get; set; }
        public List<PermissionTree> children { get; set; }
    }
}
