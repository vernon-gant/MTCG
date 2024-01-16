namespace MCTG;

public interface HttpRequestParser
{

    HttpRequest ParseRequest(string request);

}