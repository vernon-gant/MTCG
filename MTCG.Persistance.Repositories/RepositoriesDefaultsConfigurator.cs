using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using MTCG.Persistance.Repositories.Packages;
using MTCG.Persistance.Repositories.Packages.Concrete;
using MTCG.Persistance.Repositories.users;

namespace MTCG.Persistance.Repositories;

public static class RepositoriesDefaultsConfigurator
{

    public static IHostBuilder ConfigureRepositoriesDefaults(this IHostBuilder hostBuilder)
    {
        hostBuilder.ConfigureHostConfiguration(builder =>
        {
            builder.AddJsonFile("cardMappings.json");
            builder.AddJsonFile("elementMappings.json");
        });

        hostBuilder.ConfigureServices((context, services) =>
        {
            Dictionary<string, CardMapping> cardMappings = new ();
            context.Configuration.GetSection("CardMappings").Bind(cardMappings);
            services.AddSingleton(cardMappings);

            Dictionary<string, ElementMapping> elementMappings = new ();
            context.Configuration.GetSection("ElementMappings").Bind(elementMappings);
            services.AddSingleton(elementMappings);

            services.AddSingleton<UserRepository, DefaultUserRepository>();
            services.AddSingleton<CardsRepository, DefaultCardRepository>();
            services.AddSingleton<PackageRepository, DefaultPackageRepository>();
        });

        return hostBuilder;
    }

}