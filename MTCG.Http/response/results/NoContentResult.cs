namespace MCTG;

public class NoContentResult : StatusCodeResult
{

    public NoContentResult() : base() { }

    protected override int StatusCode => StatusCodes.Status204NoContent;

    protected override string StatusMessage => "No Content";

    protected override void ExecuteSpecificResult(HttpContext context) { }

}