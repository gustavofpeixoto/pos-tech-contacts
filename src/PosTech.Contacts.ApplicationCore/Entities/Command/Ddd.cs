namespace PosTech.Contacts.ApplicationCore.Entities.Command
{
    public class Ddd : Entity
    {
        public int DddCode { get; set; }
        public string State { get; set; }
        public Guid RegionId { get; set; }
        public Region Region { get; set; }
        public List<Contact> Contacts { get; set; }
    }
}
