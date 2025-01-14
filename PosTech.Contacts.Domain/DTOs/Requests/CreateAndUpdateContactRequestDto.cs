namespace PosTech.Contacts.ApplicationCore.DTOs.Requests
{
    public class CreateAndUpdateContactRequestDto
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public int Ddd { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
    }
}
