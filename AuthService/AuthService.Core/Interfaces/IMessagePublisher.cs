using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthService.Core.Interfaces
{
    public interface IMessagePublisher
    {
        Task PublishAsync<T>(string topic, T @event, CancellationToken ct = default);
    }
}