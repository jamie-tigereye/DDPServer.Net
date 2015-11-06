using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Net.DDP.Server.Messages
{
    class UnSub
    {
        public string Msg { get; set; }
        public string Id { get; set; }

        public UnSub()
        {

        }
        public static Sub Get(dynamic method)
        {
            var json = JsonConvert.SerializeObject(method);
            return JsonConvert.DeserializeObject<Sub>(json);
        }
    }
}
