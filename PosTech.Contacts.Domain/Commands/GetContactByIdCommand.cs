using MediatR;
using PosTech.Contacts.ApplicationCore.DTOs.Responses;

namespace PosTech.Contacts.ApplicationCore.Commands
{
    public class GetContactByIdCommand : IRequest<ContactResponseDto>
    {
        public Guid Id { get; set; }
    }
}
