namespace MCTG;

public interface ActionResult
{

    Task ExecuteResult(HttpContext context);

    object? Value { get; }

}