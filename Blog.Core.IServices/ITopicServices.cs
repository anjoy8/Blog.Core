using Blog.Core.IServices.BASE;
using Blog.Core.Model.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Blog.Core.IServices
{
    public interface ITopicServices : IBaseServices<Topic>
    {
        Task<List<Topic>> GetTopics();
    }
}
