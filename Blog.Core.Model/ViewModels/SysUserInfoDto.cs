using System;
using System.Collections.Generic;

namespace Blog.Core.Model.ViewModels
{
    public class SysUserInfoDto : SysUserInfoDtoRoot<int>
    {
        public string uLoginName { get; set; }
        public string uLoginPWD { get; set; }
        public string uRealName { get; set; }
        public int uStatus { get; set; }
        public string uRemark { get; set; }
        public System.DateTime uCreateTime { get; set; } = DateTime.Now;
        public System.DateTime uUpdateTime { get; set; } = DateTime.Now;
        public DateTime uLastErrTime { get; set; } = DateTime.Now;
        public int uErrorCount { get; set; }
        public string name { get; set; }
        public int sex { get; set; } = 0;
        public int age { get; set; }
        public DateTime birth { get; set; } = DateTime.Now;
        public string addr { get; set; }
        public bool tdIsDelete { get; set; }
        public List<string> RoleNames { get; set; }
    }
}
