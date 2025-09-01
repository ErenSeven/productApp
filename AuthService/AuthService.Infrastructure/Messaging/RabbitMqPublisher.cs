using System;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using AuthService.Core.Interfaces;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;

namespace AuthService.Infrastructure.Messaging
{
    public class RabbitMqPublisher : IMessagePublisher, IDisposable
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly string _exchange;

        public RabbitMqPublisher(IConfiguration configuration)
        {
            var rabbitConfig = configuration.GetSection("RabbitMq");

            _exchange = rabbitConfig["Exchange"];

            var factory = new ConnectionFactory
            {
                HostName = rabbitConfig["HostName"],
                Port = 5672,
                UserName = rabbitConfig["UserName"],
                Password = rabbitConfig["Password"],
                Ssl = { Enabled = false }
            };

            IConnection connection = null;
            int retries = 10;
            int delay = 2000; 
            for (int i = 0; i < retries; i++)
            {
                try
                {
                    Console.WriteLine($"RabbitMQ’ya bağlanılıyor... {i + 1}/{retries}");
                    connection = factory.CreateConnection();
                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Bağlantı kurulamadı: {ex.Message}. {delay / 1000} saniye sonra tekrar deneniyor...");
                    Task.Delay(delay).Wait();
                }
            }

            if (connection == null)
                throw new Exception("RabbitMQ bağlantısı kurulamadı. AMQP portunu (5672) kontrol et.");

            _connection = connection;
            _channel = _connection.CreateModel();
            _channel.ExchangeDeclare(exchange: _exchange, type: ExchangeType.Topic, durable: true);

            Console.WriteLine($"✅ RabbitMQ exchange '{_exchange}' hazır.");
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
