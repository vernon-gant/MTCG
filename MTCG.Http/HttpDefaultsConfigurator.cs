using System.Reflection;

using MCTG;
using MCTG.middleware;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace MTCG.API;

public static class HttpDefaultsConfigurator
{

    public static IHostBuilder ConfigureHttpDefaults(this IHostBuilder hostBuilder)
    {
        hostBuilder.ConfigureAppConfiguration((_, config) =>
        {
            config.AddUserSecrets(Assembly.GetExecutingAssembly());
        });

        return hostBuilder.ConfigureServices((context, services) =>
        {
            UserSecrets userSecrets = context.Configuration.GetSection("UserSecrets").Get<UserSecrets>()!;
            services.AddSingleton(userSecrets);

            services.AddHostedService<TCPListener>();
            services.AddSingleton<HttpRequestParser, DefaultHttpRequestParser>();
            services.AddSingleton<MiddlewarePipeline, MonsterTradingCardGamePipeline>();
            services.AddSingleton<RequestProcessor, HttpRequestProcessor>();

            services.AddSingleton<Middleware>(provider => new LoggingMiddleware(
                                                  new RoutingMiddleware(
                                                      new AuthorizationMiddleware(
                                                          new EndpointExecutionMiddleware(null, provider), provider.GetRequiredService<UserSecrets>()
                                                      )
                                                  ), provider.GetRequiredService<ILogger<LoggingMiddleware>>()
                                              ));
        });
    }

}