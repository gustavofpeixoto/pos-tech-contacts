using PosTech.Contacts.ApplicationCore.Messaging;
using PosTech.Contacts.ApplicationCore.Repositories.Query;
using PosTech.Contacts.ApplicationCore.Serialization;
using PosTech.Contacts.Infrastructure.Messaging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Serilog;
using System.Text;

namespace PosTech.Contacts.Worker.Consumers
{
    public class ContactDeletedMessageConsumer(
        RabbitMqConnectionManager rabbitMqConnectionManager,
        IContactRepository contactRepository)
        : RabbitMqConsumer(rabbitMqConnectionManager)
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await ExecuteAsync(QueueNames.ContactDeleted, stoppingToken);
            await Task.CompletedTask;
        }

        protected async Task ExecuteAsync(string queueName, CancellationToken cancellationToken = default)
        {
            Log.Information("Iniciando execução do consumer: {consumer}", nameof(ContactDeletedMessageConsumer));

            await ConnectAsync(queueName, cancellationToken);

            var consumer = new AsyncEventingBasicConsumer(Channel);
            consumer.ReceivedAsync += ProcessMessageAsync;

            await Channel.BasicConsumeAsync(queue: queueName, autoAck: false, consumer: consumer, cancellationToken: cancellationToken);

            Log.Information("Finalizando execução do consumer: {consumer}", nameof(ContactDeletedMessageConsumer));
        }

        protected override async Task ProcessMessageAsync(object sender, BasicDeliverEventArgs ea)
        {
            try
            {
                Log.Information("Serializando conteúdo da mensagem para o consumer: {consumer}", nameof(ContactDeletedMessageConsumer));

                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var deserializedMessage = JsonSerializerHelper.Deserialize<ContactDeletedMessage>(message);

                Log.Information("Removendo contato na base de leitura. Id do contato: {contactId} | Consumer: {consumer}", deserializedMessage.ContactId, nameof(ContactDeletedMessageConsumer));

                await contactRepository.DeleteAsync(deserializedMessage.ContactId);
                await Channel.BasicAckAsync(ea.DeliveryTag, false);
            }
            catch (Exception e)
            {
                await Channel.BasicNackAsync(ea.DeliveryTag, false, false);
                Log.Warning("Erro: {@e}", e);
            }
        }
    }
}
