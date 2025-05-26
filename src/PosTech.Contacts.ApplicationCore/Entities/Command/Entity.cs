namespace PosTech.Contacts.ApplicationCore.Entities.Command
{
    public abstract class Entity
    {
        public Guid Id { get; set; }
        protected Entity() => Id = Guid.NewGuid();
    }
}
