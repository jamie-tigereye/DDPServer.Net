using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Net.DDP.Server.Messages
{
    class Connect
    {
        public string Msg { get; set; }
        public string Session { get; set; }
        public string Version { get; set; }
        public string Support { get; set; }

        public Connect()
        {

        }

        public static Connect Get(dynamic method)
        {
            var json = JsonConvert.SerializeObject(method);
            return JsonConvert.DeserializeObject<Connect>(json);
        }
    }
}
