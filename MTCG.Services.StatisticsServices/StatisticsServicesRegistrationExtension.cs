using Microsoft.Extensions.DependencyInjection;

using MTCG.Services.StatisticsServices.Services;
using MTCG.Services.StatisticsServices.Services.Concrete;

namespace MTCG.Services.StatisticsServices;

public static class StatisticsServicesRegistrationExtension
{

    public static IServiceCollection RegisterStatisticsServices(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(StatisticsServicesRegistrationExtension));
        services.AddSingleton<StatisticsService, DefaultStatisticsService>();

        return services;
    }

}