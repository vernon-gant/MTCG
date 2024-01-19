using System.Reflection;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using MTCG.API;
using MTCG.API.Controllers;
using MTCG.Persistance.Database;
using MTCG.Persistence.Repositories;
using MTCG.Services.DeckServices;
using MTCG.Services.StatisticsServices;
using MTCG.Services.TradingServices;
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
                     services.RegisterDeckServices();
                     services.RegisterTradingServices();
                     services.RegisterStatisticsServices();
                     services.AddSingleton<UserController>();
                     services.AddSingleton<CardController>();
                     services.AddSingleton<AuthenticatedUserController>();
                     services.AddSingleton<AdminController>();
                     services.AddSingleton<PackageController>();
                     services.AddSingleton<DeckController>();
                     services.AddSingleton<TradingController>();
                     services.AddSingleton<StatisticsController>();
                 })
                 .Build();

await host.RunAsync();