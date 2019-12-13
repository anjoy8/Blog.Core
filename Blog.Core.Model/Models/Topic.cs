using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Core.Model.Models
{
    /// <summary>
    /// Tibug 类别
    /// </summary>
    public class Topic:RootEntity
    {
        public Topic()
        {
            this.TopicDetail = new List<TopicDetail>();
            this.tUpdatetime = DateTime.Now;
        }
        [SugarColumn(Length = 200, IsNullable = true)]
        public string tLogo { get; set; }

        [SugarColumn(Length = 200, IsNullable = true)]
        public string tName { get; set; }

        [SugarColumn(Length = 400, IsNullable = true)]
        public string tDetail { get; set; }

        [SugarColumn(Length = 200, IsNullable = true)]
        public string tAuthor { get; set; }

        [SugarColumn(Length = 200, IsNullable = true)]
        public string tSectendDetail { get; set; }

        public bool tIsDelete { get; set; }
        public int tRead { get; set; }
        public int tCommend { get; set; }
        public int tGood { get; set; }
        public DateTime tCreatetime { get; set; }
        public DateTime tUpdatetime { get; set; }

        [SugarColumn(IsIgnore = true)]
        public virtual ICollection<TopicDetail> TopicDetail { get; set; }
    }
}
