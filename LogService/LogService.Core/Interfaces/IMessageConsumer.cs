using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LogService.Core.Interfaces
{
    public interface IMessageConsumer
    {
        void StartListening();
    }
}