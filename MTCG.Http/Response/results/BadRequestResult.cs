namespace MCTG;

public class BadRequestResult : StatusCodeResult
{

    public BadRequestResult(List<string?> messages) : base(messages) { }

    public BadRequestResult(string? message) : base(message) { }

    public BadRequestResult(object? value) : base(value) { }

    protected override int StatusCode => StatusCodes.Status400BadRequest;

    protected override string StatusMessage => "Bad Request";

    protected override void ExecuteSpecificResult(HttpContext context) { }

}