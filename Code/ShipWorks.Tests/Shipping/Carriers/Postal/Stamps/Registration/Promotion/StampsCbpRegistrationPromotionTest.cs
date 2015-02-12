using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShipWorks.Shipping.Carriers.Postal;
using ShipWorks.Shipping.Carriers.Postal.Stamps.Registration.Promotion;

namespace ShipWorks.Tests.Shipping.Carriers.Postal.Stamps.Registration.Promotion
{
    [TestClass]
    public class StampsCbpRegistrationPromotionTest
    {
        private readonly StampsCbpRegistrationPromotion testObject;

        public StampsCbpRegistrationPromotionTest()
        {
            testObject = new StampsCbpRegistrationPromotion();
        }

        [TestMethod]
        public void GetPromoCode_ReturnsShipWorks2_Test()
        {
            string promo = testObject.GetPromoCode();

            Assert.AreEqual("ShipWorks2", promo);
        }

        [TestMethod]
        public void IsMonthlyFeeWaived_ReturnsFalse_Test()
        {
            Assert.AreEqual(false, testObject.IsMonthlyFeeWaived);
        }
    }
}
