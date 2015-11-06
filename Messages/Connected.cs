using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Net.DDP.Server.Messages
{
    class Connected
    {
        public string Session { get; set; }

        public Connected()
        {

        }
    }
}
