using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;

namespace Blog.Core.Common.Helper
{
    /// <summary>
    /// Json 配置文件通用类
    /// </summary>
    public static class JsonConfigUtils
    {
        #region 变量

        /// <summary>
        /// 锁
        /// </summary>
        private static object __Lock__ = new object();

        #endregion

        /// <summary>
        /// 读取配置文件的信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fileName">要读取json的名称</param>
        /// <param name="key">要读取的json节点名称</param>
        /// <returns></returns>
        public static T GetAppSettings<T>(IConfiguration config, string AppSettingsFileName, string key) where T : class, new()
        {
            lock (__Lock__)
            {
                if (config == null)
                {
                    config = new ConfigurationBuilder()
                        .Add(new JsonConfigurationSource
                        {
                            Path = AppSettingsFileName,
                            Optional = false,
                            ReloadOnChange = true
                        })
                        .Build();
                }
                var appconfig = new ServiceCollection()
                    .AddOptions()
                    .Configure<T>(config.GetSection(key))
                    .BuildServiceProvider()
                    .GetService<IOptions<T>>()
                    .Value;

                return appconfig;
            }
        }


        public static string GetJson(string jsonPath, string key)
        {
            IConfiguration config = new ConfigurationBuilder().AddJsonFile(jsonPath).Build(); //json文件地址
            string s = config.GetSection(key).Value; //json某个对象
            return s;
        }
    }


    /// <summary>
    /// 配置文件管理器
    /// </summary>
    public interface IConfigurationManager
    {
        T GetAppConfig<T>(string key, T defaultValue = default(T));
    }

    /// <summary>
    /// 配置读取  根据环境变量
    /// </summary>
    public class ConfigurationManager : IConfigurationManager
    {
        private readonly IConfigurationRoot config;

        public ConfigurationManager(IConfigurationRoot _config)
        {
            config = _config;
        }

        public T GetAppConfig<T>(string key, T defaultValue = default(T))
        {
            T value = default(T);
            try
            {
                var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
                var builder = new ConfigurationBuilder().AddJsonFile("appsettings.json", true, false)
                      .AddJsonFile($"appsettings.{env}.json", true, false)
                       .AddEnvironmentVariables();

                var configuration = builder.Build();
                value = (T)Convert.ChangeType(configuration[key], typeof(T));
                if (value == null)
                    value = defaultValue;
            }
            catch (Exception)
            {
                value = defaultValue;
            }

            return value;
        }
    }


    #region Nacos 配置清单
    public class JsonConfigSettings
    {
        // 从nacos 读取到的系统配置信息
        public static IConfiguration Configuration { get; set; }


        /// <summary>
        /// 配置文件名称常量
        /// </summary>
        private static string AppSettingsFileName = $"appsettings{ GetAppSettingsConfigName() }json";

        /// <summary>
        /// 根据环境变量定向配置文件名称
        /// </summary>
        /// <returns></returns>
        private static string GetAppSettingsConfigName()
        {
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") != null
                      && Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") != "")
            {
                return $".{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.";
            }
            else
            {
                return ".";
            }
        }
        /// <summary>
        /// 获取Nacos配置
        /// </summary>
        public static List<string> NacosServerAddresses
        {
            get
            {
                return JsonConfigUtils.GetAppSettings<NacosConfigDTO>(Configuration, AppSettingsFileName, "nacos").ServerAddresses;
            }
        }

        /// <summary>
        /// 获取Nacos配置
        /// </summary>
        public static int NacosDefaultTimeOut
        {
            get
            {
                return JsonConfigUtils.GetAppSettings<NacosConfigDTO>(Configuration, AppSettingsFileName, "nacos").DefaultTimeOut;
            }
        }

        /// <summary>
        /// 获取Nacos配置
        /// </summary>
        public static string NacosNamespace
        {
            get
            {
                return JsonConfigUtils.GetAppSettings<NacosConfigDTO>(Configuration, AppSettingsFileName, "nacos").Namespace;
            }
        }
        /// <summary>
        /// 获取Nacos配置
        /// </summary>
        public static string NacosServiceName
        {
            get
            {
                return JsonConfigUtils.GetAppSettings<NacosConfigDTO>(Configuration, AppSettingsFileName, "nacos").ServiceName;
            }
        }

        /// <summary>
        /// 获取Nacos配置
        /// </summary>
        public static int ListenInterval
        {
            get
            {
                return JsonConfigUtils.GetAppSettings<NacosConfigDTO>(Configuration, AppSettingsFileName, "nacos").ListenInterval;
            }
        }

        /// <summary>
        /// 获取Nacos配置
        /// </summary>
        public static string NacosIp
        {
            get
            {
                return JsonConfigUtils.GetAppSettings<NacosConfigDTO>(Configuration, AppSettingsFileName, "nacos").Ip;

            }
        }
        /// <summary>
        /// 获取Nacos配置
        /// </summary>
        public static int NacosPort
        {
            get
            {
                return int.Parse(JsonConfigUtils.GetAppSettings<NacosConfigDTO>(Configuration, AppSettingsFileName, "nacos").Port);
            }
        }
        /// <summary>
        /// 获取Nacos配置
        /// </summary>
        public static bool NacosRegisterEnabled
        {
            get
            {
                return JsonConfigUtils.GetAppSettings<NacosConfigDTO>(Configuration, AppSettingsFileName, "nacos").RegisterEnabled;
            }
        }

        /// <summary>
        /// 获取Nacos配置
        /// </summary>
        public static Dictionary<string, string> NacosMetadata
        {
            get
            {
                return JsonConfigUtils.GetAppSettings<NacosConfigDTO>(Configuration, AppSettingsFileName, "nacos").Metadata;
            }
        }

        #endregion

        #region Nacos配置

        /// <summary>
        /// Nacos配置实体
        /// </summary>
        public class NacosConfigDTO
        {
            /// <summary>
            /// 服务IP地址
            /// </summary>
            public List<string> ServerAddresses { get; set; }
            /// <summary>
            /// 默认超时时间
            /// </summary>
            public int DefaultTimeOut { get; set; }
            /// <summary>
            /// 监听间隔
            /// </summary>
            public int ListenInterval { get; set; }
            /// <summary>
            /// 服务命名空间
            /// </summary>
            public string Namespace { get; set; }
            /// <summary>
            /// 服务名称
            /// </summary>
            public string ServiceName { get; set; }
            /// <summary>
            /// IP地址
            /// </summary>
            public string Ip { get; set; }
            /// <summary>
            /// 端口
            /// </summary>
            public string Port { get; set; }
            /// <summary>
            /// 服务命名空间
            /// </summary>
            public bool RegisterEnabled { get; set; }
            /// <summary>
            /// 其他配置
            /// </summary>
            public Dictionary<string, string> Metadata { get; set; }
        }

        #endregion

    }

}
