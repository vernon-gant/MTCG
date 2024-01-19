using Microsoft.Extensions.DependencyInjection;

using MTCG.Services.DeckServices.Services;
using MTCG.Services.DeckServices.Services.Concrete;

namespace MTCG.Services.DeckServices;

public static class DeckServicesRegistrationExtension
{

    public static IServiceCollection RegisterDeckServices(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(DeckServicesRegistrationExtension));
        services.AddSingleton<DeckService, DefaultDeckService>();
        return services;
    }

}