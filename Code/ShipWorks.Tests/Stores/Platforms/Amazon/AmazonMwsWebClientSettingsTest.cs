﻿using Autofac.Extras.Moq;
using ShipWorks.Stores.Platforms.Amazon;
using ShipWorks.Stores.Platforms.Amazon.Mws;
using System;
using Xunit;
using ShipWorks.Tests.Shared;
using ShipWorks.ApplicationCore.Licensing.WebClientEnvironments;
using Autofac;

namespace ShipWorks.Tests.Stores.Amazon
{
    [Trait("Store", "Amazon")]
    public class AmazonMwsWebClientSettingsTest
    {
        private WebClientEnvironment webClientEnvironment;

        public AmazonMwsWebClientSettingsTest()
        {
            webClientEnvironment = new WebClientEnvironment();
        }

        [Fact]
        public void Endpoint_WithUnknownAmazonApiRegion_ReturnsUkEndpoint()
        {
            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<IAmazonCredentials>().Setup(c => c.Region).Returns("UK");
                
                AmazonMwsWebClientSettings testObject = mock.Create<AmazonMwsWebClientSettings>(new TypedParameter(typeof(WebClientEnvironment), webClientEnvironment));

                Assert.Equal("https://mws.amazonservices.co.uk", testObject.Endpoint);
            }
        }

