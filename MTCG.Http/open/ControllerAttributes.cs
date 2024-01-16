namespace MTCG.API.attributes;

[AttributeUsage(AttributeTargets.Class)]
public class ApiControllerAttribute : Attribute
{

    public string? RoutePrefix { get; set; }

}

[AttributeUsage(AttributeTargets.Method)]
public class RouteAttribute : Attribute
{

    public string Route { get; }

    public RouteAttribute(string route)
    {
        Route = route;
    }

}

public class HttpMethodAttribute : Attribute
{

}

public class GetAttribute : RouteAttribute
{

    public GetAttribute(string route) : base(route) { }

}

public class PostAttribute : RouteAttribute
{

    public PostAttribute(string route) : base(route) { }

}

public class PutAttribute : RouteAttribute
{

    public PutAttribute(string route) : base(route) { }

}

public class DeleteAttribute : RouteAttribute
{

    public DeleteAttribute(string route) : base(route) { }

}

public class AuthAttribute : Attribute
{

}

public class AdminAttribute : AuthAttribute
{

}

public class FromBodyAttribute : Attribute
{

}

public class FromRouteAttribute : Attribute
{

}