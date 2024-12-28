using System.Linq;

namespace Blog.Core.Model.Models
{
    ///<summary>
    ///
    ///</summary>
    [SugarTable("WeChatPushLog")]
    [MigrateVersion("1.0.0")]
    public partial class WeChatPushLog
    {

        /// <summary>
        /// 推送ID
        /// </summary>
        [SugarColumn(IsPrimaryKey = true,IsNullable = false)]
        public string id { get; set; } 
        /// <summary>
        /// 来自谁
        /// </summary>
        [SugarColumn(Length = 100, IsNullable = true)]
        public string PushLogFrom { get; set; }

        /// <summary>
        /// 推送IP
        /// </summary>
        [SugarColumn(Length = 50, IsNullable = true)]
        public string PushLogIP { get; set; }

        /// <summary>
        /// 推送客户
        /// </summary>
        [SugarColumn(Length = 100, IsNullable = true)]
        public string PushLogCompanyID { get; set; }

        /// <summary>
        /// 推送用户
        /// </summary>
        [SugarColumn(Length = 100, IsNullable = true)]
        public string PushLogToUserID { get; set; }

        /// <summary>
        /// 推送模板ID
        /// </summary>
        [SugarColumn(Length = 100, IsNullable = true)]
        public string PushLogTemplateID { get; set; }

        /// <summary>
        /// 推送内容
        /// </summary>
        [SugarColumn(Length = 2000, IsNullable = true)]
        public string PushLogContent { get; set; }

        /// <summary>
        /// 推送时间
        /// </summary>
        [SugarColumn(IsNullable = false)]
        public DateTime? PushLogTime { get; set; }

        /// <summary>
        /// 推送状态(Y/N)
        /// </summary>
        [SugarColumn(Length =1,IsNullable = false)]
        public string PushLogStatus { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [SugarColumn(Length = 200, IsNullable = false)]
        public string PushLogRemark { get; set; }

        /// <summary>
        /// 推送OpenID
        /// </summary>
        [SugarColumn(Length = 100, IsNullable = false)]
        public string PushLogOpenid { get; set; }

        /// <summary>
        /// 推送微信公众号
        /// </summary>
        [SugarColumn(Length = 100, IsNullable = false)]
        public string PushLogPublicAccount { get; set; }
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
