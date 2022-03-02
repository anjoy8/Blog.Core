using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Blog.Core.Common
{

    public static class Appsettings
    {
        /// <summary>
        /// 配置
        /// </summary>
        public static IConfiguration Configuration { get; set; }

        public static void Init(string contentPath)
        {
            string Path = "appsettings.json";

            //如果你把配置文件 是 根据环境变量来分开了，可以这样写
            //Path = $"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json";

            Configuration = new ConfigurationBuilder()
               .SetBasePath(contentPath)
               .Add(new JsonConfigurationSource { Path = Path, Optional = false, ReloadOnChange = true })//这样的话，可以直接读目录里的json文件，而不是 bin 文件夹下的，所以不用修改复制属性
               .Build();
        }


        /// <summary>
        /// 添加配置文件
        /// </summary>
        /// <param name="hostBuilder"></param>
        /// <param name="builder"></param>
        public static void AddConfigureFiles(HostBuilderContext hostBuilder, IConfigurationBuilder builder)
        {
            //清除原配置，添加默认配置
            builder.Sources.Clear();
            GetDefaultConfigFiles().ForEach(file =>builder.AddJsonFile(file));

            IConfigurationRoot configuration = builder.Build();
            // 获取自定义配置文件夹 && 文件夹存在
            IEnumerable<string> customConfigFolder = configuration.Get<IEnumerable<string>>("CustomConfigInfo", "ConfigFileFolders")?.Where(folderPath => Directory.Exists(folderPath));
            if(customConfigFolder is null || !customConfigFolder.Any())
            {
                Configuration = builder.Build();
                return;
            }

            // 获取所有配置文件
            List<string> jsonFiles = new();
            customConfigFolder.ToList().ForEach(folder => jsonFiles.AddRange(Directory.GetFiles(folder, "*.json", SearchOption.TopDirectoryOnly)));

            // 将配置文件添加到配置中
            jsonFiles.ForEach(jsonFile => builder.AddJsonFile(jsonFile, true, true));

            Configuration = builder.Build();


            /// <summary>
            /// 获取默认配置文件
            /// </summary>
            /// <returns></returns>
            static List<string> GetDefaultConfigFiles()
            {
                List<string> configFiles = new() { "appsettings.json" };
                ;
                string ASPNETCORE_ENVIRONMENT = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
                if (ASPNETCORE_ENVIRONMENT?.Length > 0) configFiles.Add($"appsettings.{ASPNETCORE_ENVIRONMENT}.json");
                return configFiles;
            }
        }


        #region 获取配置信息

        /// <summary>
        /// 判断节点是否存在
        /// </summary>
        /// <param name="configuration">配置</param>
        /// <param name="sections">节点路径</param>
        /// <returns>节点存在返回 true，否者 false</returns>
        public static bool Exists(params string[] sections) => Configuration.Exists(sections);

        /// <summary>
        /// 读取节点字符串
        /// </summary>
        /// <param name="configuration">配置</param>
        /// <param name="sections">节点路径</param>
        /// <returns>和节点路径匹配的字符串</returns>
        public static string Get(params string[] sections) => Configuration.Get(sections);

        /// <summary>
        /// 读取节点并转换成指定类型
        /// </summary>
        /// <typeparam name="T">返回的类型</typeparam>
        /// <param name="configuration">配置</param>
        /// <param name="sections">节点路径</param>
        /// <returns>和节点路径匹配的T类型对象</returns>
        public static T Get<T>(params string[] sections) => Configuration.Get<T>(sections);

        #endregion 获取配置信息

        #region 以前获取配置信息
        /// <summary>
        /// 封装要操作的字符
        /// </summary>
        /// <param name="sections">节点配置</param>
        /// <returns></returns>
        [Obsolete("推荐使用 Git 方法")]
        public static string app(params string[] sections)
        {
            try
            {

                if (sections.Any())
                {
                    return Configuration[string.Join(":", sections)];
                }
            }
            catch (Exception) { }

            return "";
        }

        /// <summary>
        /// 递归获取配置信息数组
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sections"></param>
        /// <returns></returns>
        [Obsolete("推荐使用 Git<T> 方法")]
        public static List<T> app<T>(params string[] sections)
        {
            List<T> list = new List<T>();
            // 引用 Microsoft.Extensions.Configuration.Binder 包
            Configuration.Bind(string.Join(":", sections), list);
            return list;
        }


        /// <summary>
        /// 根据路径  configuration["App:Name"];
        /// </summary>
        /// <param name="sectionsPath"></param>
        /// <returns></returns>
        [Obsolete("推荐使用 Git 方法")]
        public static string GetValue(string sectionsPath)
        {
            try
            {
                return Configuration[sectionsPath];
            }
            catch (Exception) { }

            return "";

        }
        #endregion
    }
}

namespace Microsoft.Extensions.Configuration
{
    /// <summary>
    /// <see cref="IConfiguration"/>(配置) 拓展
    /// </summary>
    public static class IConfigurationExtensions
    {
        /// <summary>
        /// 判断节点是否存在
        /// </summary>
        /// <param name="configuration">配置</param>
        /// <param name="sections">节点路径</param>
        /// <returns>节点存在返回 true，否者 false</returns>
        public static bool Exists(this IConfiguration configuration, params string[] sections) => configuration.GetSection(string.Join(':', sections)).Exists();

        /// <summary>
        /// 读取节点字符串
        /// </summary>
        /// <param name="configuration">配置</param>
        /// <param name="sections">节点路径</param>
        /// <returns>和节点路径匹配的字符串</returns>
        public static string Get(this IConfiguration configuration, params string[] sections) => configuration[string.Join(':', sections)];

        /// <summary>
        /// 读取节点并转换成指定类型
        /// </summary>
        /// <typeparam name="T">返回的类型</typeparam>
        /// <param name="configuration">配置</param>
        /// <param name="sections">节点路径</param>
        /// <returns>和节点路径匹配的T类型对象</returns>
        public static T Get<T>(this IConfiguration configuration, params string[] sections) => configuration.GetSection(string.Join(':', sections)).Get<T>();
    }
}
