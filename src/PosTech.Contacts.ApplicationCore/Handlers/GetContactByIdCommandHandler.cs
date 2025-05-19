using MediatR;
using PosTech.Contacts.ApplicationCore.Commands;
using PosTech.Contacts.ApplicationCore.DTOs.Responses;
using PosTech.Contacts.ApplicationCore.Services;
using Serilog;

namespace PosTech.Contacts.ApplicationCore.Handlers
{
    public class GetContactByIdCommandHandler(
        ICacheService cacheService,
        Repositories.Query.IContactRepository queryContactRepository)
        : IRequestHandler<GetContactByIdCommand, ContactResponseDto>
    {
        public async Task<ContactResponseDto> Handle(GetContactByIdCommand request, CancellationToken cancellationToken)
        {
            Log.Information("Iniciando busca do contato com Id: {contactId}", request.Id);

            var key = request.Id.ToString();

            if (cacheService.TryGetValue<ContactResponseDto>(key, out var contact))
            {
                Log.Information("Retornando contato armazenado no cache. Id: {contactId}", request.Id);

                return contact;
            }

            var storageContact = await queryContactRepository.GetAsync(request.Id);

            contact = (ContactResponseDto)storageContact;

            Log.Information("Armazenando contatos no cache. Id: {contactId}", request.Id);

            await cacheService.SetAsync(key, contact, TimeSpan.FromMinutes(10));

            Log.Information("Finalizando busca do contato com Id: {contactId}", request.Id);

            return contact;
        }
    }
}
