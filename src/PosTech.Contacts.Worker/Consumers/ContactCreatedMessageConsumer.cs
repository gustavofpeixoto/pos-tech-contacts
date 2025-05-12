using PosTech.Contacts.ApplicationCore.Entities.Query;
using PosTech.Contacts.ApplicationCore.Messaging;
using PosTech.Contacts.ApplicationCore.Repositories.Query;
using PosTech.Contacts.ApplicationCore.Serialization;
using PosTech.Contacts.Infrastructure.Settings;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace PosTech.Contacts.Worker.Consumers
{
    public class ContactCreatedMessageConsumer(IContactRepository contactRepository, RabbitMqSettings settings)
        : RabbitMqConsumer(settings)
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await ConnectAsync(QueueNames.ContactCreated, stoppingToken);

            var consumer = new AsyncEventingBasicConsumer(Channel);
            consumer.ReceivedAsync += ProcessMessageAsync;

            await Channel.BasicConsumeAsync(QueueNames.ContactCreated, autoAck: true, consumer: consumer, cancellationToken: stoppingToken);

            while (!stoppingToken.IsCancellationRequested)
            {
                Console.WriteLine(
                   $"Worker ativo em: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
            }
        }

        protected override async Task ProcessMessageAsync(object sender, BasicDeliverEventArgs ea)
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            var deserializedMessage = JsonSerializerHelper.Deserialize<ContactCreatedMessage>(message);
            var contact = (Contact)deserializedMessage;

            await contactRepository.AddAsync(contact);
        }
    }
}
