using System.Collections.Generic;

namespace Blog.Core.Common.Extensions;

public static class DictionaryExtensions
{
    public static void TryAdd<TKey, TValue>(this IDictionary<TKey, List<TValue>> dic, TKey key, TValue value)
    {
        if (dic.TryGetValue(key, out var old))
        {
            old.Add(value);
        }
        else
        {
            dic.Add(key, new List<TValue> {value});
        }
    }
}