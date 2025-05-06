using Microsoft.EntityFrameworkCore;
using PosTech.Contacts.ApplicationCore.Entities;
using PosTech.Contacts.ApplicationCore.Repositories;

namespace PosTech.Contacts.Infrastructure.Repositories
{
    public class DddRepository(ApplicationDbContext dbContext) : IDddRepository
    {
        private readonly ApplicationDbContext _dbContext = dbContext;

        public async Task<Ddd> GetByDddCodeAsync(int dddCode) 
            => await _dbContext.Ddds.Include(ddd => ddd.Region).FirstOrDefaultAsync(x => x.DddCode == dddCode);
    }
}
