namespace WebServer;

using System.Net.Sockets;
using System.Net;
using System.Text;

public class Server
{

   public async Task<int> Start()
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
      var handler = await listener.AcceptAsync();

      while (true)
      {
         var buffer = new byte[1024];
         var received = await handler.ReceiveAsync(buffer, SocketFlags.None);
         var response = Encoding.UTF8.GetString(buffer, 0, received);

         Console.WriteLine($"Socket server received message:");
         Console.WriteLine($"{response}");

         var receivedLines = response.Split("\n");
         var lineTokens = receivedLines[0].Split(" ");
         var requestedPath = lineTokens[1];

         var page = string.Empty;
         var statusCode = string.Empty;
         if (string.Equals(requestedPath, "/", StringComparison.InvariantCultureIgnoreCase) ||
             string.Equals(requestedPath, "/index.html", StringComparison.InvariantCultureIgnoreCase))
         {
           var sr = new StreamReader(@"../www/index.html");
           page = await sr.ReadToEndAsync();
           statusCode = "200 OK";
         }
         else
         {
           statusCode = "404 Not Found";
         }

         var ackMessage = $"HTTP/1.1 {statusCode}\r\n\r\n{page}\r\n";
         var echoBytes = Encoding.UTF8.GetBytes(ackMessage);
         await handler.SendAsync(echoBytes, 0);
         Console.WriteLine($"Socket server sent acknowledgement: {ackMessage}");

         handler.Shutdown(SocketShutdown.Both);

         break;
      }
      return 0;
   }
}
