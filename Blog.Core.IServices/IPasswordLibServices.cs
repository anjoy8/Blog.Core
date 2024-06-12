using System.Threading.Tasks;
using Blog.Core.IServices.BASE;
using Blog.Core.Model.Models;

namespace Blog.Core.IServices
{
    public partial interface IPasswordLibServices :IBaseServices<PasswordLib>
    {
        Task<bool> TestTranPropagation2();
        Task<bool> TestTranPropagationNoTranError();
        Task<bool> TestTranPropagationTran2();
        Task<bool> TestTranPropagationTran3();
    }
}
