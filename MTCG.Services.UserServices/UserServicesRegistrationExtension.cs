using Microsoft.Extensions.DependencyInjection;

using MTCG.Services.UserService.Services;
using MTCG.Services.UserService.Services.concrete;

namespace MTCG.Services.UserService;

public static class UserServicesRegistrationExtension
{

    public static IServiceCollection RegisterUserServices(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(UserServicesRegistrationExtension));
        services.AddSingleton<UserService, DefaultUserService>();
        services.AddSingleton<SecurityService, DefaultSecurityService>();
        services.AddSingleton<UserProfileService, DefaultUserProfileService>();
        return services;
    }

}