using Blog.Core.SqlSugarDbRepository.Interface;
using SqlSugar;

namespace Blog.Core.SqlSugarDbRepository
{
    public class SqlSugarProvider : ISqlSugarProvider
    {
        public string ProviderName { get; set; }
        public SqlSugarClient Sugar { get; set; }

        public SqlSugarProvider(ISqlSugarSetting SugarSetting)
        {
            this.Sugar = this.CreateSqlSugar(SugarSetting);
            this.ProviderName = SugarSetting.Name;
        }

        private SqlSugarClient CreateSqlSugar(ISqlSugarSetting SugarSetting)
        {

            var db = new SqlSugarClient(
             new ConnectionConfig()
             {
                 ConnectionString = SugarSetting.ConnectionString,
                 DbType = SugarSetting.DatabaseType,//设置数据库类型
                 IsAutoCloseConnection = true,//自动释放数据务，如果存在事务，在事务结束后释放
                 InitKeyType = InitKeyType.Attribute //从实体特性中读取主键自增列信息
             });

            //用来打印Sql方便你调式    
            db.Aop.OnLogExecuting = SugarSetting.LogExecuting;
            return db;
        }

        public void Dispose()
        {
            this.Sugar.Dispose();
        }
    }

}
