using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Net.DDP.Server.Messages
{
    internal class Methd
    {
        public string Msg { get; set; }
        public string Method { get; set; }
        public string[] Parameters { get; set; }
        public string Id { get; set; }
        public string RandomSeed { get; set; }

        public Methd()
        {
            Msg = "method";
        }

        public static Methd Get(dynamic method)
        {
            var json = JsonConvert.SerializeObject(method);
            return JsonConvert.DeserializeObject<Methd>(json);
        }
    }
}
