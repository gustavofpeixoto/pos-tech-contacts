namespace PosTech.Contacts.ApplicationCore.Messaging
{
    public class ContactDeletedMessage(Guid contactId)
    {
        public Guid ContactId { get; private set; } = contactId;
    }
}
