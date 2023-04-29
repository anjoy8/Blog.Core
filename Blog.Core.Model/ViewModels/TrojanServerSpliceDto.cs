using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Core.Model.ViewModels
{
    /// <summary>
    /// Trojan服务器拼接服务器和订阅地址
    /// </summary>
    public class TrojanServerSpliceDto
    {
        /// <summary>
        /// 普通订阅连接
        /// </summary>
        public string normalApi { get; set; }
        /// <summary>
        /// clash订阅连接
        /// </summary>
        public string clashApi { get; set; }
        /// <summary>
        /// 备用clash订阅连接
        /// </summary>
        public string clashApiBackup { get; set; }
        public List<TrojanServerDto> list { get; set; } = new List<TrojanServerDto>();
    } 
}
