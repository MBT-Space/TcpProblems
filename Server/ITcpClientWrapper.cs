using System.Net.Sockets;

namespace SampleServer
{
    public interface ITcpClientWrapper
    {
        NetworkStream GetStream();
        bool IsConnected();
        void Close();
    }
}