using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Fleck;
using Newtonsoft.Json;
using System.Threading;
using Net.DDP.Server.Interfaces;

namespace Net.DDP.Server
{
    public class DDPServer : IServer
    {
        private readonly DDPConnector _connector;
        public int Version { get; set; }
        public Methods Methods { get; set; }
        public Publications Publications { get; set; }
        public Subscriptions Subscriptions { get; set; }
        
        private readonly ResultQueue<KeyValuePair<IWebSocketConnection, string>> _messageQueue;

        private Thread _activeThread;
        public DDPServer(int port)
        {
            Version = 1;
            Methods = new Methods(this);
            Subscriptions = new Subscriptions();
            Publications = new Publications();
            new PublicationMethods(this).AttachMethods();
            var msgMethods = new MessageMethods(this).GetMethods();
            var messageProcessor = new MessageProcessor(msgMethods);
            _messageQueue = new ResultQueue<KeyValuePair<IWebSocketConnection, string>>(messageProcessor);
            _connector = new DDPConnector(this, "127.0.0.1", port);
        }

        public void Ping()
        {
            _connector.SendPing();
        }
    
        public void Start()
        {
            if (_activeThread == null || !_activeThread.IsAlive)
            {
                _activeThread = new Thread((() => _connector.Start()));
                _activeThread.Start();
            }
        }

        public void Disconnect()
        {
            _activeThread.Abort();
            _activeThread = null;
            _connector.Disconnect();
        }

        public void ProcessRequest(IWebSocketConnection connection, string message)
        {
            _messageQueue.AddItem(new KeyValuePair<IWebSocketConnection, string>(connection, message));
        }
        
        public void SendResponse(IWebSocketConnection connection, object item)
        {
            var message = JsonConvert.SerializeObject(item);
            _connector.SendMessage(connection, message);
        }
    }
}
