﻿namespace WebServer;

using System.Net.Sockets;
using System.Net;
using System.Text;

public static class Server
{
  private static readonly string WEB_FOLDER_PATH = @"../www";

   public static async Task<int> Start()
   {
      // Create an IP endpoint to bind the socket to. In this case,
      // the localhost's IP and port 5001
      IPAddress localIpAddress = IPAddress.Loopback; // the localhost or "loopback" IP address
      IPEndPoint ipEndPoint = new(localIpAddress, 5001);

      // Create a server and start listening to connections
      // 1. Create a socket
      using Socket listener = new(
         ipEndPoint.AddressFamily,
         SocketType.Stream,
         ProtocolType.Tcp
      );

      // 2. Bind the socket to the IP endpoint
      listener.Bind(ipEndPoint);
      listener.Listen(100);

      Console.WriteLine($"Server started. Listening on {ipEndPoint.Address}");

      while (true)
      {
         var buffer = new byte[1024];
         try
         {
           // Accept connections from clients
           var handler = await listener.AcceptAsync();

           // Receive data from the connected client
           var received = await handler.ReceiveAsync(buffer, SocketFlags.None);
           var response = Encoding.UTF8.GetString(buffer, 0, received);

           var receivedLines = response.Split("\n");
           var lineTokens = receivedLines[0].Split(" ");
           var requestedPath = lineTokens[1];

           var page = string.Empty;
           var statusCode = string.Empty;

           if (string.Equals(requestedPath, "/", StringComparison.InvariantCultureIgnoreCase) ||
               string.Equals(requestedPath, "/index.html", StringComparison.InvariantCultureIgnoreCase))
           {
             var sr = new StreamReader($"{WEB_FOLDER_PATH}/index.html");
             page = await sr.ReadToEndAsync();
             statusCode = "200 OK";
           }
           else
           {
             try
             {
               var sr = new StreamReader($"{WEB_FOLDER_PATH}{requestedPath}");
               page = await sr.ReadToEndAsync();
               statusCode = "200 OK";
             }
             catch (Exception e)
             {
               Console.WriteLine(e);
               statusCode = "404 Not Found";
             }
           }

           var ackMessage = $"HTTP/1.1 {statusCode}\r\n\r\n{page}\r\n";
           var echoBytes = Encoding.UTF8.GetBytes(ackMessage);
           await handler.SendAsync(echoBytes, 0);

           // Close socket for current connection and continue with next connection(s)
           handler.Close();
         }
         catch (Exception e)
         {
           Console.WriteLine(e);
           break;
         }
      }
      return 0;
   }
}
