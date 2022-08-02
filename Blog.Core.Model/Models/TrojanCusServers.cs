 
using System;
using System.Linq;
using System.Text;
using SqlSugar;

namespace Blog.Core.Model.Models
{
    ///<summary>
    ///users自定义服务器
    ///</summary>
    [SugarTable("users_cus", "WMBLOG_MYSQL_2")]
    public partial class TrojanCusServers
    {

        [SugarColumn(IsNullable = false, IsPrimaryKey = true, IsIdentity = true)]
        public int id { set; get; }
        public int userid { get; set; }
        public string servername { set; get; }
        public string serveraddress { set; get; }
        [SugarColumn(IsNullable = true)]
        public string serverremark { get; set; }
        public bool serverenable { get; set; }
    }
}
