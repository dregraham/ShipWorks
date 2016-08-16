using Xunit;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.Policies;

namespace ShipWorks.Tests.Shipping.Policies
{
    public class RateResultCountShippingPolicyTest
    {
        private readonly RateResultCountShippingPolicy testObject;

        private RateControl rateControl;

        public RateResultCountShippingPolicyTest()
        {
            testObject = new RateResultCountShippingPolicy();

            rateControl = new RateControl();
        }

        [Fact]
        public void Configure_QuantityIsOne_WhenDataIsEmptyString()
        {
            testObject.Configure(string.Empty);

            Assert.Equal(1, testObject.RateResultQuantity);
        }

        [Fact]
        public void Configure_QuantityIsOne_WhenDataIsDecimalFormat()
        {
            testObject.Configure("3.3");

            Assert.Equal(1, testObject.RateResultQuantity);
        }

        [Fact]
        public void Configure_QuantityIsOne_WhenDataHasAlphaCharacters()
        {
            testObject.Configure("3ABC");
            
            Assert.Equal(1, testObject.RateResultQuantity);
        }

        [Fact]
        public void Configure_QuantityIsOne_WhenDataContainsUnTrimmableWhitespace()
        {
            testObject.Configure("3 4");

            Assert.Equal(1, testObject.RateResultQuantity);
        }

        [Fact]
        public void Configure_AssignsQuantity_WhenDataIsInteger()
        {
            testObject.Configure("34");
            Assert.Equal(34, testObject.RateResultQuantity);
        }

        [Fact]
        public void Configure_AssignsQuantity_WhenDataIsInteger_AndContainsTrimmableWhitespace()
        {
            testObject.Configure(" 34 ");
            Assert.Equal(34, testObject.RateResultQuantity);
        }

        [Fact]
        public void Configure_RetainsGreatestQuantity_WhenPolicyIsConfiguredMultipleTimes()
        {
            testObject.Configure("12");
            testObject.Configure("3");
            testObject.Configure("16");
            testObject.Configure("2");

            Assert.Equal(16, testObject.RateResultQuantity);
        }

        [Fact]
        public void Configure_QuantityIsOne_WhenDataIsIntegerLessThanOne()
        {
            testObject.Configure("0");

            Assert.Equal(1, testObject.RateResultQuantity);
        }

        [Fact]
        public void IsApplicable_ReturnsFalse_WhenTargetIsNotRateControl()
        {
            testObject.IsApplicable("a string");
        }

        [Fact]
        public void IsApplicable_ReturnsTrue_WhenTargetIsRateControl()
        {
            testObject.IsApplicable(rateControl);
        }

        [Fact]
        public void Apply_AssignsShowAllRatesToFalse()
        {
            testObject.Apply(rateControl);

            Assert.False(rateControl.ShowAllRates);
        }

        [Fact]
        public void Apply_AssignsRateResultQuantity_ToRateControlRestrictedRateCount()
        {
            testObject.Configure("400");
            testObject.Apply(rateControl);

            Assert.Equal(testObject.RateResultQuantity, rateControl.RestrictedRateCount);
        }

        [Fact]
        public void Apply_AssignsShowSingleRateToFalse_WhenConfiguredQuantityIsNotOne()
        {
            testObject.Configure("3");
            testObject.Apply(rateControl);

            Assert.False(rateControl.ShowSingleRate);
        }

        [Fact]
        public void Apply_AssignsShowSingleRateToTrue_WhenConfiguredQuantityIsOne()
        {
            testObject.Configure("1");
            testObject.Apply(rateControl);

            Assert.True(rateControl.ShowSingleRate);
        }

        [Fact]
        public void Apply_DoesNotThrowException_WhenTargetIsNotRateControl()
        {
            testObject.Apply("not a rate control");
        }
    }
}
