using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using ShipWorks.Shipping.Carriers.Postal;
using ShipWorks.Shipping.Carriers.Postal.Usps.Registration.Promotion;

namespace ShipWorks.Tests.Shipping.Carriers.Postal.Usps.Registration.Promotion
{
    public class UspsRegistrationPromotionTest
    {
        private readonly UspsIntuishipRegistrationPromotion testObject;

        public UspsRegistrationPromotionTest()
        {
            testObject = new UspsIntuishipRegistrationPromotion();
        }

        [Fact]
        public void GetPromoCode_ReturnsShipWorks3_WhenRegistrationTypeIsExpedited_Test()
        {
            string promo = testObject.GetPromoCode();

            Assert.Equal("ShipWorks3", promo);
        }
    }
}
