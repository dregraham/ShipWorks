using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using ShipWorks.Shipping.Carriers.Postal;
using ShipWorks.Shipping.Carriers.Postal.Usps.Registration.Promotion;

namespace ShipWorks.Tests.Shipping.Carriers.Postal.Usps.Registration.Promotion
{
    public class UspsCbpRegistrationPromotionTest
    {
        private readonly UspsCbpRegistrationPromotion testObject;

        public UspsCbpRegistrationPromotionTest()
        {
            testObject = new UspsCbpRegistrationPromotion();
        }

        [Fact]
        public void GetPromoCode_ReturnsShipWorks2_Test()
        {
            string promo = testObject.GetPromoCode();

            Assert.Equal("ShipWorks2", promo);
        }

        [Fact]
        public void IsMonthlyFeeWaived_ReturnsFalse_Test()
        {
            Assert.Equal(false, testObject.IsMonthlyFeeWaived);
        }
    }
}
