﻿using ShipWorks.Stores.Platforms.Yahoo;
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
        public void DeserializeResponse_ReturnsPopulatedYahooResponseDto_WhenGivenValidXml_Test()
        {
            Assert.IsAssignableFrom<YahooResponse>(YahooApiWebClient.DeserializeResponse<YahooResponse>(validXml));
            Assert.NotNull(YahooApiWebClient.DeserializeResponse<YahooResponse>(validXml));
        }

        [Fact]
        public void DeserializeResponse_ThrowsYahooException_WhenGivenInvalidXml_Test()
        {
            Assert.Throws<YahooException>(() => YahooApiWebClient.DeserializeResponse<YahooResponse>(invalidXml));
        }
    }
}
