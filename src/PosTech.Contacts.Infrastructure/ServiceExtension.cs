using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PosTech.Contacts.ApplicationCore.Repositories;
using PosTech.Contacts.ApplicationCore.Services;
using PosTech.Contacts.Infrastructure.Repositories;
using PosTech.Contacts.Infrastructure.Services;

namespace PosTech.Contacts.Infrastructure
{
    public static class ServiceExtension
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(
                    configuration.GetConnectionString("SqlServerConnection"),
                    x =>
                    {
                        x.MigrationsAssembly("PosTech.Contacts.Infrastructure");
                        x.EnableRetryOnFailure();
                    });
            });

            services.AddScoped<IContactRepository, ContactRepository>();
            services.AddScoped<IDddRepository, DddRepository>();
            services.AddSingleton<ICacheService, CacheService>();
            services.AddDistributedMemoryCache();

            return services;
        }

        public static IConfigurationBuilder AddJsonFileByName(this IConfigurationBuilder configurationBuilder, string jsonFileName)
        {
            var currentDirectory = AppContext.BaseDirectory;
            var filePath = Path.Combine(currentDirectory, $"{jsonFileName}.json");

            return configurationBuilder.AddJsonFile(filePath, optional: false, reloadOnChange: true);
        }
    }
}
