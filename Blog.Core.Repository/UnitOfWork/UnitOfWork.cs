using Blog.Core.Common;
using Blog.Core.Common.LogHelper;
using Blog.Core.IRepository.UnitOfWork;
using SqlSugar;
using StackExchange.Profiling;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Core.Repository.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {

        private readonly ISqlSugarClient _sqlSugarClient;
        private readonly List<ISqlSugarClient> _sqlSugarClients;

        public UnitOfWork(List<ISqlSugarClient> sqlSugarClients)
        {
            // 每次取最上边的db，这个顺序已经在注册服务的时候，切换好了
            _sqlSugarClient = sqlSugarClients[0];
           
            if (Appsettings.app(new string[] { "AppSettings", "SqlAOP", "Enabled" }).ObjToBool())
            {
                _sqlSugarClient.Aop.OnLogExecuting = (sql, pars) => //SQL执行中事件
                {
                    Parallel.For(0, 1, e =>
                    {
                        MiniProfiler.Current.CustomTiming("SQL：", GetParas(pars) + "【SQL语句】：" + sql);
                        LogLock.OutSql2Log("SqlLog", new string[] { GetParas(pars), "【SQL语句】：" + sql });

                    });
                };
            }
        }

        private string GetParas(SugarParameter[] pars)
        {
            string key = "【SQL参数】：";
            foreach (var param in pars)
            {
                key += $"{param.ParameterName}:{param.Value}\n";
            }

            return key;
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
