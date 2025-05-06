using MediatR;
using PosTech.Contacts.ApplicationCore.Commands;
using PosTech.Contacts.ApplicationCore.Messaging;
using PosTech.Contacts.ApplicationCore.Repositories;
using Serilog;

namespace PosTech.Contacts.ApplicationCore.Handlers
{
    public class DeleteContactCommandHandler(IContactRepository contactRepository, IMessagingProducer messagingProducer) : IRequestHandler<DeleteContactCommand>
    {
        public async Task Handle(DeleteContactCommand request, CancellationToken cancellationToken)
        {
            Log.Information("Removendo contato. Id do contato: {contactId}", request.Id);

            await contactRepository.DeleteContactAsync(request.Id);
            await messagingProducer.SendAsync(new ContactDeletedMessage(request.Id), QueueNames.ContactDeleted);

            Log.Information("Enviando mensagem de contato removido. Id do contato: {contactId} | Nome da fila: {ContactDeleted}",
                request.Id, QueueNames.ContactDeleted);
        }
    }
}
