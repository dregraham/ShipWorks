using ShipWorks.Shipping.Carriers.Postal.Usps.Registration.Promotion;
using Xunit;

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
        public void GetPromoCode_ReturnsShipWorks2()
        {
            string promo = testObject.GetPromoCode();

            Assert.Equal("ShipWorks2", promo);
        }

        [Fact]
        public void IsMonthlyFeeWaived_ReturnsFalse()
        {
            Assert.Equal(false, testObject.IsMonthlyFeeWaived);
        }
    }
}
