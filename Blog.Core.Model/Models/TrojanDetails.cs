
//模板自动生成(请勿修改) 
//作者:胡丁文
using System;
using System.Linq;
using System.Text;
using SqlSugar;

namespace Blog.Core.Model.Models
{
    ///<summary>
    ///用户流量每月汇总表
    ///</summary>
    [SugarTable("users_detail", "用户流量每月汇总表")]
    [TenantAttribute("WMBLOG_MYSQL_2")] //('代表是哪个数据库，名字是appsettings.json 的 ConnId')
    public partial class TrojanDetails
    {

        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int userId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime calDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ulong download { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ulong upload { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(IsNullable = true)] 
        public int? CreateId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string CreateBy { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public DateTime? CreateTime { get; set; }
    }
}
