using MediatR;

namespace PosTech.Contacts.ApplicationCore.Commands
{
    public class UpdateContactCommand : IRequest 
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public int Ddd { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
    }
}
