using System.Collections.Generic;

namespace Blog.Core.Common.Extensions;

public static class UntilExtensions
{
    public static void AddOrModify<TKey, TValue>(this IDictionary<TKey, TValue> dic, TKey key, TValue value)
    {
        if (dic.TryGetValue(key, out _))
        {
            dic[key] = value;
        }
        else
        {
            dic.Add(key, value);
        }
    }
}