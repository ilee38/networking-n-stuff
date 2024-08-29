using System;
using NetFunctions;

namespace NetFunctionsTests;

public class NetFunctionsTests
{
   [Fact]
   public void Ipv4ToValueTests()
   {
      //Arrange
      var ipAddress = "255.255.0.0";
      var netFunctions = new NetFunctionsTools();

      //Act
      var result = netFunctions.Ipv4ToValue(ipAddress);

      //Assert
      Assert.Equal(4294901760, result);
   }
}
