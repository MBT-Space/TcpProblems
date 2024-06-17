using Microsoft.Extensions.Logging;
using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Channels; // Import the namespace for Channel

namespace SampleServer
{
    public class Dispatcher
    {
        public Dispatcher(ILogger<Dispatcher> logger)
        {
            _logger = logger;
        }

        private readonly ILogger<Dispatcher> _logger;
        // Define a channel that transports strings
        private readonly Channel<byte[]> _channel = System.Threading.Channels.Channel.CreateUnbounded<byte[]>();
        public bool MainIsConnected { get; set; }
        // Method to enqueue a message into the channel
        public Channel<byte[] > Channel => _channel;

        public void EnqueueMessage(byte[] message)
        {
            if (!_channel.Writer.TryWrite(message))
            {
                _logger.LogInformation("Failed to enqueue message");
            }
        }

        // Method to start processing messages from the channel
        public async Task ProcessMessagesAsync(CancellationToken cancellationToken)
        {
            await foreach (var message in _channel.Reader.ReadAllAsync(cancellationToken))
            {
                // Process the message here
                _logger.LogInformation($"Processing message: {message}");
            }
        }
    }
}