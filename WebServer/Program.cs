using WebServer;

class Program
{
   private static async Task<int> Main(string[] args)
   {
      // Start web server
      var serverExitCode = await Server.Start();
      Console.WriteLine($"Server stopped. Exit code: {serverExitCode}");

      return serverExitCode;
   }
}

