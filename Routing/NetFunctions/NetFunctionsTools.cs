using System;

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
   public UInt32 Ipv4ToValue(string ipv4Address)
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
   public string ValueToIpv4(UInt32 address)
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
   public UInt32 GetSubnetMaskValue(string slash)
   {
      string hostBitsString = slash.Split('/')[1];
      uint hostBits = uint.Parse(hostBitsString);
      uint mask = 0xFFFFFFFF << (32 - (int)hostBits);

      return (UInt32)mask;
   }
}
