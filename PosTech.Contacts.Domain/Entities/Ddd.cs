namespace PosTech.Contacts.ApplicationCore.Entities
{
    public class Ddd : Entity
    {
        public int DddCode { get; set; }
        public Guid RegionId { get; set; }
        public Region Region { get; set; }
        public List<Contact> Contacts { get; set; }
    }
}
