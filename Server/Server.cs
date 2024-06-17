using System.Net;
using System.Net.Sockets;
using Microsoft.Extensions.Logging;

namespace SampleServer
{
    public class Server
    {
        private readonly ILogger<Server> _logger;
        private readonly TcpListenerWrapper _tcpListenerWrapper;
        private ITcpClientWrapper _tcpClientWrapper;
        private NetworkStream _networkStream;
        private readonly Dispatcher _dispatcher;
        public Server(ILogger<Server> logger, Dispatcher dispatcher)
        {
            _logger = logger;
            _dispatcher = dispatcher;
            _tcpListenerWrapper = new TcpListenerWrapper();
        }
        public async Task DoWork(CancellationToken stoppingToken)
        {
            _logger.LogInformation("DoWork() enter");
            _tcpListenerWrapper.ConnectAny(8000);
            _tcpListenerWrapper.Start();
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await foreach (var buffer in _dispatcher.Channel.Reader.ReadAllAsync(stoppingToken))
                    {
                        try
                        {
                            if (_tcpClientWrapper == null || !_tcpClientWrapper.IsConnected())
                            {
                                _tcpClientWrapper = await _tcpListenerWrapper.AcceptTcpClientAsync(stoppingToken);
                                _networkStream = _tcpClientWrapper.GetStream();
                            }

                            if (_networkStream == null)
                                throw new Exception("stream null");

                            _dispatcher.MainIsConnected = true;
                            //_dataFileWriter.WriteByteArrayToFile(buffer);//for test
                            //await TryWriteAsync(buffer, stoppingToken);
                            await _networkStream.WriteAsync(buffer, 0, buffer.Length, stoppingToken);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex.Message, ex);
                            _tcpClientWrapper?.Close();
                            _tcpClientWrapper = null;
                            _dispatcher.MainIsConnected = false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message, ex);
                    _dispatcher.MainIsConnected = false;
                }
            }
            _logger.LogInformation("DoWork() exit");
        }
    }
}