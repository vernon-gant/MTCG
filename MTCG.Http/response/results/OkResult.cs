namespace MCTG;

public class OkResult : StatusCodeResult
{

    public OkResult(object? value) : base(value) { }

    protected override int StatusCode => StatusCodes.Status200OK;

    protected override string StatusMessage => "OK";

    protected override void ExecuteSpecificResult(HttpContext context) { }

}