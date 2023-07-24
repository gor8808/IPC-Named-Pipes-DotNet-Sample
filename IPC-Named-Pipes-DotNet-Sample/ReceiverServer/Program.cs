using System.IO.Pipes;

namespace ReceiverServer
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine($"Receiver server start running at: {DateTimeOffset.Now}");

                await using var pipeClient = new NamedPipeClientStream(".", "InterProcessCommunicationTest", PipeDirection.In);

                // Connect to the pipe or wait until the pipe is available.
                Console.WriteLine("Attempting to connect to pipe...");
                pipeClient.Connect();

                Console.WriteLine("Connected to pipe.");

                using var sr = new StreamReader(pipeClient);

                string? receivedText;

                // Read while its not null
                while ((receivedText = sr.ReadLine()) != null)
                {
                    Console.WriteLine("Received from server: {0}", receivedText);
                }
            }
        }
    }
}