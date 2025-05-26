using PosTech.Contacts.ApplicationCore.Entities.Query;
using PosTech.Contacts.ApplicationCore.Messaging;
using PosTech.Contacts.ApplicationCore.Repositories.Query;
using PosTech.Contacts.ApplicationCore.Serialization;
using PosTech.Contacts.Infrastructure.Messaging;
using Quartz;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Serilog;
using System.Text;

namespace PosTech.Contacts.Worker.Consumers
{
    public class ContactCreatedMessageConsumer(
        RabbitMqConnectionManager rabbitMqConnectionManager,
        IContactRepository contactRepository)
        : RabbitMqConsumer(rabbitMqConnectionManager), IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            Log.Information("Iniciando execução do consumer: {consumer}", nameof(ContactCreatedMessageConsumer));

            await ConnectAsync(QueueNames.ContactCreated, context.CancellationToken);

            var consumer = new AsyncEventingBasicConsumer(Channel);
            consumer.ReceivedAsync += ProcessMessageAsync;

            await Channel.BasicConsumeAsync(QueueNames.ContactCreated, false, consumer: consumer);

            Log.Information("Finalizando execução do consumer: {consumer}", nameof(ContactCreatedMessageConsumer));
        }

        protected override async Task ProcessMessageAsync(object sender, BasicDeliverEventArgs ea)
        {
            try
            {

                Log.Information("Serializando conteúdo da mensagem para o consumer: {consumer}", nameof(ContactCreatedMessageConsumer));

                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var deserializedMessage = JsonSerializerHelper.Deserialize<ContactCreatedMessage>(message);
                var contact = (Contact)deserializedMessage;

                Log.Information("Inserindo contato na base de leitura. Id do contato: {contactId} | Consumer: {consumer}", contact.Id, nameof(ContactCreatedMessageConsumer));

                await contactRepository.AddAsync(contact);
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
