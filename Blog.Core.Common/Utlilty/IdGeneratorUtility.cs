using Blog.Core.Common.DB;
using Blog.Core.Common.Option;
using Microsoft.Extensions.Hosting;
using Serilog;
using SnowflakeId.AutoRegister.Builder;
using SnowflakeId.AutoRegister.Interfaces;
using SqlSugar;
using Yitter.IdGenerator;

namespace Blog.Core.Common.Utlilty;

public class IdGeneratorUtility
{
    private static readonly Lazy<IAutoRegister> AutoRegister = new(() =>
    {
        var builder = new AutoRegisterBuilder()
                // Register Option
                // Use the following line to set the identifier.
                // Recommended setting to distinguish multiple applications on a single machine
               .SetExtraIdentifier(App.Configuration["urls"] ?? string.Empty)
                // Use the following line to set the WorkerId scope.
               .SetWorkerIdScope(1, 30)
            // Use the following line to set the register option.
            // .SetRegisterOption(option => {})
            ;
        var redisOptions = App.GetOptions<RedisOptions>();
        if (redisOptions.Enable)
            // Use the following line to use the Redis store.
            builder.UseRedisStore(redisOptions.ConnectionString);
        else if (BaseDBConfig.LogConfig != null && BaseDBConfig.LogConfig.DbType == DbType.SqlServer)
            // Use the following line to use the SQL Server store.
            builder.UseSqlServerStore(BaseDBConfig.LogConfig.ConnectionString);
        else
            // Use the following line to use the default store.
            // Only suitable for standalone use, local testing, etc.
            builder.UseDefaultStore();

        App.GetService<IHostApplicationLifetime>(false).ApplicationStopping.Register(UnRegister);
        return builder.Build();
    });

    private static readonly Lazy<IIdGenerator> _idGenInstance = new(() =>
    {
        var config = AutoRegister.Value.Register();

        //WorkerId DataCenterId 取值 1-31
        var options = new IdGeneratorOptions
        {
            WorkerId = (ushort)config.WorkerId,
        };
        IIdGenerator idGenInstance = new DefaultIdGenerator(options);
        return idGenInstance;
    });

    private static IIdGenerator IdGenInstance => _idGenInstance.Value;

    public static long NextId()
    {
        return IdGenInstance.NewLong();
    }

    public static void UnRegister()
    {
        if (!AutoRegister.IsValueCreated) return;

        AutoRegister.Value.UnRegister();
        Log.Information("Snowflake Id Unregistered");
    }
}