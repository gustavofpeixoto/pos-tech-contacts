namespace PosTech.Contacts.ApplicationCore.Entities.Command
{
    public class Region : Entity
    {
        public string RegionName { get; set; }
        public List<Ddd> Ddds { get; set; }
    }
}
