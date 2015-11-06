using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Net.DDP.Server;

namespace Net.DDP.Server
{
    public class Publications : IEnumerable<KeyValuePair<string, DocumentCollection>>
    {
        readonly Dictionary<string, DocumentCollection> _publications = new Dictionary<string, DocumentCollection>();

        public event PublicationEvent Created;
        public event PublicationEvent Changed;
        public event PublicationEvent Removed;

        public IEnumerator<KeyValuePair<string, DocumentCollection>> GetEnumerator()
        {
            return _publications.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public DocumentCollection Publish(string name)
        {
            if (_publications.ContainsKey(name))
            {
                throw new DuplicateNameException(String.Format("A publication named {0} already exists.", name));
            }

            var publication = new DocumentCollection();
            publication.Added += Publication_DocumentAdded;
            publication.Changed += Publication_DocumentChanged;
            publication.Removed += Publication_DocumentRemoved;
            _publications.Add(name, publication);
            return publication;
        }

        private void Publication_DocumentRemoved(object sender, DocumentEventArgs args)
        {
            if (Removed != null)
            {
                var name = _publications.FirstOrDefault(x => x.Value == sender).Key;

                Removed(this, new PublicationEventArgs() {EventType = EventType.Removed, Name = name, Collection= (DocumentCollection)sender, Document = args.Document});
            }
        }

        private void Publication_DocumentChanged(object sender, DocumentEventArgs args)
        {
            if (Changed != null)
            {
                var name = _publications.FirstOrDefault(x => x.Value == sender).Key;

                Changed(this, new PublicationEventArgs() { EventType = EventType.Changed, Name = name, Collection = (DocumentCollection)sender, Document = args.Document, PropertyEventArgs = args.PropertyEventArgs});
            }
        }

        private void Publication_DocumentAdded(object sender, DocumentEventArgs args)
        {
            if (Created != null)
            {
                var key = _publications.FirstOrDefault(x => x.Value == sender).Key;

                Created(this, new PublicationEventArgs() { EventType = EventType.Added, Name = key, Collection = (DocumentCollection)sender, Document = args.Document });
            }
        }
    }
}
