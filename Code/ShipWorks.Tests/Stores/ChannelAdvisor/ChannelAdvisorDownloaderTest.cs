using ShipWorks.Stores.Platforms.ChannelAdvisor;
using ShipWorks.Stores.Platforms.ChannelAdvisor.Enums;
using Xunit;

namespace ShipWorks.Tests.Stores.ChannelAdvisor
{
    public class ChannelAdvisorDownloaderTest
    {
        [Fact]
        public void GetIsPrime_IsUnknown_ShippingClassIsEmpty()
        {
            Assert.Equal(ChannelAdvisorIsAmazonPrime.Unknown, ChannelAdvisorDownloader.GetIsPrime(string.Empty, "Test Amazon"));
        }

        [Fact]
        public void GetIsPrime_IsUnknown_CarrierIsEmpty()
        {
            Assert.Equal(ChannelAdvisorIsAmazonPrime.Unknown, ChannelAdvisorDownloader.GetIsPrime("Prime", string.Empty));
        }

        [Fact]
        public void GetIsPrime_IsNo_ShippingClassIsAmazonButNotPrime()
        {
            Assert.Equal(ChannelAdvisorIsAmazonPrime.No, ChannelAdvisorDownloader.GetIsPrime("Test Amazon String", "Test Amazon"));
        }

        [Fact]
        public void GetIsPrime_IsNo_ShippingClassIsPrimeButNotAmazon()
        {
            Assert.Equal(ChannelAdvisorIsAmazonPrime.No, ChannelAdvisorDownloader.GetIsPrime("Test Prime String", "USPS"));
        }

        [Fact]
        public void GetIsPrime_IsYes_ShippingClassIsAmazonPrime()
        {
            Assert.Equal(ChannelAdvisorIsAmazonPrime.Yes, ChannelAdvisorDownloader.GetIsPrime("Test Prime String", "Test Amazon Carrier"));
        }
    }
}