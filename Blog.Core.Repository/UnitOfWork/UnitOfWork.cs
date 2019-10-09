using Blog.Core.IRepository.UnitOfWork;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Core.Repository.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {

        private readonly ISqlSugarClient _sqlSugarClient;

        public UnitOfWork(ISqlSugarClient sqlSugarClient)
        {
            _sqlSugarClient = sqlSugarClient;
        }

        public ISqlSugarClient GetDbClient()
        {

            return _sqlSugarClient;
        }

        public void BeginTran()
        {
            GetDbClient().Ado.BeginTran(); 
        }

        public void CommitTran()
        {
            try
            {
                GetDbClient().Ado.CommitTran(); //
            }
            catch (Exception ex)
            {
                GetDbClient().Ado.RollbackTran();
                throw ex;
            }
        }

        public void RollbackTran()
        {
            GetDbClient().Ado.RollbackTran();
        }

    }

}
