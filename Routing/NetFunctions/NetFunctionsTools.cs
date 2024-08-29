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
   /// <returns></returns>
   public Int64 Ipv4ToValue(string ipv4Address)
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
}
