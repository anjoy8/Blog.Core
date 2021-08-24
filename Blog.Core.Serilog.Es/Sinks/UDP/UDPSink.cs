using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Serilog.Core;
using Serilog.Events;
using Serilog.Formatting;
 
 
namespace Serilog.Sinks.Network.Sinks.UDP
{
    public class UDPSink : ILogEventSink, IDisposable
    {
        private Socket _socket = new Socket(SocketType.Dgram, ProtocolType.Udp);
        private readonly ITextFormatter _formatter;

        public UDPSink(IPAddress ipAddress, int port, ITextFormatter formatter)
        {
            _socket.Connect(ipAddress, port);
            _formatter = formatter;
        }

        public void Emit(LogEvent logEvent)
        {
            var sb = new StringBuilder();

            using (var sw = new StringWriter(sb))
                _formatter.Format(logEvent, sw);

            sb.Replace("RenderedMessage", "message");

            _socket.Send(Encoding.UTF8.GetBytes(sb.ToString()));
        }

        public void Dispose()
        {
            _socket?.Dispose();
            _socket = null;
        }
    }
}
