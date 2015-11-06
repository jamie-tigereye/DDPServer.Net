using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.DDP.Server.Messages
{
    class NoSub
    {
        public string Id { get; set; }
        public dynamic Error { get; set; }

        public NoSub(string id, dynamic error)
        {
            Id = id;
            Error = error;
        }
    }
}
