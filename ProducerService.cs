namespace InfoYatirim.Producer
{
    using RabbitMQ.Client;
    using System;
    using System.Text;
    using System.Text.Json;
    using System.Threading.Channels;
    using System.Threading.Tasks;

    public class PublisherService
    {
        private readonly IConnection _connection;
        private IChannel _channel;

        public PublisherService()
        {
            
        }
        public async Task CreateQueueAsync()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            var connection = await factory.CreateConnectionAsync();
            _channel= await connection.CreateChannelAsync();

            await _channel.QueueDeclareAsync(queue: "data-queue", durable: false, exclusive: false, autoDelete: false, arguments: null);
        }

        public async Task PublishDataAsync(Data data)
        {
            var message = JsonSerializer.Serialize(data);
            var body = Encoding.UTF8.GetBytes(message);

            await _channel.BasicPublishAsync(exchange: string.Empty, routingKey: "data-queue", body: body);

        }

        public void Dispose()
        {
            _channel.Dispose();
            _connection.Dispose();
        }
    }
}
