using System.Collections.Generic;

namespace Blog.Core.Model
{
    /// <summary>
    /// 表格数据，支持分页
    /// </summary>
    public class TableModel<T>
    {
        /// <summary>
        /// 返回编码
        /// </summary>
        public int Code { get; set; }
        /// <summary>
        /// 返回信息
        /// </summary>
        public string Msg { get; set; }
        /// <summary>
        /// 记录总数
        /// </summary>
        public int Count { get; set; }
        /// <summary>
        /// 返回数据集
        /// </summary>
        public List<T> Data { get; set; }
    }
}
