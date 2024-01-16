using Microsoft.Extensions.DependencyInjection;

namespace MTCG.Services.UserService;

public static class UserServiceRegistrationExtension
{

    public static IServiceCollection RegisterUserServices(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(UserServiceRegistrationExtension));
        services.AddSingleton<UserService, DefaultUserService>();
        services.AddSingleton<SecurityService, DefaultSecurityService>();
        services.AddSingleton<AuthenticatedUserService, DefaultAuthenticatedUserService>();
        return services;
    }

}