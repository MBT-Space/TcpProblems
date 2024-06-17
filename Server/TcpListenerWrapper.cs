using System.Data;
using System.Net;
using System.Net.Sockets;

namespace SampleServer
{
    public class TcpListenerWrapper : ITcpListenerWrapper
    {
        private bool _isConnected=false;
        private TcpListener? _tcpListener;

        public TcpListenerWrapper() { }

        public void Connect(string ip, int port)
        {
            if (_tcpListener == null)
            {
                _tcpListener = new TcpListener(IPAddress.Parse(ip), port);
                Console.WriteLine($"Listen to ip={ip}, port ={port}");
            }

            _isConnected= true;
        }

        public void ConnectAny(int port)
        {
            if (_tcpListener == null)
            {
                _tcpListener = new TcpListener(IPAddress.Any, port);
                Console.WriteLine($"Listen to PAddress.Any, port ={port}");
            }
            _isConnected = true;
        }

        /// <summary>Starts listening for incoming connection requests with a maximum number of pending connection.</summary>
        /// <param name="backlog">The maximum length of the pending connections queue.</param>
        /// <exception cref="T:System.Net.Sockets.SocketException">An error occurred while accessing the socket.</exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="backlog" /> parameter is less than zero or exceeds the maximum number of permitted connections.</exception>
        /// <exception cref="T:System.InvalidOperationException">The underlying <see cref="T:System.Net.Sockets.Socket" /> is null.</exception>
        public virtual void Start(/*int blocking= int.MaxValue*/)
        {
            if (_isConnected)
            {
                _tcpListener.Start( /*blocking*/);
            }
            else
            {
                throw new NoNullAllowedException("TcpListener is not connected");
            }
        }

        /// <summary>Closes the listener.</summary>
        /// <exception cref="T:System.Net.Sockets.SocketException">Use the <see cref="P:System.Net.Sockets.SocketException.ErrorCode" /> property to obtain the specific error code. When you have obtained this code, you can refer to the Windows Sockets version 2 API error code documentation for a detailed description of the error.</exception>
        public virtual void Stop()
        {
            if (_isConnected)
            {
                _tcpListener.Stop();
            }
            else
            {
                throw new NoNullAllowedException("TcpListener is not connected");
            }
            
        }
        /// <summary>Accepts a pending connection request.</summary>
        /// <exception cref="T:System.InvalidOperationException">The listener has not been started with a call to <see cref="M:System.Net.Sockets.TcpListener.Start" />.</exception>
        /// <exception cref="T:System.Net.Sockets.SocketException">Use the <see cref="P:System.Net.Sockets.SocketException.ErrorCode" /> property to obtain the specific error code. When you have obtained this code, you can refer to the Windows Sockets version 2 API error code documentation for a detailed description of the error.</exception>
        /// <returns>A <see cref="T:System.Net.Sockets.TcpClient" /> used to send and receive data.</returns>
        public virtual ITcpClientWrapper AcceptTcpClient()
        {
            if (_isConnected)
            {
                return new TcpClientWrapper(_tcpListener.AcceptTcpClient());
            }
            else
            {
                throw new NoNullAllowedException("TcpListener is not connected");
            }
           
        }

        public async Task<ITcpClientWrapper> AcceptTcpClientAsync(CancellationToken cancellationToken)
        {
            if (_isConnected)
            {
                return new TcpClientWrapper(_tcpListener.AcceptTcpClientAsync(cancellationToken).Result);
            }
            else
            {
                throw new NoNullAllowedException("TcpListener is not connected");
            }

            
        }

    }
}