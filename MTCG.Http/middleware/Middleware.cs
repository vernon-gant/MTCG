namespace MCTG.middleware;

public abstract class Middleware
{

    private readonly Middleware? _next;

    protected Middleware(Middleware? next)
    {
        _next = next;
    }

    public async Task HandleRequest(HttpContext context)
    {
        await Handle(context);

        if (_next != null)
        {
            await _next.HandleRequest(context);
        }
    }

    protected abstract Task Handle(HttpContext context);

}