using Interapptive.Shared.Enums;
using ShipWorks.Stores.Platforms.Amazon;
using ShipWorks.Stores.Platforms.ChannelAdvisor;
using ShipWorks.Stores.Platforms.ChannelAdvisor.Enums;
using Xunit;

namespace ShipWorks.Tests.Stores.ChannelAdvisor
{
    public class ChannelAdvisorHelperTest
    {
        [Fact]
        public void GetIsPrime_IsUnknown_WhenShippingClassIsNull()
        {
            Assert.Equal(AmazonIsPrime.Unknown, ChannelAdvisorHelper.GetIsPrime(null, "Test Amazon"));
        }

        [Fact]
        public void GetIsPrime_IsUnknown_WhenCarrierIsNull()
        {
            Assert.Equal(AmazonIsPrime.Unknown, ChannelAdvisorHelper.GetIsPrime("Prime", null));
        }

        [Fact]
        public void GetIsPrime_IsUnknown_WhenShippingClassIsEmpty()
        {
            Assert.Equal(AmazonIsPrime.Unknown, ChannelAdvisorHelper.GetIsPrime(string.Empty, "Test Amazon"));
        }

        [Fact]
        public void GetIsPrime_IsUnknown_WhenCarrierIsEmpty()
        {
            Assert.Equal(AmazonIsPrime.Unknown, ChannelAdvisorHelper.GetIsPrime("Prime", string.Empty));
        }

        [Fact]
        public void GetIsPrime_IsNo_WhenShippingClassIsAmazonButNotPrime()
        {
            Assert.Equal(AmazonIsPrime.No, ChannelAdvisorHelper.GetIsPrime("Test Amazon String", "Test Amazon"));
        }

        [Fact]
        public void GetIsPrime_IsNo_WhenShippingClassIsPrimeButNotAmazon()
        {
            Assert.Equal(AmazonIsPrime.No, ChannelAdvisorHelper.GetIsPrime("Test Prime String", "USPS"));
        }

        [Fact]
        public void GetIsPrime_IsYes_WhenShippingClassIsAmazonPrime()
        {
            Assert.Equal(AmazonIsPrime.Yes, ChannelAdvisorHelper.GetIsPrime("Test Prime String", "Test Amazon Carrier"));
        }

        [Fact]
        public void GetIsPrime_IsYes_WhenShippingClassIsPrimeAndIsMixedCase()
        {
            Assert.Equal(AmazonIsPrime.Yes, ChannelAdvisorHelper.GetIsPrime("pRiMe", "AmAzoN"));
        }
    }
}