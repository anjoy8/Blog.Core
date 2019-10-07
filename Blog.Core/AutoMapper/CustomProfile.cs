using AutoMapper;
using Blog.Core.Model.Models;
using Blog.Core.Model.ViewModels;

namespace Blog.Core.AutoMapper
{
    public class CustomProfile : Profile
    {
        /// <summary>
        /// 配置构造函数，用来创建关系映射
        /// </summary>
        public CustomProfile()
        {
            CreateMap<BlogArticle, BlogViewModels>();
            CreateMap<BlogViewModels, BlogArticle>();
        }
    }
}
