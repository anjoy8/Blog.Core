using System;
using System.Threading.Tasks;
using Blog.Core.Common;
using Blog.Core.Common.DB;
using Blog.Core.IRepository.Base;
using Blog.Core.IServices;
using Blog.Core.Model.Models;
using Blog.Core.Repository.UnitOfWorks;
using Blog.Core.Services.BASE;
using SqlSugar;

namespace Blog.Core.Services
{
    public partial class PasswordLibServices : BaseServices<PasswordLib>, IPasswordLibServices
    {
        IBaseRepository<PasswordLib> _dal;
        private readonly IUnitOfWorkManage _unitOfWorkManage;
        private readonly ISqlSugarClient _db;
        private SqlSugarScope db => _db as SqlSugarScope;

        public PasswordLibServices(IBaseRepository<PasswordLib> dal, IUnitOfWorkManage unitOfWorkManage, ISqlSugarClient db)
        {
            this._dal = dal;
            _unitOfWorkManage = unitOfWorkManage;
            _db = db;
            base.BaseDal = dal;
        }

        [UseTran(Propagation = Propagation.Required)]
        public async Task<bool> TestTranPropagation2()
        {
            await _dal.Add(new PasswordLib()
            {
                PLID = SnowFlakeSingle.Instance.NextId(),
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
            await db.Insertable(new PasswordLib()
            {
                PLID = SnowFlakeSingle.Instance.NextId(),
                IsDeleted = false,
                plAccountName = "aaa",
                plCreateTime = DateTime.Now
            }).ExecuteReturnSnowflakeIdAsync();

            throw new Exception("123");
            return true;
        }

        public async Task<bool> TestTranPropagationTran3()
        {
            Console.WriteLine("Begin Transaction Before:" + db.ContextID);
            db.BeginTran();
            Console.WriteLine("Begin Transaction After:" + db.ContextID);
            Console.WriteLine("");
            await db.Insertable(new PasswordLib()
            {
                PLID = SnowFlakeSingle.Instance.NextId(),
                IsDeleted = false,
                plAccountName = "aaa",
                plCreateTime = DateTime.Now
            }).ExecuteReturnSnowflakeIdAsync();

            throw new Exception("123");
            return true;
        }
    }
}