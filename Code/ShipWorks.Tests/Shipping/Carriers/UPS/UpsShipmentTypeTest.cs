using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers.UPS;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Shipping.Carriers.UPS.OnLineTools;
using ShipWorks.Shipping.Carriers.UPS.OnLineTools.Api;
using ShipWorks.Shipping.Editing.Enums;

namespace ShipWorks.Tests.Shipping.Carriers.UPS
{
    [TestClass]
    public class UpsShipmentTypeTest
    {
        UpsShipmentType testObject = new UpsOltShipmentType();


        [TestMethod]
        public void SupportsMultiplePackages_ReturnsTrue_Test()
        {
            Assert.IsTrue(testObject.SupportsMultiplePackages);
        }
    }
}
