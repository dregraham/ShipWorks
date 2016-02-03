using ShipWorks.Stores.Platforms.Yahoo;
using ShipWorks.Stores.Platforms.Yahoo.ApiIntegration;
using ShipWorks.Stores.Platforms.Yahoo.ApiIntegration.DTO;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.Yahoo
{
    public class YahooApiWebClientTest
    {
        private readonly string validXml;
        private readonly string invalidXml;

        public YahooApiWebClientTest()
        {
            validXml = EmbeddedResourceHelper.GetEmbeddedResourceXml("ShipWorks.Stores.Tests.Platforms.Yahoo.Artifacts.YahooGetOrderResponse.xml");
            invalidXml = "This sure isn't valid xml";
        }

        [Fact]
        public void DeserializeResponse_ReturnsPopulatedYahooResponseDto_WhenGivenValidXml()
        {
            YahooResponse response = YahooApiWebClient.DeserializeResponse<YahooResponse>(validXml);

            Assert.IsAssignableFrom<YahooResponse>(response);
            Assert.NotNull(response);
        }

        [Fact]
        public void DeserializeResponse_ThrowsYahooException_WhenGivenInvalidXml()
        {
            Assert.Throws<YahooException>(() => YahooApiWebClient.DeserializeResponse<YahooResponse>(invalidXml));
        }
    }
}
