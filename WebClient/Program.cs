using WebClient;

class Program
{
   static async Task<int> Main(string[] args)
   {
      var clientExitCode = await Client.Start();

      return clientExitCode;
   }
}


