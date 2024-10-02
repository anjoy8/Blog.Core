using System;
using System.Collections.Generic;
using System.Linq;

namespace Blog.Core.Common.Helper
{
    /// <summary>
    /// 泛型递归求树形结构
    /// </summary>
    public static class RecursionHelper
    {
        public static void LoopToAppendChildren(List<PermissionTree> all, PermissionTree curItem, long pid, bool needbtn)
        {
            var subItems = all.Where(ee => ee.Pid == curItem.value).ToList();

            var btnItems = subItems.Where(ss => ss.isbtn == true).ToList();
            if (subItems.Count > 0)
            {
                curItem.btns = new List<PermissionTree>();
                curItem.btns.AddRange(btnItems);
            }
            else
            {
                curItem.btns = null;
            }

            if (!needbtn)
            {
                subItems = subItems.Where(ss => ss.isbtn == false).ToList();
            }

            if (subItems.Count > 0)
            {
                curItem.children = new List<PermissionTree>();
                curItem.children.AddRange(subItems);
            }
            else
            {
                curItem.children = null;
            }

            if (curItem.isbtn)
            {
                //curItem.label += "按钮";
            }

            foreach (var subItem in subItems)
            {
                if (subItem.value == pid && pid > 0)
                {
                    //subItem.disabled = true;//禁用当前节点
                }

                LoopToAppendChildren(all, subItem, pid, needbtn);
            }
        }
        public static void LoopToAppendChildren(List<DepartmentTree> all, DepartmentTree curItem, long pid)
        {
            var subItems = all.Where(ee => ee.Pid == curItem.value).ToList();

            if (subItems.Count > 0)
            {
                curItem.children = new List<DepartmentTree>();
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
                    //subItem.disabled = true;//禁用当前节点
                }

                LoopToAppendChildren(all, subItem, pid);
            }
        }


        public static void LoopNaviBarAppendChildren(List<NavigationBar> all, NavigationBar curItem)
        {
            var subItems = all.Where(ee => ee.pid == curItem.id).ToList();

            if (subItems.Count > 0)
            {
                curItem.children = new List<NavigationBar>();
                curItem.children.AddRange(subItems);
            }
            else
            {
                curItem.children = null;
            }


            foreach (var subItem in subItems)
            {
                LoopNaviBarAppendChildren(all, subItem);
            }
        }


        public static void LoopToAppendChildrenT<T>(List<T> all, T curItem, string parentIdName = "Pid", string idName = "value", string childrenName = "children")
        {
            var subItems = all.Where(ee => ee.GetType().GetProperty(parentIdName).GetValue(ee, null).ToString() == curItem.GetType().GetProperty(idName).GetValue(curItem, null).ToString()).ToList();

            if (subItems.Count > 0) curItem.GetType().GetField(childrenName).SetValue(curItem, subItems);
            foreach (var subItem in subItems)
            {
                LoopToAppendChildrenT(all, subItem);
            }
        }

        /// <summary>
        /// 将父子级数据结构转换为普通list
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static List<T> TreeToList<T>(List<T> list, Action<T, T, List<T>> action = null)
        {
            List<T> results = new List<T>();
            foreach (var item in list)
            {
                results.Add(item);
                OperationChildData(results, item, action);
            }

            return results;
        }

        /// <summary>
        /// 递归子级数据
        /// </summary>
        /// <param name="allList">树形列表数据</param>
        /// <param name="item">Item</param>
        public static void OperationChildData<T>(List<T> allList, T item, Action<T, T, List<T>> action)
        {
            dynamic dynItem = item;
            if (dynItem.Children == null) return;
            if (dynItem.Children.Count <= 0) return;
            allList.AddRange(dynItem.Children);
            foreach (var subItem in dynItem.Children)
            {
                action?.Invoke(item, subItem, allList);
                OperationChildData(allList, subItem, action);
            }
        }
    }

    public class PermissionTree
    {
        public long value { get; set; }
        public long Pid { get; set; }
        public string label { get; set; }
        public int order { get; set; }
        public bool isbtn { get; set; }
        public bool disabled { get; set; }
        public List<PermissionTree> children { get; set; }
        public List<PermissionTree> btns { get; set; }
    }

    public class DepartmentTree
    {
        public long value { get; set; }
        public long Pid { get; set; }
        public string label { get; set; }
        public int order { get; set; }
        public bool disabled { get; set; }
        public List<DepartmentTree> children { get; set; }
    }

    public class NavigationBar
    {
        public long id { get; set; }
        public long pid { get; set; }
        public int order { get; set; }
        public string name { get; set; }
        public bool IsHide { get; set; } = false;
        public bool IsButton { get; set; } = false;
        public string path { get; set; }
        public string Func { get; set; }
        public string iconCls { get; set; }
        public NavigationBarMeta meta { get; set; }
        public List<NavigationBar> children { get; set; }
    }

    public class NavigationBarMeta
    {
        public string title { get; set; }
        public bool requireAuth { get; set; } = true;
        public bool NoTabPage { get; set; } = false;
        public bool keepAlive { get; set; } = false;
        public string icon { get; set; }
    }


    public class NavigationBarPro
    {
        public long id { get; set; }
        public long parentId { get; set; }
        public int order { get; set; }
        public string name { get; set; }
        public bool IsHide { get; set; } = false;
        public bool IsButton { get; set; } = false;
        public string path { get; set; }
        public string component { get; set; }
        public string Func { get; set; }
        public string iconCls { get; set; }
        public NavigationBarMetaPro meta { get; set; }
    }

    public class NavigationBarMetaPro
    {
        public string title { get; set; }
        public string icon { get; set; }
        public bool show { get; set; } = false;
    }
}