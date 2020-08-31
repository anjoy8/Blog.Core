using Blog.Core.SqlSugarDbRepository.Interface;
using SqlSugar;
using System;

namespace Blog.Core.SqlSugarDbRepository
{
    public class SqlSugarSetting : ISqlSugarSetting
    {
        public string Name { get; set; }
        public string ConnectionString { get; set; }
        public DbType DatabaseType { get; set; }
        public Action<string, SugarParameter[]> LogExecuting { get; set; }
    }
}
