using AutoMapper;
using MediatR;
using PosTech.Contacts.ApplicationCore.Commands;
using PosTech.Contacts.ApplicationCore.DTOs.Responses;
using PosTech.Contacts.ApplicationCore.Entities;
using PosTech.Contacts.ApplicationCore.Messaging;
using PosTech.Contacts.ApplicationCore.Repositories;
using PosTech.Contacts.ApplicationCore.Repositories.Sql;
using Serilog;

namespace PosTech.Contacts.ApplicationCore.Handlers
{
    public class CreateContactCommandHandler(
        IContactRepository contactRepository,
        IDddRepository dddRepository,
        IMapper mapper,
        IMessagingProducer messagingProducer) : IRequestHandler<CreateContactCommand, ContactResponseDto>
    {
        public async Task<ContactResponseDto> Handle(CreateContactCommand request, CancellationToken cancellationToken)
        {
            Log.Information("Iniciando persistência do contato na base de dados");

            var ddd = await dddRepository.GetByDddCodeAsync(request.Ddd);
            var contact = new Contact
            {
                Ddd = ddd,
                DddId = ddd.Id,
                Email = request.Email,
                Name = request.Name,
                Phone = request.Phone,
                Surname = request.Surname,
            };

            await contactRepository.AddContactAsync(contact);

            Log.Information("Contato persistido no banco de dados com sucesso. Id do contato: {contactId}", contact.Id);

            var contactDto = mapper.Map<ContactResponseDto>(contact);
            var contactCreatedMessage = (ContactCreatedMessage)contact;

            Log.Information("Enviando mensagem de contato criado. Id do contato: {contactId} | Nome da fila: {ContactCreated}", 
                contact.Id, QueueNames.ContactCreated);

            await messagingProducer.SendAsync(contactCreatedMessage, QueueNames.ContactCreated);

            return contactDto;
        }
    }
}
