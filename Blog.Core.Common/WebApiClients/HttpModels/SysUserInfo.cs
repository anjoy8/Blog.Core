using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebApiClient.DataAnnotations;
namespace Blog.Core.Common.WebApiClients
{
    /// <summary>用户信息表</summary>
    public class SysUserInfo
    {
        /// <summary>用户ID</summary>
        [AliasAs("uID")]
        public int UID { get; set; }

        /// <summary>登录账号</summary>
        [AliasAs("uLoginName")]
        public string ULoginName { get; set; }

        /// <summary>登录密码</summary>
        [AliasAs("uLoginPWD")]
        public string ULoginPWD { get; set; }

        /// <summary>真实姓名</summary>
        [AliasAs("uRealName")]
        public string URealName { get; set; }

        /// <summary>状态</summary>
        [AliasAs("uStatus")]
        public int UStatus { get; set; }

        /// <summary>备注</summary>
        [AliasAs("uRemark")]
        public string URemark { get; set; }

        /// <summary>创建时间</summary>
        [AliasAs("uCreateTime")]
        public System.DateTimeOffset UCreateTime { get; set; }

        /// <summary>更新时间</summary>
        [AliasAs("uUpdateTime")]
        public System.DateTimeOffset UUpdateTime { get; set; }

        /// <summary>最后登录时间</summary>
        [AliasAs("uLastErrTime")]
        public System.DateTimeOffset ULastErrTime { get; set; }

        /// <summary>错误次数</summary>
        [AliasAs("uErrorCount")]
        public int UErrorCount { get; set; }

        /// <summary>登录账号</summary>
        [AliasAs("name")]
        public string Name { get; set; }

        [AliasAs("sex")]
        public int Sex { get; set; }

        [AliasAs("age")]
        public int Age { get; set; }

        [AliasAs("birth")]
        public System.DateTimeOffset Birth { get; set; }

        [AliasAs("addr")]
        public string Addr { get; set; }

        [AliasAs("tdIsDelete")]
        public bool TdIsDelete { get; set; }

        [AliasAs("riDs")]
        public List<int> RiDs { get; set; }

        [AliasAs("roleNames")]
        public List<string> RoleNames { get; set; }

    }
}