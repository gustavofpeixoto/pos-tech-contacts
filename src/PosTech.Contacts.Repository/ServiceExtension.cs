using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PosTech.Contacts.ApplicationCore.Repositories;
using PosTech.Contacts.Infrastructure.Repositories;

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

            return services;
        }
    }
}
