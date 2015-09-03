using ShipWorks.Stores.Platforms.Amazon.Mws;
using System;
using Xunit;

namespace ShipWorks.Tests.Stores.Amazon
{
    [Trait("Store", "Amazon")]
    public class AmazonMwsWebClientSettingsTest
    {
        [Fact]
        public void Endpoint_WithUnknownAmazonApiRegion_ReturnsUkEndpoint()
        {
            AmazonMwsConnection testConnection = new AmazonMwsConnection("", "", "unknown");

            AmazonMwsWebClientSettings testObject = new AmazonMwsWebClientSettings(testConnection);

            Assert.Equal("https://mws.amazonservices.co.uk", testObject.Endpoint);
        }

        [Fact]
        public void Endpoint_WithUsAmazonApiRegion_ReturnsUsEndpoint()
        {
            AmazonMwsConnection testConnection = new AmazonMwsConnection("", "", "US");

            AmazonMwsWebClientSettings testObject = new AmazonMwsWebClientSettings(testConnection);

            Assert.Equal("https://mws.amazonservices.com", testObject.Endpoint);
        }

        [Fact]
        public void Endpoint_WithMxAmazonApiRegion_ReturnsMxEndpoint()
        {
            AmazonMwsConnection testConnection = new AmazonMwsConnection("", "", "MX");

            AmazonMwsWebClientSettings testObject = new AmazonMwsWebClientSettings(testConnection);

            Assert.Equal("https://mws.amazonservices.com.mx", testObject.Endpoint);
        }


        [Fact]
        public void Endpoint_WithCaAmazonApiRegion_ReturnsCaEndpoint()
        {
            AmazonMwsConnection testConnection = new AmazonMwsConnection("", "", "CA");

            AmazonMwsWebClientSettings testObject = new AmazonMwsWebClientSettings(testConnection);

            Assert.Equal("https://mws.amazonservices.ca", testObject.Endpoint);
        }

        [Fact]
        public void InterapptiveAccessKeyID_WithNorthAmericaApiRegion_ReturnsNorthAmericaInterapptiveAccessKeyID()
        {
            AmazonMwsConnection testConnection = new AmazonMwsConnection("", "", "US");
   
            AmazonMwsWebClientSettings testObject = new AmazonMwsWebClientSettings(testConnection);

            Assert.Equal("FMrhIncQWseTBwglDs00lVdXyPVgObvu", testObject.InterapptiveAccessKeyID);
        }

        [Fact]
        public void InterapptiveAccessKeyID_WithNonNorthAmericaApiRegion_ReturnsNonNorthAmericaInterapptiveAccessKeyID()
        {
            AmazonMwsConnection testConnection = new AmazonMwsConnection("","","UK");

            AmazonMwsWebClientSettings testObject = new AmazonMwsWebClientSettings(testConnection);

            Assert.Equal("6bFMt0mymaWE0aWiaWT3SGs9LjvI//db", testObject.InterapptiveAccessKeyID);
        }

        [Fact]
        public void InterapptiveSecretKey_WithNorthAmericaApiRegion_ReturnsNorthAmericaInterapptiveSecretKey()
        {
            AmazonMwsConnection testConnection = new AmazonMwsConnection("", "", "US");

            AmazonMwsWebClientSettings testObject = new AmazonMwsWebClientSettings(testConnection);

            Assert.Equal("JIX6YaY03qfP5LO31sssIzlVV2kAskmIPw/mj7X+M3EQpsyocKz062su7+INVas5", testObject.InterapptiveSecretKey);
        }

        [Fact]
        public void InterapptiveSecretKey_WithNonNorthAmericaApiRegion_ReturnsNonNorthAmericaInterapptiveSecretKey()
        {
            AmazonMwsConnection testConnection = new AmazonMwsConnection("", "", "")
            {
                AmazonApiRegion = "UK"
            };

            AmazonMwsWebClientSettings testObject = new AmazonMwsWebClientSettings(testConnection);

            Assert.Equal("JjHvzq+MGZuxJu9EkjDv0QGSNQC/FYFg4lSe5PP5HMHRinkOWJhMLPeRH2057Ohd", testObject.InterapptiveSecretKey);
        }

        [Fact]
        public void GetApiEndpointPath_WithOrdersAPICall_ReturnsOrdersEndpoint()
        {
            AmazonMwsConnection testConnection = new AmazonMwsConnection("", "", "");

            AmazonMwsWebClientSettings testObject = new AmazonMwsWebClientSettings(testConnection);
            
            Assert.Equal("/Orders/2013-09-01", testObject.GetApiEndpointPath(AmazonMwsApiCall.ListOrders));
        }

        [Fact]
        public void GetActionName_WithOrdersAPICall_ReturnsOrdersActioon()
        {
            AmazonMwsConnection testConnection = new AmazonMwsConnection("", "", "");

            AmazonMwsWebClientSettings testObject = new AmazonMwsWebClientSettings(testConnection);

            Assert.Equal("ListOrders", testObject.GetActionName(AmazonMwsApiCall.ListOrders));
        }

        [Fact]
        public void GetApiNamespace_WithOrdersAPICall_ReturnsOrdersActioon()
        {
            AmazonMwsConnection testConnection = new AmazonMwsConnection("", "", "");

            AmazonMwsWebClientSettings testObject = new AmazonMwsWebClientSettings(testConnection);

            Assert.Equal("https://mws.amazonservices.com/Orders/2013-09-01", testObject.GetApiNamespace(AmazonMwsApiCall.ListOrders));
        }

        [Fact]
        public void GetApiNamespace_ForEachAmazonMwsApiCall_DoesNotThrowException()
        {
            AmazonMwsConnection testConnection = new AmazonMwsConnection("", "", "");

            AmazonMwsWebClientSettings testObject = new AmazonMwsWebClientSettings(testConnection);

            foreach (AmazonMwsApiCall call in Enum.GetValues(typeof(AmazonMwsApiCall)))
            {
                testObject.GetApiNamespace(call);
            }
        }

        [Fact]
        public void GetActionName_ForEachAmazonMwsApiCall_DoesNotThrowException()
        {
            AmazonMwsConnection testConnection = new AmazonMwsConnection("", "", "");

            AmazonMwsWebClientSettings testObject = new AmazonMwsWebClientSettings(testConnection);

            foreach (AmazonMwsApiCall call in Enum.GetValues(typeof(AmazonMwsApiCall)))
            {
                testObject.GetActionName(call);
            }
        }

        [Fact]
        public void GetApiVersion_ForEachAmazonMwsApiCall_DoesNotThrowException()
        {
            AmazonMwsConnection testConnection = new AmazonMwsConnection("", "", "");

            AmazonMwsWebClientSettings testObject = new AmazonMwsWebClientSettings(testConnection);

            foreach (AmazonMwsApiCall call in Enum.GetValues(typeof(AmazonMwsApiCall)))
            {
                testObject.GetApiVersion(call);
            }
        }

    }
}
