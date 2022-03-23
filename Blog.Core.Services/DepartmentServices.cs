using Blog.Core.IServices;
using Blog.Core.Model.Models;
using Blog.Core.Services.BASE;
using Blog.Core.IRepository.Base;

namespace Blog.Core.Services
{
    /// <summary>
    /// DepartmentServices
    /// </summary>
    public class DepartmentServices : BaseServices<Department>, IDepartmentServices
    {
        private readonly IBaseRepository<Department> _dal;
        public DepartmentServices(IBaseRepository<Department> dal)
        {
            this._dal = dal;
            base.BaseDal = dal;
        }
    }
}