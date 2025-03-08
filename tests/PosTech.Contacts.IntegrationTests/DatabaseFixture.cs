using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PosTech.Contacts.Infrastructure;

namespace PosTech.Contacts.IntegrationTests
{
    public class DatabaseFixture : IAsyncLifetime
    {
        public ApplicationDbContext Context { get; private set; }

        public DatabaseFixture()
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.Test.json")
                .Build();
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlServer(configuration.GetConnectionString("SqlServerConnection"))
                .Options;

            Context = new ApplicationDbContext(options);
        }

        public async Task DisposeAsync()
        {
            await Context.Database.EnsureDeletedAsync();
            await Context.DisposeAsync();
        }

        public async Task InitializeAsync()
        {
            await Context.Database.EnsureDeletedAsync();
            await Context.Database.MigrateAsync();
        }
    }
}
