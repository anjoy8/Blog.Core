using System;

namespace Blog.Core.Model.ViewModels
{
    /// <summary>
    /// 广告类
    /// </summary>
    public class AdvertisementViewModels
    {
        /// <summary>
        /// 分类ID
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime Createdate { get; set; }

        /// <summary>
        /// 广告图片
        /// </summary>
        public string ImgUrl { get; set; }

        /// <summary>
        /// 广告标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 广告链接
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
}
