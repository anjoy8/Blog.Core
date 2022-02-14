using Nacos;
using Nacos.V2.Naming.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ocelot.Provider.Nacos.NacosClient
{
    public class WeightRandomLBStrategy : ILBStrategy
    {
        public LBStrategyName Name => LBStrategyName.WeightRandom;

        public Instance GetHost(List<Instance> list)
        {
            var dict = BuildScore(list);

            Instance instance = null;

            var rd = new Random().NextDouble();

            foreach (var item in dict)
            {
                if (item.Value >= rd)
                {
                    instance = list.FirstOrDefault(x => x.InstanceId.Equals(item.Key));

                    if (instance == null)
                    {
                        var arr = item.Key.Split("#");
                        var ip = arr[0];
                        int.TryParse(arr[1], out var port);
                        var cluster = arr[2];

                        instance = list.First(x => x.Ip.Equals(ip) && x.Port == port && x.ClusterName.Equals(cluster));
                    }

                    break;
                }
            }

            return instance;
        }

        private Dictionary<string, double> BuildScore(List<Instance> list)
        {
            var dict = new Dictionary<string, double>();

            // aliyun sae, the instanceid returns empty string
            // when the instanceid is empty, create a new one, but the group was missed.
            list.ForEach(x => { x.InstanceId = string.IsNullOrWhiteSpace(x.InstanceId) ? $"{x.Ip}#{x.Port}#{x.ClusterName}#{x.ServiceName}" : x.InstanceId; });

            var tmp = list.Select(x => new LbKv
            {
                InstanceId = x.InstanceId,
                Weight = x.Weight
            }).GroupBy(x => x.InstanceId).Select(x => new LbKv
            {
                InstanceId = x.Key,
                Weight = x.Max(y => y.Weight)
            }).ToList();

            var total = tmp.Sum(x => x.Weight);
            var cur = 0d;

            foreach (var item in tmp)
            {
                cur += item.Weight;
                dict.TryAdd(item.InstanceId, cur / total);
            }

            return dict;
        }
    }
}
