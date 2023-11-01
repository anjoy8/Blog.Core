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
        var constructorTypes = new List<Type>() {typeof(string)};
        for (int i = 0; i < indexAttribute.IndexFields.Count; i++)
        {
            constructorTypes.AddRange(new[] {typeof(string), typeof(OrderByType)});
        }

        constructorTypes.Add(typeof(bool));

        var values = new List<object>() {indexAttribute.IndexName};
        foreach (var indexField in indexAttribute.IndexFields)
        {
            values.AddRange(new object[] {indexField.Key, indexField.Value});
        }

        values.Add(indexAttribute.IsUnique);
        return new CustomAttributeBuilder(type.GetConstructor(constructorTypes.ToArray())!, values.ToArray());
    }

    public static DynamicProperyBuilder CreateIndex(this DynamicProperyBuilder builder, SugarIndexAttribute indexAttribute)
    {
        var classBuilder = builder.baseBuilder;
        var entityAttr = classBuilder.GetEntityAttr();
        entityAttr.Add(CreateIndex(indexAttribute));
        return builder;
    }
}