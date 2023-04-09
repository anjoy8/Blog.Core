using Blog.Core.Model;
using Blog.Core.Model.Models.RootTkey;
using Blog.Core.Model.Tenants;
using SqlSugar;
using System;

namespace Blog.Core.Common.DB.Aop;

public static class SqlSugarAop
{
    public static void DataExecuting(object oldValue, DataFilterModel entityInfo)
    {
        if (entityInfo.EntityValue is RootEntityTkey<long> rootEntity)
        {
            if (rootEntity.Id == 0)
            {
                rootEntity.Id = SnowFlakeSingle.Instance.NextId();
            }
        }

        if (entityInfo.EntityValue is BaseEntity baseEntity)
        {
            // 新增操作
            if (entityInfo.OperationType == DataFilterType.InsertByObject)
            {
                if (baseEntity.CreateTime == DateTime.MinValue)
                {
                    baseEntity.CreateTime = DateTime.Now;
                }
            }

            if (entityInfo.OperationType == DataFilterType.UpdateByObject)
            {
                baseEntity.ModifyTime = DateTime.Now;
            }


            if (App.User?.ID > 0)
            {
                if (baseEntity is ITenantEntity tenant && App.User.TenantId > 0)
                {
                    if (tenant.TenantId == 0)
                    {
                        tenant.TenantId = App.User.TenantId;
                    }
                }

                switch (entityInfo.OperationType)
                {
                    case DataFilterType.UpdateByObject:
                        baseEntity.ModifyId = App.User.ID;
                        baseEntity.ModifyBy = App.User.Name;
                        break;
                    case DataFilterType.InsertByObject:
                        if (baseEntity.CreateBy.IsNullOrEmpty() || baseEntity.CreateId is null or <= 0)
                        {
                            baseEntity.CreateId = App.User.ID;
                            baseEntity.CreateBy = App.User.Name;
                        }

                        break;
                }
            }
        }
        else
        {
            //兼容以前的表 
            //这里要小心 在AOP里用反射 数据量多性能就会有问题
            //要么都统一使用基类
            //要么考虑老的表没必要兼容老的表
            //

            var getType = entityInfo.EntityValue.GetType();

            switch (entityInfo.OperationType)
            {
                case DataFilterType.InsertByObject:
                    var dyCreateBy = getType.GetProperty("CreateBy");
                    var dyCreateId = getType.GetProperty("CreateId");
                    var dyCreateTime = getType.GetProperty("CreateTime");

                    if (App.User?.ID > 0 && dyCreateBy != null && dyCreateBy.GetValue(entityInfo.EntityValue) == null)
                        dyCreateBy.SetValue(entityInfo.EntityValue, App.User.Name);

                    if (App.User?.ID > 0 && dyCreateId != null && dyCreateId.GetValue(entityInfo.EntityValue) == null)
                        dyCreateId.SetValue(entityInfo.EntityValue, App.User.ID);

                    if (dyCreateTime != null && (DateTime)dyCreateTime.GetValue(entityInfo.EntityValue) == DateTime.MinValue)
                        dyCreateTime.SetValue(entityInfo.EntityValue, DateTime.Now);

                    break;
                case DataFilterType.UpdateByObject:
                    var dyModifyBy = getType.GetProperty("ModifyBy");
                    var dyModifyId = getType.GetProperty("ModifyId");
                    var dyModifyTime = getType.GetProperty("ModifyTime");

                    if (App.User?.ID > 0 && dyModifyBy != null)
                        dyModifyBy.SetValue(entityInfo.EntityValue, App.User.Name);

                    if (App.User?.ID > 0 && dyModifyId != null)
                        dyModifyId.SetValue(entityInfo.EntityValue, App.User.ID);

                    if (dyModifyTime != null)
                        dyModifyTime.SetValue(entityInfo.EntityValue, DateTime.Now);
                    break;
            }
        }
    }

    private static string GetWholeSql(SugarParameter[] paramArr, string sql)
    {
        foreach (var param in paramArr)
        {
            sql = sql.Replace(param.ParameterName, $@"'{param.Value.ObjToString()}'");
        }

        return sql;
    }

    private static string GetParas(SugarParameter[] pars)
    {
        string key = "【SQL参数】：";
        foreach (var param in pars)
        {
            key += $"{param.ParameterName}:{param.Value}\n";
        }

        return key;
    }
}