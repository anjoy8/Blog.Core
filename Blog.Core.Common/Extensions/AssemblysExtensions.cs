using Microsoft.Extensions.DependencyModel;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;

namespace Blog.Core.Common.Extensions;

public static class AssemblysExtensions
{
    public static List<Assembly> GetAllAssemblies()
    {
        var list = new List<Assembly>();
        var deps = DependencyContext.Default;
        var libs = deps.CompileLibraries.Where(lib => !lib.Serviceable && lib.Type != "package" );
        foreach (var lib in libs)
        {
            var assembly = AssemblyLoadContext.Default.LoadFromAssemblyName(new AssemblyName(lib.Name));
            list.Add(assembly);
        }

        return list;
    }
}