using System.Threading.Tasks;
using Blog.Core.IServices.BASE;
using Blog.Core.Model.IDS4DbModels;

namespace Blog.Core.IServices
{
    public partial interface IApplicationUserServices : IBaseServices<ApplicationUser>
    {
        bool IsEnable();
    }
}