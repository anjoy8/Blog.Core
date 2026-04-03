using System.Collections;
using System.Reflection;
using System.Reflection.Emit;
using Microsoft.Extensions.Caching.Memory;

namespace Blog.Core.Common.Caches.Extensions;

public static class MemoryCacheExtensions
{
    #region Microsoft.Extensions.Caching.Memory_6_OR_OLDER

    /// <summary>
    /// 6.x <br/>
    /// 6.0.2 调整了字段名，使用 StringKeyEntriesCollection
    /// </summary>
    private static readonly Lazy<Func<MemoryCache, object>> GetEntries6 = new(() =>
        (Func<MemoryCache, object>)Delegate.CreateDelegate(typeof(Func<MemoryCache, object>),
            typeof(MemoryCache).GetProperty("EntriesCollection", BindingFlags.NonPublic | BindingFlags.Instance)?.GetGetMethod(true)
            ?? typeof(MemoryCache).GetProperty("StringKeyEntriesCollection", BindingFlags.NonPublic | BindingFlags.Instance)?.GetGetMethod(true)
            ?? throw new InvalidOperationException("Cannot find property 'EntriesCollection' or 'StringKeyEntriesCollection' on MemoryCache."),
            true));

    #endregion

    #region Microsoft.Extensions.Caching.Memory_7_OR_NEWER

    private static readonly Lazy<Func<MemoryCache, object>> GetCoherentState7 = new(() =>
        CreateGetter<MemoryCache, object>(typeof(MemoryCache)
           .GetField("_coherentState", BindingFlags.NonPublic | BindingFlags.Instance)));

    private static readonly Lazy<Func<object, IDictionary>> GetEntries7 = new(() =>
        CreateGetter<object, IDictionary>(typeof(MemoryCache)
           .GetNestedType("CoherentState", BindingFlags.NonPublic)?
           .GetField("_entries", BindingFlags.NonPublic | BindingFlags.Instance)));

    #endregion

    #region Microsoft.Extensions.Caching.Memory_8_OR_NEWER

    private static readonly Lazy<Func<MemoryCache, object>> GetCoherentState8 = new(() =>
        CreateGetter<MemoryCache, object>(typeof(MemoryCache)
           .GetField("_coherentState", BindingFlags.NonPublic | BindingFlags.Instance)));

    private static readonly Lazy<Func<object, IDictionary>> GetEntries8 = new(() =>
        CreateGetter<object, IDictionary>(typeof(MemoryCache)
           .GetNestedType("CoherentState", BindingFlags.NonPublic)?
           .GetField("_stringEntries", BindingFlags.NonPublic | BindingFlags.Instance)));

    #endregion

    private static Func<TParam, TReturn> CreateGetter<TParam, TReturn>(FieldInfo field)
    {
        if (field == null)
        {
            throw new ArgumentNullException(nameof(field), "Field cannot be null.");
        }

        var methodName = $"{field.ReflectedType!.FullName}.get_{field.Name}";
        var method = new DynamicMethod(methodName, typeof(TReturn), new[] { typeof(TParam) }, typeof(TParam), true);
        var ilGen = method.GetILGenerator();
        ilGen.Emit(OpCodes.Ldarg_0);
        ilGen.Emit(OpCodes.Ldfld, field);
        ilGen.Emit(OpCodes.Ret);
        return (Func<TParam, TReturn>)method.CreateDelegate(typeof(Func<TParam, TReturn>));
    }

    private static readonly Func<MemoryCache, IDictionary> GetEntries =
        Assembly.GetAssembly(typeof(MemoryCache))?.GetName().Version?.Major switch
        {
            < 7 => cache => (IDictionary)GetEntries6.Value(cache),
            7   => cache => GetEntries7.Value(GetCoherentState7.Value(cache)),
            _   => cache => GetEntries8.Value(GetCoherentState8.Value(cache)),
        };

    public static ICollection GetKeys(this IMemoryCache memoryCache) =>
        GetEntries((MemoryCache)memoryCache).Keys;

    public static IEnumerable<T> GetKeys<T>(this IMemoryCache memoryCache) =>
        memoryCache.GetKeys().OfType<T>();
}