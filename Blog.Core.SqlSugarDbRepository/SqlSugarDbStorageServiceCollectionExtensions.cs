using Blog.Core.SqlSugarDbRepository.Interface;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Blog.Core.SqlSugarDbRepository
{
    public static class SqlSugarDbStorageServiceCollectionExtensions
    {
        public static IServiceCollection AddSqlSugarDbStorage(this IServiceCollection services
            //, ISqlSugarSetting defaultDbSetting
            )
        {
            //if (defaultDbSetting == null)
            //{
            //    throw new ArgumentNullException(nameof(defaultDbSetting));
            //}

            //services.AddSingleton<ISqlSugarProvider>(new SqlSugarProvider(defaultDbSetting));
            services.AddTransient(typeof(ISqlSugarRepository<>), typeof(SqlSugarRepository<>));
            services.AddSingleton<ISqlSugarProviderStorage<ISqlSugarProvider>, DefaultSqlSugarProviderStorage>();

            return services;

        }

        public static IServiceProvider AddSqlSugarDatabaseProvider(this IServiceProvider serviceProvider, ISqlSugarSetting dbSetting)
        {
            if (dbSetting == null)
            {
                throw new ArgumentNullException(nameof(dbSetting));
            }

            var fSqlProviderStorage = serviceProvider.GetRequiredService<ISqlSugarProviderStorage<ISqlSugarProvider>>();

            fSqlProviderStorage.AddOrUpdate(dbSetting.Name, new SqlSugarProvider(dbSetting));

            return serviceProvider;
        }

    }

}
