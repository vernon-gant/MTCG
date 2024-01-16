using Microsoft.Extensions.Logging;

namespace MCTG.middleware;

public class LoggingMiddleware : Middleware
{

    private readonly ILogger<LoggingMiddleware> _logger;

    public LoggingMiddleware(Middleware? next, ILogger<LoggingMiddleware> logger) : base(next)
    {
        _logger = logger;
    }

    protected override Task Handle(HttpContext context)
    {
        _logger.LogInformation("Request: {0}", context.Request.ToString());
        return Task.CompletedTask;
    }

}