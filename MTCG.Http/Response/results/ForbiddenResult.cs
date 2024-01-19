namespace MCTG;

public class ForbiddenResult : StatusCodeResult
{

    public ForbiddenResult(string? message) : base(message) { }

    public ForbiddenResult(object? value) : base(value) { }

    protected override int StatusCode => StatusCodes.Status403Forbidden;

    protected override string StatusMessage => "Forbidden";

    protected override void ExecuteSpecificResult(HttpContext context) { }

}