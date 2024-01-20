using Microsoft.Extensions.DependencyInjection;

using MTCG.Services.Cards.Services;
using MTCG.Services.Cards.Services.Concrete;

namespace MTCG.Services.UserService;

public static class CardServicesRegistrationExtension
{

    public static IServiceCollection RegisterCardServices(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(CardServicesRegistrationExtension));
        services.AddSingleton<CardsService, DefaultCardsService>();
        services.AddSingleton<CardMapperService, DefaultCardMapperService>();

        return services;
    }

}