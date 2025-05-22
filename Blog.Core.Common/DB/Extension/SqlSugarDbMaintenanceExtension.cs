using SqlSugar;

namespace Blog.Core.Common.DB.Extension;

/// <summary>
/// SqlSugar DbMaintenance 扩展
/// </summary>
public static class SqlSugarDbMaintenanceExtension
{
    /// <summary>
    /// 清除 SqlSugar 中表信息的缓存。
    /// <para>使用场景：</para>
    /// <list type="bullet">
    ///   <item>
    ///     <description>
    ///       调用 <see cref="IDbMaintenance.IsAnyTable"/> 时会缓存表的元数据信息，
    ///       SqlSugar 未提供显式清理该缓存的方法。
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       如果通过 <c>DbMaintenance.IsAnyTable(tableName, false)</c> 绕过缓存，
    ///       则每次调用都会重新查询所有表信息，影响性能。
    ///     </description>
    ///   </item>
    /// </list>
    /// 建议在InitTable后进行清理缓存.
    /// </summary>
    /// <param name="context">SqlSugar的数据库上下文实例。</param>
    public static void ClearDbTableCache(this ISqlSugarClient context)
    {
        var fullCacheKey = GetDbTableCacheKey(context, "DbMaintenanceProvider.GetTableInfoList" + context.CurrentConnectionConfig.ConfigId);
        context.CurrentConnectionConfig.ConfigureExternalServices.ReflectionInoCacheService.Remove<List<DbTableInfo>>(fullCacheKey);
    }

    private static string GetDbTableCacheKey(ISqlSugarClient context, string cacheKey) =>
        $"{context.CurrentConnectionConfig.DbType}.{context.Ado.Connection.Database}.{cacheKey}";
}