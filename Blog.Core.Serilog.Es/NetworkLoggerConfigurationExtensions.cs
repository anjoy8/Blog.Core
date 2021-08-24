using System;
using System.Linq;
using System.Net;
using Serilog;
using Serilog.Configuration;
using Serilog.Debugging;
using Serilog.Events;
using Serilog.Formatting;
using Blog.Core.Serilog.Es.Formatters;
using Blog.Core.Serilog.Es.Sinks.TCP;

namespace Blog.Core.Serilog.Es
{
    /// <summary>
    ///     Extends Serilog configuration to write events to the network.
    /// </summary>
    public static class NetworkLoggerConfigurationExtensions
    {
        private static string TcpAddressHost = "";
        private static int TcpAddressProt = 0;
        /// <summary>
        /// 获得tcpAddress
        /// </summary>
        private static void GetTcpAddress() 
        {
            // 读取相关配置
            var logConfigRootDTOInfo = JsonConfigUtils.GetAppSettings<LogConfigRootDTO>(AppSettingsFileNameConfig.AppSettingsFileName, "LogFiedOutPutConfigs");
            if (logConfigRootDTOInfo == null)
            {
                return;
            }
            TcpAddressHost = logConfigRootDTOInfo.tcpAddressHost;
            TcpAddressProt = logConfigRootDTOInfo.tcpAddressPort;
        }

        public static LoggerConfiguration TCPSink(
            this LoggerSinkConfiguration loggerConfiguration,
            ITextFormatter textFormatter = null,
            LogEventLevel restrictedToMinimumLevel = LevelAlias.Minimum)
        {
            GetTcpAddress();
            if (!string.IsNullOrEmpty(TcpAddressHost))
            {
                var sink = new TCPSink(BuildUri($"tcp://{TcpAddressHost}:{TcpAddressProt}"), textFormatter ?? new LogstashJsonFormatter());
                return loggerConfiguration.Sink(sink, restrictedToMinimumLevel);
            }
            else {
                return new LoggerConfiguration();
            }
        }

        private static IPAddress ResolveAddress(string uri)
        {
            // Check if it is IP address
            IPAddress address;

            if (IPAddress.TryParse(uri, out address))
                return address;

            address = ResolveIP(uri);
            if (address != null)
                return address;

            SelfLog.WriteLine("Unable to determine the destination IP-Address");
            return IPAddress.Loopback;
        }

        private static IPAddress ResolveIP(string uri)
        {
            try
            {
                var ipHostEntry = Dns.GetHostEntryAsync(uri).Result;
                if (!ipHostEntry.AddressList.Any())
                    return null;
                return ipHostEntry.AddressList.First();
            }
            catch (Exception)
            {
                SelfLog.WriteLine("Could not resolve " + uri);
                return null;
            }
        }

        private static Uri BuildUri(string s)
        {
            Uri uri;
            try
            {
                uri = new Uri(s);
            }
            catch (UriFormatException ex)
            {
                throw new ArgumentNullException("Uri should be in the format tcp://server:port", ex);
            }
            if (uri.Port == 0)
                throw new UriFormatException("Uri port cannot be 0");
            if (!(uri.Scheme.ToLower() == "tcp" || uri.Scheme.ToLower() == "tls"))
                throw new UriFormatException("Uri scheme must be tcp or tls");
            return uri;
        }
    }
}