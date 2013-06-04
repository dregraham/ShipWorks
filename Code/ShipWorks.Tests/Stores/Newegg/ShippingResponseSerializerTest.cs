using System;
using System.Text;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShipWorks.Stores.Platforms.Newegg.Net.Orders.Shipping;
using ShipWorks.Stores.Platforms.Newegg.Enums;
using ShipWorks.Stores.Platforms.Newegg.Net.Orders.Shipping.Response;

namespace ShipWorks.Tests.Stores.Newegg
{
    [TestClass]
    public class ShippingResponseSerializerTest
    {
        private string successfulResponseXml;
        private string errorResponseXml;

        private ShippingResponseSerializer serializer;

        [TestInitialize]
        public void Initialize()
        {
            serializer = new ShippingResponseSerializer();

            successfulResponseXml = GetSuccessfulShippingXml();

            errorResponseXml = @"<?xml version=""1.0"" encoding=""utf-8""?>
<Errors xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"">
  <Error>
    <Code>CE010</Code>
    <Message>Invalid Date From.</Message>
  </Error>
</Errors>";

        }

        private string GetSuccessfulShippingXml()
        {
            string xml = string.Empty;
            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("ShipWorks.Tests.Stores.Newegg.Artifacts.ShippingResult.xml"))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    xml = reader.ReadToEnd();
                }
            }

            return xml;
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Deserialize_ThrowsInvalidOperationException_WhenDeserializingErrorResponseXml_Test()
        {
            serializer.Deserialize(errorResponseXml);
        }

        [TestMethod]
        public void Deserialize_ReturnsShippingResult_WhenDeserializingSuccessfulResponseXml_Test()
        {
            object result = serializer.Deserialize(successfulResponseXml);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ShippingResult));
        }

        [TestMethod]
        public void Deserialize_ShippingResultContainsPackageSummary_WhenDeserializingSuccessfulResponseXml_Test()
        {
            ShippingResult result = serializer.Deserialize(successfulResponseXml) as ShippingResult;
            Assert.IsNotNull(result.PackageSummary);            
        }

        [TestMethod]
        public void Deserialize_ShippingResultIsSuccessful_WhenDeserializingSuccessfulResponseXml_Test()
        {
            ShippingResult result = serializer.Deserialize(successfulResponseXml) as ShippingResult;
            Assert.IsTrue(result.IsSuccessful);
        }
        
        [TestMethod]
        public void Deserialize_ShippingResultContainsDetail_WhenDeserializingSuccessfulResponseXml_Test()
        {
            ShippingResult result = serializer.Deserialize(successfulResponseXml) as ShippingResult;
            Assert.IsNotNull(result.Detail);
        }

        [TestMethod]
        public void Deserialize_DetailContainsOrderNumber_WhenDeserializingSuccessfulResponseXml_Test()
        {
            ShippingResult result = serializer.Deserialize(successfulResponseXml) as ShippingResult;
            Assert.AreEqual(159243598, result.Detail.OrderNumber);
        }

        [TestMethod]
        public void Deserialize_DetailContainsSellerId_WhenDeserializingSuccessfulResponseXml_Test()
        {
            ShippingResult result = serializer.Deserialize(successfulResponseXml) as ShippingResult;
            Assert.AreEqual("A006", result.Detail.SellerId);
        }

        [TestMethod]
        public void Deserialize_DetailContainsOrderStatus_WhenDeserializingSuccessfulResponseXml_Test()
        {
            ShippingResult result = serializer.Deserialize(successfulResponseXml) as ShippingResult;
            Assert.AreEqual("Shipped", result.Detail.OrderStatus);
        }

        [TestMethod]
        public void Deserialize_DetailContainsShipment_WhenDeserializingSuccessfulResponseXml_Test()
        {
            ShippingResult result = serializer.Deserialize(successfulResponseXml) as ShippingResult;
            Assert.IsNotNull(result.Detail.Shipment);
        }

        [TestMethod]
        public void Deserialize_ShipmentContainsPackages_WhenDeserializingSuccessfulResponseXml_Test()
        {
            ShippingResult result = serializer.Deserialize(successfulResponseXml) as ShippingResult;

            Assert.IsNotNull(result.Detail.Shipment.Packages);
            Assert.AreEqual(1, result.Detail.Shipment.Packages.Count);
        }

        [TestMethod]
        public void Deserialize_PackageContainsTrackingNumber_WhenDeserializingSuccessfulResponseXml_Test()
        {
            ShippingResult result = serializer.Deserialize(successfulResponseXml) as ShippingResult;
            Assert.AreEqual("lztestA0060001", result.Detail.Shipment.Packages[0].TrackingNumber);
        }

        [TestMethod]
        public void Deserialize_PackageContainsShipDate_WhenDeserializingSuccessfulResponseXml_Test()
        {
            ShippingResult result = serializer.Deserialize(successfulResponseXml) as ShippingResult;
            Assert.AreEqual(DateTime.Parse("2012-02-10T15:30:01"), result.Detail.Shipment.Packages[0].ShipDateInPacificStandardTime);
        }

        [TestMethod]
        public void Deserialize_PackageIsSuccessfullyProcessed_WhenDeserializingSuccessfulResponseXml_Test()
        {
            ShippingResult result = serializer.Deserialize(successfulResponseXml) as ShippingResult;
            Assert.IsTrue(result.Detail.Shipment.Packages[0].IsSuccessfullyProcessed);
        }

        [TestMethod]
        public void Deserialize_PackageContainsProcessingDescription_WhenDeserializingSuccessfulResponseXml_Test()
        {
            ShippingResult result = serializer.Deserialize(successfulResponseXml) as ShippingResult;
            Assert.AreEqual("Success", result.Detail.Shipment.Packages[0].ProcessingDescription);
        }

        [TestMethod]
        public void Deserialize_PackageContainsItems_WhenDeserializingSuccessfulResponseXml_Test()
        {
            ShippingResult result = serializer.Deserialize(successfulResponseXml) as ShippingResult;

            Assert.IsNotNull(result.Detail.Shipment.Packages[0].Items);
            Assert.AreEqual(1, result.Detail.Shipment.Packages[0].Items.Count);
        }

        [TestMethod]
        public void Deserialize_ItemContainsSellerPartNumber_WhenDeserializingSuccessfulResponseXml_Test()
        {
            ShippingResult result = serializer.Deserialize(successfulResponseXml) as ShippingResult;
            Assert.AreEqual("A006ZX-35833", result.Detail.Shipment.Packages[0].Items[0].SellerPartNumber);
        }

        [TestMethod]
        public void Deserialize_ItemContainsNeweggItemNumber_WhenDeserializingSuccessfulResponseXml_Test()
        {
            ShippingResult result = serializer.Deserialize(successfulResponseXml) as ShippingResult;
            Assert.AreEqual("9SIA0060845543", result.Detail.Shipment.Packages[0].Items[0].NeweggItemNumber);
        }

    }
}
