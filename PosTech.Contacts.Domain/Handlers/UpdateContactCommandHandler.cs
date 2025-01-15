using AutoMapper;
using MediatR;
using PosTech.Contacts.ApplicationCore.Commands;
using PosTech.Contacts.ApplicationCore.DTOs.Responses;
using PosTech.Contacts.ApplicationCore.Entities;
using PosTech.Contacts.ApplicationCore.Repositories;
using Serilog;

namespace PosTech.Contacts.ApplicationCore.Handlers
{
    public class UpdateContactCommandHandler(IContactRepository contactRepository, IMapper mapper) : IRequestHandler<UpdateContactCommand, ContactResponseDto>
    {
        private readonly IContactRepository _contactRepository = contactRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<ContactResponseDto> Handle(UpdateContactCommand request, CancellationToken cancellationToken)
        {
            Log.Information("Iniciando atualização do contato. Id: {contactId}", request.Id);

            var storagedContact = await _contactRepository.GetByIdAsync(request.Id);

            if (storagedContact is null)
            {
                Log.Information("Contato não localizado para o Id: {contactId}", request.Id);

                return null;
            }

            var updatedContact = UpdateContact(storagedContact, request);

            await _contactRepository.UpdateContactAsync(updatedContact);

            Log.Information("Contato atualizado com sucesso. Id: {contactId}", request.Id);

            return _mapper.Map<ContactResponseDto>(updatedContact);
        }

        private static Contact UpdateContact(Contact storagedContact, UpdateContactCommand request)
        {
            storagedContact.Ddd = request.Ddd;
            storagedContact.Email = request.Email;
            storagedContact.Name = request.Name;
            storagedContact.Phone = request.Phone;
            storagedContact.Surname = request.Surname;

            return storagedContact;
        }
    }
}
