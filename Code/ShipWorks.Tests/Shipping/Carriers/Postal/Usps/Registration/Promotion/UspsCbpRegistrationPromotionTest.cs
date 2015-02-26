using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShipWorks.Shipping.Carriers.Postal;
using ShipWorks.Shipping.Carriers.Postal.Usps.Registration.Promotion;

namespace ShipWorks.Tests.Shipping.Carriers.Postal.Usps.Registration.Promotion
{
    [TestClass]
    public class UspsCbpRegistrationPromotionTest
    {
        private readonly UspsCbpRegistrationPromotion testObject;

        public UspsCbpRegistrationPromotionTest()
        {
            testObject = new UspsCbpRegistrationPromotion();
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
