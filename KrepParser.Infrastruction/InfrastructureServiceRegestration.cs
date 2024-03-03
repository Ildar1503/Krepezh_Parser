using KrepParser.Application.Shared;
using KrepParser.Domain.Repositories;
using KrepParser.Infrastruction.DataBase;
using KrepParser.Infrastruction.Repositories;
using KrepParser.Infrastruction.Services.Implementations;
using KrepParser.Infrastruction.Services.Primitives;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace KrepParser.Infrastruction
{
    public static class InfrastructureServiceRegestration
    {
        //Делегат для получения реализации сервиса.
        public delegate IParseService ParseServiceResolver(string shop);

        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            //Контекст базы данных.
            services.AddDbContext<ProductDbContext>(options =>
            {
                options.UseSqlServer("Server=DESKTOP-S059N9O; Database=Krepezh; Trusted_Connection=True;TrustServerCertificate=True;");
            });

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IProductRepository, ProductRepository>();

            //Сервисы парсинга.
            services.AddScoped<LevshaParseService>();
            services.AddScoped<StroiSnabParseService>();
            services.AddScoped<SaturnParseService>();

            //Фабрика.
            services.AddScoped<ParseServiceResolver>(provider => shop =>
            {
                switch (shop)
                {
                    case "levsha":
                        return provider.GetService<LevshaParseService>()!;
                    case "stroi_snab":
                        return provider.GetService<StroiSnabParseService>()!;
                    case "saturn":
                        return provider.GetService<SaturnParseService>()!;
                    default:
                        throw new KeyNotFoundException($"Не удалось получить данные из {shop}");
                }
            });

            return services;
        }
    }
}
