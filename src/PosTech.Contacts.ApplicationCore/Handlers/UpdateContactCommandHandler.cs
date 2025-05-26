using AutoMapper;
using MediatR;
using PosTech.Contacts.ApplicationCore.Commands;
using PosTech.Contacts.ApplicationCore.DTOs.Responses;
using PosTech.Contacts.ApplicationCore.Entities.Command;
using PosTech.Contacts.ApplicationCore.Messaging;
using PosTech.Contacts.ApplicationCore.Repositories.Command;
using Serilog;

namespace PosTech.Contacts.ApplicationCore.Handlers
{
    public class UpdateContactCommandHandler(
        IContactRepository contactRepository,
        IDddRepository dddRepository,
        IMapper mapper,
        IMessagingProducer messagingProducer) : IRequestHandler<UpdateContactCommand, ContactResponseDto>
    {
        public async Task<ContactResponseDto> Handle(UpdateContactCommand request, CancellationToken cancellationToken)
        {
            Log.Information("Iniciando atualização do contato. Id do contato: {contactId}", request.Id);

            var storagedContact = await contactRepository.GetByIdAsync(request.Id);

            if (storagedContact is null)
            {
                Log.Information("Contato não localizado para o Id do contato: {contactId}", request.Id);

                return null;
            }

            var updatedContact = await UpdateContactAsync(storagedContact, request);

            await contactRepository.UpdateContactAsync(updatedContact);

            Log.Information("Contato atualizado com sucesso. Id do contato: {contactId}", request.Id);

            var contactUpdatedMessage = (ContactUpdatedMessage)updatedContact;

            Log.Information("Enviando mensagem de contato atualizado. Id do contato: {contactId} | Nome da fila: {ContactUpdated}", request.Id, QueueNames.ContactUpdated);

            await messagingProducer.SendAsync(contactUpdatedMessage, QueueNames.ContactUpdated);

            return mapper.Map<ContactResponseDto>(updatedContact);
        }

        private async Task<Contact> UpdateContactAsync(Contact storagedContact, UpdateContactCommand request)
        {
            if (!Equals(request.Ddd, storagedContact.Ddd.DddCode))
            {
                var ddd = await dddRepository.GetByDddCodeAsync(request.Ddd);

                storagedContact.Ddd = ddd;
                storagedContact.DddId = ddd.Id;
            }

            storagedContact.Email = request.Email;
            storagedContact.Name = request.Name;
            storagedContact.Phone = request.Phone;
            storagedContact.Surname = request.Surname;

            return storagedContact;
        }
    }
}
