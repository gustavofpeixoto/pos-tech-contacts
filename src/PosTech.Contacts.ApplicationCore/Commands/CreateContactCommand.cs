using MediatR;
using PosTech.Contacts.ApplicationCore.DTOs.Responses;

namespace PosTech.Contacts.ApplicationCore.Commands
{
    public class CreateContactCommand : IRequest<ContactResponseDto> 
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public int Ddd { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
    }
}
