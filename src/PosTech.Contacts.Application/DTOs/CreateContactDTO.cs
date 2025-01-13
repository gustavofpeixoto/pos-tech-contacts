namespace PosTech.Contacts.Application.DTOs
{
    public class CreateContactDTO
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public int Phone { get; set; }
        public int Ddd { get; set; }
    }
}
