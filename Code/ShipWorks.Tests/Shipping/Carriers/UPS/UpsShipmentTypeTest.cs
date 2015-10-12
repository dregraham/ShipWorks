using System;
using Xunit;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers.UPS;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Shipping.Carriers.UPS.OnLineTools;
using ShipWorks.Shipping.Carriers.UPS.OnLineTools.Api;
using ShipWorks.Shipping.Editing.Enums;
using ShipWorks.Tests.Shared;

namespace ShipWorks.Tests.Shipping.Carriers.UPS
{
    public class UpsShipmentTypeTest
    {
        [Fact]
        public void SupportsMultiplePackages_ReturnsTrue_Test()
        {
            using (var mock = AutoMockExtensions.GetLooseThatReturnsMocks())
            {
                UpsOltShipmentType testObject = mock.Create<UpsOltShipmentType>();
                Assert.True(testObject.SupportsMultiplePackages);
            }
        }
    }
}
