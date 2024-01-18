using System.Text;

namespace MCTG;

public class HttpResponse
{

    public int StatusCode { get; set; }

    public string StatusMessage { get; set; } = "";

    public string? ContentType { get; set; }

    public int ContentLength { get; set; }

    public DateTimeOffset? Date { get; set; }

    public string? Location { get; set; }

    private const string Server = "MCTG/1.0";

    public string? Body { get; set; }

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("HTTP/1.1 ");
        sb.Append(StatusCode);
        sb.Append(" ");
        sb.Append(StatusMessage);
        sb.Append("\r\n");
        sb.Append("Content-Length: ");
        sb.Append(ContentLength);
        sb.Append("\r\n");

        if (Body != null)
        {
            sb.Append("Content-Type: ");
            sb.Append(ContentType);
            sb.Append("\r\n");
        }

        sb.Append("Date: ");
        sb.Append(Date);
        sb.Append("\r\n");
        sb.Append("Server: ");
        sb.Append(Server);
        sb.Append("\r\n");

        if (Location != null)
        {
            sb.Append("Location: ");
            sb.Append(Location);
            sb.Append("\r\n");
        }

        if (Body != null)
        {
            sb.Append("\r\n");
            sb.Append(Body);
        }

        return sb.ToString();
    }

}