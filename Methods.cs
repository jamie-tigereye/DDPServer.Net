using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using Fleck;
using ImpromptuInterface;
using Newtonsoft.Json;
using Net.DDP.Server.Interfaces;

namespace Net.DDP.Server
{
    public delegate string MethodDelegate(params string[] args);
    public class Methods : Dictionary<string, MethodDelegate>
    {
        private IServer _server;

        public Methods(IServer server)
        {
            _server = server;
        }
        
        public new void Add(string name, MethodDelegate deligate)
        {
            if (ContainsKey(name))
            {
                throw new DuplicateNameException(String.Format("A method named {0} already exists.", name));
            }

            base.Add(name, deligate);
        }

        public string Call(string name, dynamic @params)
        {
            return this[name].Invoke(@params);
        }
    }
}
