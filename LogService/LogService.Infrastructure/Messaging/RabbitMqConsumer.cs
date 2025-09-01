using System;
using System.Text;
using System.Text.Json;
using LogService.Application.Logs.Commands;
using LogService.Core.Interfaces;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace LogService.Infrastructure.Messaging
{
    public class RabbitMqConsumer : IMessageConsumer
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ConnectionFactory _factory;

        public RabbitMqConsumer(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _factory = new ConnectionFactory() 
            { 
                HostName = "rabbitmq", 
                UserName = "guest", 
                Password = "guest" 
            };
        }

        public void StartListening()
        {
            var connection = _factory.CreateConnection();

            // 🔹 ProductService event'leri
            ListenQueue(connection, "product_created_queue", "product_exchange", "product.created");
            ListenQueue(connection, "product_updated_queue", "product_exchange", "product.updated");
            ListenQueue(connection, "product_deleted_queue", "product_exchange", "product.deleted");

            // 🔹 AuthService event'leri
            ListenQueue(connection, "user_registered_queue", "auth_exchange", "user.registered");
            ListenQueue(connection, "user_loggedin_queue", "auth_exchange", "user.loggedin");

            Console.WriteLine("✅ RabbitMQ Consumers started...");
        }

        private void ListenQueue(IConnection connection, string queue, string exchange, string routingKey)
        {
            var channel = connection.CreateModel();
            
            channel.ExchangeDeclare(exchange: exchange, type: ExchangeType.Topic, durable: true);

            channel.QueueDeclare(queue: queue, durable: true, exclusive: false, autoDelete: false, arguments: null);
            channel.QueueBind(queue: queue, exchange: exchange, routingKey: routingKey);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                using var scope = _serviceProvider.CreateScope();
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

                var cmd = new AddLogCommand
                {
                    Message = $"[{queue}] {message}",
                    CreatedAt = DateTime.UtcNow
                };

                await mediator.Send(cmd);
            };

            channel.BasicConsume(queue: queue, autoAck: true, consumer: consumer);
        }
    }

}
