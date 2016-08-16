using System.Collections.Generic;
using System.Linq;
using Xunit;
using ShipWorks.Shipping.Carriers.Postal;
using ShipWorks.Shipping.Carriers.Postal.Usps.Registration.Promotion;

namespace ShipWorks.Tests.Shipping.Carriers.Postal.Usps.Registration.Promotion
{
    public class EndiciaCbpRegistrationPromotionTest
    {
        private readonly EndiciaCbpRegistrationPromotion testObject;

        public EndiciaCbpRegistrationPromotionTest()
        {
            testObject = new EndiciaCbpRegistrationPromotion();
        }

        [Fact]
        public void GetPromoCode_ReturnsShipWorks5_WhenRegistrationTypeIsExpedited()
        {
            string promo = testObject.GetPromoCode();

            Assert.Equal("ShipWorks4", promo);
        }
    }
}
