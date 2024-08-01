using WebClient;

class Program
{
   static async Task<int> Main(string[] args)
   {
      var client = new Client();
      var clientExitCode = await client.Start();

      return clientExitCode;
   }
}


