using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShipWorks.Stores.Platforms.Amazon.Mws;
using System;

namespace ShipWorks.Tests.Stores.Amazon
{
    [TestClass]
    public class AmazonMwsWebClientSettingsTest
    {
        [TestMethod]
        public void Endpoint_WithUnknownAmazonApiRegion_ReturnsUkEndpoint()
        {
            AmazonMwsConnection testConnection = new AmazonMwsConnection("", "", "unknown");

            AmazonMwsWebClientSettings testObject = new AmazonMwsWebClientSettings(testConnection);

            Assert.AreEqual("https://mws.amazonservices.co.uk", testObject.Endpoint);
        }

        [TestMethod]
        public void Endpoint_WithUsAmazonApiRegion_ReturnsUsEndpoint()
        {
            AmazonMwsConnection testConnection = new AmazonMwsConnection("", "", "US");

            AmazonMwsWebClientSettings testObject = new AmazonMwsWebClientSettings(testConnection);

            Assert.AreEqual("https://mws.amazonservices.com", testObject.Endpoint);
        }

        [TestMethod]
        public void Endpoint_WithMxAmazonApiRegion_ReturnsMxEndpoint()
        {
            AmazonMwsConnection testConnection = new AmazonMwsConnection("", "", "MX");

            AmazonMwsWebClientSettings testObject = new AmazonMwsWebClientSettings(testConnection);

            Assert.AreEqual("https://mws.amazonservices.com.mx", testObject.Endpoint);
        }


        [TestMethod]
        public void Endpoint_WithCaAmazonApiRegion_ReturnsCaEndpoint()
        {
            AmazonMwsConnection testConnection = new AmazonMwsConnection("", "", "CA");

            AmazonMwsWebClientSettings testObject = new AmazonMwsWebClientSettings(testConnection);

            Assert.AreEqual("https://mws.amazonservices.ca", testObject.Endpoint);
        }

        [TestMethod]
        public void InterapptiveAccessKeyID_WithNorthAmericaApiRegion_ReturnsNorthAmericaInterapptiveAccessKeyID()
        {
            AmazonMwsConnection testConnection = new AmazonMwsConnection("", "", "US");
   
            AmazonMwsWebClientSettings testObject = new AmazonMwsWebClientSettings(testConnection);

            Assert.AreEqual("FMrhIncQWseTBwglDs00lVdXyPVgObvu", testObject.InterapptiveAccessKeyID);
        }

        [TestMethod]
        public void InterapptiveAccessKeyID_WithNonNorthAmericaApiRegion_ReturnsNonNorthAmericaInterapptiveAccessKeyID()
        {
            AmazonMwsConnection testConnection = new AmazonMwsConnection("","","UK");

            AmazonMwsWebClientSettings testObject = new AmazonMwsWebClientSettings(testConnection);

            Assert.AreEqual("6bFMt0mymaWE0aWiaWT3SGs9LjvI//db", testObject.InterapptiveAccessKeyID);
        }

        [TestMethod]
        public void InterapptiveSecretKey_WithNorthAmericaApiRegion_ReturnsNorthAmericaInterapptiveSecretKey()
        {
            AmazonMwsConnection testConnection = new AmazonMwsConnection("", "", "US");

            AmazonMwsWebClientSettings testObject = new AmazonMwsWebClientSettings(testConnection);

            Assert.AreEqual("JIX6YaY03qfP5LO31sssIzlVV2kAskmIPw/mj7X+M3EQpsyocKz062su7+INVas5", testObject.InterapptiveSecretKey);
        }

        [TestMethod]
        public void InterapptiveSecretKey_WithNonNorthAmericaApiRegion_ReturnsNonNorthAmericaInterapptiveSecretKey()
        {
            AmazonMwsConnection testConnection = new AmazonMwsConnection("", "", "")
            {
                AmazonApiRegion = "UK"
            };

            AmazonMwsWebClientSettings testObject = new AmazonMwsWebClientSettings(testConnection);

            Assert.AreEqual("JjHvzq+MGZuxJu9EkjDv0QGSNQC/FYFg4lSe5PP5HMHRinkOWJhMLPeRH2057Ohd", testObject.InterapptiveSecretKey);
        }

        [TestMethod]
        public void GetApiEndpointPath_WithOrdersAPICall_ReturnsOrdersEndpoint()
        {
            AmazonMwsConnection testConnection = new AmazonMwsConnection("", "", "");

            AmazonMwsWebClientSettings testObject = new AmazonMwsWebClientSettings(testConnection);
            
            Assert.AreEqual("/Orders/2013-09-01", testObject.GetApiEndpointPath(AmazonMwsApiCall.ListOrders));
        }

        [TestMethod]
        public void GetActionName_WithOrdersAPICall_ReturnsOrdersActioon()
        {
            AmazonMwsConnection testConnection = new AmazonMwsConnection("", "", "");

            AmazonMwsWebClientSettings testObject = new AmazonMwsWebClientSettings(testConnection);

            Assert.AreEqual("ListOrders", testObject.GetActionName(AmazonMwsApiCall.ListOrders));
        }

        [TestMethod]
        public void GetApiNamespace_WithOrdersAPICall_ReturnsOrdersActioon()
        {
            AmazonMwsConnection testConnection = new AmazonMwsConnection("", "", "");

            AmazonMwsWebClientSettings testObject = new AmazonMwsWebClientSettings(testConnection);

            Assert.AreEqual("https://mws.amazonservices.com/Orders/2013-09-01", testObject.GetApiNamespace(AmazonMwsApiCall.ListOrders));
        }

        [TestMethod]
        public void GetApiNamespace_ForEachAmazonMwsApiCall_DoesNotThrowException()
        {
            AmazonMwsConnection testConnection = new AmazonMwsConnection("", "", "");

            AmazonMwsWebClientSettings testObject = new AmazonMwsWebClientSettings(testConnection);

            foreach (AmazonMwsApiCall call in Enum.GetValues(typeof(AmazonMwsApiCall)))
            {
                testObject.GetApiNamespace(call);
            }
        }

        [TestMethod]
        public void GetActionName_ForEachAmazonMwsApiCall_DoesNotThrowException()
        {
            AmazonMwsConnection testConnection = new AmazonMwsConnection("", "", "");

            AmazonMwsWebClientSettings testObject = new AmazonMwsWebClientSettings(testConnection);

            foreach (AmazonMwsApiCall call in Enum.GetValues(typeof(AmazonMwsApiCall)))
            {
                testObject.GetActionName(call);
            }
        }

        [TestMethod]
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
