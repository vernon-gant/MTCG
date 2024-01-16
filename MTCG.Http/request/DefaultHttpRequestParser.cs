using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;

namespace MCTG;

public class DefaultHttpRequestParser : HttpRequestParser
{

    public HttpRequest ParseRequest(string request)
    {
        string[] lines = request.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
        string[] requestLine = lines.First().Split(' ');
        string method = requestLine[0];
        string path = requestLine[1];

        Dictionary<string, string> headers = lines.Skip(1)
                                                  .TakeWhile(line => line != string.Empty)
                                                  .Select(headerLine => headerLine.Split(new[] { ':' }, 2))
                                                  .ToDictionary(parts => parts[0].Trim(), parts => parts.Length > 1 ? parts[1].Trim() : string.Empty);

        string? rawToken = headers.ContainsKey("Authorization") ? headers["Authorization"].Substring("Bearer ".Length) : null;
        JwtSecurityToken? jwtToken = rawToken != null ? new JwtSecurityTokenHandler().ReadJwtToken(rawToken) : null;

        int bodyLinesIndex = lines.ToList().FindIndex(line => line == string.Empty) + 1;
        JsonElement? body = bodyLinesIndex > 0 && bodyLinesIndex < lines.Length ? ParseBody(lines, bodyLinesIndex) : null;
        return new HttpRequest(path, jwtToken, body, method);
    }

    private JsonElement? ParseBody(string[] lines, int bodyLinesIndex)
    {
        JsonElement? body = null;
        string bodyContent = string.Join("\n", lines.Skip(bodyLinesIndex));
        try
        {
           body = JsonSerializer.Deserialize<JsonElement>(bodyContent);
        } catch (JsonException) {}
        return body;
    }

}