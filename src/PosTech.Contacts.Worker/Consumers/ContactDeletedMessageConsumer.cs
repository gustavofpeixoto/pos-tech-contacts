using PosTech.Contacts.ApplicationCore.Messaging;
using PosTech.Contacts.ApplicationCore.Repositories.Query;
using PosTech.Contacts.ApplicationCore.Serialization;
using PosTech.Contacts.Infrastructure.Settings;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Serilog;
using System.Text;

namespace PosTech.Contacts.Worker.Consumers
{
    public class ContactDeletedMessageConsumer(IContactRepository contactRepository, RabbitMqSettings settings)
        : RabbitMqConsumer(settings)
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Log.Information("Iniciando execução do consumer: {consumer}", nameof(ContactDeletedMessageConsumer));

            await ConnectAsync(QueueNames.ContactDeleted, stoppingToken);

            var consumer = new AsyncEventingBasicConsumer(Channel);
            consumer.ReceivedAsync += ProcessMessageAsync;

            await Channel.BasicConsumeAsync(QueueNames.ContactDeleted, autoAck: true, consumer: consumer, cancellationToken: stoppingToken);

            Log.Information("Finalizando execução do consumer: {consumer}", nameof(ContactDeletedMessageConsumer));

            while (!stoppingToken.IsCancellationRequested)
            {
                Log.Information($"Worker {nameof(ContactDeletedMessageConsumer)} ativo em: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
            }
        }

        protected override async Task ProcessMessageAsync(object sender, BasicDeliverEventArgs ea)
        {
            Log.Information("Serializando conteúdo da mensagem para o consumer: {consumer}", nameof(ContactDeletedMessageConsumer));

            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            var deserializedMessage = JsonSerializerHelper.Deserialize<ContactDeletedMessage>(message);

            Log.Information("Removendo contato na base de leitura. Id do contato: {contactId} | Consumer: {consumer}", deserializedMessage.ContactId, nameof(ContactDeletedMessageConsumer));

            await contactRepository.DeleteAsync(deserializedMessage.ContactId);
        }
    }
}
