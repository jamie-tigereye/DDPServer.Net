using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.DDP.Server.Messages
{
    class Removed
    {
        public string Msg { get; set; }
        public string Id { get; set; }
        public string Collection { get; set; }
        
        public Removed()
        {
            Msg = "removed";
        }
    }
}
