using System;
using NetFunctions;

namespace NetFunctionsTests;

public class NetFunctionsTests
{
   [Fact]
   public void Ipv4ToValueTests()
   {
      //Arrange
      var netFunctions = new NetFunctionsTools();
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
      var netFunctions = new NetFunctionsTools();
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
      var netFunctions = new NetFunctionsTools();
      var slash1 = "/16";
      var slash2 = "10.20.30.40/23";

      // Act
      var result1 = netFunctions.GetSubnetMaskValue(slash1);
      var result2 = netFunctions.GetSubnetMaskValue(slash2);

      // Assert
      Assert.Equal(4294901760, result1);
      Assert.Equal(4294966784, result2);
   }
}
