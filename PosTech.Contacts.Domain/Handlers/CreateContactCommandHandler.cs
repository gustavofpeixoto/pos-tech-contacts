using MediatR;
using PosTech.Contacts.ApplicationCore.Commands;
using PosTech.Contacts.ApplicationCore.Entities;
using PosTech.Contacts.ApplicationCore.Repositories;

namespace PosTech.Contacts.ApplicationCore.Handlers
{
    public class CreateContactCommandHandler(IContactRepository contactRepository) : IRequestHandler<CreateContactCommand>
    {
        private readonly IContactRepository _contactRepository = contactRepository;

        public async Task Handle(CreateContactCommand request, CancellationToken cancellationToken)
        {
            var contact = new Contact
            {
                Ddd = request.Ddd,
                Email = request.Email,
                Name = request.Name,
                Phone = request.Phone,
                Surname = request.Surname,
            };
            await _contactRepository.AddContactAsync(contact);
        }
    }
}
