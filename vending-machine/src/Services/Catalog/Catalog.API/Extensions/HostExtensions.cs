using Vending.Infrastructure.Persistence;

namespace Vending.API.Extensions
{
    public static class HostExtensions
    {
        /// <summary>
        /// Extension method that populates the database with test entities on application startup
        /// </summary>
        /// <typeparam name="TContext"></typeparam>
        /// <param name="host"></param>
        /// <param name="seeder"></param>
        /// <returns></returns>
        public static IHost PopulateDatabase<TContext>(this IHost host, Action<TContext, IServiceProvider> seeder) where TContext : VendingContext
        {
            //1. Find the service layer within our scope.
            using (var scope = host.Services.CreateScope())
            {
                //2. Get the instance of InventoryContext in our services layer
                var services = scope.ServiceProvider;
                var logger = services.GetRequiredService<ILogger<TContext>>();
                var context = services.GetService<TContext>();

                try
                {
                    logger.LogInformation("Populating database associated with context {DbContextName}", typeof(TContext).Name);

                    InvokeSeeder(seeder, context, services);

                    logger.LogInformation("Migrated database associated with context {DbContextName}", typeof(TContext).Name);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occurred while populating the database used on context {DbContextName}", typeof(TContext).Name);

                    //TODO A good practice would be to implement resilience mechanisms, such as retries, using for example the Polly library
                }
            }

            return host;
        }

        private static void InvokeSeeder<TContext>(Action<TContext, IServiceProvider> seeder, TContext context, IServiceProvider services) where TContext : VendingContext
        {
            seeder(context, services);
        }
    }
}
