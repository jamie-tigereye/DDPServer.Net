using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.DDP.Server.Messages
{
    internal class Reslt
    {
        public string Msg { get; set; }
        public string Id { get; set; }
        public dynamic Error { get; set; }
        public dynamic Result { get; set; }

        public Reslt(string id)
        {
            Msg = "result";
            Id = id;
        }
    }
}
