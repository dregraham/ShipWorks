using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.Shipping.Carriers.UPS.WorldShip;
using ShipWorks.Shipping.Carriers.UPS.WorldShip.BestRate;

namespace ShipWorks.Tests.Shipping.Carriers.UPS.WorldShip
{
    public class WorldShipShipmentTypeTest
    {
        private WorldShipShipmentType testObject;

        public WorldShipShipmentTypeTest()
        {
            testObject = new WorldShipShipmentType();
        }

        [Fact]
        public void GetShippingBroker_ReturnsWorldShipShippingBroker()
        {
            IBestRateShippingBroker broker = testObject.GetShippingBroker(new ShipmentEntity());

            Assert.IsAssignableFrom<WorldShipBestRateBroker>(broker);
        }
    }
}
