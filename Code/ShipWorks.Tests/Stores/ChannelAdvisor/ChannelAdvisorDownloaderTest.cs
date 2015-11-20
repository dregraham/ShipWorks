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
            Assert.Equal(ChannelAdvisorIsAmazonPrime.Unknown, ChannelAdvisorDownloader.GetIsPrime(string.Empty));
        }

        public void GetIsPrime_IsNo_ShippingClassIsAmazonButNotPrime()
        {
            Assert.Equal(ChannelAdvisorIsAmazonPrime.No, ChannelAdvisorDownloader.GetIsPrime("Test Amazon String"));
        }

        public void GetIsPrime_IsNo_ShippingClassIsPrimeButNotAmazon()
        {
            Assert.Equal(ChannelAdvisorIsAmazonPrime.No, ChannelAdvisorDownloader.GetIsPrime("Test Prime String"));
        }

        public void GetIsPrime_IsYes_ShippingClassIsAmazonPrime()
        {
            Assert.Equal(ChannelAdvisorIsAmazonPrime.Yes, ChannelAdvisorDownloader.GetIsPrime("Amazon Prime"));
        }
    }
}