namespace System
{
    /// <summary>
    /// 表格显示样式
    /// </summary>
    public enum TableStyle
    {
        /// <summary>
        /// 默认格式的表格
        /// </summary>
        Default = 0,

        /// <summary>
        /// Markdwon格式的表格
        /// </summary>
        MarkDown = 1,

        /// <summary>
        /// 交替格式的表格
        /// </summary>
        Alternative = 2,

        /// <summary>
        /// 最简格式的表格
        /// </summary>
        Minimal = 3
    }

    /// <summary>
    /// 表格显示样式信息
    /// 通过 Format 获取到的
    /// </summary>
    public class StyleInfo
    {
        public StyleInfo(string delimiterStr = "|", bool isShowTop_Down_DataBorder = true, string angleStr = "-")
        {
            DelimiterStr = delimiterStr;
            IsShowTop_Down_DataBorder = isShowTop_Down_DataBorder;
            AngleStr = angleStr;
        }

        /// <summary>
        /// 每一列数据之间的间隔字符串
        /// </summary>
        public string DelimiterStr { get; set; }

        /// <summary>
        /// 是否显示顶部，底部，和每一行数据之间的横向边框
        /// </summary>
        public bool IsShowTop_Down_DataBorder { get; set; }

        /// <summary>
        /// 边角字符串
        /// </summary>
        public string AngleStr { get; set; }
    }
}