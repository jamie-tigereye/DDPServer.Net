using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Net.DDP.Server.Messages
{
    class Ping
    {
        public string Msg { get; set; }
        public string Id { get; set; }

        public static Ping Get(dynamic method)
        {
            var json = JsonConvert.SerializeObject(method);
            return JsonConvert.DeserializeObject<Ping>(json);
        }
    }
}
