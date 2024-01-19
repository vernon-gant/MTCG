using Microsoft.Extensions.DependencyInjection;

using MTCG.Services.TradingServices.Services;
using MTCG.Services.TradingServices.Services.Concrete;

namespace MTCG.Services.TradingServices;

public static class TradingServicesRegistrationExtension
{

    public static IServiceCollection RegisterTradingServices(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(TradingServicesRegistrationExtension));
        services.AddScoped<TradingService, DefaultTradingService>();
        return services;
    }

}