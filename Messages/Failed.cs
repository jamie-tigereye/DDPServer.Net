using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Net.DDP.Server.Messages
{
    class Failed
    {
        public string Msg { get; set; }
        public string Version { get; set; }

        public Failed()
        {
            Msg = "failed";
        }
    }
}
