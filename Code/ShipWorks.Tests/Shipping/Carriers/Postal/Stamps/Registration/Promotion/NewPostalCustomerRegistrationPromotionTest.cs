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
    public class NewPostalCustomerRegistrationPromotionTest
    {
        private readonly NewPostalCustomerRegistrationPromotion testObject;

        public NewPostalCustomerRegistrationPromotionTest()
        {
            testObject = new NewPostalCustomerRegistrationPromotion();
        }

        [TestMethod]
        public void GetPromoCode_ReturnsShipWorks3_WhenRegistrationTypeIsExpedited_Test()
        {
            string promo = testObject.GetPromoCode();

            Assert.AreEqual("ShipWorks6", promo);
        }
    }
}
