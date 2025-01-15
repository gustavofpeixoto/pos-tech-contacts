using AutoMapper;
using MediatR;
using PosTech.Contacts.ApplicationCore.Commands;
using PosTech.Contacts.ApplicationCore.DTOs.Responses;
using PosTech.Contacts.ApplicationCore.Entities;
using PosTech.Contacts.ApplicationCore.Repositories;
using Serilog;

namespace PosTech.Contacts.ApplicationCore.Handlers
{
    public class UpdateContactCommandHandler(IContactRepository contactRepository, IDddRepository dddRepository, IMapper mapper)

        : IRequestHandler<UpdateContactCommand, ContactResponseDto>
    {
        private readonly IContactRepository _contactRepository = contactRepository;
        private readonly IDddRepository _dddRepository = dddRepository;
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

            var updatedContact = await UpdateContactAsync(storagedContact, request);

            await _contactRepository.UpdateContactAsync(updatedContact);

            Log.Information("Contato atualizado com sucesso. Id: {contactId}", request.Id);

            return _mapper.Map<ContactResponseDto>(updatedContact);
        }

        private async Task<Contact> UpdateContactAsync(Contact storagedContact, UpdateContactCommand request)
        {
            if (!Equals(request.Ddd, storagedContact.Ddd.DddCode))
            {
                var ddd = await _dddRepository.GetByDddCodeAsync(request.Ddd);

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
