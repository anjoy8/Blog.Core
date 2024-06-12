using System.Threading.Tasks;
using Blog.Core.Common.DB;
using Blog.Core.Common.DB.Extension;
using Blog.Core.IRepository.Base;
using Blog.Core.Model.IDS4DbModels;
using Blog.Core.Services.BASE;

namespace Blog.Core.IServices
{
    public class ApplicationUserServices : BaseServices<ApplicationUser>, IApplicationUserServices
    {
        public bool IsEnable()
        {
            var configId = typeof(ApplicationUser).GetEntityTenant();
            return Db.AsTenant().IsAnyConnection(configId);
        }
    }
}