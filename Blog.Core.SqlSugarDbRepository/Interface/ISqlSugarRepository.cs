using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Blog.Core.SqlSugarDbRepository.Interface
{
    public interface ISqlSugarRepository<TEntity> where TEntity : class
    {
        /// <summary>
        /// 修改Provider
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        IDisposable ChangeProvider(string name);


        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        int Insert(TEntity entity);

        /// <summary>
        /// 查询数据
        /// </summary>
        /// <returns></returns>
        List<TEntity> GetQuery(Expression<Func<TEntity, bool>> expression = null);

        /// <summary>
        /// 获取sqlSugar对象
        /// </summary>
        /// <returns></returns>
        SqlSugarClient GetCurrentSqlSugar();
    }

  
}
