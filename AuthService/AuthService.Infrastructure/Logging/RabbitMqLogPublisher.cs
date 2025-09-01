using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;

public class RabbitMqLogPublisher
{
    private readonly IConnection _connection;

    public RabbitMqLogPublisher(IConnection connection)
    {
        _connection = connection;
    }

    public void PublishLog(string level, string message, string source, string? exception = null)
    {
        using var channel = _connection.CreateModel();
        channel.QueueDeclare(queue: "logQueue", durable: true, exclusive: false, autoDelete: false, arguments: null);

        var log = new
        {
            Level = level,
            Message = message,
            Source = source,
            Exception = exception,
            Timestamp = DateTime.UtcNow
        };

        var body = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(log);
        channel.BasicPublish(exchange: "", routingKey: "logQueue", basicProperties: null, body: body);
    }
}
