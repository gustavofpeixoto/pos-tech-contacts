﻿using Microsoft.EntityFrameworkCore;
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
                    configuration["SQLSERVER_CONNECTION_STRING"],
                    x =>
                    {
                        x.MigrationsAssembly("PosTech.Contacts.Infrastructure");
                        x.EnableRetryOnFailure();
                    });
            });

            services.AddScoped<IContactRepository, ContactRepository>();
            services.AddScoped<IDddRepository, DddRepository>();
            services.AddSingleton<ICacheService, CacheService>();
            services.AddSingleton<RabbitMqConnectionManager>();
            services.AddSingleton<ApplicationCore.Repositories.Query.IContactRepository, Repositories.Query.ContactRepository>(sp =>
            {
                var mongoDbConnectionString = configuration["MONGODB_CONNECTION_STRING"];
                var mongoDbDatabaseName = configuration["MONGODB_DATABASE_NAME"];
                var mongoDbSettings = new MongoDbSettings
                {
                    ConnectionString = mongoDbConnectionString,
                    DatabaseName = mongoDbDatabaseName
                };
                return new Repositories.Query.ContactRepository(mongoDbSettings);
            });
            services.AddSingleton<IMessagingProducer, RabbitMqMessagingProducer>();
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
