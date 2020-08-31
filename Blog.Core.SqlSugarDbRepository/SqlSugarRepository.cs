using Blog.Core.SqlSugarDbRepository.Interface;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;

namespace Blog.Core.SqlSugarDbRepository
{
    public class SqlSugarRepository<TEntity> : ISqlSugarRepository<TEntity>
      where TEntity : class, new()
    {

        public string ProviderName { get; private set; }
        public string OldProviderName { get; private set; }
        protected readonly ISqlSugarProviderStorage<ISqlSugarProvider> _sqlSugarProviderStorage;

        public SqlSugarRepository(ISqlSugarProviderStorage<ISqlSugarProvider> sqlSugarProviderStorage)
        {
            _sqlSugarProviderStorage = sqlSugarProviderStorage;
        }



        public IDisposable ChangeProvider(string name)
        {
            OldProviderName = ProviderName;
            ProviderName = name;
            return new DisposeAction(() =>
            {
                ProviderName = OldProviderName;
                OldProviderName = null;
            });
        }


        public SqlSugarClient GetCurrentSqlSugar()
        {
            return this._sqlSugarProviderStorage.GetByName(this.ProviderName, SqlSugarDbStorageConsts.DefaultProviderName).Sugar;
        }

        public int Insert(TEntity entity)
        {
            return this.GetCurrentSqlSugar().Insertable<TEntity>(entity).ExecuteCommand();
        }

        public List<TEntity> GetQuery(Expression<Func<TEntity, bool>> expression = null)
        {
            return this.GetCurrentSqlSugar().Queryable<TEntity>().Where(expression).ToList();
        }

        public void Dispose()
        {

        }


    }

    public class DisposeAction : IDisposable
    {
        public static readonly DisposeAction Empty = new DisposeAction(null);

        private Action _action;


        public DisposeAction(Action action)
        {
            _action = action;
        }

        public void Dispose()
        {
            var action = Interlocked.Exchange(ref _action, null);
            action?.Invoke();
        }
    }

    public class SqlSugarDbStorageConsts
    {
        public static string DefaultProviderName = "DefaultProviderName";
    }

}
