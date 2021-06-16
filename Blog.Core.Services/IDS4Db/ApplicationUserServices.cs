using Blog.Core.IRepository.Base;
using Blog.Core.Model.IDS4DbModels;
using Blog.Core.Services.BASE;

namespace Blog.Core.IServices
{
    public class ApplicationUserServices : BaseServices<ApplicationUser>, IApplicationUserServices
    {

        IBaseRepository<ApplicationUser> _dal;
        public ApplicationUserServices(IBaseRepository<ApplicationUser> dal)
        {
            this._dal = dal;
            base.BaseDal = dal;
        }

    }
}