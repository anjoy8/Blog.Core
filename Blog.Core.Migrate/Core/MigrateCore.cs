using System.Reflection;
using Blog.Core.Migrate.Attributes;
using Serilog;
using SqlSugar;

namespace Blog.Core.Migrate.Core;

public static class MigrateCore
{
    private static async Task<Dictionary<string, string>> GetTableVersionsAsync(ISqlSugarClient db)
    {
        if (!db.DbMaintenance.IsAnyTable(nameof(TableVersion))) db.CodeFirst.InitTables(typeof(TableVersion));
        return (await db.Queryable<TableVersion>().ToListAsync()).ToDictionary(s => s.TableName, s => s.Version);
    }

    public static MigrateVersionAttribute GetMigrateVersion<T>()
    {
        return GetMigrateVersion(typeof(T));
    }

    public static MigrateVersionAttribute GetMigrateVersion(Type type)
    {
        return type.GetCustomAttribute<MigrateVersionAttribute>();
    }

    public static bool IsSplitTable(Type type)
    {
        return type.GetCustomAttribute<SplitTableAttribute>() != null;
    }

    public static bool IsMigrate(Type type)
    {
        var ma = type.GetCustomAttribute<MigrateAttribute>();
        return ma == null || ma.Enable;
    }

    public static void Migrate(ISqlSugarClient context, Type type)
    {
        if (!IsMigrate(type)) return;

        if (IsSplitTable(type))
            context.CodeFirst.SplitTables().InitTables(type);
        else
            context.CodeFirst.InitTables(type);
    }

    public static async Task MigrateAsync(ISqlSugarClient context, IEnumerable<Type> types)
    {
        var oldVersions = await GetTableVersionsAsync(context);

        foreach (var type in types)
        {
            if (!IsMigrate(type)) continue;

            var version = GetMigrateVersion(type);
            if (version == null)
            {
                //没有标记版本号的表 只会初始化 不会迁移
                //表不存在初始化
                if (!context.DbMaintenance.IsAnyTable(type.Name))
                {
                    Log.Logger.Information("Init Table:{TableName}", type.Name);
                    Migrate(context, type);
                }
                continue;
            }

            var tableName = context.MappingTables.FirstOrDefault(s => s.EntityName == type.Name)?.DbTableName;
            if (string.IsNullOrWhiteSpace(tableName))
            {
                tableName = context.EntityMaintenance.GetEntityInfo(type).DbTableName;
            }

            var ver = oldVersions.GetValueOrDefault(tableName);
            if (ver != null)
            {
                var oldVersion = new Version(ver);
                if (oldVersion >= version.Version) continue;
            }

            Log.Logger.Information("Modify Table:{TableName} oldVersion:{Ver} newVersion:{NewVersion}",
                tableName, ver, version.Version);
            Migrate(context, type);

            if (ver == null)
                await context.Insertable(new TableVersion
                {
                    TableName = tableName,
                    Version = version.Version.ToString()
                }).ExecuteCommandAsync();
            else
                await context.Updateable(new TableVersion
                {
                    TableName = tableName,
                    Version = version.Version.ToString()
                }).ExecuteCommandAsync();
        }
    }
}