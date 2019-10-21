using Blog.Core.IServices.BASE;
using Blog.Core.Model.Models;
using System.Threading.Tasks;

namespace Blog.Core.IServices
{
    public partial interface IGuestbookServices : IBaseServices<Guestbook>
    {
        Task<bool> TestTranInRepository();
        Task<bool> TestTranInRepositoryAOP();
    }
}
