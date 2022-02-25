using System.Collections.Generic;

namespace System
{
    /// <summary>
    /// 绘制表格需要的信息
    /// </summary>
    public class DrawTableInfo
    {
        /// <summary>
        /// 顶部和底部字符串分隔线
        /// </summary>
        public string Top_DownDivider { get; set; }

        /// <summary>
        /// 分隔线
        /// </summary>
        public string Divider { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 头部
        /// </summary>
        public string Header { get; set; }

        /// <summary>
        /// 数据
        /// </summary>
        public List<string> Data { get; set; }
    }
}