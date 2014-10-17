using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShipWorks.Shipping.Carriers.Postal.Usps;

namespace ShipWorks.Tests.Shipping.Carriers.Postal.Usps
{
    [TestClass]
    public class UspsShipmentTypeTest
    {
        [TestMethod]
        public void ShouldRetrieveExpress1Rates_ReturnsFalse_Test()
        {
            UspsShipmentType uspsShipmentType = new UspsShipmentType();

            // Never get Express1 rates for stamps expedited
            Assert.IsFalse(uspsShipmentType.ShouldRetrieveExpress1Rates);
        }
    }
}
