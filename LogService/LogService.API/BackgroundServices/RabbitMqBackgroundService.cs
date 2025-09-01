using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using LogService.Core.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace LogService.API.BackgroundServices
{
    public class RabbitMqBackgroundService : IHostedService
    {
        private readonly IMessageConsumer _consumer;

        public RabbitMqBackgroundService(IMessageConsumer consumer)
        {
            _consumer = consumer;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _consumer.StartListening();
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}