using System.Net;
using System.Net.Sockets;
using System.Text;

namespace LoadBalancer;

public class LoadBalancer
{
    public static async Task<int> Start()
    {
        int returnCode = 0;

        // Create localhost endpoint for communication
        var localHostIPAddress = IPAddress.Loopback;
        var localHostEndpoint = new IPEndPoint(localHostIPAddress, 8080);

        // Create socket to listen to client's requests
        using Socket listener = new(localHostEndpoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

        // Bind the socket to the localhost endpoint and start listening
        listener.Bind(localHostEndpoint);
        listener.Listen(100);
        Console.WriteLine($"Listening on {localHostEndpoint.Address}:{localHostEndpoint.Port}");

        while (true)
        {
            var buffer = new byte[1024];
            try
            {
                // Start accepting connections from clients asynchronously
                var handler = await listener.AcceptAsync();

                // Receive data from connected clients
                var received = await handler.ReceiveAsync(buffer, SocketFlags.None);
                var message = Encoding.UTF8.GetString(buffer, 0, received);
                Console.WriteLine($"Received request from\r\n {message}");

                var statusCode = "200 OK";
                var ackMessage = $"HTTP/1.1 {statusCode}\r\n";
                var echoBytes = Encoding.UTF8.GetBytes(ackMessage);
                await handler.SendAsync(echoBytes, SocketFlags.None);

                // Close socket for the current connection--continue accepting connections asynchronously
                handler.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                returnCode = 1;
                break;
            }
        }
        return returnCode;
    }
}
