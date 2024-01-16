using System.Reflection;

using MCTG.middleware;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using MTCG.API;
using MTCG.API.Controllers;
using MTCG.Persistance.Database;
using MTCG.Persistance.Repositories;
using MTCG.Services.UserService;

IHost host = Host.CreateDefaultBuilder(args)
                 .ConfigureHostConfiguration(config =>
                 {
                     config.AddJsonFile("appsettings.json");
                     config.AddUserSecrets(Assembly.GetExecutingAssembly());
                 })
                 .ConfigureHttpDefaults()
                 .ConfigureRepositoriesDefaults()
                 .ConfigureDatabaseDefaults()
                 .ConfigureServices((_, services) =>
                 {
                     services.RegisterUserServices();
                     services.RegisterCardServices();
                     services.RegisterPackageServices();
                     services.AddSingleton<UserController>();
                     services.AddSingleton<CardsController>();
                     services.AddSingleton<AuthenticatedUserController>();
                     services.AddSingleton<AdminController>();
                 })
                 .Build();

await host.RunAsync();