﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShipWorks.Shipping.Carriers.Postal.Stamps;

namespace ShipWorks.Tests.Shipping.Carriers.Postal.Stamps
{
    [TestClass]
    public class StampsResellerTypeTest
    {
        [TestMethod]
        public void Express1_HasOneAsValue_Test()
        {
            // The upgrade script depends on Express1 value being 1
            Assert.AreEqual(1, (int) StampsResellerType.Express1);
        }

        [TestMethod]
        public void None_HasZeroAsValue_Test()
        {
            // The upgrade script depends on None value being 0
            Assert.AreEqual(0, (int)StampsResellerType.None);
        }
    }
}
