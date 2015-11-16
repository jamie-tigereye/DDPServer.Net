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

        /// <summary>
        /// Uses JSON serialization to convert any object to a reactive document. 
        /// Probably not the most elegant solution.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="document"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Uses JSON serialization to convert an object inheriting from the IDocument 
        /// interface to a reactive document. Probably not the most elegant solution.
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
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
}
