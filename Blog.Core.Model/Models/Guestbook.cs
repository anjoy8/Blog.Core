namespace Blog.Core.Model.Models
{
    [MigrateVersion("1.0.0")]
    public class Guestbook : RootEntityTkey<long>
    {

        /// <summary>博客ID
        /// 
        /// </summary>
        public long? blogId { get; set; }
        /// <summary>创建时间
        /// 
        /// </summary>
        public DateTime createdate { get; set; }

        [SugarColumn(Length = 2000, IsNullable = true)]
        public string username { get; set; }

        /// <summary>手机
        /// 
        /// </summary>
        [SugarColumn(Length = 2000, IsNullable = true)]
        public string phone { get; set; }
        /// <summary>qq
        /// 
        /// </summary>
        [SugarColumn(Length = 2000, IsNullable = true)]
        public string QQ { get; set; }

        /// <summary>留言内容
        /// 
        /// </summary>
        [SugarColumn(Length = 2000, IsNullable = true)]
        public string body { get; set; }
        /// <summary>ip地址
        /// 
        /// </summary>
        [SugarColumn(Length = 2000, IsNullable = true)]
        public string ip { get; set; }

        /// <summary>是否显示在前台,0否1是
        /// 
        /// </summary>
        public bool isshow { get; set; }

        [SugarColumn(IsIgnore = true)]
        public BlogArticle blogarticle { get; set; }
    }
}
