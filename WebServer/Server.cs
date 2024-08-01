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

      var handler = await listener.AcceptAsync();
      Console.WriteLine($"Server started. Listening on {ipEndPoint.Address}");
      while (true)
      {
         var buffer = new byte[1024];
         var received = await handler.ReceiveAsync(buffer, SocketFlags.None);
         var response = Encoding.UTF8.GetString(buffer, 0, received);

         var eom = "<|EOM|>";
         if (response.IndexOf(eom) > -1) // is end of message
         {
            Console.WriteLine($"Socket server received message \"{response.Replace(eom, "")}\"");

            var ackMessage = "<|ACK|>";
            var echoBytes = Encoding.UTF8.GetBytes(ackMessage);
            await handler.SendAsync(echoBytes, 0);
            Console.WriteLine($"Socket server sent acknowledgement: \"{ackMessage}\"");

            break;
         }
      }
      return 0;
   }
}
