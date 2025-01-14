using AutoMapper;
using MediatR;
using PosTech.Contacts.ApplicationCore.Commands;
using PosTech.Contacts.ApplicationCore.DTOs.Responses;
using PosTech.Contacts.ApplicationCore.Entities;
using PosTech.Contacts.ApplicationCore.Repositories;

namespace PosTech.Contacts.ApplicationCore.Handlers
{
    public class GetContactByIdCommandHandler(IMapper mapper, IContactRepository contactRepository) 
        : IRequestHandler<GetContactByIdCommand, ContactResponseDto>
    {
        private readonly IMapper _mapper = mapper;
        private readonly IContactRepository _contactRepository = contactRepository;

        public async Task<ContactResponseDto> Handle(GetContactByIdCommand request, CancellationToken cancellationToken)
        {
            var storageContact = await _contactRepository.GetByIdAsync(request.Id);

            var contact = _mapper.Map<ContactResponseDto>(storageContact);

            return contact;
        }
    }
}
