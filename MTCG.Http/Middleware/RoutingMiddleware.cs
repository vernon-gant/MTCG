using System.Reflection;

using MTCG.API.attributes;

namespace MCTG.middleware;

public class RoutingMiddleware : Middleware
{

    public RoutingMiddleware(Middleware? next) : base(next) { }

    protected override Task Handle(HttpContext context)
    {
        var controllerTypes = AppDomain.CurrentDomain
                                       .GetAssemblies()
                                       .SelectMany(assembly => assembly.GetTypes()
                                                                       .Where(type => type.IsClass && !type.IsAbstract &&
                                                                                      type.GetCustomAttribute<ApiControllerAttribute>() != null));

        foreach (var controllerType in controllerTypes)
        {
            MethodInfo? method = controllerType
                                 .GetMethods(BindingFlags.Public | BindingFlags.Instance)
                                 .FirstOrDefault(method => IsRequestEndpoint(controllerType, method, context));

            if (method == null) continue;

            context.Controller = controllerType;
            context.Endpoint = method;
            context.RouteParameters = ExtractRouteParameters(controllerType, method, context);

            return Task.CompletedTask;
        }

        throw new ShortCircuitException(new NotFoundResult("No matching route found"));
    }

    private bool IsRequestEndpoint(Type controllerType, MethodInfo method, HttpContext context)
    {
        RouteAttribute? routeAttribute = method.GetCustomAttributes<RouteAttribute>().FirstOrDefault();

        if (routeAttribute == null) return false;

        if (!TemplateMatchesUrl(controllerType, routeAttribute, context)) return false;

        string routeAttributeMethod = routeAttribute.GetType().Name.Replace("Attribute", "").ToLower();
        string requestMethod = context.Request.Method.ToLower();

        return routeAttributeMethod == requestMethod;
    }

    private Dictionary<string, string> ExtractRouteParameters(Type controllerType, MethodInfo method, HttpContext context)
    {
        Dictionary<string, string> routeParameters = new ();

        string route = controllerType.GetCustomAttribute<ApiControllerAttribute>()?.RoutePrefix + method.GetCustomAttribute<RouteAttribute>()?.Route;

        string[] routeParts = route.Split('/');

        string[] urlParts = context.Request.Path.Split('/');

        for (int i = 0; i < routeParts.Length; i++)
        {
            if (routeParts[i].StartsWith('{') && routeParts[i].EndsWith('}'))
            {
                routeParameters.Add(routeParts[i].Substring(1, routeParts[i].Length - 2), urlParts[i]);
            }
        }

        return routeParameters;
    }

    private bool TemplateMatchesUrl(Type controllerType, RouteAttribute routeAttribute, HttpContext context)
    {
        string route = controllerType.GetCustomAttribute<ApiControllerAttribute>()?.RoutePrefix + routeAttribute.Route;

        string[] routeParts = route.Split('/');

        string[] urlParts = context.Request.Path.Split('/');

        if (routeParts.Length != urlParts.Length) return false;

        for (int i = 0; i < routeParts.Length; i++)
        {
            if (routeParts[i].StartsWith('{') && routeParts[i].EndsWith('}')) continue;

            if (routeParts[i] != urlParts[i]) return false;
        }

        return true;
    }

}