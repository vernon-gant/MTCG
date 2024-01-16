namespace MCTG;

public class MethodNotAllowedResult : StatusCodeResult
{
    public MethodNotAllowedResult() : base() { }

    protected override int StatusCode => StatusCodes.Status405MethodNotAllowed;

    protected override string StatusMessage => "Method Not Allowed";

    protected override void ExecuteSpecificResult(HttpContext context) { }

}