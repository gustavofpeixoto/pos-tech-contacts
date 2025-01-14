using MediatR;

namespace PosTech.Contacts.ApplicationCore.Commands
{
    public class DeleteContactCommand : IRequest
    {
        public Guid Id { get; set; }
    }
}
