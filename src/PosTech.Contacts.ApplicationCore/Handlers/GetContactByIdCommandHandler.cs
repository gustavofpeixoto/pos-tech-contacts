using AutoMapper;
using MediatR;
using PosTech.Contacts.ApplicationCore.Commands;
using PosTech.Contacts.ApplicationCore.DTOs.Responses;
using PosTech.Contacts.ApplicationCore.Repositories;
using PosTech.Contacts.ApplicationCore.Services;
using Serilog;

namespace PosTech.Contacts.ApplicationCore.Handlers
{
    public class GetContactByIdCommandHandler(ICacheService cacheService, IContactRepository contactRepository, IMapper mapper)
        : IRequestHandler<GetContactByIdCommand, ContactResponseDto>
    {
        private readonly ICacheService _cacheService = cacheService;
        private readonly IContactRepository _contactRepository = contactRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<ContactResponseDto> Handle(GetContactByIdCommand request, CancellationToken cancellationToken)
        {
            Log.Information("Iniciando busca do contato com Id: {contactId}", request.Id);

            var key = request.Id.ToString();

            if (_cacheService.TryGetValue<ContactResponseDto>(key, out var contact))
            {
                Log.Information("Retornando contato armazenado no cache. Id: {contactId}", request.Id);

                return contact;
            }

            var storageContact = await _contactRepository.GetByIdAsync(request.Id);

            contact = _mapper.Map<ContactResponseDto>(storageContact);

            Log.Information("Armazenando contatos no cache. Id: {contactId}", request.Id);

            await _cacheService.SetAsync(key, contact, TimeSpan.FromMinutes(10));

            Log.Information("Finalizando busca do contato com Id: {contactId}", request.Id);

            return contact;
        }
    }
}
