namespace PosTech.Contacts.ApplicationCore.Entities
{
    public class Contact : Entity
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public int Ddd { get; set; }

        public Contact() { }
        public Contact(Guid id) => Id = id;
    }
}
