namespace MCTG.middleware;

public class ShortCircuitException : Exception
{

    public ActionResult ActionResult { get; }

    public ShortCircuitException(ActionResult actionResult)
    {
        ActionResult = actionResult;
    }

}