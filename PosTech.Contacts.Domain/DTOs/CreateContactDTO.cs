namespace PosTech.Contacts.ApplicationCore.DTOs
{
    public class CreateContactDTO
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public int Ddd { get; set; }
    }
}
