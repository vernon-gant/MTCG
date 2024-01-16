namespace MCTG;

public class UnauthorizedResult : StatusCodeResult
{
    public UnauthorizedResult(string? message) : base(message) { }

    protected override int StatusCode => StatusCodes.Status401Unauthorized;

    protected override string StatusMessage => "Unauthorized";

    protected override void ExecuteSpecificResult(HttpContext context) { }

}