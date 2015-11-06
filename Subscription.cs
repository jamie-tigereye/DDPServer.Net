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
        public IWebSocketConnection Connection { get; set; }
        public string Name { get; set; }

        public Subscription(string name, IWebSocketConnection connection)
        {
            Connection = connection;
            Name = name;
        }
    }
}
