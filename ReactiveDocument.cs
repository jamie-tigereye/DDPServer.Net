using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImpromptuInterface;
using Newtonsoft.Json;
using Net.DDP.Server.Interfaces;

namespace Net.DDP.Server
{
    public class ReactiveDocument : NotifiableExpandoObject, IDocument
    {
        public string Id { get; set; }

        public static ReactiveDocument TryConvertDocument(string id, object document)
        {
            if (id == null)
            {
                throw new NullReferenceException("The document id cannot be null");
            }

            var serializedObject = JsonConvert.SerializeObject(document);
            var dynamicDocument = JsonConvert.DeserializeObject<ReactiveDocument>(serializedObject);
            dynamicDocument.Id = id;
            return dynamicDocument;
        }

        public static ReactiveDocument TryConvertDocument(IDocument document)
        {
            if (document.Id == null)
            {
                throw new NullReferenceException("The document id cannot be null");
            }   

            var serializedObject = JsonConvert.SerializeObject(document);
            var dynamicDocument = JsonConvert.DeserializeObject<ReactiveDocument>(serializedObject);
            return dynamicDocument;
        }
    }

    public static class ReactiveDocumentHelper
    {
        public static ReactiveDocument ToReactiveDocument(this IDocument document)
        {
            return ReactiveDocument.TryConvertDocument(document);
        }
        public static ReactiveDocument ToReactiveDocument(this object document, string name)
        {
            return ReactiveDocument.TryConvertDocument(name, document);
        }
    }
}
