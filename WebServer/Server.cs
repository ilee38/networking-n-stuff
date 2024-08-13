namespace WebServer;

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

      Console.WriteLine($"Server started. Listening on {ipEndPoint.Address}:{ipEndPoint.Port}");

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
           var contentExtension = string.Empty;
           var httpContentType = string.Empty;
           var contentLength = 0;

           if (string.Equals(requestedPath, "/", StringComparison.InvariantCultureIgnoreCase))
           {
               requestedPath = "/index.html";
           }

           try
           {
             var sr = new StreamReader($"{WEB_FOLDER_PATH}{requestedPath}");
             page = await sr.ReadToEndAsync();
             // TODO: handle case when there's no file extension specified in the request path.
             contentExtension = requestedPath.Split(".")[^1];
             httpContentType = HttpConstants.MimeTypes[contentExtension];
             contentLength = Encoding.UTF8.GetByteCount(page);
             statusCode = "200 OK";
           }
           catch (Exception e)
           {
             Console.WriteLine(e);
             statusCode = "404 Not Found";
           }

           var ackMessage = $"HTTP/1.1 {statusCode}\r\nContent-Type: {httpContentType}\r\nContent-Length: {contentLength}\r\n\r\n{page}\r\n";
           Console.WriteLine($"Response message: {ackMessage}");
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
