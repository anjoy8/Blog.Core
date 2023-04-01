using Blog.Core.Model;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Blog.Core.IRepository.Base
{
    public interface IBaseRepository<TEntity> where TEntity : class
    {
        /// <summary>
        /// SqlsugarClient实体
        /// </summary>
        ISqlSugarClient Db { get;}
        /// <summary>
        /// 根据Id查询实体
        /// </summary>
        /// <param name="objId"></param>
        /// <returns></returns>
        Task<TEntity> QueryById(object objId);
        Task<TEntity> QueryById(object objId, bool blnUseCache = false);
        /// <summary>
        /// 根据id数组查询实体list
        /// </summary>
        /// <param name="lstIds"></param>
        /// <returns></returns>
        Task<List<TEntity>> QueryByIDs(object[] lstIds);
        
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<long> Add(TEntity model);

        /// <summary>
        /// 批量添加
        /// </summary>
        /// <param name="listEntity"></param>
        /// <returns></returns>
        Task<List<long>> Add(List<TEntity> listEntity);

        /// <summary>
        /// 根据id 删除某一实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> DeleteById(object id);

        /// <summary>
        /// 根据对象，删除某一实体
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<bool> Delete(TEntity model);

        /// <summary>
        /// 根据id数组，删除实体list
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<bool> DeleteByIds(object[] ids);

        /// <summary>
        /// 更新model
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<bool> Update(TEntity model);

        /// <summary>
        /// 根据model，更新，带where条件
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        Task<bool> Update(TEntity entity, string where);
        Task<bool> Update(object operateAnonymousObjects);

        /// <summary>
        /// 根据model，更新，指定列
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="lstColumns"></param>
        /// <param name="lstIgnoreColumns"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        Task<bool> Update(TEntity entity, List<string> lstColumns = null, List<string> lstIgnoreColumns = null, string where = "");

        /// <summary>
        /// 查询
        /// </summary>
        /// <returns></returns>
        Task<List<TEntity>> Query();

        /// <summary>
        /// 带sql where查询
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        Task<List<TEntity>> Query(string where);

        /// <summary>
        /// 根据表达式查询
        /// </summary>
        /// <param name="whereExpression"></param>
        /// <returns></returns>
        Task<List<TEntity>> Query(Expression<Func<TEntity, bool>> whereExpression);

        /// <summary>
        /// 根据表达式，指定返回对象模型，查询
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        Task<List<TResult>> Query<TResult>(Expression<Func<TEntity, TResult>> expression);

        /// <summary>
        /// 根据表达式，指定返回对象模型，排序，查询
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="expression"></param>
        /// <param name="whereExpression"></param>
        /// <param name="orderByFields"></param>
        /// <returns></returns>
        Task<List<TResult>> Query<TResult>(Expression<Func<TEntity, TResult>> expression, Expression<Func<TEntity, bool>> whereExpression, string orderByFields);
        Task<List<TEntity>> Query(Expression<Func<TEntity, bool>> whereExpression, string orderByFields);
        Task<List<TEntity>> Query(Expression<Func<TEntity, bool>> whereExpression, Expression<Func<TEntity, object>> orderByExpression, bool isAsc = true);
        Task<List<TEntity>> Query(string where, string orderByFields);

        Task<List<TEntity>> Query(Expression<Func<TEntity, bool>> whereExpression, int intTop, string orderByFields);
        Task<List<TEntity>> Query(string where, int intTop, string orderByFields);
        Task<List<TEntity>> QuerySql(string sql, SugarParameter[] parameters = null);
        Task<DataTable> QueryTable(string sql, SugarParameter[] parameters = null);

        Task<List<TEntity>> Query(
            Expression<Func<TEntity, bool>> whereExpression, int pageIndex, int pageSize, string orderByFields);
        Task<List<TEntity>> Query(string where, int pageIndex, int pageSize, string orderByFields);

        /// <summary>
        /// 根据表达式，排序字段，分页查询
        /// </summary>
        /// <param name="whereExpression"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="orderByFields"></param>
        /// <returns></returns>
        Task<PageModel<TEntity>> QueryPage(Expression<Func<TEntity, bool>> whereExpression, int pageIndex = 1, int pageSize = 20, string orderByFields = null);

        /// <summary>
        /// 三表联查
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="joinExpression"></param>
        /// <param name="selectExpression"></param>
        /// <param name="whereLambda"></param>
        /// <returns></returns>
        Task<List<TResult>> QueryMuch<T, T2, T3, TResult>(
            Expression<Func<T, T2, T3, object[]>> joinExpression,
            Expression<Func<T, T2, T3, TResult>> selectExpression,
            Expression<Func<T, T2, T3, bool>> whereLambda = null) where T : class, new();

        /// <summary>
        /// 两表联查-分页
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="joinExpression"></param>
        /// <param name="selectExpression"></param>
        /// <param name="whereExpression"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="orderByFields"></param>
        /// <returns></returns>
        Task<PageModel<TResult>> QueryTabsPage<T, T2, TResult>(
            Expression<Func<T, T2, object[]>> joinExpression,
            Expression<Func<T, T2, TResult>> selectExpression,
            Expression<Func<TResult, bool>> whereExpression,
            int pageIndex = 1,
            int pageSize = 20,
            string orderByFields = null);

        /// <summary>
        /// 两表联合查询-分页-分组
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="joinExpression"></param>
        /// <param name="selectExpression"></param>
        /// <param name="whereExpression"></param>
        /// <param name="groupExpression"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="orderByFields"></param>
        /// <returns></returns>
        Task<PageModel<TResult>> QueryTabsPage<T, T2, TResult>(
            Expression<Func<T, T2, object[]>> joinExpression,
            Expression<Func<T, T2, TResult>> selectExpression,
            Expression<Func<TResult, bool>> whereExpression,
            Expression<Func<T, object>> groupExpression,
            int pageIndex = 1,
            int pageSize = 20,
            string orderByFields = null);

        #region 分表
        /// <summary>
        /// 通过ID查询
        /// </summary>
        /// <param name="objId"></param>
        /// <returns></returns>
        Task<TEntity> QueryByIdSplit(object objId);
        /// <summary>
        /// 自动分表插入
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<List<long>> AddSplit(TEntity entity);
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        Task<bool> DeleteSplit(TEntity entity, DateTime dateTime);
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        Task<bool> UpdateSplit(TEntity entity, DateTime dateTime);
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="whereExpression"></param>
        /// <param name="beginTime"></param>
        /// <param name="endTime"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="orderByFields"></param>
        /// <returns></returns>
        Task<PageModel<TEntity>> QueryPageSplit(Expression<Func<TEntity, bool>> whereExpression, DateTime beginTime, DateTime endTime, int pageIndex = 1, int pageSize = 20, string orderByFields = null);
        #endregion
    }
}
