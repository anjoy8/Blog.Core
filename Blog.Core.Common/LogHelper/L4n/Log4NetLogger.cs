using log4net;
using log4net.Repository;
using Microsoft.Extensions.Logging;
using System;
using System.Reflection;
using System.Xml;

namespace Blog.Core.Common.LogHelper
{
    public class Log4NetLogger : ILogger
    {
        private readonly string _name;
        private readonly XmlElement _xmlElement;
        private readonly ILog _log;
        private ILoggerRepository _loggerRepository;
        public Log4NetLogger(string name, XmlElement xmlElement)
        {
            _name = name;
            _xmlElement = xmlElement;
            _loggerRepository = log4net.LogManager.CreateRepository(
                Assembly.GetEntryAssembly(), typeof(log4net.Repository.Hierarchy.Hierarchy));
            _log = LogManager.GetLogger(_loggerRepository.Name, name);
            log4net.Config.XmlConfigurator.Configure(_loggerRepository, xmlElement);
        }
        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            switch (logLevel)
            {
                case LogLevel.Critical:
                    return _log.IsFatalEnabled;
                case LogLevel.Debug:
                case LogLevel.Trace:
                    return _log.IsDebugEnabled;
                case LogLevel.Error:
                    return _log.IsErrorEnabled;
                case LogLevel.Information:
                    return _log.IsInfoEnabled;
                case LogLevel.Warning:
                    return _log.IsWarnEnabled;
                default:
                    throw new ArgumentOutOfRangeException(nameof(logLevel));
            }
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state,
            Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel))
            {
                return;
            }

            if (formatter == null)
            {
                throw new ArgumentNullException(nameof(formatter));
            }
            string message = null;
            if (null != formatter)
            {
                message = formatter(state, exception);
            }
            if (!string.IsNullOrEmpty(message) || exception != null)
            {
                switch (logLevel)
                {
                    case LogLevel.Critical:
                        _log.Fatal(message);
                        break;
                    case LogLevel.Debug:
                    case LogLevel.Trace:
                        _log.Debug(message);
                        break;
                    case LogLevel.Error:
                        _log.Error(message);
                        break;
                    case LogLevel.Information:
                        _log.Info(message);
                        break;
                    case LogLevel.Warning:
                        _log.Warn(message);
                        break;
                    default:
                        _log.Warn($"Encountered unknown log level {logLevel}, writing out as Info.");
                        _log.Info(message, exception);
                        break;
                }
            }
        }
    }
}
