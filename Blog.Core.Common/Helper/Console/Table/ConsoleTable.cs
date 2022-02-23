using Blog.Core.Common;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System
{
    public class ConsoleTable
    {
        #region 属性

        /// <summary>
        /// 表格头部字符串
        /// </summary>
        public string TitleString { get; set; }

        /// <summary>
        /// 表格的列
        /// </summary>
        public IList<string> Columns
        {
            get
            {
                if (_columns == null) _columns = new List<string>();
                return _columns;
            }
            set
            {
                _columns = value;
                _finalColumnWides = new List<int>();
            }
        }

        /// <summary>
        /// 行
        /// </summary>
        public List<string[]> Rows { get; set; } = new List<string[]>();

        /// <summary>
        /// 列宽
        /// </summary>
        public List<int> ColumnWides { get; set; } = new List<int>();

        /// <summary>
        /// 空白字符数量
        /// </summary>
        public int ColumnBlankNum { get; set; } = 4;

        /// <summary>
        /// 对其方式
        /// </summary>
        public Alignment Alignment { get; set; } = Alignment.Left;

        /// <summary>
        /// 是否显示行数
        /// </summary>
        public bool EnableCount { get; set; } = false;

        /// <summary>
        /// 表格显示样式
        /// 每次设置样子后就会重置 StyleInfo
        /// </summary>
        public TableStyle TableStyle
        {
            get
            {
                return _tableStyle;
            }
            set
            {
                if (_tableStyle == value) return;
                _tableStyle = value;
                _formatInfo = null;
            }
        }

        #endregion 属性

        #region 私有信息
        private IList<string> _columns;
        private TableStyle _tableStyle;
        private StyleInfo _formatInfo;
        private List<ColumnShowFormat> _columnShowFormats = new List<ColumnShowFormat>();
        private List<int> _finalColumnWides = new List<int>();

        /// <summary>
        /// 通过 Format 获得到表格显示样式
        /// </summary>
        private StyleInfo FormatInfo
        {
            get
            {
                if (_formatInfo == null)
                    _formatInfo = _tableStyle.GetFormatInfo();//得到样式信息
                return _formatInfo;
            }
            set
            {
                _formatInfo = value;
            }
        }

        /// <summary>
        /// 每一列的宽度
        /// </summary>
        private List<int> FinalColumnWides
        {
            get
            {
                if (_finalColumnWides is null || _finalColumnWides.Count < 1)
                {
                    // 得到每一列最大的宽度
                    List<int> _columnWides = Columns.GetColumnWides(Rows);
                    // 替换用户输入长度
                    ColumnWides ??= new List<int>();
                    for (int i = 0; i < ColumnWides.Count; i++) _columnWides[i] = ColumnWides[i];
                    _finalColumnWides = _columnWides;
                }
                return _finalColumnWides;
            }
        }

        /// <summary>
        /// 每一列显示的基本信息
        /// </summary>
        private List<ColumnShowFormat> ColumnShowFormats
        {
            get
            {
                if (_columnShowFormats.Count == 0)
                {
                    for (int i = 0; i < Columns.Count; i++) _columnShowFormats.Add(new ColumnShowFormat(i, FinalColumnWides[i], Alignment));
                }
                return _columnShowFormats;
            }
        }

        #endregion 私有信息

        #region 配置数据

        /// <summary>
        /// 添加列
        /// </summary>
        /// <param name="columnName">列明</param>
        /// <param name="columnWide">列的宽</param>
        /// <returns></returns>
        public ConsoleTable AddColumn(string columnName, int columnWide = 0)
        {
            Columns.Add(columnName);
            columnWide = columnWide == 0 ? columnName.Length : columnWide;
            _finalColumnWides.Add(columnWide);
            return this;
        }

        /// <summary>
        /// 添加行
        /// </summary>
        /// <param name="values">该行数据</param>
        /// <returns></returns>
        public ConsoleTable AddRow(params string[] values)
        {
            _ = values ?? throw new ArgumentNullException(nameof(values));

            Rows.Add(values);
            return this;
        }

        /// <summary>
        /// 加载 List 对象的数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="values"></param>
        /// <returns></returns>
        public static ConsoleTable From<T>(IEnumerable<T> values)
        {
            ConsoleTable table = new();

            List<string> columns = GetColumns<T>().Where(c => !string.IsNullOrWhiteSpace(c)).ToList();
            columns.ForEach(c =>
             {
                 table.AddColumn(c);
             });

            values.ToList().ForEach(value =>
            {
                table.AddRow(columns.Select(c => GetColumnValue(value, c)).ToArray());
            });

            return table;
        }

        #endregion 配置数据

        /// <summary>
        /// 获取表格字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder builder = new();

            builder.AppendLine(GetHeader());
            builder.AppendLine(GetExistData());
            builder.AppendLine(GetEnd());

            return builder.ToString();
        }

        /// <summary>
        /// 绘制表格
        /// </summary>
        /// <param name="format">样式</param>
        /// <param name="color">title颜色</param>
        public void Writer(ConsoleColor color = ConsoleColor.White)
        {
            ConsoleHelper.WriteColorLine(GetHeader(), color);
            ConsoleHelper.WriteInfoLine(GetExistData());
            ConsoleHelper.WriteColorLine(GetEnd(), color);
        }

        #region 帮助方法

        /// <summary>
        /// 获取完成头
        /// </summary>
        /// <returns></returns>
        public string GetHeader()
        {
            // 创建顶部和底部分隔线
            string top_DownDividerdivider = FinalColumnWides.GetTopAndDwon(FormatInfo.AngleStr, ColumnBlankNum);
            // 创建分隔线
            string divider = FinalColumnWides.GetDivider(FormatInfo.AngleStr, ColumnBlankNum);
            // 获取标题字符串
            string tilte = FinalColumnWides.GetTitleStr(TitleString, ColumnBlankNum, FormatInfo.DelimiterStr);
            // 得到头部字符串
            string headers = ColumnShowFormats.FillFormatTostring(Columns.ToArray(), FormatInfo.DelimiterStr, ColumnBlankNum);

            //绘制表格头
            StringBuilder top = new();
            if (FormatInfo.IsShowTop_Down_DataBorder) top.AppendLine(top_DownDividerdivider);
            if (!string.IsNullOrWhiteSpace(tilte))
            {
                top.AppendLine(tilte);
                top.AppendLine(divider);
            }
            top.AppendLine(headers);
            top.AppendLine(divider);
            return top.ToString().Trim();
        }

        /// <summary>
        /// 获取现有数据
        /// </summary>
        /// <returns></returns>
        public string GetExistData()
        {
            // 创建分隔线
            string divider = FinalColumnWides.GetDivider(FormatInfo.AngleStr, ColumnBlankNum);
            // 得到每行数据的字符串
            List<string> rowStrs = Rows.Select(row => ColumnShowFormats.FillFormatTostring(row, FormatInfo.DelimiterStr, ColumnBlankNum)).ToList();
            StringBuilder data = new();
            for (int i = 0; i < rowStrs.Count; i++)
            {
                if (FormatInfo.IsShowTop_Down_DataBorder && i != 0) data.AppendLine(divider);
                data.AppendLine(rowStrs[i]);
            }
            return data.ToString().Trim();
        }

        /// <summary>
        /// 获取新行数据
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        public string GetNewRow(string[] row)
        {
            if (row is null) return "";

            Rows.Add(row);
            //内容
            StringBuilder data = new();
            if (Rows.Count > 1) data.AppendLine(FinalColumnWides.GetDivider(FormatInfo.AngleStr, ColumnBlankNum));
            data.AppendLine(ColumnShowFormats.FillFormatTostring(row, FormatInfo.DelimiterStr, ColumnBlankNum));
            return data.ToString().Trim();
        }

        /// <summary>
        /// 获取底
        /// </summary>
        /// <returns></returns>
        public string GetEnd()
        {
            StringBuilder down = new();
            if (FormatInfo.IsShowTop_Down_DataBorder) down.AppendLine(FinalColumnWides.GetTopAndDwon(FormatInfo.AngleStr, ColumnBlankNum));
            if (EnableCount) down.AppendLine($" Count: {Rows.Count}");
            return down.ToString().Trim();
        }

        /// <summary>
        /// 获取列名
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private static IEnumerable<string> GetColumns<T>()
        {
            return typeof(T).GetProperties().Select(x => x.Name).ToArray();
        }

        /// <summary>
        /// 获取列值
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="obj">数据</param>
        /// <param name="column">列名</param>
        /// <returns></returns>
        private static string GetColumnValue<T>(T obj, string column)
        {
            if (obj == null) return null;

            JObject o = obj as JObject ?? (JObject)JsonConvert.DeserializeObject(JsonConvert.SerializeObject(obj));

            return o.GetValue(column).ToString();
        }

        #endregion 帮助方法
    }
}