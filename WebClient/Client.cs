using System.Net;
using System.Net.Sockets;
using System.Text;

namespace WebClient;

public class Client
{
   public async Task<int> Start()
   {
      // Create an IP endpoint to bind the socket to. In this case,
      // the localhost's IP and port 5001
      IPAddress localIpAddress = IPAddress.Loopback; // the localhost or "loopback" IP address
      IPEndPoint ipEndPoint = new(localIpAddress, 5001);

      using Socket client = new(ipEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

      await client.ConnectAsync(ipEndPoint);
      while (true)
      {
         // Send message
         var message = "Hi friend!<|EOM|>";
         var messageBytes = Encoding.UTF8.GetBytes(message);
         _ = await client.SendAsync(messageBytes, SocketFlags.None);
         Console.WriteLine($"Socket client sent message: \"{message}\"");

         // Receive ack.
         var buffer = new byte[1024];
         var received = await client.ReceiveAsync(buffer, SocketFlags.None);
         var response = Encoding.UTF8.GetString(buffer, 0, received);
         if (response == "<|ACK|>")
         {
            Console.WriteLine($"Socket client received acknowledgement: \"{response}\"");
            break;
         }
      }
      client.Shutdown(SocketShutdown.Both);

      return 0;
   }
}
