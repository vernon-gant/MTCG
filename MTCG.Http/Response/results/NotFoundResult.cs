namespace MCTG;

public class NotFoundResult : StatusCodeResult
{

    public NotFoundResult(string? message) : base(message) { }

    protected override int StatusCode => StatusCodes.Status404NotFound;

    protected override string StatusMessage => "Not Found";

    protected override void ExecuteSpecificResult(HttpContext context) { }

}