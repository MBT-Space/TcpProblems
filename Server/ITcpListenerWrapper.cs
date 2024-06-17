namespace SampleServer;
public interface ITcpListenerWrapper
{
    Task<ITcpClientWrapper> AcceptTcpClientAsync(CancellationToken stoppingToken);
    void Start();
    void Stop();
}