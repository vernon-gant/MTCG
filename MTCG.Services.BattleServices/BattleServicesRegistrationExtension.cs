using Microsoft.Extensions.DependencyInjection;

using MTCG.Services.BattleServices.Battle;
using MTCG.Services.BattleServices.Battle.Concrete;
using MTCG.Services.BattleServices.Services;
using MTCG.Services.BattleServices.Services.Concrete;

namespace MTCG.Services.BattleServices;

public static class BattleServicesRegistrationExtension
{

    public static IServiceCollection RegisterBattleServices(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(BattleServicesRegistrationExtension));
        services.AddSingleton<BattleService, DefaultBattleService>();
        services.AddSingleton<BattleEngine, DefaultBattleEngine>();
        services.AddSingleton<BattleArena>();
        services.AddSingleton<BattleLobby>();
        services.AddSingleton<BattleResultsStorage>();

        return services;
    }

}