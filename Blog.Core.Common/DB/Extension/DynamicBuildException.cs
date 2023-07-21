using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using Blog.Core.Common.Extensions;
using SqlSugar;

namespace Blog.Core.Common.DB.Extension;

public static class DynamicBuildException
{
    private static List<CustomAttributeBuilder> GetEntityAttr(this DynamicBuilder builder)
    {
        FieldInfo fieldInfo = builder.GetType().GetField("entityAttr", BindingFlags.Instance | BindingFlags.NonPublic);
        List<CustomAttributeBuilder> entityAttr = (List<CustomAttributeBuilder>) fieldInfo.GetValue(builder);
        return entityAttr;
    }

    private static CustomAttributeBuilder CreateIndex(SugarIndexAttribute indexAttribute)
    {
        Type type = typeof(SugarIndexAttribute);
        return new CustomAttributeBuilder(type.GetConstructor(new[]
            {
                typeof(string), typeof(string), typeof(OrderByType), typeof(bool)
            })!,
            new object[]
            {
                indexAttribute.IndexName, indexAttribute.IndexFields.First().Key, indexAttribute.IndexFields.First().Value, indexAttribute.IsUnique
            },
            new PropertyInfo[]
            {
                type.GetProperty("IndexName"),
                type.GetProperty("IndexFields"),
                type.GetProperty("IsUnique"),
            },
            new object[]
            {
                indexAttribute.IndexName, indexAttribute.IndexFields, indexAttribute.IsUnique
            });
    }

    public static DynamicProperyBuilder CreateIndex(this DynamicProperyBuilder builder, SugarIndexAttribute indexAttribute)
    {
        var classBuilder = builder.baseBuilder;
        var entityAttr = classBuilder.GetEntityAttr();
        entityAttr.Add(CreateIndex(indexAttribute));
        return builder;
    }
}