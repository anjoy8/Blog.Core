using System.Threading.Tasks;
using Blog.Core.Common.LogHelper;
using Microsoft.AspNetCore.SignalR;

namespace Blog.Core.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        //定于一个通讯管道，用来管理我们和客户端的连接
        //1、客户端调用 GetLatestCount，就像订阅
        public async Task GetLatestCount(string random)
        {
            //2、服务端主动向客户端发送数据，名字千万不能错
            await Clients.All.SendAsync("ReceiveUpdate", LogLock.GetLogData());
            
            //3、客户端再通过 ReceiveUpdate ，来接收

        }
    }
}
