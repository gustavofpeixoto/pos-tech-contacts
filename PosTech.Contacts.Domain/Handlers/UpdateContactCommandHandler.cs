using MediatR;
using PosTech.Contacts.ApplicationCore.Commands;
using PosTech.Contacts.ApplicationCore.Entities;
using PosTech.Contacts.ApplicationCore.Repositories;

namespace PosTech.Contacts.ApplicationCore.Handlers
{
    public class UpdateContactCommandHandler(IContactRepository contactRepository) : IRequestHandler<UpdateContactCommand>
    {
        private readonly IContactRepository _contactRepository = contactRepository;

        public async Task Handle(UpdateContactCommand request, CancellationToken cancellationToken)
        {
            var storagedContact = await _contactRepository.GetByIdAsync(request.Id);

            UpdateContact(storagedContact, request);

            await _contactRepository.UpdateContactAsync(storagedContact);
        }

        private static void UpdateContact(Contact storagedContact, UpdateContactCommand request)
        {
            storagedContact.Ddd = request.Ddd;
            storagedContact.Email = request.Email;
            storagedContact.Name = request.Name;
            storagedContact.Phone = request.Phone;
            storagedContact.Surname = request.Surname;
        }
    }
}
