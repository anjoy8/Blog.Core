using Blog.Core.Model;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Blog.Core.IServices.BASE
{
    public interface IBaseServices<TEntity> where TEntity : class
    {

        Task<TEntity> QueryById(object objId);
        Task<TEntity> QueryById(object objId, bool blnUseCache = false);
        Task<List<TEntity>> QueryByIDs(object[] lstIds);

        Task<int> Add(TEntity model);

        Task<int> Add(List<TEntity> listEntity);

        Task<bool> DeleteById(object id);

        Task<bool> Delete(TEntity model);

        Task<bool> DeleteByIds(object[] ids);

        Task<bool> Update(TEntity model);
        Task<bool> Update(TEntity entity, string where);

        Task<bool> Update(object operateAnonymousObjects);

        Task<bool> Update(TEntity entity, List<string> lstColumns = null, List<string> lstIgnoreColumns = null, string where = "");

        Task<List<TEntity>> Query();
        Task<List<TEntity>> Query(string where);
        Task<List<TEntity>> Query(Expression<Func<TEntity, bool>> whereExpression);
        Task<List<TEntity>> Query(Expression<Func<TEntity, bool>> whereExpression, string orderByFields);
        Task<List<TResult>> Query<TResult>(Expression<Func<TEntity, TResult>> expression);
        Task<List<TResult>> Query<TResult>(Expression<Func<TEntity, TResult>> expression, Expression<Func<TEntity, bool>> whereExpression, string orderByFields);
        Task<List<TEntity>> Query(Expression<Func<TEntity, bool>> whereExpression, Expression<Func<TEntity, object>> orderByExpression, bool isAsc = true);
        Task<List<TEntity>> Query(string where, string orderByFields);
        Task<List<TEntity>> QuerySql(string sql, SugarParameter[] parameters = null);
        Task<DataTable> QueryTable(string sql, SugarParameter[] parameters = null);

        Task<List<TEntity>> Query(Expression<Func<TEntity, bool>> whereExpression, int top, string orderByFields);
        Task<List<TEntity>> Query(string where, int top, string orderByFields);

        Task<List<TEntity>> Query(
            Expression<Func<TEntity, bool>> whereExpression, int pageIndex, int pageSize, string orderByFields);
        Task<List<TEntity>> Query(string where, int pageIndex, int pageSize, string orderByFields);


        Task<PageModel<TEntity>> QueryPage(Expression<Func<TEntity, bool>> whereExpression, int pageIndex = 1, int pageSize = 20, string orderByFields = null);

        Task<List<TResult>> QueryMuch<T, T2, T3, TResult>(
            Expression<Func<T, T2, T3, object[]>> joinExpression,
            Expression<Func<T, T2, T3, TResult>> selectExpression,
            Expression<Func<T, T2, T3, bool>> whereLambda = null) where T : class, new();
        Task<PageModel<TEntity>> QueryPage(PaginationModel pagination);
    }

}
