using System.Net;
using System.Net.Sockets;
using System.Text;

using Castle.Core.Logging;

using Microsoft.Extensions.Logging;

using Moq;

using MTCG.API;

namespace MTCG.Http.Test;

[TestClass]
public class TestTcpListener
{

    private Mock<RequestProcessor> _mockRequestProcessor;

    private TCPListener _tcpListener;

    private ILogger<TCPListener> _logger;

    private const int BUFFER_SIZE = 1024;

    [TestInitialize]
    public void SetUp()
    {
        _mockRequestProcessor = new Mock<RequestProcessor>();

        _logger = new Mock<ILogger<TCPListener>>().Object;

        _mockRequestProcessor.Setup(p => p.ProcessRequest(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                             .ReturnsAsync("Hello");

        _tcpListener = new TCPListener(_mockRequestProcessor.Object, _logger);
    }

    [TestMethod]
    public async Task TCPListener_StartAsync_ShouldProcessRequestAndReturnHello()
    {
        CancellationTokenSource cancellationTokenSource = new ();
        Task listeningTask = _tcpListener.StartAsync(cancellationTokenSource.Token);

        using TcpClient client = new TcpClient();

        await client.ConnectAsync(IPAddress.Parse("127.0.0.1"), 10001);
        NetworkStream stream = client.GetStream();
        byte[] requestBytes = "Test Request"u8.ToArray();
        await stream.WriteAsync(requestBytes, 0, requestBytes.Length);

        await Task.Delay(500);

        string response = string.Empty;
        byte[] buffer = new byte[BUFFER_SIZE];

        while (stream.DataAvailable)
        {
            int read = await stream.ReadAsync(buffer, 0, buffer.Length);
            response += Encoding.UTF8.GetString(buffer, 0, read);
        }

        Assert.AreEqual("Hello", response);
        _mockRequestProcessor.Verify(rp => rp.ProcessRequest(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.AtLeastOnce());

        cancellationTokenSource.Cancel();
        await _tcpListener.StopAsync(cancellationTokenSource.Token);
    }

    [TestMethod]
    public async Task TCPListener_StartAsync_ShouldAbortOnCancellation()
    {
        CancellationTokenSource cancellationTokenSource = new ();
        Task listeningTask = _tcpListener.StartAsync(cancellationTokenSource.Token);

        using TcpClient client = new TcpClient();

        await client.ConnectAsync(IPAddress.Parse("127.0.0.1"), 10001);
        NetworkStream stream = client.GetStream();
        byte[] requestBytes = "Test Request"u8.ToArray();
        await stream.WriteAsync(requestBytes, 0, requestBytes.Length);

        cancellationTokenSource.Cancel();

        await Task.Delay(500);

        Assert.IsTrue(listeningTask.IsCompleted);
    }

}