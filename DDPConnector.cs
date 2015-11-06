using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using Fleck;
using Net.DDP.Server.Interfaces;

namespace Net.DDP.Server
{
    class DDPConnector
    {
        private WebSocketServer _socket;
        public List<IWebSocketConnection> Connections;
         
        private readonly IServer _server;

        private readonly string _ip;
        private readonly int _port;

        public DDPConnector(IServer server, string ip, int port)
        {
            Connections = new List<IWebSocketConnection>();
            _server = server;
            _ip = ip;
            _port = port;
        }

        public void Start()
        {
            _socket = new WebSocketServer(String.Format("ws://{0}:{1}", _ip, _port));
            _socket.Start(socket =>
            {
                socket.OnOpen = () => _socket_onOpen(socket);
                socket.OnClose = () => _socket_onClose(socket);
                socket.OnMessage = (message) => _socket_onMessage(socket, message);
                socket.OnPing = (bytes) => _socket_onPing(socket, bytes);
            });
        }

        public void Disconnect()
        {
            foreach (var connection in Connections)
            {
                connection.Close();
            }
        }

        public void SendPing()
        {
            foreach (var connection in Connections)
            {
                // TODO
            }
        }

        private void _socket_onOpen(IWebSocketConnection socket)
        {
            Connections.Add(socket);
        }

        private void _socket_onPing(IWebSocketConnection socket, byte[] bytes)
        {
            // TODO
        }

        private void _socket_onClose(IWebSocketConnection socket)
        {
            Connections.Remove(socket);
        }

        public void SendMessage(IWebSocketConnection socket, string message)
        {
            socket.Send(message);
        }

        private void _socket_onMessage(IWebSocketConnection socket, string message)
        {
            _server.ProcessRequest(socket, message);
        }
    }
}
