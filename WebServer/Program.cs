﻿using WebServer;

class Program
{
   static async Task<int> Main(string[] args)
   {
      Console.WriteLine("Starting server...");
      var webServer = new Server();
      int serverExitCode = await webServer.Start();
      Console.WriteLine("Server stopped.");
      return serverExitCode;
   }
}
