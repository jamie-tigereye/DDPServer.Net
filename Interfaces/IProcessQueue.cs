using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.DDP.Server.Interfaces
{
    internal interface IProcessQueues<T>
    {
        void ProcessItem(T item);
    }
}
