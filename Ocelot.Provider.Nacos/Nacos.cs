using Ocelot.ServiceDiscovery.Providers;
using Ocelot.Values;
using Nacos.V2;
using Microsoft.Extensions.Options;
using Ocelot.Provider.Nacos.NacosClient.V2;
using NacosConstants = Nacos.V2.Common.Constants;

namespace Ocelot.Provider.Nacos
{
    public class Nacos : IServiceDiscoveryProvider
    {
        private readonly INacosNamingService _client;
        private readonly string _serviceName;
        private readonly string _groupName;
        private readonly List<string> _clusters;

        public Nacos(string serviceName, INacosNamingService client, IOptions<NacosAspNetOptions> options)
        {
            _serviceName = serviceName;
            _client = client;
            _groupName = string.IsNullOrWhiteSpace(options.Value.GroupName) ? 
                NacosConstants.DEFAULT_GROUP : options.Value.GroupName;
            _clusters = (string.IsNullOrWhiteSpace(options.Value.ClusterName) ? NacosConstants.DEFAULT_CLUSTER_NAME : options.Value.ClusterName).Split(",").ToList();
        }

        public async Task<List<Service>> Get()
        {
            var services = new List<Service>();

            var instances = await _client.GetAllInstances(_serviceName, _groupName, _clusters);

            if (instances != null && instances.Any())
            {
                foreach (var Sitem in instances)
                {
                    string sip = Sitem.Ip;
                    int sport = Sitem.Port;
                    if (Sitem.Metadata.ContainsKey("endpoint"))
                    {
                        string[] ipport = Sitem.Metadata["endpoint"].Split(':');
                        sip = ipport[0];
                        sport =int.Parse( ipport[1]);
                    }
                    services.Add(new Service(Sitem.InstanceId, new ServiceHostAndPort(sip, sport), "", "", new List<string>()));
                }
               // services.AddRange(instances.Select(i => new Service(i.InstanceId, new ServiceHostAndPort(i.Ip, i.Port), "", "", new List<string>())));
            }

            return await Task.FromResult(services);
        }

        public Task<List<Service>> GetAsync()
        {
            throw new NotImplementedException();
        }
    }
}
