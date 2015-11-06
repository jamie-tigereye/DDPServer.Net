using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fleck;

namespace Net.DDP.Server.Interfaces
{
    public interface IServer
    {
        int Version { get; set; }
        Methods Methods { get; set; }
        Publications Publications { get; set; }
        Subscriptions Subscriptions { get; set; }
        void ProcessRequest(IWebSocketConnection connection, string item);
        void SendResponse(IWebSocketConnection connection, object item);
        void Start();
        void Disconnect();
    }
}
