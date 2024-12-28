using Blog.Core.Common.Extensions;
using Blog.Core.Model;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Blog.Core.Model.Base.RootTkey;

namespace Blog.Core.Common.DB;

public class EntityUtility
{
    private static readonly Lazy<Dictionary<string, List<Type>>> _tenantEntitys = new(() =>
    {
        Dictionary<string, List<Type>> dic = new Dictionary<string, List<Type>>();
        var assembly = Assembly.Load("Blog.Core.Model");
        //扫描 实体
        foreach (var type in assembly.GetTypes().Where(s => s.IsClass && !s.IsAbstract))
        {
            var tenant = type.GetCustomAttribute<TenantAttribute>();
            if (tenant != null)
            {
                dic.TryAdd(tenant.configId.ToString(), type);
                continue;
            }

            if (type.IsSubclassOf(typeof(RootEntityTkey<>)))
            {
                dic.TryAdd(MainDb.CurrentDbConnId, type);
                continue;
            }

            var table = type.GetCustomAttribute<SugarTable>();
            if (table != null)
            {
                dic.TryAdd(MainDb.CurrentDbConnId, type);
                continue;
            }

            Debug.Assert(type.Namespace != null, "type.Namespace != null");
            if (type.Namespace.StartsWith("Blog.Core.Model.Models"))
            {
                dic.TryAdd(MainDb.CurrentDbConnId, type);
                continue;
            }
        }

        return dic;
    });

    public static Dictionary<string, List<Type>> TenantEntitys => _tenantEntitys.Value;
}