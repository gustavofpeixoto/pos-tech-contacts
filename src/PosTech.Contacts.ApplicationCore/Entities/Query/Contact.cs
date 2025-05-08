namespace PosTech.Contacts.ApplicationCore.Entities.Query
{
    public class Contact(Guid id)
    {
        public Guid Id { get; private set; } = id;
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public int DddCode { get; set; }
        public string DddState { get; set; }
        public string RegionName { get; set; }
    }
}
