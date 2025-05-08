using PosTech.Contacts.ApplicationCore.Entities;
using System.Linq.Expressions;

namespace PosTech.Contacts.ApplicationCore.Repositories
{
    public interface IAddContactRepository
    {
        Task AddContactAsync(Contact contact);
        Task DeleteContactAsync(Guid id);
        Task<IEnumerable<Contact>> FindContactsAsync(Expression<Func<Contact, bool>> predicate);
        Task<Contact> GetByIdAsync(Guid id);
        Task UpdateContactAsync(Contact contact);
    }
}
