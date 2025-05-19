using MediatR;
using PosTech.Contacts.ApplicationCore.DTOs.Responses;

namespace PosTech.Contacts.ApplicationCore.Commands
{
    public class SearchContactsCommand : IRequest<List<ContactResponseDto>>
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public int DddCode { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
    }
}
