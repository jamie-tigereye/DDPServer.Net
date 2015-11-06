using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fleck;
using Net.DDP.Server;

namespace Net.DDP.Server
{
    public class Subscriptions :IEnumerable<KeyValuePair<string, Subscription>>
    {
        private readonly Dictionary<string, Subscription> _subscriptions =
            new Dictionary<string, Subscription>();

        public void Subscribe(string id, string name, IWebSocketConnection connection)
        {
            _subscriptions.Add(id, new Subscription(name, connection));
        }

        public void Unsubscribe(string id, string name, IWebSocketConnection connection)
        {
            _subscriptions.Remove(id);
        }

        public IEnumerable<KeyValuePair<string, Subscription>> GetSubscriptions(string name)
        {
            return _subscriptions.Where(x => x.Value.Name == name);
        }


        public IEnumerator<KeyValuePair<string, Subscription>> GetEnumerator()
        {
            return _subscriptions.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
