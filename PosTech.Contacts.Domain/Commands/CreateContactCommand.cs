using MediatR;

namespace PosTech.Contacts.ApplicationCore.Commands
{
    public class CreateContactCommand : IRequest
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public int Ddd { get; set; }
    }
}
