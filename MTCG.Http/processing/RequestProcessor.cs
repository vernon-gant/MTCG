namespace MTCG.API;

public interface RequestProcessor
{

    ValueTask<string> ProcessRequest(string rawRequest, CancellationToken cancellationToken);

}