        [Fact]
        public void Endpoint_WithUsAmazonApiRegion_ReturnsUsEndpoint()
        {
            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<IAmazonCredentials>().Setup(c => c.Region).Returns("US");

                AmazonMwsWebClientSettings testObject = mock.Create<AmazonMwsWebClientSettings>(new TypedParameter(typeof(WebClientEnvironment), webClientEnvironment));

                Assert.Equal("https://mws.amazonservices.com", testObject.Endpoint);
            }
        }

        [Fact]
        public void Endpoint_WithMxAmazonApiRegion_ReturnsMxEndpoint()
        {
            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<IAmazonCredentials>().Setup(c => c.Region).Returns("MX");

                AmazonMwsWebClientSettings testObject = mock.Create<AmazonMwsWebClientSettings>(new TypedParameter(typeof(WebClientEnvironment), webClientEnvironment));

                Assert.Equal("https://mws.amazonservices.com.mx", testObject.Endpoint);
            }
        }


        [Fact]
        public void Endpoint_WithCaAmazonApiRegion_ReturnsCaEndpoint()
        {
            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<IAmazonCredentials>().Setup(c => c.Region).Returns("CA");

                AmazonMwsWebClientSettings testObject = mock.Create<AmazonMwsWebClientSettings>(new TypedParameter(typeof(WebClientEnvironment), webClientEnvironment));

                Assert.Equal("https://mws.amazonservices.ca", testObject.Endpoint);
            }
        }

        [Fact]
        public void InterapptiveAccessKeyID_WithNorthAmericaApiRegion_ReturnsNorthAmericaInterapptiveAccessKeyID()
        {
            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<IAmazonCredentials>().Setup(c => c.Region).Returns("US");

                AmazonMwsWebClientSettings testObject = mock.Create<AmazonMwsWebClientSettings>(new TypedParameter(typeof(WebClientEnvironment), webClientEnvironment));

                Assert.Equal("FMrhIncQWseTBwglDs00lVdXyPVgObvu", testObject.InterapptiveAccessKeyID);
            }
        }

        [Fact]
        public void InterapptiveAccessKeyID_WithNonNorthAmericaApiRegion_ReturnsNonNorthAmericaInterapptiveAccessKeyID()
        {
            using (var mock = AutoMock.GetLoose())
            {
                AmazonMwsWebClientSettings testObject = mock.Create<AmazonMwsWebClientSettings>(new TypedParameter(typeof(WebClientEnvironment), webClientEnvironment));

                Assert.Equal("6bFMt0mymaWE0aWiaWT3SGs9LjvI//db", testObject.InterapptiveAccessKeyID);
            }
        }

        [Fact]
        public void InterapptiveSecretKey_WithNorthAmericaApiRegion_ReturnsNorthAmericaInterapptiveSecretKey()
        {
            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<IAmazonCredentials>().Setup(c => c.Region).Returns("US");

                AmazonMwsWebClientSettings testObject = mock.Create<AmazonMwsWebClientSettings>(new TypedParameter(typeof(WebClientEnvironment), webClientEnvironment));

                Assert.Equal("JIX6YaY03qfP5LO31sssIzlVV2kAskmIPw/mj7X+M3EQpsyocKz062su7+INVas5", testObject.InterapptiveSecretKey);
            }
        }

        [Fact]
        public void InterapptiveSecretKey_WithNonNorthAmericaApiRegion_ReturnsNonNorthAmericaInterapptiveSecretKey()
        {
            using (var mock = AutoMock.GetLoose())
            {
                AmazonMwsWebClientSettings testObject = mock.Create<AmazonMwsWebClientSettings>(new TypedParameter(typeof(WebClientEnvironment), webClientEnvironment));

                Assert.Equal("JjHvzq+MGZuxJu9EkjDv0QGSNQC/FYFg4lSe5PP5HMHRinkOWJhMLPeRH2057Ohd", testObject.InterapptiveSecretKey);
            }
        }

        [Fact]
        public void GetApiEndpointPath_WithOrdersAPICall_ReturnsOrdersEndpoint()
        {
            using (var mock = AutoMock.GetLoose())
            {
                AmazonMwsWebClientSettings testObject = mock.Create<AmazonMwsWebClientSettings>(new TypedParameter(typeof(WebClientEnvironment), webClientEnvironment));


                Assert.Equal("/Orders/2013-09-01", testObject.GetApiEndpointPath(AmazonMwsApiCall.ListOrders));
            }
        }

        [Fact]
        public void GetActionName_WithOrdersAPICall_ReturnsOrdersActioon()
        {
            using (var mock = AutoMock.GetLoose())
            {
                AmazonMwsWebClientSettings testObject = mock.Create<AmazonMwsWebClientSettings>(new TypedParameter(typeof(WebClientEnvironment), webClientEnvironment));


                Assert.Equal("ListOrders", testObject.GetActionName(AmazonMwsApiCall.ListOrders));
            }
        }

        [Fact]
        public void GetApiNamespace_WithOrdersAPICall_ReturnsOrdersActioon()
        {
            using (var mock = AutoMock.GetLoose())
            {
                AmazonMwsWebClientSettings testObject = mock.Create<AmazonMwsWebClientSettings>(new TypedParameter(typeof(WebClientEnvironment), webClientEnvironment));


                Assert.Equal("https://mws.amazonservices.com/Orders/2013-09-01", testObject.GetApiNamespace(AmazonMwsApiCall.ListOrders));
            }
        }

        [Fact]
        public void GetApiNamespace_ForEachAmazonMwsApiCall_DoesNotThrowException()
        {
            using (var mock = AutoMock.GetLoose())
            {
                AmazonMwsWebClientSettings testObject = mock.Create<AmazonMwsWebClientSettings>(new TypedParameter(typeof(WebClientEnvironment), webClientEnvironment));

                foreach (AmazonMwsApiCall call in Enum.GetValues(typeof(AmazonMwsApiCall)))
                {
                    testObject.GetApiNamespace(call);
                }
            }
        }

        [Fact]
        public void GetActionName_ForEachAmazonMwsApiCall_DoesNotThrowException()
        {
            using (var mock = AutoMock.GetLoose())
            {
                AmazonMwsWebClientSettings testObject = mock.Create<AmazonMwsWebClientSettings>(new TypedParameter(typeof(WebClientEnvironment), webClientEnvironment));

                foreach (AmazonMwsApiCall call in Enum.GetValues(typeof(AmazonMwsApiCall)))
                {
                    testObject.GetActionName(call);
                }
            }
        }

        [Fact]
        public void GetApiVersion_ForEachAmazonMwsApiCall_DoesNotThrowException()
        {
            using (var mock = AutoMock.GetLoose())
            {                
                AmazonMwsWebClientSettings testObject = mock.Create<AmazonMwsWebClientSettings>(new TypedParameter(typeof(WebClientEnvironment), webClientEnvironment));

                foreach (AmazonMwsApiCall call in Enum.GetValues(typeof(AmazonMwsApiCall)))
                {
                    testObject.GetApiVersion(call);
                }
            }
        }
    }
}
