using Microsoft.Extensions.Logging;

namespace MCTG.middleware;

public class MonsterTradingCardGamePipeline : MiddlewarePipeline
{

    private readonly Middleware _pipeline;

    private readonly ILogger<MonsterTradingCardGamePipeline> _logger;

    public MonsterTradingCardGamePipeline(Middleware pipeline, ILogger<MonsterTradingCardGamePipeline> logger)
    {
        _pipeline = pipeline;
        _logger = logger;
    }

    public async Task ExecutePipeline(HttpContext context)
    {
        try
        {
            await _pipeline.HandleRequest(context);
        }
        catch (ShortCircuitException exception)
        {
            await exception.ActionResult.ExecuteResult(context);
            context.Response.Body = exception.ActionResult.Value as string;
        }
        catch (Exception e)
        {
            context.Response.StatusCode = 500;
            context.Response.StatusMessage = "Internal Server Error";
            context.Response.Body = "An error occurred while processing the request.";
            _logger.LogError(e, "An error occurred while processing the request.");
        }
        finally
        {
            context.Response.ContentType = DefineContentType(context);
            context.Response.ContentLength = context.Response.Body?.Length ?? 0;
            context.Response.Date = DateTimeOffset.UtcNow;
        }
    }

    private string? DefineContentType(HttpContext context)
    {
        string? contentType = context.Response.ContentType;

        if (context.Response.Body == null) return null;

        // If it's already set then it is application/json set in EndpointExecutionMiddleware
        if (contentType != null) return contentType;

        return "text/plain";
    }

}