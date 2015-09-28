using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using ShipWorks.Shipping.Carriers.Postal;
using ShipWorks.Shipping.Carriers.Postal.Usps.Registration.Promotion;

namespace ShipWorks.Tests.Shipping.Carriers.Postal.Usps.Registration.Promotion
{
    public class NewPostalCustomerRegistrationPromotionTest
    {
        private readonly NewPostalCustomerRegistrationPromotion testObject;

        public NewPostalCustomerRegistrationPromotionTest()
        {
            testObject = new NewPostalCustomerRegistrationPromotion();
        }

        [Fact]
        public void GetPromoCode_ReturnsShipWorks3_WhenRegistrationTypeIsExpedited_Test()
        {
            string promo = testObject.GetPromoCode();

            Assert.Equal("ShipWorks6", promo);
        }
    }
}
