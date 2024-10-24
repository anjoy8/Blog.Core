using Microsoft.Extensions.DependencyModel;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;

namespace Blog.Core.Common.Extensions;

public static class RuntimeExtension
{
    private static readonly List<string> ProjectAssemblies =
    [
        "Blog.Core"
    ];

    /// <summary>
    /// 获取项目程序集，排除所有的系统程序集(Microsoft.***、System.***等)、Nuget下载包
    /// </summary>
    /// <returns></returns>
    public static IList<Assembly> GetAllAssemblies()
    {
        var assemblies = new List<Assembly>();
        var dllFiles = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.dll");

        foreach (var dllFile in dllFiles)
        {
            var fileName = Path.GetFileNameWithoutExtension(dllFile);
            if (!ProjectAssemblies.Any(s => fileName.StartsWith(s))) continue;
            try
            {
                var assembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(dllFile);
                assemblies.Add(assembly);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Failed to load assembly: {fileName}. Error: {e.Message}");
            }
        }

        return assemblies;
    }

    public static Assembly GetAssembly(string assemblyName)
    {
        return GetAllAssemblies().FirstOrDefault(assembly => assembly.FullName.Contains(assemblyName));
    }

    public static IList<Type> GetAllTypes()
    {
        var list = new List<Type>();
        foreach (var assembly in GetAllAssemblies())
        {
            var typeInfos = assembly.DefinedTypes;
            foreach (var typeInfo in typeInfos)
            {
                list.Add(typeInfo.AsType());
            }
        }

        return list;
    }

    public static IList<Type> GetTypesByAssembly(string assemblyName)
    {
        var list = new List<Type>();
        var assembly = AssemblyLoadContext.Default.LoadFromAssemblyName(new AssemblyName(assemblyName));
        var typeInfos = assembly.DefinedTypes;
        foreach (var typeInfo in typeInfos)
        {
            list.Add(typeInfo.AsType());
        }

        return list;
    }

    public static Type GetImplementType(string typeName, Type baseInterfaceType)
    {
        return GetAllTypes().FirstOrDefault(t =>
        {
            if (t.Name == typeName &&
                t.GetTypeInfo().GetInterfaces().Any(b => b.Name == baseInterfaceType.Name))
            {
                var typeInfo = t.GetTypeInfo();
                return typeInfo.IsClass && !typeInfo.IsAbstract && !typeInfo.IsGenericType;
            }

            return false;
        });
    }
}