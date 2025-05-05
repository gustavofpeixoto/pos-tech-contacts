namespace PosTech.Contacts.ApplicationCore.Messaging
{
    public interface IMessagingProducer
    {
        Task SendAsync<TMessage>(TMessage message, string queue);
    }
}
