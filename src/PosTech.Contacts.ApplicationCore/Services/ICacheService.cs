namespace PosTech.Contacts.ApplicationCore.Services
{
    public interface ICacheService
    {
        Task SetAsync<T>(string key, T value, TimeSpan expiresIn);
        bool TryGetValue<T>(string key, out T value);
    }
}
