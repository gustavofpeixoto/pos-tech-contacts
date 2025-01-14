using MediatR;
using PosTech.Contacts.ApplicationCore.Commands;
using PosTech.Contacts.ApplicationCore.Repositories;

namespace PosTech.Contacts.ApplicationCore.Handlers
{
    internal class DeleteContactCommandHandler(IContactRepository contactRepository) : IRequestHandler<DeleteContactCommand>
    {
        private readonly IContactRepository _contactRepository = contactRepository;

        public async Task Handle(DeleteContactCommand request, CancellationToken cancellationToken)
        {
            await _contactRepository.DeleteContactAsync(request.Id);
        }
    }
}
