using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;

namespace MCTG;

public class HttpRequest
{

    public HttpRequest(string path, JwtSecurityToken? token, JsonElement? body, string method)
    {
        Method = method;
        Path = path;
        Token = token;
        Body = body;
    }

    public string Method { get; }

    public string Path { get; }

    public JwtSecurityToken? Token { get; }

    public JsonElement? Body { get; }

    public override string ToString()
    {
        return $"{Method} {Path} HTTP/1.1";
    }

}