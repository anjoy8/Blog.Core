using SqlSugar;
using System;
using System.Collections.Concurrent;

namespace Blog.Core.SqlSugarDbRepository.Interface
{
    public interface ISqlSugarProviderStorage<T> where T : ISqlSugarProvider
    {
        ConcurrentDictionary<string, T> DataMap { get; }

        T GetByName(string name, string defaultName);


        void AddOrUpdate(string name, T val);


        void Remove(string name);


        void Clear();
    }

    public interface ISqlSugarProvider : IDisposable
    {
        /// <summary>
        /// 针对这个连接起别名
        /// </summary>
        string ProviderName { get; }

        /// <summary>
        /// SqlSugar实例
        /// </summary>
        SqlSugarClient Sugar { get; }
    }
}
