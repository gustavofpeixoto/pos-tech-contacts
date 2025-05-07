using PosTech.Contacts.ApplicationCore.Messaging;
using PosTech.Contacts.ApplicationCore.Serialization;
using PosTech.Contacts.Infrastructure.Settings;
using RabbitMQ.Client;
using System.Text;

namespace PosTech.Contacts.Infrastructure.Messaging
{
    public class RabbitMqMessagingProducer(RabbitMqSettings settings) : IMessagingProducer
    {
        public async Task SendAsync<TMessage>(TMessage message, string queue)
        {
            var connectionFactory = new ConnectionFactory
            {
                HostName = settings.HostName,
                VirtualHost = settings.VirtualHost,
                UserName = settings.UserName,
                Password = settings.Password,
            };

            var connection = await connectionFactory.CreateConnectionAsync();

            using var channel = await connection.CreateChannelAsync();

            await channel.QueueDeclareAsync(queue, 
                exclusive: false, 
                autoDelete: false);

            var serializedMessage = JsonSerializerHelper.Serialize(message);
            var messageBody = Encoding.UTF8.GetBytes(serializedMessage);

            await channel.BasicPublishAsync(exchange: "", routingKey: queue, body: messageBody);
        }
    }
}
