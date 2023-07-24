using System.IO.Pipes;

namespace SenderServer
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine($"Sender server start running at: {DateTimeOffset.Now}");

                await using var pipeServer = new NamedPipeServerStream("InterProcessCommunicationTest", PipeDirection.Out);

                // Wait for a client to connect
                Console.WriteLine("Waiting for Receiver client connection...");
                await pipeServer.WaitForConnectionAsync();

                Console.WriteLine("Client connected.");

                try
                {
                    Console.WriteLine("Enter text: ");

                    var enteredText = Console.ReadLine();

                    if (enteredText == "exit")
                        break;

                    // Read user input and send that to the client process.
                    await using var sw = new StreamWriter(pipeServer);

                    sw.AutoFlush = true;

                    sw.WriteLine(enteredText);
                }
                catch (IOException e)
                {
                    Console.WriteLine($"Error occurred while sending message: {e.Message}");
                }
            }
        }
    }
}