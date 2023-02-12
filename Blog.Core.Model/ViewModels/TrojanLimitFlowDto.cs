using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Core.Model.ViewModels
{
    /// <summary>
    /// 限制流量dto
    /// 作者:胡丁文
    /// 时间:2020-4-27 16:57:07
    /// </summary>
    public class TrojanLimitFlowDto
    {
        /// <summary>
        /// 用户
        /// </summary>
        public int[] users { get; set; }
        /// <summary>
        /// 流量(-1为无限,单位为最小单位byte)
        /// </summary>
        public Int64 quota { get; set; }
    }
}
