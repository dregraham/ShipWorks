using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.Shipping.Carriers.FedEx;
using ShipWorks.Shipping.Carriers.FedEx.BestRate;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx
{
    [TestClass]
    public class FedExShipmentTypeTest
    {
        private FedExShipmentType testObject;

        [TestInitialize]
        public void Intialize()
        {
            testObject = new FedExShipmentType();
        }

        [TestMethod]
        public void GetShippingBroker_ReturnsFedExShippingBroker_Test()
        {
            IBestRateShippingBroker broker = testObject.GetShippingBroker(new ShipmentEntity());

            Assert.IsInstanceOfType(broker, typeof(FedExBestRateBroker));
        }
    }
}
