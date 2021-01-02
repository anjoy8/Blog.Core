namespace Blog.Core.Model.ViewModels
{
    /// <summary>
    /// 留言排名展示类
    /// </summary>
    public class TopgbViewModels
    {
        /// <summary>博客ID
        /// 
        /// </summary>
        public int? blogId { get; set; }

        /// <summary>
        /// 评论数量
        /// </summary>
        public int counts { get; set; }

        /// <summary>博客标题
        /// 
        /// </summary>
        public string btitle { get; set; }
    }
}
