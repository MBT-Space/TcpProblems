using System;
using System.Net.Sockets;
using System.Text;

namespace TcpClientExample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
               
                // Replace "localhost" and 8000 with your server's IP and port

                var hostName = "localhost";
                if(args.Length>0){
                    
                    hostName = args[0];
                    Console.WriteLine($"Take host name from argument {hostName}");
                }
                var port = 8000;
                if(args.Length>1){
                    
                    port = int.Parse(args[1]);
                    Console.WriteLine($"Take port name from argument {port}");
                }

                using (var tcpClient = new TcpClient(hostName, port))
                {
                    Console.WriteLine("Connected to server. Waiting for data...");

                    // Assuming NetworkStreamWrapper encapsulates a NetworkStream
                    using (var streamWrapper = new NetworkStreamWrapper(tcpClient.GetStream()))
                    {
                        byte[] buffer = new byte[1024];
                        int bytesRead;

                        // Read data in a loop until the server closes the connection
                        while ((bytesRead = streamWrapper.Read(buffer, 0, buffer.Length)) != 0)
                        {
                            string response = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                            Console.WriteLine("Received: " + response);
                        }
                    }
                }
                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }

            Console.WriteLine("Connection closed.");
        }
    }
}