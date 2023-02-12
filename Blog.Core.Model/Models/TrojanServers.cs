 
using System;
using System.Linq;
using System.Text;
using SqlSugar;

namespace Blog.Core.Model.Models
{
    ///<summary>
    ///Trojan服务器
    ///</summary>
    [SugarTable("servers", "Trojan服务器")]
    [TenantAttribute("WMBLOG_MYSQL_2")] //('代表是哪个数据库，名字是appsettings.json 的 ConnId')
    public partial class TrojanServers
    {

        [SugarColumn(IsNullable = false, IsPrimaryKey = true, IsIdentity = true)]
        public int id { set; get; }
        public int userid { get; set; }
        public string servername { set; get; }
        public string serveraddress { set; get; }
        public int serverport { get; set; }
        [SugarColumn(IsNullable = true)]
        public string serverremark { get; set; }
        public bool serverenable { get; set; }
        public string serverpeer { get; set; }
        [SugarColumn(IsNullable = true)]
        public string serverpath { get; set; }
        public string servertype { get; set; }
    }
}
