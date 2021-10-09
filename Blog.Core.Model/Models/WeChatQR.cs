using System;
using System.Linq;
using System.Text;
using SqlSugar;

namespace Blog.Core.Model.Models
{
    ///<summary>
    ///
    ///</summary>
    [SugarTable("WeChatQR")]
    public partial class WeChatQR
    {

        /// <summary>
        /// 主键id,ticket
        /// </summary>
        [SugarColumn(Length = 200,IsPrimaryKey =true, IsNullable = false)]
        public string QRticket { get; set; }

        /// <summary>
        /// 需要绑定的公司
        /// </summary>
        [SugarColumn(Length = 100, IsNullable = false)]
        public string QRbindCompanyID { get; set; }

        /// <summary>
        /// 需要绑定的员工id
        /// </summary>
        [SugarColumn(Length = 100, IsNullable = false)]
        public string QRbindJobID { get; set; }
        /// <summary>
        /// 需要绑定的员工昵称
        /// </summary>
        [SugarColumn(Length = 100, IsNullable = true)]
        public string QRbindJobNick { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime QRcrateTime { get; set; }

        /// <summary>
        /// 关联的公众号
        /// </summary>
        [SugarColumn(Length = 100, IsNullable = false)]
        public string QRpublicAccount { get; set; }

        /// <summary>
        /// 是否已使用
        /// </summary>
        public bool QRisUsed { get; set; }

        /// <summary>
        /// 使用时间
        /// </summary>
        [SugarColumn(Length = 100, IsNullable = true)]
        public DateTime? QRuseTime { get; set; }

        /// <summary>
        /// 关联的微信用户id
        /// </summary>
        [SugarColumn(Length = 100, IsNullable = true)]
        public string QRuseOpenid { get; set; }
        /// <summary>
        /// 创建者id
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public int? CreateId { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string CreateBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public DateTime? CreateTime { get; set; }
        /// <summary>
        /// 修改者id
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public int? ModifyId { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string ModifyBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public DateTime? ModifyTime { get; set; }
    }
}
