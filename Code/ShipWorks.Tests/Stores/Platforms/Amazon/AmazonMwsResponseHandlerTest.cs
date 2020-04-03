using System;
using Autofac.Extras.Moq;
using Interapptive.Shared.Net;
using Interapptive.Shared.Utility;
using Moq;
using ShipWorks.ApplicationCore;
using ShipWorks.ApplicationCore.Licensing.WebClientEnvironments;
using ShipWorks.Stores.Platforms.Amazon;
using ShipWorks.Stores.Platforms.Amazon.Mws;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Tests.Stores.Amazon
{
    [Trait("Store", "Amazon")]
    public class AmazonMwsResponseHandlerTest
    {
        private Mock<IHttpResponseReader> mockedHttpResponseReader;
        private readonly AmazonMwsWebClientSettings mwsSettings;
        private const string amazonGetServiceStatusResponseFormat =
            "<?xml version=\"1.0\"?><GetServiceStatusResponse xmlns=\"https://mws.amazonservices.com/Orders/2013-09-01\"><GetServiceStatusResult><Status>{0}</Status><Timestamp>2015-08-24T18:25:28.458Z</Timestamp></GetServiceStatusResult><ResponseMetadata><RequestId>722ad8a6-9571-416c-ad1f-2052d291d434</RequestId></ResponseMetadata></GetServiceStatusResponse>";
        private const string amazonListOrdersResponseFormat =
            "<?xml version=\"1.0\"?><ListOrdersResponse xmlns=\"https://mws.amazonservices.com/Orders/2013-09-01\"><ListOrdersResult><Orders>{0}</Orders><LastUpdatedBefore>2015-08-24T18:23:28Z</LastUpdatedBefore></ListOrdersResult><ResponseMetadata><RequestId>4cde7bfc-c0c9-4e31-810b-789f176a9bc4</RequestId></ResponseMetadata></ListOrdersResponse>";

        public AmazonMwsResponseHandlerTest()
        {
            //mwsConnection = new AmazonMwsConnection("", "US", "");

            var mock = AutoMock.GetLoose();

            IAmazonCredentials creds = mock.Build<IAmazonCredentials>();
            IInterapptiveOnly interapptiveOnly = mock.Build<IInterapptiveOnly>();

            mwsSettings = new AmazonMwsWebClientSettings(creds, interapptiveOnly, new WebClientEnvironment());

            //Setup mock object that holds response from request
            mockedHttpResponseReader = new Mock<IHttpResponseReader>();
        }

        [Fact]
        public void RaiseErrors_WhenNoErrorPresent_DoesNotThrowAmazonException_()
        {
            mockedHttpResponseReader.Setup(r => r.ReadResult()).Returns(String.Format(amazonListOrdersResponseFormat, ""));

            AmazonMwsResponseHandler.RaiseErrors(AmazonMwsApiCall.GetServiceStatus, mockedHttpResponseReader.Object, mwsSettings);
        }

        [Fact]
        public void RaiseErrors_throws_AmazonException_When_Error_Present()
        {
            mockedHttpResponseReader.Setup(r => r.ReadResult()).Returns(String.Format(amazonListOrdersResponseFormat, "<Error><Code>1</Code><Message>Some Message</Message></Error>"));

            Assert.Throws<AmazonException>(() => AmazonMwsResponseHandler.RaiseErrors(AmazonMwsApiCall.GetServiceStatus, mockedHttpResponseReader.Object, mwsSettings));
        }

        [Fact]
        public void GetXPathNavigator_Returns_XPathNamespaceNavigator()
        {
            mockedHttpResponseReader.Setup(r => r.ReadResult()).Returns(String.Format(amazonGetServiceStatusResponseFormat, "GREEN"));

            XPathNamespaceNavigator xpath = AmazonMwsResponseHandler.GetXPathNavigator(mockedHttpResponseReader.Object, AmazonMwsApiCall.GetServiceStatus, mwsSettings);

            Assert.IsAssignableFrom<XPathNamespaceNavigator>(xpath);
        }
    }
}
