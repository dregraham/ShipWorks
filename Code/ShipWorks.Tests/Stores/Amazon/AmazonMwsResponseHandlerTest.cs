using Interapptive.Shared.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Stores.Platforms.Amazon.Mws;
using Interapptive.Shared.Utility;
using ShipWorks.Stores.Platforms.Amazon;

namespace ShipWorks.Tests.Stores.Amazon
{
    [TestClass]
    public class AmazonMwsResponseHandlerTest
    {
        Mock<IHttpResponseReader> mockedHttpResponseReader;

        AmazonMwsConnection mwsConnection;

        AmazonMwsWebClientSettings mwsSettings;

        const string amazonGetServiceStatusResponseFormat =
            "<?xml version=\"1.0\"?><GetServiceStatusResponse xmlns=\"https://mws.amazonservices.com/Orders/2013-09-01\"><GetServiceStatusResult><Status>{0}</Status><Timestamp>2015-08-24T18:25:28.458Z</Timestamp></GetServiceStatusResult><ResponseMetadata><RequestId>722ad8a6-9571-416c-ad1f-2052d291d434</RequestId></ResponseMetadata></GetServiceStatusResponse>";

        const string amazonListOrdersResponseFormat =
            "<?xml version=\"1.0\"?><ListOrdersResponse xmlns=\"https://mws.amazonservices.com/Orders/2013-09-01\"><ListOrdersResult><Orders>{0}</Orders><LastUpdatedBefore>2015-08-24T18:23:28Z</LastUpdatedBefore></ListOrdersResult><ResponseMetadata><RequestId>4cde7bfc-c0c9-4e31-810b-789f176a9bc4</RequestId></ResponseMetadata></ListOrdersResponse>";

        [TestInitialize]
        public void Initialize()
        {
            mwsConnection = new AmazonMwsConnection()
            {
                MerchantId = "",
                AmazonApiRegion = "US",
                AuthToken = ""
            };

            mwsSettings = new AmazonMwsWebClientSettings(mwsConnection);

            //Setup mock object that holds response from request
            mockedHttpResponseReader = new Mock<IHttpResponseReader>();
            
        }

        [TestMethod]
        public void RaiseErrors_WhenNoErrorPresent_DoesNotThrowAmazonException_()
        {
            mockedHttpResponseReader.Setup(r => r.ReadResult()).Returns(String.Format(amazonListOrdersResponseFormat, ""));

            try
            {
                AmazonMwsResponseHandler.RaiseErrors(AmazonMwsApiCall.GetServiceStatus, mockedHttpResponseReader.Object, mwsSettings);
            }
            catch (AmazonException)
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void RaiseErrors_throws_AmazonException_When_Error_Present()
        {
            mockedHttpResponseReader.Setup(r => r.ReadResult()).Returns(String.Format(amazonListOrdersResponseFormat, "<Error><Code>1</Code><Message>Some Message</Message></Error>"));

            try
            {
                AmazonMwsResponseHandler.RaiseErrors(AmazonMwsApiCall.GetServiceStatus, mockedHttpResponseReader.Object, mwsSettings);
                Assert.Fail();
            }
            catch (AmazonException)
            {

            }
        }

        [TestMethod]
        public void GetXPathNavigator_Returns_XPathNamespaceNavigator()
        {
            mockedHttpResponseReader.Setup(r => r.ReadResult()).Returns(String.Format(amazonGetServiceStatusResponseFormat, "GREEN"));

            XPathNamespaceNavigator xpath = AmazonMwsResponseHandler.GetXPathNavigator(mockedHttpResponseReader.Object, AmazonMwsApiCall.GetServiceStatus, mwsSettings);

            Assert.IsInstanceOfType(xpath, typeof(XPathNamespaceNavigator));
        }

    }



}
