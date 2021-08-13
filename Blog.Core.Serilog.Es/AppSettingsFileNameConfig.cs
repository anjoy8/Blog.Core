using System;

namespace Blog.Core.Serilog.Es
{
    public class AppSettingsFileNameConfig
    {
        /// <summary>
        /// 配置文件名称常量
        /// </summary>
        public static string AppSettingsFileName = $"appsettings{ GetAppSettingsConfigName() }json";


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
    }
}
