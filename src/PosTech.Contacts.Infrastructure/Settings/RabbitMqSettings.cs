namespace PosTech.Contacts.Infrastructure.Settings
{
    public class RabbitMqSettings(string hostName, string virtualHost, string userName, string password)
    {
        public string HostName { get; private set; } = hostName;
        public string VirtualHost { get; set; } = virtualHost;
        public string UserName { get; set; } = userName;
        public string Password { get; set; } = password;
    }
}
