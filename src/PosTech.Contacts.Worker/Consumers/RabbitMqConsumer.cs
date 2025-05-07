using PosTech.Contacts.Infrastructure.Settings;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace PosTech.Contacts.Worker.Consumers
{
    public abstract class RabbitMqConsumer(RabbitMqSettings settings) : BackgroundService, IAsyncDisposable
    {
        protected IConnection Connection { get; set; }
        protected IChannel Channel { get; set; }
        protected virtual async Task ConnectAsync(string queueName, CancellationToken stoppingToken)
        {
            var connectionFactory = new ConnectionFactory
            {
                HostName = settings.HostName,
                VirtualHost = settings.VirtualHost,
                UserName = settings.UserName,
                Password = settings.Password,
            };

            if (Connection is null || !Connection.IsOpen) Connection = await connectionFactory.CreateConnectionAsync(stoppingToken);
            if (Channel is null || !Channel.IsOpen) Channel = await Connection.CreateChannelAsync(cancellationToken: stoppingToken);

            await Channel.QueueDeclareAsync(queue: queueName,
                exclusive: false,
                autoDelete: false,
                cancellationToken: stoppingToken);
        }

        protected abstract Task ProcessMessageAsync(object sender, BasicDeliverEventArgs ea);

        public async ValueTask DisposeAsync()
        {
            await Channel?.CloseAsync();
            Channel?.Dispose();
            Channel = null;

            await Connection?.CloseAsync();
            Connection?.Dispose();
            Connection = null;

            GC.SuppressFinalize(this);
        }
    }
}
