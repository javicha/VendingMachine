using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Vending.Application.Contracts.Persistence;
using Vending.Infrastructure.Persistence;
using Vending.Infrastructure.Repositories;

namespace Vending.Infrastructure
{
    /// <summary>
    /// Class that centralizes dependency injection management for the Infrastrucutre layer. Single Responsability
    /// </summary>
    public static class InfrastructureServiceRegistration
    {
        /// <summary>
        /// Extension method to register all injection dependencies
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<VendingContext>(options =>
                options.UseInMemoryDatabase(databaseName: "Vending_Database"));

            // Add MassTransit and configure it with RabbitMQ connection
            services.AddMassTransit(config => {
                config.UsingRabbitMq((ctx, cfg) => {
                    cfg.Host(configuration["EventBusSettings:HostAddress"]);
                });
            });

            services.AddScoped(typeof(IAsyncRepository<>), typeof(RepositoryBase<>)); //Per-request lifecycle
            services.AddScoped<IProductRepository, ProductRepository>(); //Per-request lifecycle
            services.AddScoped<IVendingMachineRepository, VendingMachineRepository>(); //Per-request lifecycle

            return services;
        }
    }
}
