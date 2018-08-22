using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Core.Model.Models
{
    /// <summary>博客文章
    /// 
    /// </summary
    public class TopicDetail
    {
        public TopicDetail()
        {
            this.tdUpdatetime = DateTime.Now;
        }
        public int Id { get; set; }
        public int TopicId { get; set; }
        public string tdLogo { get; set; }
        public string tdName { get; set; }
        public string tdContent { get; set; }
        public string tdDetail { get; set; }
        public string tdSectendDetail { get; set; }
        public bool tdIsDelete { get; set; }
        public int tdRead { get; set; }
        public int tdCommend { get; set; }
        public int tdGood { get; set; }
        public DateTime tdCreatetime { get; set; }
        public DateTime tdUpdatetime { get; set; }
        public int tdTop { get; set; }

        public virtual Topic Topic { get; set; }

    }
}
