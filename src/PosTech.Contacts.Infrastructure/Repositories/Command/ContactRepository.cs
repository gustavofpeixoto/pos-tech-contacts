using Microsoft.EntityFrameworkCore;
using PosTech.Contacts.ApplicationCore.Entities;
using PosTech.Contacts.ApplicationCore.Repositories.Command;
using System.Linq.Expressions;

namespace PosTech.Contacts.Infrastructure.Repositories.Command
{
    public class ContactRepository(ApplicationDbContext dbContext) : IContactRepository
    {
        private readonly ApplicationDbContext _dbContext = dbContext;

        public async Task AddContactAsync(Contact contact)
        {
            await _dbContext.Contacts.AddAsync(contact);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteContactAsync(Guid id)
        {
            _dbContext.Contacts.Remove(new Contact(id));
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Contact>> FindContactsAsync(Expression<Func<Contact, bool>> predicate)
        {
            var result = await _dbContext.Contacts.AsNoTracking().Include(x => x.Ddd).Where(predicate).ToListAsync();

            return result;
        }

        public async Task<Contact> GetByIdAsync(Guid id)
        {
            var result = await _dbContext.Contacts
                .Include(contact => contact.Ddd)
                .ThenInclude(ddd => ddd.Region)
                .FirstOrDefaultAsync(x => x.Id == id);

            return result;
        }

        public async Task UpdateContactAsync(Contact contact)
        {
            _dbContext.Update(contact);
            await _dbContext.SaveChangesAsync();
        }
    }
}
