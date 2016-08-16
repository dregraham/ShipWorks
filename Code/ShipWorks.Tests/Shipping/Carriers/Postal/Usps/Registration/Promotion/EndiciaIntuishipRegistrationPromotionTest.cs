using System.Collections.Generic;
using System.Linq;
using Xunit;
using ShipWorks.Shipping.Carriers.Postal;
using ShipWorks.Shipping.Carriers.Postal.Usps.Registration.Promotion;

namespace ShipWorks.Tests.Shipping.Carriers.Postal.Usps.Registration.Promotion
{
    public class EndiciaIntuishipRegistrationPromotionTest
    {
        private readonly EndiciaIntuishipRegistrationPromotion testObject;

        public EndiciaIntuishipRegistrationPromotionTest()
        {
            testObject = new EndiciaIntuishipRegistrationPromotion();
        }

        [Fact]
        public void GetPromoCode_ReturnsShipWorks5_WhenRegistrationTypeIsExpedited()
        {
            string promo = testObject.GetPromoCode();

            Assert.Equal("ShipWorks5", promo);
        }
    }
}
