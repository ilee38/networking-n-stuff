using System;
using NetFunctions;

namespace NetFunctionsTests;

public class NetFunctionsTests
{
   private readonly NetFunctionsTools netFunctions = new();

   [Fact]
   public void Ipv4ToValueTests()
   {
      //Arrange
      var ipAddress1 = "255.255.0.0";
      var ipAddress2 = "198.51.100.10";

      //Act
      var result1 = netFunctions.Ipv4ToValue(ipAddress1);
      var result2 = netFunctions.Ipv4ToValue(ipAddress2);

      //Assert
      Assert.Equal(4294901760, result1);
      Assert.Equal(3325256714, result2);
   }

   [Fact]
   public void ValueToIpv4Tests()
   {
      // Arrange
      UInt32 address = 3325256714;

      // Act
      var result = netFunctions.ValueToIpv4(address);

      // Assert
      Assert.Equal("198.51.100.10", result);
   }

   [Fact]
   public void GetSubnetMaskValueTests()
   {
      // Arrange
      var slash1 = "/16";
      var slash2 = "10.20.30.40/23";

      // Act
      var result1 = netFunctions.GetSubnetMaskValue(slash1);
      var result2 = netFunctions.GetSubnetMaskValue(slash2);

      // Assert
      Assert.Equal(4294901760, result1);
      Assert.Equal(4294966784, result2);
   }

   [Fact]
   public void IPsSameSubnetTests()
   {
      //Arrange
      var ipAddress1 = "10.23.121.17";
      var ipAddress2 = "10.23.121.255";
      var slash1 = "/23";

      var ipAddress3 = "10.23.230.22";
      var ipAddress4 = "10.24.121.225";
      var slash2 = "/16";

      //Act
      var result1 = netFunctions.IPsSameSubnet(ipAddress1, ipAddress2, slash1);
      var result2 = netFunctions.IPsSameSubnet(ipAddress3, ipAddress4, slash2);

      //Assert
      Assert.True(result1);
      Assert.False(result2);
   }

   [Fact]
   public void GetNetworkTests()
   {
      // Arrange
      UInt32 iPValue = 0x01020304;
      UInt32 netMask = 0xffffff00;

      // Act
      var result = netFunctions.GetNetwork(iPValue, netMask);

      // Assert
      UInt32 expected = 0x01020300;
      Assert.Equal(expected, result);
   }

   [Fact]
   public void FindRouterForIPTests()
   {
      // Arrange
      string routersFilePath = "routers.json";
      string ipAddress1 = "1.2.3.5";
      string ipAddress2 = "1.2.5.6";

      // Act
      string sameSubnetResult = netFunctions.FindRouterForIP(routersFilePath, ipAddress1);
      string differentSubnetResult = netFunctions.FindRouterForIP(routersFilePath, ipAddress2);

      // Assert
      Assert.Equal("1.2.3.4", sameSubnetResult);
      Assert.True(string.IsNullOrEmpty(differentSubnetResult));
   }
}
