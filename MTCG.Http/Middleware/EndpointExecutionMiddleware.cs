using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text.Encodings.Web;
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

        if (actionResult.Value is string message) context.Response.Body = message;

        else if (actionResult.Value is not null) context.Response.Body = JsonSerializer.Serialize(actionResult.Value);
    }

    private bool NeedsValidation(Type type) => !type.IsPrimitive && type != typeof(string);

    private void ValidateModel(object model)
    {
        var validationContext = new ValidationContext(model, serviceProvider: null, items: null);

        var validationResults = new List<ValidationResult>();

        bool isValid = Validator.TryValidateObject(model, validationContext, validationResults, true);

        if (isValid) return;

        List<string?> mappedErrors = validationResults.Select(validationResult => validationResult.ErrorMessage).Select(error => error?.Replace("'", "")).ToList();

        string serializedResults = JsonSerializer.Serialize(mappedErrors);

        throw new ShortCircuitException(new BadRequestResult(serializedResults));
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

            if (NeedsValidation(parameterInfo.ParameterType)) ValidateModel(parameters[parameterInfo.Position]);
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