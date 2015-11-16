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
            _server.Publications.DocumentAdded += Publication_DocumentAdded;
            _server.Publications.DocumentChanged += Publication_DocumentChanged;
            _server.Publications.DocumentRemoved += Publication_DocumentRemoved;
        }

        public void RemoveMethods()
        {
            _server.Publications.DocumentAdded -= Publication_DocumentAdded;
            _server.Publications.DocumentChanged -= Publication_DocumentChanged;
            _server.Publications.DocumentRemoved -= Publication_DocumentRemoved;
        }

        /// <summary>
        /// Every time a document is removed from a publication a message is sent to all subscribers
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void Publication_DocumentRemoved(object sender, PublicationEventArgs args)
        {
            var subcriptions = _server.Subscriptions.GetSubscriptions(args.Name);

            foreach (var subscription in subcriptions)
            {
                var message = new Messages.Removed {Collection = args.Name, Id = subscription.Id};
                _server.SendResponse(subscription.Connection, message);
            }
        }


        /// <summary>
        /// Every time a document is changed within a publication a message is sent to all subscribers
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void Publication_DocumentChanged(object sender, PublicationEventArgs args)
        {
            var subcriptions = _server.Subscriptions.GetSubscriptions(args.Name);
            var fields = JsonConvert.SerializeObject(args.Document);

            foreach (var subscription in subcriptions)
            {
                var message = new Messages.Changed() { Id = args.Document.Id, Fields = fields, Collection = args.Name };
                _server.SendResponse(subscription.Connection, message);
            }
        }

        /// <summary>
        /// Every time a document is added to a publication a message is sent to all subscribers
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void Publication_DocumentAdded(object sender, PublicationEventArgs args)
        {
            var subcriptions = _server.Subscriptions.GetSubscriptions(args.Name);
            var fields = JsonConvert.SerializeObject(args.Document);

            foreach (var subscription in subcriptions)
            {
                var message = new Messages.Added() { Id = args.Document.Id, Fields = fields, Collection = args.Name };
                _server.SendResponse(subscription.Connection, message);
            }
        }
     }
}
