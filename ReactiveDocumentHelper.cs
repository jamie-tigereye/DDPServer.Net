using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Net.DDP.Server;
using Net.DDP.Server.Interfaces;

namespace Net.DDP.Server
{
    public static class ReactiveDocumentHelper
    {
        /// <summary>
        /// Attempts to convert any document inheriting from the IDocument interface 
        /// to a reactive document without passing an id as a parameter
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        public static ReactiveDocument ToReactiveDocument(this IDocument document)
        {
            return ReactiveDocument.TryConvertDocument(document);
        }

        /// <summary>
        /// Attempts to convert any object to a reactive document. An id for the document
        /// must be passed as a parameter
        /// </summary>
        /// <param name="document"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static ReactiveDocument ToReactiveDocument(this object document, string id)
        {
            return ReactiveDocument.TryConvertDocument(id, document);
        }
    }
}
