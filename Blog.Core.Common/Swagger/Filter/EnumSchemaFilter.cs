using System.ComponentModel;
using System.Reflection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Blog.Core.Common.Swagger.Filter;

/// <summary>
/// Enum 转换
/// </summary>
public class EnumSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (schema.Enum == null || schema.Enum.Count == 0 ||
            context.Type == null || !context.Type.IsEnum)
            return;

        schema.Description += "<p><strong>枚举说明：</strong></p><ul>";

        var enumMembers = context.Type.GetFields(BindingFlags.Public | BindingFlags.Static);

        foreach (var enumMember in enumMembers)
        {
            var enumValue = Convert.ToInt64(Enum.Parse(context.Type, enumMember.Name));

            var descriptionAttribute = enumMember.GetCustomAttribute<DescriptionAttribute>();
            var description          = descriptionAttribute != null ? descriptionAttribute.Description : enumMember.Name;

            schema.Description += $"<li><strong>{enumValue}:{enumMember.Name}</strong> - {description}</li>";
        }

        schema.Description += "</ul>";
    }
}