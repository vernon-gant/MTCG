namespace MCTG;

public class InternalServerErrorResult : StatusCodeResult
{
    public InternalServerErrorResult() : base() { }

    protected override int StatusCode => StatusCodes.Status500InternalServerError;

    protected override string StatusMessage => "Internal Server Error";

    protected override void ExecuteSpecificResult(HttpContext context) { }

}