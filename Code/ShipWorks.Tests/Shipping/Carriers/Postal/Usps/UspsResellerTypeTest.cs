using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShipWorks.Shipping.Carriers.Postal.Stamps;
using ShipWorks.Shipping.Carriers.Postal.Usps;

namespace ShipWorks.Tests.Shipping.Carriers.Postal.Stamps
{
    [TestClass]
    public class StampsResellerTypeTest
    {
        [TestMethod]
        public void Express1_HasOneAsValue_Test()
        {
            // The upgrade script depends on Express1 value being 1
            Assert.AreEqual(1, (int) UspsResellerType.Express1);
        }

        [TestMethod]
        public void None_HasZeroAsValue_Test()
        {
            // The upgrade script depends on None value being 0
            Assert.AreEqual(0, (int)UspsResellerType.None);
        }

        [TestMethod]
        public void UspsExpedited_HasTwoAsValue_Test()
        {
            Assert.AreEqual(2, (int)UspsResellerType.StampsExpedited);
        }
    }
}
