using System.Collections.Generic;
using System.Linq;
using Xunit;
using ShipWorks.Shipping.Carriers.Postal;
using ShipWorks.Shipping.Carriers.Postal.Usps.Registration.Promotion;

namespace ShipWorks.Tests.Shipping.Carriers.Postal.Usps.Registration.Promotion
{
    public class Express1OnlyRegistrationPromotionTest
    {
        private readonly Express1OnlyRegistrationPromotion testObject;

        public Express1OnlyRegistrationPromotionTest()
        {
            testObject = new Express1OnlyRegistrationPromotion();
        }

        [Fact]
        public void GetPromoCode_ReturnsShipWorks7_WhenRegistrationTypeIsExpedited()
        {
            string promo = testObject.GetPromoCode();

            Assert.Equal("ShipWorks7", promo);
        }
    }
}
