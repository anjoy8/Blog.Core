using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Core.Model.ViewModels
{
    /// <summary>
    /// Trojan用户流量统计分组
    /// </summary>
    public class TrojanUseDetailDto
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public int userId { get; set; }
        /// <summary>
        /// 月度
        /// </summary>
        public string moth { get; set; } 
        /// <summary>
        /// 上传流量
        /// </summary>
        public decimal up { get; set; }
        /// <summary>
        /// 下载流量
        /// </summary>
        public decimal down { get; set; }
        /// <summary>
        /// 下载流量
        /// </summary>
        public decimal total {  get { return up + down; } }
    }
}
