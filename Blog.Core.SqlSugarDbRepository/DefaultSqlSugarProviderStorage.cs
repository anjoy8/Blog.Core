using Blog.Core.SqlSugarDbRepository.Interface;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Concurrent;
using System.Linq;

namespace Blog.Core.SqlSugarDbRepository
{
    public class DefaultSqlSugarProviderStorage : ISqlSugarProviderStorage<ISqlSugarProvider>
    {
        public ConcurrentDictionary<string, ISqlSugarProvider> DataMap { get; private set; }

        public DefaultSqlSugarProviderStorage(IServiceProvider serviceProvider)
        {
            DataMap = new ConcurrentDictionary<string, ISqlSugarProvider>();

            var tmpDataMap = serviceProvider.GetServices<ISqlSugarProvider>()
                .ToDictionary(item => item.ProviderName);

            foreach (var item in tmpDataMap)
            {
                this.AddOrUpdate(item.Key, item.Value);
            }
        }
        public void AddOrUpdate(string name, ISqlSugarProvider val)
        {
            DataMap[name] = val;
        }

        public void Clear()
        {
            DataMap.Clear();
        }

        public ISqlSugarProvider GetByName(string name, string defaultName)
        {
            ISqlSugarProvider result = null;

            if (name == null)
            {
                if (!DataMap.TryGetValue(defaultName, out result))
                {
                    throw new Exception("没有找到 DefaultName Provider");
                }
                return result;
            }
            else if (DataMap.TryGetValue(name, out result))
            {
                return result;
            }

            throw new ArgumentException($"没有找到  {name}  Provider");
        }

        public void Remove(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return;
            }

            this.DataMap.TryRemove(name, out ISqlSugarProvider result);
        }
    }

}
