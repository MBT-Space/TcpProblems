public interface INetworkStream
{
    void WriteAsync(byte[] buffer, int offset, int size);
    void Write(byte[] buffer, int offset, int size);
    int Read(byte[] buffer, int offset, int count);
    void Close();
    void Dispose();
    Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken stoppingToken);
    Task WriteAsync(byte[] buffer, int offset, int size, CancellationToken stoppingToken);
}