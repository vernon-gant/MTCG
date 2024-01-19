using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using MTCG.Persistence.Repositories.Cards;
using MTCG.Persistence.Repositories.Cards.Concrete;
using MTCG.Persistence.Repositories.Cards.Mappings;
using MTCG.Persistence.Repositories.Decks;
using MTCG.Persistence.Repositories.Decks.Concrete;
using MTCG.Persistence.Repositories.Packages;
using MTCG.Persistence.Repositories.Packages.Concrete;
using MTCG.Persistence.Repositories.Users;
using MTCG.Persistence.Repositories.Users.Concrete;

namespace MTCG.Persistence.Repositories;

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
            services.AddSingleton<DeckRepository, DefaultDeckRepository>();
        });

        return hostBuilder;
    }

}