using System;
using Xunit;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers.UPS;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Shipping.Carriers.UPS.OnLineTools;
using ShipWorks.Shipping.Carriers.UPS.OnLineTools.Api;
using ShipWorks.Shipping.Editing.Enums;

namespace ShipWorks.Tests.Shipping.Carriers.UPS
{
    public class UpsShipmentTypeTest
    {
        UpsShipmentType testObject = (UpsShipmentType) ShipmentTypeManager.GetType(ShipmentTypeCode.UpsOnLineTools);


        [Fact]
        public void SupportsMultiplePackages_ReturnsTrue_Test()
        {
            Assert.True(testObject.SupportsMultiplePackages);
        }
    }
}
