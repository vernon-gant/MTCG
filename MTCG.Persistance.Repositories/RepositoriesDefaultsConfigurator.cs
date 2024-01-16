using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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
            List<CardMapping> cardMappings = new ();
            context.Configuration.GetSection("CardMappings").Bind(cardMappings);
            services.AddSingleton(cardMappings);

            List<ElementMapping> elementMappings = new ();
            context.Configuration.GetSection("ElementMappings").Bind(elementMappings);
            services.AddSingleton(elementMappings);

            services.AddSingleton<UserRepository, DefaultUserRepository>();
            services.AddSingleton<CardsRepository, DefaultCardRepository>();
        });

        return hostBuilder;
    }

}