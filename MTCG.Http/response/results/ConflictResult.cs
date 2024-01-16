namespace MCTG;

public class ConflictResult : StatusCodeResult
{

    protected override int StatusCode => StatusCodes.Status409Conflict;

    protected override string StatusMessage => "Conflict";

    public ConflictResult(string? message) : base(message) { }

    protected override void ExecuteSpecificResult(HttpContext context) { }

}