using Microsoft.Extensions.DependencyInjection;

using MTCG.Services.Cards.cards;

namespace MTCG.Services.UserService;

public static class CardServiceRegistrationExtension
{

    public static IServiceCollection RegisterCardServices(this IServiceCollection services)
    {
        services.AddSingleton<CardsService, DefaultCardsService>();
        return services;
    }

}