namespace PosTech.Contacts.ApplicationCore.DTOs.Responses
{
    public class ContactResponseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public int Ddd { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
    }
}
