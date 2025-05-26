using PosTech.Contacts.ApplicationCore.Messaging;
using PosTech.Contacts.ApplicationCore.Serialization;
using RabbitMQ.Client;
using System.Text;

namespace PosTech.Contacts.Infrastructure.Messaging
{
    public class RabbitMqMessagingProducer(RabbitMqConnectionManager rabbitMqConnectionManager) : IMessagingProducer, IAsyncDisposable
    {
        private IConnection _connection { get; set; }
        private IChannel _channel { get; set; }

        public async Task SendAsync<TMessage>(TMessage message, string queue)
        {
            _connection = await rabbitMqConnectionManager.GetConnectionAsync();

            _channel ??= await rabbitMqConnectionManager.GetChannelAsync();

            await _channel.QueueDeclareAsync(queue,

                durable: true,
                exclusive: false,
                autoDelete: false);

            var serializedMessage = JsonSerializerHelper.Serialize(message);
            var messageBody = Encoding.UTF8.GetBytes(serializedMessage);

            var properties = new BasicProperties { Persistent = true };

            await _channel.BasicPublishAsync(
                exchange: string.Empty,
                routingKey: queue,
                true,
                basicProperties: properties,
                body: messageBody);
        }

        public async ValueTask DisposeAsync()
        {
            if (_channel is not null)
            {
                await _channel.CloseAsync();
                _channel.Dispose();
                _channel = null;
            }

            if (_connection is not null)
            {
                await _connection.CloseAsync();
                _connection.Dispose();
                _connection = null;
            }

            GC.SuppressFinalize(this);
        }
    }

}
