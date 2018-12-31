using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Core.Model.Models
{
    /// <summary>
    /// 用户跟角色关联表
    /// </summary>
    public class PasswordLib
    {
        public int PLID { get; set; }

        /// <summary>
        ///获取或设置是否禁用，逻辑上的删除，非物理删除
        /// </summary>
        public bool? IsDeleted { get; set; }
        public string plURL { get; set; }
        public string plPWD { get; set; }
        public string plAccountName { get; set; }
        public int? plStatus { get; set; }
        public int? plErrorCount { get; set; }
        public string plHintPwd { get; set; }
        public string plHintquestion { get; set; }
        public DateTime? plCreateTime { get; set; }
        public DateTime? plUpdateTime { get; set; }
        public DateTime? plLastErrTime { get; set; }
        public string test { get; set; }


    }
}
