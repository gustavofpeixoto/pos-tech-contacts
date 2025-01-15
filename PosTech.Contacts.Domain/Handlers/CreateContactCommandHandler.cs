using MediatR;
using PosTech.Contacts.ApplicationCore.Commands;
using PosTech.Contacts.ApplicationCore.Entities;
using PosTech.Contacts.ApplicationCore.Repositories;
using Serilog;

namespace PosTech.Contacts.ApplicationCore.Handlers
{
    public class CreateContactCommandHandler(IContactRepository contactRepository) : IRequestHandler<CreateContactCommand>
    {
        private readonly IContactRepository _contactRepository = contactRepository;

        public async Task Handle(CreateContactCommand request, CancellationToken cancellationToken)
        {
            Log.Information("Iniciando persistência do contato na base de dados");

            var contact = new Contact
            {
                Ddd = request.Ddd,
                Email = request.Email,
                Name = request.Name,
                Phone = request.Phone,
                Surname = request.Surname,
            };
            await _contactRepository.AddContactAsync(contact);

            Log.Information("Contato persistido no banco de dados com sucesso. Id: {contactId}", contact.Id);
        }
    }
}
