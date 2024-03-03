#region usings

using KrepParser.Application.Services.Implementations;
using KrepParser.Application.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

#endregion

namespace KrepParser.Application
{
    public static class ApplicationServiceRegistration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddMediatR(configuration => configuration.RegisterServicesFromAssemblies(AssemblyRef.Assembly));
            services.AddScoped<IProductAppService, ProductAppService>();
            services.AddScoped<IWorkWithText, WorkWithText>();
            return services;
        }
    }
}
