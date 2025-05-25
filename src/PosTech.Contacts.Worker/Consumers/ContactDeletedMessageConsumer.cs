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
    public class ContactDeletedMessageConsumer(IConfiguration configuration, IContactRepository contactRepository)
        : RabbitMqConsumer(configuration), IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            Log.Information("Iniciando execução do consumer: {consumer}", nameof(ContactDeletedMessageConsumer));

            await ConnectAsync(QueueNames.ContactDeleted);

            var consumer = new AsyncEventingBasicConsumer(Channel);
            consumer.ReceivedAsync += ProcessMessageAsync;

            await Channel.BasicConsumeAsync(QueueNames.ContactDeleted, false, consumer: consumer);

            Log.Information("Finalizando execução do consumer: {consumer}", nameof(ContactDeletedMessageConsumer));
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
