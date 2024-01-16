using System.Reflection;

namespace MCTG;

public class HttpContext
{

    public HttpRequest Request { get; }

    public HttpResponse Response { get; }

    public string? UserName { get; set; }

    public Type Controller { get; set; }

    public MethodInfo Endpoint { get; set; }

    public bool IsAdmin { get; set; }

    public Dictionary<string,string> RouteParameters { get; set; } = new();

    public HttpContext(HttpRequest request, HttpResponse response)
    {
        Request = request;
        Response = response;
    }

}