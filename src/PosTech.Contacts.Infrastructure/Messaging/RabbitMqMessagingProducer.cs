using Microsoft.Extensions.Configuration;
using PosTech.Contacts.ApplicationCore.Messaging;
using PosTech.Contacts.ApplicationCore.Serialization;
using RabbitMQ.Client;
using Serilog;
using System.Text;

namespace PosTech.Contacts.Infrastructure.Messaging
{
    public class RabbitMqMessagingProducer(IConfiguration configuration) : IMessagingProducer, IAsyncDisposable
    {
        private IConnection Connection { get; set; }
        private IChannel Channel { get; set; }

        public async Task SendAsync<TMessage>(TMessage message, string queue)
        {
            var connectionFactory = new ConnectionFactory
            {
                HostName = configuration["RabbitMq:HostName"],
                VirtualHost = configuration["RabbitMq:VirtualHost"],
                UserName = configuration["RabbitMq:UserName"],
                Password = configuration["RabbitMq:Password"],
            };

            if (Connection is null || !Connection.IsOpen)
            {
                Log.Information("Criando conexão para fila: {queueName}");

                Connection = await connectionFactory.CreateConnectionAsync();
            }
            if (Channel is null || !Channel.IsOpen)
            {
                Log.Information("Criando canal para fila: {queueName}");

                Channel = await Connection.CreateChannelAsync();
            }

            await Channel.QueueDeclareAsync(queue,

                durable: true,
                exclusive: false,
                autoDelete: false);

            var serializedMessage = JsonSerializerHelper.Serialize(message);
            var messageBody = Encoding.UTF8.GetBytes(serializedMessage);

            var properties = new BasicProperties { Persistent = true };

            await Channel.BasicPublishAsync(
                exchange: string.Empty,
                routingKey: queue,
                true,
                basicProperties: properties,
                body: messageBody);
        }

        public async ValueTask DisposeAsync()
        {
            if (Channel is not null)
            {
                await Channel.CloseAsync();
                Channel.Dispose();
                Channel = null;
            }

            if (Connection is not null)
            {
                await Connection.CloseAsync();
                Connection.Dispose();
                Connection = null;
            }

            GC.SuppressFinalize(this);
        }
    }

}
