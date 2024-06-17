using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using SampleServer;
using System.Text;

namespace ConsoleAppWithLogger
{
    class Program
    {
        static void Main(string[] args)
        {
            var seconds = 130;
            if(args.Length>0){
                seconds = int.Parse(args[0]);

            }

            var message = Encoding.ASCII.GetBytes("Hello world!");
           
            // Set up the dependency injection container
            var serviceProvider = new ServiceCollection()
                .AddLogging(configure => configure.AddConsole()) // Add logging and configure to output to console
                .BuildServiceProvider();

            // Get the server logger from the service provider
            var loggerServer = serviceProvider.GetService<ILogger<Server>>();
            var loggerDispatcher = serviceProvider.GetService<ILogger<Dispatcher>>();
            var dispatcher = new Dispatcher(loggerDispatcher);
            Server server = new(loggerServer, dispatcher);
            Task t = server.DoWork(new System.Threading.CancellationToken());
            var logger = serviceProvider.GetService<ILogger<Program>>();
            if (logger != null)
            {
                while (true)
                {
                    logger.LogInformation($"Before wait {seconds} seconds;");
                    
                    for (int i = 0; i < seconds; i++)
                    {
                        Task.Delay(1000).Wait();
                        Console.Write("=");
                    }

                    Console.WriteLine();
                    logger.LogInformation($"After  wait {seconds} seconds");

                    logger.LogInformation("Send data to client");
                    dispatcher.EnqueueMessage(message);
                    logger.LogInformation("After send data to client");
                }
            }

        }
    }
}