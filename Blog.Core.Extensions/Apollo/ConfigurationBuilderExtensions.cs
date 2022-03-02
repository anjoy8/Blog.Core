using Com.Ctrip.Framework.Apollo;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using Com.Ctrip.Framework.Apollo.Enums;
using Com.Ctrip.Framework.Apollo.Logging;
using Microsoft.Extensions.Primitives;
using System.Reflection;

namespace Blog.Core.Extensions.Apollo
{
   public static class ConfigurationBuilderExtensions
    {
        /// <summary>
        /// 接入Apollo
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="jsonPath">apollo配置文件路径 如果写入appsettings.json中 则jsonPath传null即可</param>
        public static void AddConfigurationApollo(this IConfigurationBuilder builder,string jsonPath= null)
        {
            if (!string.IsNullOrEmpty(jsonPath))
            {
                builder.AddJsonFile(jsonPath, true, false);
            }
            //阿波罗的日志级别调整
            LogManager.UseConsoleLogging(LogLevel.Warn);
            var options = new ApolloOptions();
            var root = builder.Build();
            root.Bind("Apollo", options);
            if (options.Enable)
            {
                var apolloBuilder = builder.AddApollo(root.GetSection("Apollo:Config"));

                foreach (var item in options.Namespaces)
                {
                    apolloBuilder.AddNamespace(item.Name, MatchConfigFileFormat(item.Format));
                }
                //监听apollo配置
                Monitor(builder.Build());
            }

        }
        #region private
        /// <summary>
        /// 监听配置
        /// </summary>
        private static void Monitor(IConfigurationRoot root)
        {
            //TODO 需要根据改变执行特定的操作 如 mq redis  等其他跟配置相关的中间件
            //TODO 初步思路：将需要执行特定的操作key和value放入内存字典中，在赋值操作时通过标准事件来执行特定的操作。

            //要重新Build 此时才将Apollo provider加入到ConfigurationBuilder中
            ChangeToken.OnChange(() => root.GetReloadToken(), () =>
            {
                foreach (var apolloProvider in root.Providers.Where(p => p is ApolloConfigurationProvider))
                {
                    var property = apolloProvider.GetType().BaseType.GetProperty("Data", BindingFlags.Instance | BindingFlags.NonPublic);
                    var data = property.GetValue(apolloProvider) as IDictionary<string, string>;
                    foreach (var item in data)
                    {
                        Console.WriteLine($"key {item.Key}   value {item.Value}");
                    }
                }
            });
        }

        //匹配格式
        private static ConfigFileFormat MatchConfigFileFormat(string value) => value switch
        {
            "json" => ConfigFileFormat.Json,
            "properties" => ConfigFileFormat.Properties,
            "xml" => ConfigFileFormat.Xml,
            "yml" => ConfigFileFormat.Yml,
            "yaml" => ConfigFileFormat.Yaml,
            "txt" => ConfigFileFormat.Txt,
            _ => throw new FormatException($"与apollo命名空间的所允许的类型不匹配：{string.Join(",", GetConfigFileFormat())}"),
        };
        //获取数据格式对应的枚举
        private static IEnumerable<string> GetConfigFileFormat() => Enum.GetValues<ConfigFileFormat>().Select(u => u.ToString().ToLower());
        #endregion

    }
}
