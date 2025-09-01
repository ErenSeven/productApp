using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using System.Text.Json;
using ProductService.Core.Interfaces;
using RabbitMQ.Client;

namespace ProductService.Infrastructure.Messaging
{
    public class RabbitMqPublisher : IMessagePublisher, IDisposable
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly string _exchange;

        public RabbitMqPublisher(string hostName = "rabbitmq", string exchange = "product_exchange",
                                string userName = "guest", string password = "guest")
        {
            _exchange = exchange;
            var factory = new ConnectionFactory { HostName = hostName, Port = 5672, UserName = userName, Password = password };

                        IConnection connection = null;
            int retries = 10;
            int delay = 2000; // 2 saniye
            for (int i = 0; i < retries; i++)
            {
                try
                {
                    connection = factory.CreateConnection();
                    break;
                }
                catch
                {
                    Console.WriteLine($"RabbitMQ bağlantısı kurulamadı. {i + 1}/{retries} tekrar deneniyor...");
                    Task.Delay(delay).Wait();
                }
            }

            if (connection == null)
                throw new Exception("RabbitMQ bağlantısı kurulamadı.");

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.ExchangeDeclare(exchange: _exchange, type: ExchangeType.Topic, durable: true);
        }

        public Task PublishAsync<T>(string topic, T @event, CancellationToken ct = default)
        {
            var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(@event));
            _channel.BasicPublish(exchange: _exchange, routingKey: topic, basicProperties: null, body: body);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _channel?.Dispose();
            _connection?.Dispose();
        }
    }
}