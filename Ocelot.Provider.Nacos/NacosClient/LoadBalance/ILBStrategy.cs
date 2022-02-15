using Nacos;
using Nacos.V2.Naming.Dtos;
using System.Collections.Generic;

namespace Ocelot.Provider.Nacos.NacosClient
{
    public interface ILBStrategy
    {
        /// <summary>
        /// Strategy Name
        /// </summary>
        LBStrategyName Name { get; }

        /// <summary>
        /// Get host
        /// </summary>
        /// <param name="list">host list</param>
        /// <returns>The Host</returns>
        Instance GetHost(List<Instance> list);
    }


  
    
  
}
