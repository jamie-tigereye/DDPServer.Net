using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Net.DDP.Server.Messages
{
    class AddedAfter
    {
        public string Msg { get; set; }
        public string Collection { get; set; }
        public string Id { get; set; }
        public string Before { get; set; }

        public AddedAfter()
        {
            Msg = "addedAfter";
        }
    }
}
