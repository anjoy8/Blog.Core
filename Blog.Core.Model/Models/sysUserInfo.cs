using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Core.Model.Models
{
     public class sysUserInfo
    {
        public sysUserInfo()
        {
            this.UserRole = new List<UserRole>();
            this.OperateLog = new List<OperateLog>();
        }
        /// <summary>
        /// 用户ID
        /// </summary>
        public int uID { get; set; }
        /// <summary>
        /// 登录账号
        /// </summary>
        public string uLoginName { get; set; }
        /// <summary>
        /// 登录密码
        /// </summary>
        public string uLoginPWD { get; set; }
        /// <summary>
        /// 真实姓名
        /// </summary>
        public string uRealName { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public int uStatus { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string uRemark { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public System.DateTime uCreateTime { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        public System.DateTime uUpdateTime { get; set; }

        /// <summary>
        ///最后登录时间 
        /// </summary>
        public DateTime uLastErrTime { get; set; }

        /// <summary>
        ///错误次数 
        /// </summary>
        public int uErrorCount { get; set; }

        public virtual ICollection<UserRole> UserRole { get; set; }
        public virtual ICollection<OperateLog> OperateLog { get; set; }
    }
}
