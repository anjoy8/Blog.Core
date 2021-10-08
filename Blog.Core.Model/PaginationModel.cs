using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Core.Model
{
    /// <summary>
    /// 所需分页参数
    /// 作者:胡丁文
    /// 时间:2020-4-3 20:31:26
    /// </summary>
    public class PaginationModel
    {
        /// <summary>
        /// 当前页
        /// </summary>
        public int intPageIndex { get; set; } = 1;
        /// <summary>
        /// 每页大小
        /// </summary>
        public int intPageSize { get; set; } = 10;
        /// <summary>
        /// 排序字段(例如:id desc,time asc)
        /// </summary>
        public string strOrderByFileds { get; set; }
        /// <summary>
        /// 查询条件( 例如:id = 1 and name = 小明)
        /// </summary>
        public string conditions { get; set; }
    }
}
