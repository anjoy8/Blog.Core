 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Blog.Core.Model.ViewModels;
using SqlSugar;

namespace Blog.Core.Model.Models
{
    ///<summary>
    ///Trojan用户
    ///</summary>
    [SugarTable("users", "Trojan用户表")]
    [TenantAttribute("WMBLOG_MYSQL_2")] //('代表是哪个数据库，名字是appsettings.json 的 ConnId')
    public partial class TrojanUsers
    {

        [SugarColumn(IsNullable = false, IsPrimaryKey = true, IsIdentity = true)]
        public int id { set; get; }
        public string username { set; get; }
        public string password { set; get; }
        public Int64 quota { set; get; }
        public UInt64 download { set; get; }
        public UInt64 upload { set; get; } 
        public string passwordshow { set; get; }
        [SugarColumn(IsNullable = true)]
        public int CreateId { get; set; }
        [SugarColumn(IsNullable = true)]
        public string CreateBy { get; set; }
        [SugarColumn(IsNullable = true)]
        public DateTime? CreateTime { get; set; }
        /// <summary>
        /// 历史流量记录
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public List<TrojanUseDetailDto> useList { get; set; }
    }
}
