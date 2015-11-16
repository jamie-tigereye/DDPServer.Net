using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fleck;
using Newtonsoft.Json.Linq;
using Net.DDP.Server.Interfaces;

namespace Net.DDP.Server
{
    internal class MessageMethods
    {
        private readonly IServer _server;

        public MessageMethods(IServer server)
        {
            _server = server;
        }
        
        public Dictionary<string, Action<KeyValuePair<IWebSocketConnection, dynamic>>> GetMethods()
        {
            var methods = new Dictionary<string, Action<KeyValuePair<IWebSocketConnection, dynamic>>>
            {
                {"connect", Connect},
                {"method", Method},
                {"sub", Sub},
                {"unsub", Unsub},
                {"ping", Ping}
            };
            
            return methods;
        }
        
        private void Ping(KeyValuePair<IWebSocketConnection, dynamic> item)
        {
            Messages.Ping message = Messages.Ping.Get(item.Value);
            message.Msg = "pong";
            _server.SendResponse(item.Key, message);
        }
        
        private void Connect(KeyValuePair<IWebSocketConnection, dynamic> item)
        {
            if (item.Value.version <= _server.Version)
            {
                var connected = new Messages.Connected {Session = item.Key.ConnectionInfo.Id.ToString()};
                _server.SendResponse(item.Key, connected);
            }
            else
            {
                var failed = new Messages.Failed() {Version = _server.Version.ToString()};
                _server.SendResponse(item.Key, failed);
            }
        }

        private void Method(KeyValuePair<IWebSocketConnection, dynamic> item)
        {
            var message = Messages.Methd.Get(item.Value);

            try
            {
                var result = new Messages.Reslt(message.Id);
                var returnValue = _server.Methods.Call(message.Method, message.Parameters);
                result.Result = returnValue;
                _server.SendResponse(item.Key, result);
            }
            catch (Exception ex)
            {
                var result = new Messages.Reslt(message.Id) {Error = ex};
                _server.SendResponse(item.Key, result);
            }
        }

        private void Sub(KeyValuePair<IWebSocketConnection, dynamic> item)
        {
            var message = Messages.Sub.Get(item.Value);

            try
            {
                var subscription = new Subscription(message.Id, message.Name, item.Key);
                _server.Subscriptions.Subscribe(subscription);
            }
            catch (Exception ex)
            {
                var result = new Messages.NoSub(message.Id, ex);
                _server.SendResponse(item.Key, result);
            }
        }

        private void Unsub(KeyValuePair<IWebSocketConnection, dynamic> item)
        {
            var message = Messages.UnSub.Get(item.Value);

            try
            {
                _server.Subscriptions.Unsubscribe(item.Value.id, item.Value.name, item.Key);
            }
            catch (Exception ex)
            {
                var result = new Messages.NoSub(message.Id, ex);
                _server.SendResponse(item.Key, result);
            }
        }
    }
}
