namespace MCTG;

public abstract class StatusCodeResult : ActionResult
{

    protected abstract int StatusCode { get; }

    protected abstract string StatusMessage { get; }

    public object? Value { get; }

    protected StatusCodeResult()
    {
        Value = default;
    }

    protected StatusCodeResult(object? value)
    {
        Value = value;
    }

    public Task ExecuteResult(HttpContext context)
    {
        context.Response.StatusCode = StatusCode;
        context.Response.StatusMessage = StatusMessage;
        ExecuteSpecificResult(context);

        return Task.CompletedTask;
    }

    protected abstract void ExecuteSpecificResult(HttpContext context);

}