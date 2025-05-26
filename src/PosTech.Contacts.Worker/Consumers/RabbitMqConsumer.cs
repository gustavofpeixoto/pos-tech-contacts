using PosTech.Contacts.Infrastructure.Messaging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace PosTech.Contacts.Worker.Consumers
{
    public abstract class RabbitMqConsumer(RabbitMqConnectionManager connectionManager)
    {
        protected readonly RabbitMqConnectionManager _connectionManager = connectionManager;
        protected IChannel Channel { get; private set; }
        protected IConnection Connection { get; private set; }

        protected virtual async Task ConnectAsync(string queueName, CancellationToken stoppingToken = default)
        {
            Connection ??= await _connectionManager.GetConnectionAsync(stoppingToken);
            Channel ??= await _connectionManager.GetChannelAsync(stoppingToken);

            await Channel.QueueDeclareAsync(queue: queueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                cancellationToken: stoppingToken);
        }

        protected abstract Task ProcessMessageAsync(object sender, BasicDeliverEventArgs ea);
    }
}
