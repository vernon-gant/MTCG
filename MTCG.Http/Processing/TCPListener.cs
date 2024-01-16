using System.Net;
using System.Net.Sockets;
using System.Text;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace MTCG.API;

public class TCPListener : IHostedService
{

    private readonly TcpListener _listener = new (IPAddress.Parse("127.0.0.1"), PORT);

    private ILogger<TCPListener> _logger;

    private readonly RequestProcessor _requestProcessor;

    private const int PORT = 10001;

    private const int BUFFER_SIZE = 1024;

    public TCPListener(RequestProcessor requestProcessor, ILogger<TCPListener> logger)
    {
        _requestProcessor = requestProcessor;
        _logger = logger;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _listener.Start();

        while (!cancellationToken.IsCancellationRequested)
        {
            TcpClient client = await _listener.AcceptTcpClientAsync(cancellationToken);
            _ = HandlerClientAsync(client, cancellationToken);
        }
    }

    private async Task HandlerClientAsync(TcpClient client, CancellationToken cancellationToken)
    {
        string requestData = await ReadRequest(cancellationToken, client);
        string response = await _requestProcessor.ProcessRequest(requestData, cancellationToken);
        await WriteResponse(cancellationToken, client, response);
        client.Close();
    }

    private async ValueTask<string> ReadRequest(CancellationToken token, TcpClient tcpClient)
    {
        string requestData = string.Empty;
        NetworkStream stream = tcpClient.GetStream();
        byte[] buffer = new byte[BUFFER_SIZE];

        for (; tcpClient.GetStream().DataAvailable || string.IsNullOrEmpty(requestData);)
        {
            int read = await stream.ReadAsync(buffer, 0, buffer.Length, token);
            requestData += Encoding.UTF8.GetString(buffer, 0, read);
        }

        return requestData;
    }

    private async ValueTask WriteResponse(CancellationToken token, TcpClient tcpClient, string response)
    {
        NetworkStream stream = tcpClient.GetStream();
        byte[] buffer = Encoding.UTF8.GetBytes(response);
        await stream.WriteAsync(buffer, 0, buffer.Length, token);
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        _listener.Stop();
        await Task.CompletedTask;
    }

}