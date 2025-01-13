using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using PosTech.Contacts.ApplicationCore.Validators;

namespace PosTech.Contacts.ApplicationCore
{
    public static class ServiceExtension
    {
        public static IServiceCollection AddApplicationCoreServices(this IServiceCollection services) 
        {
            services.AddValidatorsFromAssemblyContaining<CreateContactCommandValidator>();

            return services;
        }
    }
}
