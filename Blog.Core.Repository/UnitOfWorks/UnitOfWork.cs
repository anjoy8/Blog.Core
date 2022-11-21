using System;
using Microsoft.Extensions.Logging;
using SqlSugar;

namespace Blog.Core.Repository.UnitOfWorks;

public class UnitOfWork : IDisposable
{
    public ILogger Logger { get; set; }
    public ISqlSugarClient Db { get; internal set; }

    public ITenant Tenant { get; internal set; }

    public bool IsTran { get; internal set; }

    public bool IsCommit { get; internal set; }

    public bool IsClose { get; internal set; }

    public void Dispose()
    {
        if (this.IsTran && !this.IsCommit)
        {
            Logger.LogDebug("UnitOfWork RollbackTran");
            this.Tenant.RollbackTran();
        }

        if (this.Db.Ado.Transaction != null || this.IsClose)
            return;
        this.Db.Close();
    }

    public bool Commit()
    {
        if (this.IsTran && !this.IsCommit)
        {
            Logger.LogDebug("UnitOfWork CommitTran");
            this.Tenant.CommitTran();
            this.IsCommit = true;
        }

        if (this.Db.Ado.Transaction == null && !this.IsClose)
        {
            this.Db.Close();
            this.IsClose = true;
        }

        return this.IsCommit;
    }
}