using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.Shipping.Carriers.UPS.WorldShip;
using ShipWorks.Shipping.Carriers.UPS.WorldShip.BestRate;

namespace ShipWorks.Tests.Shipping.Carriers.UPS.WorldShip
{
    [TestClass]
    public class WorldShipShipmentTypeTest
    {
        private WorldShipShipmentType testObject;

        [TestInitialize]
        public void Intialize()
        {
            testObject = new WorldShipShipmentType();
        }

        [TestMethod]
        public void GetShippingBroker_ReturnsWorldShipShippingBroker_Test()
        {
            IBestRateShippingBroker broker = testObject.GetShippingBroker();

            Assert.IsInstanceOfType(broker, typeof(WorldShipBestRateBroker));
        }
    }
}
