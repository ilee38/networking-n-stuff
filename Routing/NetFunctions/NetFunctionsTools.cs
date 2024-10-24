using System;
using System.Text.Json;

namespace NetFunctions;

public class NetFunctionsTools
{
   public NetFunctionsTools()
   {}

   /// <summary>
   /// Converts a dots-and-numbers IP address to a single 32-bit numeric value (integer)
   /// </summary>
   /// <param name="ipv4Address"></param>
   /// <returns>An integer value representing the IP address</returns>
   public static UInt32 Ipv4ToValue(string ipv4Address)
   {
      string[] octetList = ipv4Address.Split('.');
      byte[] bytes = new byte[octetList.Length];

      for (int i = 0; i < octetList.Length; i++)
      {
         bytes[i] = Convert.ToByte(octetList[i]);
      }

      string hexString = Convert.ToHexString(bytes);
      var combinedValue = Convert.ToUInt32(hexString, 16);

      return combinedValue;
   }

   /// <summary>
   /// Converts a 32-bit integer value to dots-and-numbers IP address
   /// <param name="address"></param>
   /// <returns>A string representation of the IP address</returns>
   public static string ValueToIpv4(UInt32 address)
   {
      var mask = 0x_000000FF;

      var octet1 = (address >> 24) & mask;
      var octet2 = (address >> 16) & mask;
      var octet3 = (address >> 8) & mask;
      var octet4 = address & mask;

      return $"{octet1}.{octet2}.{octet3}.{octet4}";

   }

   /// <summary>
   /// Given a subnet mask in slash notation, return the value of the mask
   ///  as a single number of integer type. The input can contain an IP
   ///  address optionally, but that part should be discarded.
   ///  Example:
   ///  There is only one return value, but it is shown here in 3 bases.
   ///  slash:  "/16"
   ///  return: 0xffff0000 0b11111111111111110000000000000000 4294901760
   ///  slash:  "10.20.30.40/23"
   ///  return: 0xfffffe00 0b11111111111111111111111000000000 4294966784
   /// </summary>
   /// <param name="slash"></param>
   /// <returns>An integer value for the subnet mask</returns>
   public static UInt32 GetSubnetMaskValue(string slash)
   {
      string hostBitsString = slash.Split('/')[1];
      int hostBits = int.Parse(hostBitsString);
      uint mask = 0xFFFFFFFF << (32 - hostBits);

      return mask;
   }

   /// <summary>
   /// Given two IP addresses and a subnet mask in slash notation, return true
   /// if the two IP addresses are in the same subnet, and false otherwise.
   /// </summary>
   /// <param name="iP1"></param>
   /// <param name="iP2"></param>
   /// <param name="slash"></param>
   /// <returns></returns>
   public static bool IPsSameSubnet(string iP1, string iP2, string slash)
   {
      UInt32 subnetMask = GetSubnetMaskValue(slash);
      UInt32 iP1Value = Ipv4ToValue(iP1);
      UInt32 iP2Value = Ipv4ToValue(iP2);

      bool sameSubnet = (iP1Value & subnetMask) == (iP2Value & subnetMask);
      return sameSubnet;
   }

   /// <summary>
   ///  Returns the network portion of an IP address value as an integer type.
   /// </summary>
   /// <param name="iPValue">IP address as integer value</param>
   /// <param name="netMask">Mask as integer value</param>
   /// <returns>The network portion of the address.</returns>
   public static UInt32 GetNetwork(UInt32 iPValue, UInt32 netMask)
   {
      return iPValue & netMask;
   }

   /// <summary>
   ///  Given a JSON file with router information and an IP address, return the IP address of the router
   ///  that belongs in the same subnet as the given IP address.
   /// </summary>
   /// <param name="routerInfoFilePath">Path to JSON file with routers (keyed by router IP)</param>
   /// <param name="iPAddress"></param>
   /// <returns>
   /// The IP address of the corresponding router, or an empty string if no router matches the subnet
   /// of the given address.
   /// </returns>
   public static string FindRouterForIP(string routerInfoFilePath, string iPAddress)
   {
      string jsonString = File.ReadAllText(routerInfoFilePath);

      // Parse JSON using JsonDocument
      using (JsonDocument doc = JsonDocument.Parse(jsonString))
      {
         JsonElement root = doc.RootElement;

         var routers = root.GetProperty("routers").EnumerateObject();
         while (routers.MoveNext())
         {
            var currentRouter = routers.Current.Value;
            var currentRouterIP = routers.Current.Name;
            var currentRouterNetMask = currentRouter.GetProperty("netmask").GetString();

            if (IPsSameSubnet(iPAddress, currentRouterIP, currentRouterNetMask))
            {
               return currentRouterIP;
            }
         }
      }
      return string.Empty;
   }
}
