using MCTG;
using MCTG.middleware;

namespace MTCG.API;

public class HttpRequestProcessor : RequestProcessor
{

    private readonly HttpRequestParser _parser;

    private readonly MiddlewarePipeline _pipeline;


    public HttpRequestProcessor(HttpRequestParser parser, MiddlewarePipeline pipeline)
    {
        _parser = parser;
        _pipeline = pipeline;
    }

    public async ValueTask<string> ProcessRequest(string rawRequest, CancellationToken cancellationToken)
    {
        HttpRequest parsedRequest = _parser.ParseRequest(rawRequest);
        HttpContext context = new(parsedRequest, new HttpResponse());
        await _pipeline.ExecutePipeline(context);
        return context.Response.ToString();
    }

}