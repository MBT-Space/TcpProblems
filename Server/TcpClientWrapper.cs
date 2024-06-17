using System.Net.Sockets;

namespace SampleServer;

public class TcpClientWrapper : ITcpClientWrapper
{
    private TcpClient _tcpClient;

    public TcpClientWrapper(TcpClient tcpClient)
    {
        _tcpClient = tcpClient;
    }

    public void Close()
    {
        _tcpClient.Close();
    }

    public NetworkStream GetStream()
    {
        return _tcpClient.GetStream();
    }
    public bool IsConnected()
    {
        return _tcpClient.Connected;
    }
}