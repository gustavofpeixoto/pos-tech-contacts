using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PosTech.Contacts.ApplicationCore.Messaging;
using PosTech.Contacts.ApplicationCore.Repositories.Command;
using PosTech.Contacts.ApplicationCore.Services;
using PosTech.Contacts.Infrastructure.Messaging;
using PosTech.Contacts.Infrastructure.Repositories.Command;
using PosTech.Contacts.Infrastructure.Services;
using PosTech.Contacts.Infrastructure.Settings;

namespace PosTech.Contacts.Infrastructure
{
    public static class ExtensionMethods
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
            services.AddSingleton<ApplicationCore.Repositories.Query.IContactRepository, Repositories.Query.ContactRepository>(sp => 
            {
                var mongoDbSettings = configuration.GetSection("MongoDb").Get<MongoDbSettings>();
                return new Repositories.Query.ContactRepository(mongoDbSettings);
            });
            services.AddSingleton<IMessagingProducer>(serviceProvider =>
            {
                var rabbitMqSettings = configuration.GetSection("RabbitMq").Get<RabbitMqSettings>();
                return new RabbitMqMessagingProducer(rabbitMqSettings);
            });

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
