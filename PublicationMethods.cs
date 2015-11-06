using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fleck;
using Newtonsoft.Json;
using Net.DDP.Server.Interfaces;

namespace Net.DDP.Server
{
    internal class PublicationMethods
    {
        private readonly IServer _server;

        public PublicationMethods(IServer server)
        {
            _server = server;
        }

        public void AttachMethods()
        {
            _server.Publications.Created += Publications_Created;
            _server.Publications.Changed += Publications_Changed;
            _server.Publications.Removed += Publications_Removed;
        }

        public void RemoveMethods()
        {
            _server.Publications.Created -= Publications_Created;
            _server.Publications.Changed -= Publications_Changed;
            _server.Publications.Removed -= Publications_Removed;
        }

        private void Publications_Removed(object sender, PublicationEventArgs args)
        {
            var subcriptions = _server.Subscriptions.GetSubscriptions(args.Name);

            foreach (var subscription in subcriptions)
            {
                var message = new Messages.Removed {Collection = args.Name, Id = subscription.Key};
                _server.SendResponse(subscription.Value.Connection, message);
            }
        }

        private void Publications_Changed(object sender, PublicationEventArgs args)
        {
            var subcriptions = _server.Subscriptions.GetSubscriptions(args.Name);
            var fields = JsonConvert.SerializeObject(args.Document);

            foreach (var subscription in subcriptions)
            {
                var message = new Messages.Changed() { Id = args.Document.Id, Fields = fields, Collection = args.Name };
                _server.SendResponse(subscription.Value.Connection, message);
            }
        }

        private void Publications_Created(object sender, PublicationEventArgs args)
        {
            var subcriptions = _server.Subscriptions.GetSubscriptions(args.Name);
            var fields = JsonConvert.SerializeObject(args.Document);

            foreach (var subscription in subcriptions)
            {
                var message = new Messages.Added() { Id = args.Document.Id, Fields = fields, Collection = args.Name };
                _server.SendResponse(subscription.Value.Connection, message);
            }
        }
     }
}
