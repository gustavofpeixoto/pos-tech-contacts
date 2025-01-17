using AutoMapper;
using MediatR;
using PosTech.Contacts.ApplicationCore.Commands;
using PosTech.Contacts.ApplicationCore.DTOs.Responses;
using PosTech.Contacts.ApplicationCore.Entities;
using PosTech.Contacts.ApplicationCore.Repositories;
using Serilog;

namespace PosTech.Contacts.ApplicationCore.Handlers
{
    public class CreateContactCommandHandler(IContactRepository contactRepository, IDddRepository dddRepository, IMapper mapper)
        : IRequestHandler<CreateContactCommand, ContactResponseDto>
    {
        private readonly IContactRepository _contactRepository = contactRepository;
        private readonly IDddRepository _dddRepository = dddRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<ContactResponseDto> Handle(CreateContactCommand request, CancellationToken cancellationToken)
        {
            Log.Information("Iniciando persistência do contato na base de dados");

            var ddd = await _dddRepository.GetByDddCodeAsync(request.Ddd);
            var contact = new Contact
            {
                Ddd = ddd,
                DddId = ddd.Id,
                Email = request.Email,
                Name = request.Name,
                Phone = request.Phone,
                Surname = request.Surname,
            };

            await _contactRepository.AddContactAsync(contact);

            Log.Information("Contato persistido no banco de dados com sucesso. Id: {contactId}", contact.Id);

            var contactDto = _mapper.Map<ContactResponseDto>(contact);

            return contactDto;
        }
    }
}
