using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Net.DDP.Server;

namespace Net.DDP.Server
{
    public delegate void PublicationEvent(object sender, PublicationEventArgs args);
    public delegate void DocumentEvent(object sender, DocumentEventArgs args);

    public enum EventType
    {
        Added,
        Changed,
        Removed
    }

    public class PublicationEventArgs : DocumentEventArgs
    {
        public string Name { get; set; }
        public DocumentCollection Collection { get; set; }
    }
    
    public class DocumentEventArgs
    {
        public EventType EventType { get; set; }
        public ReactiveDocument Document { get; set; }
        public PropertyChangedEventArgs PropertyEventArgs { get; set; }
    }
}
