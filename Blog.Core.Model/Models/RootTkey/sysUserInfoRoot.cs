using SqlSugar;
using System;
using System.Collections.Generic;

namespace Blog.Core.Model
{
    /// <summary>
    /// 用户信息表
    /// </summary>
    public class sysUserInfoRoot<Tkey> where Tkey : IEquatable<Tkey>
    {
        /// <summary>
        /// uID
        /// 泛型主键Tkey
        /// </summary>
        [SugarColumn(IsNullable = false, IsPrimaryKey = true)]
        public Tkey uID { get; set; }

        [SugarColumn(IsIgnore = true)]
        public List<Tkey> RIDs { get; set; }

    }
}
