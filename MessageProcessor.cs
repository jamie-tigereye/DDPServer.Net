using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Fleck;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Net.DDP.Server.Interfaces;

namespace Net.DDP.Server
{
    internal class MessageProcessor : IProcessQueues<KeyValuePair<IWebSocketConnection, string>>
    {
        private readonly Dictionary<string, Action<KeyValuePair<IWebSocketConnection, dynamic>>> _processingMethods;

        public MessageProcessor(Dictionary<string, Action<KeyValuePair<IWebSocketConnection, dynamic>>> processingMethods)
        {
            _processingMethods = processingMethods;
        }
        
        public void ProcessItem(KeyValuePair<IWebSocketConnection, string> item)
        {
            dynamic message = JsonConvert.DeserializeObject(item.Value);

            if (message.msg != null)
            {
                foreach (var method in _processingMethods)
                {
                    if (message.msg == method.Key)
                    {
                        method.Value(new KeyValuePair<IWebSocketConnection, dynamic>(item.Key, message));
                    }
                }

            }
        }
    }
}
