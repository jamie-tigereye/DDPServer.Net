using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fleck;

namespace Net.DDP.Server
{
    public class Subscription
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public IWebSocketConnection Connection { get; set; }

        /// <summary>
        /// Subscription object stores name of the publication being subscribed to,
        /// And the websocket connection subscribed to that publication.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="connection"></param>
        public Subscription(string id, string name, IWebSocketConnection connection)
        {
            Id = id;
            Connection = connection;
            Name = name;
        }
    }
}
