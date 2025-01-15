using PosTech.Contacts.ApplicationCore.Entities;

namespace PosTech.Contacts.ApplicationCore.Repositories
{
    public interface IDddRepository
    {
        Task<Ddd> GetByDddCodeAsync(int dddCode);
    }
}
