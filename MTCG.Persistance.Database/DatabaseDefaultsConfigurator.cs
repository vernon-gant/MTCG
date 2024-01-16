using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace MTCG.Persistance.Database;

public static class DatabaseDefaultsConfigurator
{

    public static IHostBuilder ConfigureDatabaseDefaults(this IHostBuilder hostBuilder)
    {
        hostBuilder.ConfigureServices((context, services) =>
        {
            DatabaseConfig databaseConfig = new () { ConnectionString = context.Configuration.GetConnectionString("PostgresConnection")! };
            services.AddSingleton(databaseConfig);
        });

        return hostBuilder;
    }

}