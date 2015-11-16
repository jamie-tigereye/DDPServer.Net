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
    /*
        An IEnumerable collection of clients subscribed to publications.
    */
    public class Subscriptions : IEnumerable<Subscription>
    {
        private readonly Dictionary<string, Subscription> _subscriptions;


        internal Subscriptions()
        {
            _subscriptions = new Dictionary<string, Subscription>();
        }

        /// <summary>
        /// Subscribes a client websocket connection to a publication
        /// </summary>
        /// <param name="subscription"></param>
        public void Subscribe(Subscription subscription)
        {
            _subscriptions.Add(subscription.Id, subscription);
        }

        /// <summary>
        /// Removes a client websocket connection from a subscription to a publication
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="connection"></param>
        public void Unsubscribe(string id, string name, IWebSocketConnection connection)
        {
            _subscriptions.Remove(id);
        }

        /// <summary>
        /// Returns an IEnumerable collection of subscriptions to publication
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public IEnumerable<Subscription> GetSubscriptions(string name)
        {
            return _subscriptions.Where(subscription => subscription.Value.Name == name).Select(x => x.Value);
        }


        public IEnumerator<Subscription> GetEnumerator()
        {
            return _subscriptions.Select(subscription => subscription.Value).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
