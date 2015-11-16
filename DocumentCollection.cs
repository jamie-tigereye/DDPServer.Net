using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Net.DDP.Server;

namespace Net.DDP.Server
{
    public class DocumentCollection : IEnumerable<KeyValuePair<string, ReactiveDocument>>
    {
        public string Name { get; }

        readonly Dictionary<string, ReactiveDocument> _documents = new Dictionary<string, ReactiveDocument>();
        
        public event DocumentEvent Changed;
        public event DocumentEvent Added;
        public event DocumentEvent Removed;

        internal DocumentCollection(string name)
        {
            Name = name;
        }

        protected void OnChanged(ReactiveDocument document, PropertyChangedEventArgs args)
        {
            Changed?.Invoke(this, new DocumentEventArgs() {Document = document, EventType = EventType.Changed, PropertyEventArgs = args});
        }
        
        protected void OnAdded(ReactiveDocument document)
        {
            Added?.Invoke(this, new DocumentEventArgs() {Document = document, EventType = EventType.Added});
        }

        protected void OnRemoved(ReactiveDocument document)
        {
            Removed?.Invoke(this, new DocumentEventArgs() { Document = document, EventType = EventType.Removed });
        }

        public void Add(ReactiveDocument document)
        {
            if (_documents.ContainsKey(document.Id))
            {
                throw new DuplicateNameException(String.Format("A document with the same id already exists. ID {0}", document.Id));
            }

            document.PropertyChanged += (sender, args) => OnChanged(sender as ReactiveDocument, args);
            _documents.Add(document.Id, document);
            OnAdded(document);
        }

        public void Change(ReactiveDocument document)
        {
            if (!_documents.ContainsKey(document.Id))
            {
                throw new DirectoryNotFoundException(String.Format("A document with the same id cannot be found. ID {0}", document.Id));
            }


            _documents[document.Id] = document;
            OnChanged(document, null);
        }
        
        public void Remove(string id)
        {
            if (!_documents.ContainsKey(id))
            {
                throw new DirectoryNotFoundException(String.Format("A document with the same id cannot be found. ID {0}", id));

            }

            var document = _documents[id];
            _documents.Remove(id);
            OnRemoved(document);
        }
        
        public IEnumerator<KeyValuePair<string, ReactiveDocument>> GetEnumerator()
        {
            return _documents.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
