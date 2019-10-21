using Blog.Core.Common.LogHelper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Core.Extensions
{
    public static class Log4netExtensions
    {
        public static ILoggerFactory AddLog4Net(this ILoggerFactory factory, string log4NetConfigFile)
        {
            factory.AddProvider(new Log4NetProvider(log4NetConfigFile));
            return factory;
        }

        public static ILoggerFactory AddLog4Net(this ILoggerFactory factory)
        {
            factory.AddProvider(new Log4NetProvider("Log4net.config"));
            return factory;
        }
    }
}
