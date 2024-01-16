namespace MCTG;

public class CreatedResult : StatusCodeResult
{

    private readonly string _location;

    protected override int StatusCode => StatusCodes.Status201Created;

    protected override string StatusMessage => "Created";


    public CreatedResult(string location, object? value) : base(value)
    {
        _location = location;
    }

    protected override void ExecuteSpecificResult(HttpContext context)
    {
        context.Response.Location = _location;
    }

}