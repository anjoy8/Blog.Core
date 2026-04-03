using System.Reflection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Blog.Core.Common.Swagger.Filter;

public class EnumTypesDocumentFilter : IDocumentFilter
{
    private static Type UnwrapEnumType(Type type)
    {
        if (type == null)
            return null;

        // Nullable<Enum>
        if (Nullable.GetUnderlyingType(type)?.IsEnum == true)
            return Nullable.GetUnderlyingType(type);

        // 直接是 Enum
        if (type.IsEnum)
            return type;

        // List<Enum> / IEnumerable<Enum>
        if (type.IsGenericType &&
            (type.GetGenericTypeDefinition() == typeof(IEnumerable<>) ||
                type.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEnumerable<>))))
        {
            var itemType = type.GetGenericArguments().FirstOrDefault();
            return UnwrapEnumType(itemType);
        }

        // Enum[]
        if (type.IsArray)
            return UnwrapEnumType(type.GetElementType());

        return null;
    }

    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        foreach (var path in swaggerDoc.Paths.Values)
        {
            foreach (var operation in path.Operations.Values)
            {
                foreach (var parameter in operation.Parameters)
                {
                    OpenApiSchema schema = null;
                    if (parameter.Schema != null && !string.IsNullOrWhiteSpace(parameter.Description) && parameter.Description.Contains("<ul>"))
                    {
                        schema = parameter.Schema;
                    }
                    else if (parameter.Schema is { Reference: not null })
                    {
                        // 引用类型Enum
                        schema = context.SchemaRepository.Schemas[parameter.Schema.Reference.Id];
                    }
                    else if (parameter.Schema.Type == "array" && parameter.Schema.Items.Reference != null)
                    {
                        // 数组类型Enum
                        schema = context.SchemaRepository.Schemas[parameter.Schema.Items.Reference.Id];
                    }
                    else if (parameter.Schema is { Items: { Enum: { Count: > 0 } } })
                    {
                        schema = parameter.Schema.Items;
                    }
                    else if (parameter.Schema.Enum is { Count: > 0 })
                    {
                        // 基础类型Enum (integer)
                        var apiParam = context.ApiDescriptions
                           .SelectMany(desc => desc.ParameterDescriptions)
                           .FirstOrDefault(desc => desc.Name == parameter.Name);

                        Type paramType = apiParam?.ParameterDescriptor?.ParameterType;
                        Type enumType  = null;

                        if (paramType != null)
                        {
                            // 如果是 DTO 类型，从其属性中反查当前参数名
                            if (!paramType.IsEnum && !paramType.IsValueType && !paramType.IsPrimitive && paramType != typeof(string))
                            {
                                var dtoProp = paramType.GetProperty(apiParam.Name,
                                    BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                                if (dtoProp != null)
                                    paramType = dtoProp.PropertyType;
                            }

                            enumType = UnwrapEnumType(paramType);

                            if (enumType?.IsEnum == true)
                            {
                                schema = context.SchemaGenerator.GenerateSchema(enumType, context.SchemaRepository);
                            }
                        }
                    }

                    if (schema == null || schema.Description == null) continue;

                    var cutStart = schema.Description.IndexOf("<ul>");
                    var cutEnd   = schema.Description.IndexOf("</ul>") + 5;

                    if (cutStart < 0 || cutEnd <= cutStart) continue;

                    parameter.Description += "<p>说明:</p>" + schema.Description.Substring(cutStart, cutEnd - cutStart);
                }
            }
        }
    }
}