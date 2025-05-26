using PosTech.Contacts.ApplicationCore.Entities.Query;
using System.Linq.Expressions;

namespace PosTech.Contacts.ApplicationCore.Repositories.Query
{
    public interface IContactRepository
    {
        Task AddAsync(Contact contact);
        Task<Contact> GetAsync(Guid id);
        Task UpdateAsync(Contact contact);
        Task DeleteAsync(Guid id);
        Task<List<Contact>> FindContactsAsync(Expression<Func<Contact, bool>> predicate);
        /// <summary>
        /// Versão com suporte a paginação.
        /// </summary>
        /// <param name="predicate">Função utilizada para busca</param>
        /// <param name="pageNumber">Número da página</param>
        /// <param name="pageSize">Quantidade de itens por página</param>
        /// <returns></returns>
        Task<List<Contact>> FindContactsAsync(Expression<Func<Contact, bool>> predicate, int pageNumber, int pageSize);
    }
}
