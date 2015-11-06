using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.DDP.Server.Messages
{
    class Changed
    {
        public string Msg { get; set; }
        public string Id { get; set; }
        public string Collection { get; set; }
        public dynamic Fields { get; set; }
        public string[] Cleared { get; set; }
        
        public Changed()
        {
            Msg = "changed";
        }
    }
}
