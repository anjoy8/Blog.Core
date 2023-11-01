using SqlSugar;
using System;

namespace Blog.Core.Model.Models
{
    /// <summary>
    /// 密码库表
    /// </summary>
    [SugarTable("PasswordLib", "密码库表")]//('数据库表名'，'数据库表备注')
    //[TenantAttribute("WMBLOG_MYSQL_2")] //('代表是哪个数据库，名字是appsettings.json 的 ConnId')
    public class PasswordLib
    {
        [SugarColumn(IsNullable = false, IsPrimaryKey = true, IsIdentity = false)]
        public long PLID { get; set; }

        /// <summary>
        ///获取或设置是否禁用，逻辑上的删除，非物理删除
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public bool? IsDeleted { get; set; }

        [SugarColumn(Length = 200, IsNullable = true)]
        public string plURL { get; set; }

        [SugarColumn(Length = 100, IsNullable = true)]
        public string plPWD { get; set; }

        [SugarColumn(Length = 200, IsNullable = true)]
        public string plAccountName { get; set; }

        [SugarColumn(IsNullable = true)]
        public int? plStatus { get; set; }

        [SugarColumn(IsNullable = true)]
        public int? plErrorCount { get; set; }

        [SugarColumn(Length = 200, IsNullable = true)]
        public string plHintPwd { get; set; }

        [SugarColumn(Length = 200, IsNullable = true)]
        public string plHintquestion { get; set; }

        [SugarColumn(IsNullable = true)]
        public DateTime? plCreateTime { get; set; }

        [SugarColumn(IsNullable = true)]
        public DateTime? plUpdateTime { get; set; }

        [SugarColumn(IsNullable = true)]
        public DateTime? plLastErrTime { get; set; }

        [SugarColumn(Length = 200, IsNullable = true)]
        public string test { get; set; }


    }
}
