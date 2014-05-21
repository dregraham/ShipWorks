using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShipWorks.Stores.Platforms.Amazon.Mws;
using ShipWorks.Data.Model.EntityClasses;
using Interapptive.Shared.Utility;

namespace ShipWorks.Tests.Stores.Amazon
{
    [TestClass]
    public class MwsClientTests
    {
        [TestMethod]
        public void ClockSyncTest()
        {
            using (AmazonMwsClient client = new AmazonMwsClient(new AmazonStoreEntity {AmazonApiRegion = "US"}))
            {
                Assert.IsTrue(client.ClockInSyncWithMWS());   
            }
        }

        [TestMethod]
        public void ValidateSigning()
        {
            string stringToSign = "POST\nmws.amazonservices.com\n/Orders/2011-01-01\nAWSAccessKeyId=AKIAJU465C2O2U6WNQTA&Action=ListOrderItems&AmazonOrderId=SHIPWORKS_CONNECT_ATTEMPT&SellerId=A2OIXR4TA6XKOD&SignatureMethod=HmacSHA256&SignatureVersion=2&Timestamp=2011-08-19T15%3A55%3A39Z&Version=2011-01-01";
            string signature = "XKab7VXEklL8EYE0HcqRF97fgjqTNsM7NFe/gJ7eWLE=";

            string signed = RequestSignature.CreateRequestSignature(stringToSign, "0+NcTFU11/qooQriFFO7k0JxY64t/P38DIAAAgeW", SigningAlgorithm.SHA256);

            Assert.AreEqual(signature, signed);
        }
    }
}
