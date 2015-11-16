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
    public class Publications : IEnumerable<DocumentCollection>
    {
        readonly Dictionary<string, DocumentCollection> _publications;

        public event PublicationEvent DocumentAdded;
        public event PublicationEvent DocumentChanged;
        public event PublicationEvent DocumentRemoved;

        internal Publications()
        {
            _publications = new Dictionary<string, DocumentCollection>();
        }
        
        /// <summary>
        /// Returns a new published document collection
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public DocumentCollection Publish(string name)
        {
            if (_publications.ContainsKey(name))
            {
                throw new DuplicateNameException(String.Format("A publication named {0} already exists.", name));
            }

            var publication = new DocumentCollection(name);
            publication.Added += Publication_DocumentAdded;
            publication.Changed += Publication_DocumentChanged;
            publication.Removed += Publication_DocumentRemoved;
            _publications.Add(name, publication);
            return publication;
        }

        /// <summary>
        /// Raises an event when a document is removed from the publication
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void Publication_DocumentRemoved(object sender, DocumentEventArgs args)
        {
            if (DocumentRemoved != null)
            {
                var name = _publications.FirstOrDefault(x => x.Value == sender).Key;

                DocumentRemoved(this, new PublicationEventArgs() {EventType = EventType.Removed, Name = name, Collection= (DocumentCollection)sender, Document = args.Document});
            }
        }

        /// <summary>
        /// Raises an event when a document is changed within a publication
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void Publication_DocumentChanged(object sender, DocumentEventArgs args)
        {
            if (DocumentChanged != null)
            {
                var name = _publications.FirstOrDefault(x => x.Value == sender).Key;
                DocumentChanged(this, new PublicationEventArgs() { EventType = EventType.Changed, Name = name, Collection = (DocumentCollection)sender, Document = args.Document, PropertyEventArgs = args.PropertyEventArgs});
            }
        }

        /// <summary>
        /// Raises an event when a document is added to a publication
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void Publication_DocumentAdded(object sender, DocumentEventArgs args)
        {
            if (DocumentAdded != null)
            {
                var key = _publications.FirstOrDefault(x => x.Value == sender).Key;

                DocumentAdded(this, new PublicationEventArgs() { EventType = EventType.Added, Name = key, Collection = (DocumentCollection)sender, Document = args.Document });
            }
        }
        
        public IEnumerator<DocumentCollection> GetEnumerator()
        {
            return _publications.Select(x => x.Value).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
