using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Serilog;

namespace PosTech.Contacts.Worker.Consumers
{
    public abstract class RabbitMqConsumer(IConfiguration configuration) : IAsyncDisposable
    {
        protected IConnection Connection { get; set; }
        protected IChannel Channel { get; set; }
        protected virtual async Task ConnectAsync(string queueName, CancellationToken stoppingToken = default)
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
                Log.Information("Criando conexão para fila: {queueName}", queueName);

                Connection = await connectionFactory.CreateConnectionAsync(stoppingToken);
            }
            if (Channel is null || !Channel.IsOpen)
            {
                Log.Information("Criando canal para fila: {queueName}", queueName);

                Channel = await Connection.CreateChannelAsync(cancellationToken: stoppingToken);
            }

            await Channel.QueueDeclareAsync(queue: queueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                cancellationToken: stoppingToken);
        }

        protected abstract Task ProcessMessageAsync(object sender, BasicDeliverEventArgs ea);

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
