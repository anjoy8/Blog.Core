using SqlSugar;

namespace Blog.Core.Migrate.Core;

public interface IMigrate<T>
{
    ISqlSugarClient Orm { get; set; }
    string TableName { get; set; }
    DbType DbType { get; set; }
    Version Version { get; set; }

    void Execution();
}