using PosTech.Contacts.ApplicationCore.Entities.Command;
using System.Linq.Expressions;

namespace PosTech.Contacts.ApplicationCore.Repositories.Command
{
    public interface IContactRepository
    {
        Task AddContactAsync(Contact contact);
        Task DeleteContactAsync(Guid id);
        Task<IEnumerable<Contact>> FindContactsAsync(Expression<Func<Contact, bool>> predicate);
        Task<Contact> GetByIdAsync(Guid id);
        Task UpdateContactAsync(Contact contact);
    }
}
