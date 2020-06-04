using Blog.Core.IServices.BASE;
using Blog.Core.Model.Models;
using Blog.Core.Model.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Blog.Core.IServices
{
    public interface IBlogArticleServices :IBaseServices<BlogArticle>
    {
        Task<List<BlogArticle>> GetBlogs();
        Task<BlogViewModels> GetBlogDetails(int id);

    }

}
