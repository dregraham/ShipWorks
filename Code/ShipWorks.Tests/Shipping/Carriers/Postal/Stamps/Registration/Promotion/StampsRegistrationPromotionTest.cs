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
    public class StampsRegistrationPromotionTest
    {
        private readonly StampsIntuishipRegistrationPromotion testObject;

        public StampsRegistrationPromotionTest()
        {
            testObject = new StampsIntuishipRegistrationPromotion();
        }

        [TestMethod]
        public void GetPromoCode_ReturnsShipWorks3_WhenRegistrationTypeIsExpedited_Test()
        {
            string promo = testObject.GetPromoCode();

            Assert.AreEqual("ShipWorks3", promo);
        }
    }
}
