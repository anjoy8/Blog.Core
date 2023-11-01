using System.Linq;
using SqlSugar;

namespace Blog.Core.Common.DB.Aop;

public class SqlSugarReuse
{
    public static void AutoChangeAvailableConnect(SqlSugarClient db)
    {
        if (db == null) return;
        if (db.Ado.IsValidConnection()) return;
        if (!BaseDBConfig.ReuseConfigs.Any()) return;

        foreach (var connectionConfig in BaseDBConfig.ReuseConfigs)
        {
            var config = db.CurrentConnectionConfig.ConfigId;
            db.ChangeDatabase(connectionConfig.ConfigId);
            //移除旧的连接,只会在本次上下文移除,因为主库已经故障会导致多库事务无法使用
            db.RemoveConnection(config);
            if (db.Ado.IsValidConnection()) return;
        }
    }
}