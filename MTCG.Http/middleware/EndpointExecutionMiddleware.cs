using System.Reflection;
using System.Text.Json;

using MTCG.API.attributes;

namespace MCTG.middleware;

public class EndpointExecutionMiddleware : Middleware
{

    private readonly IServiceProvider _serviceProvider;

    public EndpointExecutionMiddleware(Middleware? next, IServiceProvider serviceProvider) : base(next)
    {
        _serviceProvider = serviceProvider;
    }

    protected override async Task Handle(HttpContext context)
    {
        object? controller = _serviceProvider.GetService(context.Controller);

        object[]? parameters = BindModels(context);

        ValueTask<ActionResult> actionResultTask = (ValueTask<ActionResult>)context.Endpoint.Invoke(controller, parameters)!;

        await actionResultTask;

        ActionResult actionResult = actionResultTask.Result;

        await actionResultTask.Result.ExecuteResult(context);

        if (actionResult.Value == null) return;

        context.Response.Body = actionResult.Value is string ? actionResult.Value.ToString() : JsonSerializer.Serialize(actionResult.Value);
    }

    private object[]? BindModels(HttpContext context)
    {
        if (context.Endpoint.GetParameters().Length == 0) return null;

        object[] parameters = new object[context.Endpoint.GetParameters().Length];

        foreach (var parameterInfo in context.Endpoint.GetParameters())
        {
            if (parameterInfo.ParameterType == typeof(HttpContext)) parameters[parameterInfo.Position] = context;
            else if (IsBoundFromRoute(parameterInfo)) parameters[parameterInfo.Position] = BindFromRoute(parameterInfo, context);
            else parameters[parameterInfo.Position] = BindFromBody(parameterInfo, context);
        }

        return parameters;
    }

    private bool IsBoundFromRoute(ParameterInfo parameterInfo) => parameterInfo.GetCustomAttribute<FromRouteAttribute>() != null;

    private object BindFromRoute(ParameterInfo parameterInfo, HttpContext context)
    {
        object value = context.RouteParameters[parameterInfo.Name!.ToLower()];

        if (int.TryParse(value.ToString(), out int intValue)) return intValue;

        return value;
    }

    private object BindFromBody(ParameterInfo parameterInfo, HttpContext context)
    {
        try
        {
            return context.Request.Body!.Value.Deserialize(parameterInfo.ParameterType, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;
        }
        catch (Exception)
        {
            throw new ShortCircuitException(new BadRequestResult("Invalid request body format, see documentation for more information"));
        }
    }

}