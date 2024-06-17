using System.Net.Sockets;

public class NetworkStreamWrapper : INetworkStream, IDisposable
{
    private readonly NetworkStream _stream;

    public NetworkStreamWrapper(NetworkStream stream)
    {
        _stream = stream;
    }

    public void WriteAsync(byte[] buffer, int offset, int size)
    {
        _stream.WriteAsync(buffer, offset, size);
    }

    public void Write(byte[] buffer, int offset, int size)
    {
        _stream.Write(buffer, offset, size);
    }
    /// <summary>Reads data from the <see cref="T:System.Net.Sockets.NetworkStream" /> and stores it to a span of bytes in memory.</summary>
    /// <param name="buffer">A region of memory to store data read from the <see cref="T:System.Net.Sockets.NetworkStream" />.</param>
    /// <exception cref="T:System.InvalidOperationException">The <see cref="T:System.Net.Sockets.NetworkStream" /> does not support reading.</exception>
    /// <exception cref="T:System.IO.IOException">An error occurred when accessing the socket.
    /// 
    /// -or-
    /// 
    /// There is a failure reading from the network.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.NetworkStream" /> is closed.</exception>
    /// <returns>The number of bytes read from the <see cref="T:System.Net.Sockets.NetworkStream" />.</returns>
    public int Read(byte[] buffer, int offset, int count)
    {
        return _stream.Read(buffer, offset, count);
    }

    public void Close()
    {
        _stream.Close();
        _stream.ReadTimeout = 0;
    }
    // Implement other methods as needed

    public void Dispose()
    {
        _stream?.Dispose();
    }

    public async Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken stoppingToken)
    {
        return await _stream.ReadAsync(buffer, offset, count, stoppingToken);
    }

    public async Task WriteAsync(byte[] buffer, int offset, int size, CancellationToken stoppingToken)
    {
        await _stream.WriteAsync(buffer, offset, size, stoppingToken);
    }
}