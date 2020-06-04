using Blog.Core.Common;
using Blog.Core.Common.DB;
using Blog.Core.IRepository.Base;
using Blog.Core.IRepository.UnitOfWork;
using Blog.Core.Model;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Blog.Core.Repository.Base
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class, new()
    {
        private readonly IUnitOfWork _unitOfWork;
        private SqlSugarClient _dbBase;

        private ISqlSugarClient _db
        {
            get
            {
                /* 如果要开启多库支持，
                 * 1、在appsettings.json 中开启MutiDBEnabled节点为true，必填
                 * 2、设置一个主连接的数据库ID，节点MainDB，对应的连接字符串的Enabled也必须true，必填
                 */
                if (Appsettings.app(new string[] { "MutiDBEnabled" }).ObjToBool())
                {
                    if (typeof(TEntity).GetTypeInfo().GetCustomAttributes(typeof(SugarTable), true).FirstOrDefault((x => x.GetType() == typeof(SugarTable))) is SugarTable sugarTable && !string.IsNullOrEmpty(sugarTable.TableDescription))
                    {
                        _dbBase.ChangeDatabase(sugarTable.TableDescription.ToLower());
                    }
                    else
                    {
                        _dbBase.ChangeDatabase(MainDb.CurrentDbConnId.ToLower());
                    }
                }

                return _dbBase;
            }
        }

        internal ISqlSugarClient Db
        {
            get { return _db; }
        }

        public BaseRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _dbBase = unitOfWork.GetDbClient();
        }



        public async Task<TEntity> QueryById(object objId)
        {
            //return await Task.Run(() => _db.Queryable<TEntity>().InSingle(objId));
            return await _db.Queryable<TEntity>().In(objId).SingleAsync();
        }
        /// <summary>
        /// 功能描述:根据ID查询一条数据
        /// 作　　者:Blog.Core
        /// </summary>
        /// <param name="objId">id（必须指定主键特性 [SugarColumn(IsPrimaryKey=true)]），如果是联合主键，请使用Where条件</param>
        /// <param name="blnUseCache">是否使用缓存</param>
        /// <returns>数据实体</returns>
        public async Task<TEntity> QueryById(object objId, bool blnUseCache = false)
        {
            //return await Task.Run(() => _db.Queryable<TEntity>().WithCacheIF(blnUseCache).InSingle(objId));
            return await _db.Queryable<TEntity>().WithCacheIF(blnUseCache).In(objId).SingleAsync();
        }

        /// <summary>
        /// 功能描述:根据ID查询数据
        /// 作　　者:Blog.Core
        /// </summary>
        /// <param name="lstIds">id列表（必须指定主键特性 [SugarColumn(IsPrimaryKey=true)]），如果是联合主键，请使用Where条件</param>
        /// <returns>数据实体列表</returns>
        public async Task<List<TEntity>> QueryByIDs(object[] lstIds)
        {
            //return await Task.Run(() => _db.Queryable<TEntity>().In(lstIds).ToList());
            return await _db.Queryable<TEntity>().In(lstIds).ToListAsync();
        }

        /// <summary>
        /// 写入实体数据
        /// </summary>
        /// <param name="entity">博文实体类</param>
        /// <returns></returns>
        public async Task<int> Add(TEntity entity)
        {
            //var i = await Task.Run(() => _db.Insertable(entity).ExecuteReturnBigIdentity());
            ////返回的i是long类型,这里你可以根据你的业务需要进行处理
            //return (int)i;

            var insert = _db.Insertable(entity);
            return await insert.ExecuteReturnIdentityAsync();
        }


        /// <summary>
        /// 写入实体数据
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <param name="insertColumns">指定只插入列</param>
        /// <returns>返回自增量列</returns>
        public async Task<int> Add(TEntity entity, Expression<Func<TEntity, object>> insertColumns = null)
        {
            var insert = _db.Insertable(entity);
            if (insertColumns == null)
            {
                return await insert.ExecuteReturnIdentityAsync();
            }
            else
            {
                return await insert.InsertColumns(insertColumns).ExecuteReturnIdentityAsync();
            }
        }

        /// <summary>
        /// 批量插入实体(速度快)
        /// </summary>
        /// <param name="listEntity">实体集合</param>
        /// <returns>影响行数</returns>
        public async Task<int> Add(List<TEntity> listEntity)
        {
            return await _db.Insertable(listEntity.ToArray()).ExecuteCommandAsync();
        }

        /// <summary>
        /// 更新实体数据
        /// </summary>
        /// <param name="entity">博文实体类</param>
        /// <returns></returns>
        public async Task<bool> Update(TEntity entity)
        {
            ////这种方式会以主键为条件
            //var i = await Task.Run(() => _db.Updateable(entity).ExecuteCommand());
            //return i > 0;
            //这种方式会以主键为条件
            return await _db.Updateable(entity).ExecuteCommandHasChangeAsync();
        }

        public async Task<bool> Update(TEntity entity, string strWhere)
        {
            //return await Task.Run(() => _db.Updateable(entity).Where(strWhere).ExecuteCommand() > 0);
            return await _db.Updateable(entity).Where(strWhere).ExecuteCommandHasChangeAsync();
        }

        public async Task<bool> Update(string strSql, SugarParameter[] parameters = null)
        {
            //return await Task.Run(() => _db.Ado.ExecuteCommand(strSql, parameters) > 0);
            return await _db.Ado.ExecuteCommandAsync(strSql, parameters) > 0;
        }

        public async Task<bool> Update(object operateAnonymousObjects)
        {
            return await _db.Updateable<TEntity>(operateAnonymousObjects).ExecuteCommandAsync() > 0;
        }

        public async Task<bool> Update(
          TEntity entity,
          List<string> lstColumns = null,
          List<string> lstIgnoreColumns = null,
          string strWhere = ""
            )
        {
            //IUpdateable<TEntity> up = await Task.Run(() => _db.Updateable(entity));
            //if (lstIgnoreColumns != null && lstIgnoreColumns.Count > 0)
            //{
            //    up = await Task.Run(() => up.IgnoreColumns(it => lstIgnoreColumns.Contains(it)));
            //}
            //if (lstColumns != null && lstColumns.Count > 0)
            //{
            //    up = await Task.Run(() => up.UpdateColumns(it => lstColumns.Contains(it)));
            //}
            //if (!string.IsNullOrEmpty(strWhere))
            //{
            //    up = await Task.Run(() => up.Where(strWhere));
            //}
            //return await Task.Run(() => up.ExecuteCommand()) > 0;

            IUpdateable<TEntity> up = _db.Updateable(entity);
            if (lstIgnoreColumns != null && lstIgnoreColumns.Count > 0)
            {
                up = up.IgnoreColumns(lstIgnoreColumns.ToArray());
            }
            if (lstColumns != null && lstColumns.Count > 0)
            {
                up = up.UpdateColumns(lstColumns.ToArray());
            }
            if (!string.IsNullOrEmpty(strWhere))
            {
                up = up.Where(strWhere);
            }
            return await up.ExecuteCommandHasChangeAsync();
        }

        /// <summary>
        /// 根据实体删除一条数据
        /// </summary>
        /// <param name="entity">博文实体类</param>
        /// <returns></returns>
        public async Task<bool> Delete(TEntity entity)
        {
            //var i = await Task.Run(() => _db.Deleteable(entity).ExecuteCommand());
            //return i > 0;
            return await _db.Deleteable(entity).ExecuteCommandHasChangeAsync();
        }

        /// <summary>
        /// 删除指定ID的数据
        /// </summary>
        /// <param name="id">主键ID</param>
        /// <returns></returns>
        public async Task<bool> DeleteById(object id)
        {
            //var i = await Task.Run(() => _db.Deleteable<TEntity>(id).ExecuteCommand());
            //return i > 0;
            return await _db.Deleteable<TEntity>(id).ExecuteCommandHasChangeAsync();
        }

        /// <summary>
        /// 删除指定ID集合的数据(批量删除)
        /// </summary>
        /// <param name="ids">主键ID集合</param>
        /// <returns></returns>
        public async Task<bool> DeleteByIds(object[] ids)
        {
            //var i = await Task.Run(() => _db.Deleteable<TEntity>().In(ids).ExecuteCommand());
            //return i > 0;
            return await _db.Deleteable<TEntity>().In(ids).ExecuteCommandHasChangeAsync();
        }



        /// <summary>
        /// 功能描述:查询所有数据
        /// 作　　者:Blog.Core
        /// </summary>
        /// <returns>数据列表</returns>
        public async Task<List<TEntity>> Query()
        {
            return await _db.Queryable<TEntity>().ToListAsync();
        }

        /// <summary>
        /// 功能描述:查询数据列表
        /// 作　　者:Blog.Core
        /// </summary>
        /// <param name="strWhere">条件</param>
        /// <returns>数据列表</returns>
        public async Task<List<TEntity>> Query(string strWhere)
        {
            //return await Task.Run(() => _db.Queryable<TEntity>().WhereIF(!string.IsNullOrEmpty(strWhere), strWhere).ToList());
            return await _db.Queryable<TEntity>().WhereIF(!string.IsNullOrEmpty(strWhere), strWhere).ToListAsync();
        }

        /// <summary>
        /// 功能描述:查询数据列表
        /// 作　　者:Blog.Core
        /// </summary>
        /// <param name="whereExpression">whereExpression</param>
        /// <returns>数据列表</returns>
        public async Task<List<TEntity>> Query(Expression<Func<TEntity, bool>> whereExpression)
        {
            return await _db.Queryable<TEntity>().WhereIF(whereExpression != null, whereExpression).ToListAsync();
        }

        /// <summary>
        /// 功能描述:查询一个列表
        /// 作　　者:Blog.Core
        /// </summary>
        /// <param name="whereExpression">条件表达式</param>
        /// <param name="strOrderByFileds">排序字段，如name asc,age desc</param>
        /// <returns>数据列表</returns>
        public async Task<List<TEntity>> Query(Expression<Func<TEntity, bool>> whereExpression, string strOrderByFileds)
        {
            //return await Task.Run(() => _db.Queryable<TEntity>().OrderByIF(!string.IsNullOrEmpty(strOrderByFileds), strOrderByFileds).WhereIF(whereExpression != null, whereExpression).ToList());
            return await _db.Queryable<TEntity>().WhereIF(whereExpression != null, whereExpression).OrderByIF(strOrderByFileds != null, strOrderByFileds).ToListAsync();
        }
        /// <summary>
        /// 功能描述:查询一个列表
        /// </summary>
        /// <param name="whereExpression"></param>
        /// <param name="orderByExpression"></param>
        /// <param name="isAsc"></param>
        /// <returns></returns>
        public async Task<List<TEntity>> Query(Expression<Func<TEntity, bool>> whereExpression, Expression<Func<TEntity, object>> orderByExpression, bool isAsc = true)
        {
            //return await Task.Run(() => _db.Queryable<TEntity>().OrderByIF(orderByExpression != null, orderByExpression, isAsc ? OrderByType.Asc : OrderByType.Desc).WhereIF(whereExpression != null, whereExpression).ToList());
            return await _db.Queryable<TEntity>().OrderByIF(orderByExpression != null, orderByExpression, isAsc ? OrderByType.Asc : OrderByType.Desc).WhereIF(whereExpression != null, whereExpression).ToListAsync();
        }

        /// <summary>
        /// 功能描述:查询一个列表
        /// 作　　者:Blog.Core
        /// </summary>
        /// <param name="strWhere">条件</param>
        /// <param name="strOrderByFileds">排序字段，如name asc,age desc</param>
        /// <returns>数据列表</returns>
        public async Task<List<TEntity>> Query(string strWhere, string strOrderByFileds)
        {
            //return await Task.Run(() => _db.Queryable<TEntity>().OrderByIF(!string.IsNullOrEmpty(strOrderByFileds), strOrderByFileds).WhereIF(!string.IsNullOrEmpty(strWhere), strWhere).ToList());
            return await _db.Queryable<TEntity>().OrderByIF(!string.IsNullOrEmpty(strOrderByFileds), strOrderByFileds).WhereIF(!string.IsNullOrEmpty(strWhere), strWhere).ToListAsync();
        }


        /// <summary>
        /// 功能描述:查询前N条数据
        /// 作　　者:Blog.Core
        /// </summary>
        /// <param name="whereExpression">条件表达式</param>
        /// <param name="intTop">前N条</param>
        /// <param name="strOrderByFileds">排序字段，如name asc,age desc</param>
        /// <returns>数据列表</returns>
        public async Task<List<TEntity>> Query(
            Expression<Func<TEntity, bool>> whereExpression,
            int intTop,
            string strOrderByFileds)
        {
            //return await Task.Run(() => _db.Queryable<TEntity>().OrderByIF(!string.IsNullOrEmpty(strOrderByFileds), strOrderByFileds).WhereIF(whereExpression != null, whereExpression).Take(intTop).ToList());
            return await _db.Queryable<TEntity>().OrderByIF(!string.IsNullOrEmpty(strOrderByFileds), strOrderByFileds).WhereIF(whereExpression != null, whereExpression).Take(intTop).ToListAsync();
        }

        /// <summary>
        /// 功能描述:查询前N条数据
        /// 作　　者:Blog.Core
        /// </summary>
        /// <param name="strWhere">条件</param>
        /// <param name="intTop">前N条</param>
        /// <param name="strOrderByFileds">排序字段，如name asc,age desc</param>
        /// <returns>数据列表</returns>
        public async Task<List<TEntity>> Query(
            string strWhere,
            int intTop,
            string strOrderByFileds)
        {
            //return await Task.Run(() => _db.Queryable<TEntity>().OrderByIF(!string.IsNullOrEmpty(strOrderByFileds), strOrderByFileds).WhereIF(!string.IsNullOrEmpty(strWhere), strWhere).Take(intTop).ToList());
            return await _db.Queryable<TEntity>().OrderByIF(!string.IsNullOrEmpty(strOrderByFileds), strOrderByFileds).WhereIF(!string.IsNullOrEmpty(strWhere), strWhere).Take(intTop).ToListAsync();
        }

        /// <summary>
        /// 根据sql语句查询
        /// </summary>
        /// <param name="strSql">完整的sql语句</param>
        /// <param name="parameters">参数</param>
        /// <returns>泛型集合</returns>
        public async Task<List<TEntity>> QuerySql(string strSql, SugarParameter[] parameters = null)
        {
            return await _db.Ado.SqlQueryAsync<TEntity>(strSql, parameters);
        }

        /// <summary>
        /// 根据sql语句查询
        /// </summary>
        /// <param name="strSql">完整的sql语句</param>
        /// <param name="parameters">参数</param>
        /// <returns>DataTable</returns>
        public async Task<DataTable> QueryTable(string strSql, SugarParameter[] parameters = null)
        {
            return await _db.Ado.GetDataTableAsync(strSql, parameters);
        }

        /// <summary>
        /// 功能描述:分页查询
        /// 作　　者:Blog.Core
        /// </summary>
        /// <param name="whereExpression">条件表达式</param>
        /// <param name="intPageIndex">页码（下标0）</param>
        /// <param name="intPageSize">页大小</param>
        /// <param name="intTotalCount">数据总量</param>
        /// <param name="strOrderByFileds">排序字段，如name asc,age desc</param>
        /// <returns>数据列表</returns>
        public async Task<List<TEntity>> Query(
            Expression<Func<TEntity, bool>> whereExpression,
            int intPageIndex,
            int intPageSize,
            string strOrderByFileds)
        {
            //return await Task.Run(() => _db.Queryable<TEntity>().OrderByIF(!string.IsNullOrEmpty(strOrderByFileds), strOrderByFileds).WhereIF(whereExpression != null, whereExpression).ToPageList(intPageIndex, intPageSize));
            return await _db.Queryable<TEntity>().OrderByIF(!string.IsNullOrEmpty(strOrderByFileds), strOrderByFileds).WhereIF(whereExpression != null, whereExpression).ToPageListAsync(intPageIndex, intPageSize);
        }

        /// <summary>
        /// 功能描述:分页查询
        /// 作　　者:Blog.Core
        /// </summary>
        /// <param name="strWhere">条件</param>
        /// <param name="intPageIndex">页码（下标0）</param>
        /// <param name="intPageSize">页大小</param>
        /// <param name="intTotalCount">数据总量</param>
        /// <param name="strOrderByFileds">排序字段，如name asc,age desc</param>
        /// <returns>数据列表</returns>
        public async Task<List<TEntity>> Query(
          string strWhere,
          int intPageIndex,
          int intPageSize,

          string strOrderByFileds)
        {
            //return await Task.Run(() => _db.Queryable<TEntity>().OrderByIF(!string.IsNullOrEmpty(strOrderByFileds), strOrderByFileds).WhereIF(!string.IsNullOrEmpty(strWhere), strWhere).ToPageList(intPageIndex, intPageSize));
            return await _db.Queryable<TEntity>().OrderByIF(!string.IsNullOrEmpty(strOrderByFileds), strOrderByFileds).WhereIF(!string.IsNullOrEmpty(strWhere), strWhere).ToPageListAsync(intPageIndex, intPageSize);
        }



        /// <summary>
        /// 分页查询[使用版本，其他分页未测试]
        /// </summary>
        /// <param name="whereExpression">条件表达式</param>
        /// <param name="intPageIndex">页码（下标0）</param>
        /// <param name="intPageSize">页大小</param>
        /// <param name="strOrderByFileds">排序字段，如name asc,age desc</param>
        /// <returns></returns>
        public async Task<PageModel<TEntity>> QueryPage(Expression<Func<TEntity, bool>> whereExpression, int intPageIndex = 1, int intPageSize = 20, string strOrderByFileds = null)
        {

            RefAsync<int> totalCount = 0;
            var list = await _db.Queryable<TEntity>()
             .OrderByIF(!string.IsNullOrEmpty(strOrderByFileds), strOrderByFileds)
             .WhereIF(whereExpression != null, whereExpression)
             .ToPageListAsync(intPageIndex, intPageSize, totalCount);

            int pageCount = (Math.Ceiling(totalCount.ObjToDecimal() / intPageSize.ObjToDecimal())).ObjToInt();
            return new PageModel<TEntity>() { dataCount = totalCount, pageCount = pageCount, page = intPageIndex, PageSize = intPageSize, data = list };
        }


        /// <summary> 
        ///查询-多表查询
        /// </summary> 
        /// <typeparam name="T">实体1</typeparam> 
        /// <typeparam name="T2">实体2</typeparam> 
        /// <typeparam name="T3">实体3</typeparam>
        /// <typeparam name="TResult">返回对象</typeparam>
        /// <param name="joinExpression">关联表达式 (join1,join2) => new object[] {JoinType.Left,join1.UserNo==join2.UserNo}</param> 
        /// <param name="selectExpression">返回表达式 (s1, s2) => new { Id =s1.UserNo, Id1 = s2.UserNo}</param>
        /// <param name="whereLambda">查询表达式 (w1, w2) =>w1.UserNo == "")</param> 
        /// <returns>值</returns>
        public async Task<List<TResult>> QueryMuch<T, T2, T3, TResult>(
            Expression<Func<T, T2, T3, object[]>> joinExpression,
            Expression<Func<T, T2, T3, TResult>> selectExpression,
            Expression<Func<T, T2, T3, bool>> whereLambda = null) where T : class, new()
        {
            if (whereLambda == null)
            {
                return await _db.Queryable(joinExpression).Select(selectExpression).ToListAsync();
            }
            return await _db.Queryable(joinExpression).Where(whereLambda).Select(selectExpression).ToListAsync();
        }

    }

}
