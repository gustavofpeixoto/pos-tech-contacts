using PosTech.Contacts.ApplicationCore.Entities.Query;
using PosTech.Contacts.ApplicationCore.Messaging;
using PosTech.Contacts.ApplicationCore.Repositories.Query;
using PosTech.Contacts.ApplicationCore.Serialization;
using Quartz;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Serilog;
using System.Text;

namespace PosTech.Contacts.Worker.Consumers
{
    public class ContactUpdatedMessageConsumer(IConfiguration configuration, IContactRepository contactRepository)
        : RabbitMqConsumer(configuration), IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            Log.Information("Iniciando execução do consumer: {consumer}", nameof(ContactUpdatedMessageConsumer));

            await ConnectAsync(QueueNames.ContactUpdated);

            var consumer = new AsyncEventingBasicConsumer(Channel);
            consumer.ReceivedAsync += ProcessMessageAsync;

            await Channel.BasicConsumeAsync(QueueNames.ContactUpdated, false, consumer: consumer);

            Log.Information("Finalizando execução do consumer: {consumer}", nameof(ContactUpdatedMessageConsumer));
        }

        protected override async Task ProcessMessageAsync(object sender, BasicDeliverEventArgs ea)
        {
            Log.Information("Serializando conteúdo da mensagem para o consumer: {consumer}", nameof(ContactUpdatedMessageConsumer));

            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            var deserializedMessage = JsonSerializerHelper.Deserialize<ContactUpdatedMessage>(message);
            var contact = (Contact)deserializedMessage;

            Log.Information("Atualizando contato na base de leitura. Id do contato: {contactId} | Consumer: {consumer}", contact.Id, nameof(ContactUpdatedMessageConsumer));

            await contactRepository.UpdateAsync(contact);
            await Channel.BasicAckAsync(ea.DeliveryTag, false);
        }
    }
}
