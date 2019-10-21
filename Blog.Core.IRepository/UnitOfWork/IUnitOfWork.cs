using SqlSugar;

namespace Blog.Core.IRepository.UnitOfWork
{
    public interface IUnitOfWork
    {
        ISqlSugarClient GetDbClient();

        void BeginTran();

        void CommitTran();
        void RollbackTran();
    }
}
