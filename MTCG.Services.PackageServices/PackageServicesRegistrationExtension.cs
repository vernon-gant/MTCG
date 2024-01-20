using Microsoft.Extensions.DependencyInjection;

using MTCG.Services.PackageServices.Services;
using MTCG.Services.PackageServices.Services.Concrete;

namespace MTCG.Services.UserService;

public static class PackageServicesRegistrationExtension
{

    public static IServiceCollection RegisterPackageServices(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(PackageServicesRegistrationExtension));
        services.AddScoped<PackageService, DefaultPackageService>();

        return services;
    }

}