using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShipWorks.Shipping.Carriers.Postal;
using ShipWorks.Shipping.Carriers.Postal.Usps.Registration.Promotion;

namespace ShipWorks.Tests.Shipping.Carriers.Postal.Usps.Registration.Promotion
{
    [TestClass]
    public class EndiciaCbpRegistrationPromotionTest
    {
        private readonly EndiciaCbpRegistrationPromotion testObject;

        public EndiciaCbpRegistrationPromotionTest()
        {
            testObject = new EndiciaCbpRegistrationPromotion();
        }

        [TestMethod]
        public void GetPromoCode_ReturnsShipWorks5_WhenRegistrationTypeIsExpedited_Test()
        {
            string promo = testObject.GetPromoCode();

            Assert.AreEqual("ShipWorks4", promo);
        }
    }
}
