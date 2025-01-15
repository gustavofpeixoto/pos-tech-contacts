using Microsoft.EntityFrameworkCore;
using PosTech.Contacts.ApplicationCore.Entities;
using PosTech.Contacts.ApplicationCore.Repositories;

namespace PosTech.Contacts.Infrastructure.Repositories
{
    public class DddRepository(ApplicationDbContext dbContext) : IDddRepository
    {
        private readonly ApplicationDbContext _dbContext = dbContext;

        public async Task<Ddd> GetByDddCodeAsync(int dddCode)
        {
            return await _dbContext.Ddds.FirstOrDefaultAsync(x => x.DddCode == dddCode);
        }
    }
}
