using Newtonsoft.Json;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Blog.Core.Model.Models
{
    [SplitTable(SplitType.Day)]//按天分表 （自带分表支持 年、季、月、周、日）
    [SugarTable("SplitDemo_{year}{month}{day}")]//3个变量必须要有，这么设计为了兼容开始按年，后面改成按月、按日
    [MigrateVersion("1.0.0")]
    public class SplitDemo
    {
        [SugarColumn(IsPrimaryKey = true)]
        public long Id { get; set; }

        public string Name { get; set; }

        [SugarColumn(IsNullable = true)]//设置为可空字段 (更多用法看文档 迁移)
        public DateTime UpdateTime { get; set; }

        [SplitField] //分表字段 在插入的时候会根据这个字段插入哪个表，在更新删除的时候用这个字段找出相关表
        public DateTime CreateTime { get; set; }
    }
}
