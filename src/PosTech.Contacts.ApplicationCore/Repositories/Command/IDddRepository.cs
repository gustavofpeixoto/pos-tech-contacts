using PosTech.Contacts.ApplicationCore.Entities.Command;

namespace PosTech.Contacts.ApplicationCore.Repositories.Command
{
    public interface IDddRepository
    {
        Task<Ddd> GetByDddCodeAsync(int dddCode);
    }
}
