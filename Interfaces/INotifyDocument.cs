using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Net.DDP.Server.Interfaces;

namespace Net.DDP.Server.Interfaces
{
    public interface NotifyIDocument : IDocument
    {
        event PropertyChangedEventHandler PropertyChanged;
    }
}
