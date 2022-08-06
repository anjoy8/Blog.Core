using System;
using System.Threading.Tasks;
using Blog.Core.Common;
using Blog.Core.Common.DB;
using Blog.Core.IRepository.Base;
using Blog.Core.IServices;
using Blog.Core.Model.Models;
using Blog.Core.Services.BASE;

namespace Blog.Core.Services
{
    public partial class PasswordLibServices : BaseServices<PasswordLib>, IPasswordLibServices
    {
        IBaseRepository<PasswordLib> _dal;

        public PasswordLibServices(IBaseRepository<PasswordLib> dal)
        {
            this._dal = dal;
            base.BaseDal = dal;
        }

        [UseTran(Propagation = Propagation.Required)]
        public async Task<bool> TestTranPropagation2()
        {
            await _dal.Add(new PasswordLib()
            {
                IsDeleted = false,
                plAccountName = "aaa",
                plCreateTime = DateTime.Now
            });

            return true;
        }

        [UseTran(Propagation = Propagation.Mandatory)]
        public async Task<bool> TestTranPropagationNoTranError()
        {
            await _dal.Add(new PasswordLib()
            {
                IsDeleted = false,
                plAccountName = "aaa",
                plCreateTime = DateTime.Now
            });

            return true;
        }

        [UseTran(Propagation = Propagation.Nested)]
        public async Task<bool> TestTranPropagationTran2()
        {
            await _dal.Add(new PasswordLib()
            {
                IsDeleted = false,
                plAccountName = "aaa",
                plCreateTime = DateTime.Now
            });

            return true;
        }
    }
}