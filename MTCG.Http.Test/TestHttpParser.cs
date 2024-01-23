using MCTG;

namespace MTCG.Http.Test;

[TestClass]
public class TestHttpParser
{

    private readonly DefaultHttpRequestParser _parser = new();

    [TestMethod]
    public void Parser_ParseGetRequestWithouthBody()
    {
        string rawRequest = "GET /messages HTTP/1.1\r\n" +
                            "Host: localhost:10001\r\n" +
                            "User-Agent: insomnia/2021.4.1\r\n" +
                            "Content-Type: application/json\r\n" +
                            "Accept: */*\r\n" +
                            "Content-Length: 0\r\n\r\n";
        HttpRequest request = _parser.ParseRequest(rawRequest);
        Assert.AreEqual("GET", request.Method);
        Assert.AreEqual("/messages", request.Path);
        Assert.IsNull(request.Token);
        Assert.IsNull(request.Body);
    }

    [TestMethod]
    public void Parser_ParseGetRequestWithBody()
    {
        string rawRequest = "GET /messages HTTP/1.1\r\n" +
                            "Host: localhost:10001\r\n" +
                            "User-Agent: insomnia/2021.4.1\r\n" +
                            "Content-Type: application/json\r\n" +
                            "Accept: */*\r\n" +
                            "Content-Length: 0\r\n\r\n" +
                            "{\"test\": \"test\"}";
        HttpRequest request = _parser.ParseRequest(rawRequest);
        Assert.AreEqual("GET", request.Method);
        Assert.AreEqual("/messages", request.Path);
        Assert.IsNull(request.Token);
        Assert.IsNotNull(request.Body);
        Assert.AreEqual("test", request.Body.Value.GetProperty("test").GetString());
    }

    [TestMethod]
    public void Parser_ParsePostRequestWithBodyAndWithToken()
    {
        string rawRequest = "POST /messages HTTP/1.1\r\n" +
                            "Host: localhost:10001\r\n" +
                            "User-Agent: insomnia/2021.4.1\r\n" +
                            "Content-Type: application/json\r\n" +
                            "Accept: */*\r\n" +
                            "Content-Length: 0\r\n" +
                            "Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpZCI6MTIzNDU2Nzg5LCJuYW1lIjoiSm9zZXBoIn0.OpOSSw7e485LOP5PrzScxHb7SR6sAOMRckfFwi4rp7o\r\n\r\n" +
                            "{\"test\": \"test\"}";
        HttpRequest request = _parser.ParseRequest(rawRequest);
        Assert.AreEqual("POST", request.Method);
        Assert.AreEqual("/messages", request.Path);
        Assert.IsNotNull(request.Token);
        Assert.IsNotNull(request.Body);
        Assert.AreEqual("test", request.Body.Value.GetProperty("test").GetString());
    }


}