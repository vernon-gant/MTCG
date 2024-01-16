namespace MCTG.middleware;

public interface MiddlewarePipeline
{

    Task ExecutePipeline(HttpContext context);

}