using System;
using System.Text;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using ShipWorks.Stores.Platforms.Newegg.Net.Orders.Shipping;
using ShipWorks.Stores.Platforms.Newegg.Enums;
using ShipWorks.Stores.Platforms.Newegg.Net.Orders.Shipping.Response;

namespace ShipWorks.Tests.Stores.Newegg
{
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

        [Fact]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Deserialize_ThrowsInvalidOperationException_WhenDeserializingErrorResponseXml_Test()
        {
            serializer.Deserialize(errorResponseXml);
        }

        [Fact]
        public void Deserialize_ReturnsShippingResult_WhenDeserializingSuccessfulResponseXml_Test()
        {
            object result = serializer.Deserialize(successfulResponseXml);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ShippingResult));
        }

        [Fact]
        public void Deserialize_ShippingResultContainsPackageSummary_WhenDeserializingSuccessfulResponseXml_Test()
        {
            ShippingResult result = serializer.Deserialize(successfulResponseXml) as ShippingResult;
            Assert.IsNotNull(result.PackageSummary);            
        }

        [Fact]
        public void Deserialize_ShippingResultIsSuccessful_WhenDeserializingSuccessfulResponseXml_Test()
        {
            ShippingResult result = serializer.Deserialize(successfulResponseXml) as ShippingResult;
            Assert.IsTrue(result.IsSuccessful);
        }
        
        [Fact]
        public void Deserialize_ShippingResultContainsDetail_WhenDeserializingSuccessfulResponseXml_Test()
        {
            ShippingResult result = serializer.Deserialize(successfulResponseXml) as ShippingResult;
            Assert.IsNotNull(result.Detail);
        }

        [Fact]
        public void Deserialize_DetailContainsOrderNumber_WhenDeserializingSuccessfulResponseXml_Test()
        {
            ShippingResult result = serializer.Deserialize(successfulResponseXml) as ShippingResult;
            Assert.AreEqual(159243598, result.Detail.OrderNumber);
        }

        [Fact]
        public void Deserialize_DetailContainsSellerId_WhenDeserializingSuccessfulResponseXml_Test()
        {
            ShippingResult result = serializer.Deserialize(successfulResponseXml) as ShippingResult;
            Assert.AreEqual("A006", result.Detail.SellerId);
        }

        [Fact]
        public void Deserialize_DetailContainsOrderStatus_WhenDeserializingSuccessfulResponseXml_Test()
        {
            ShippingResult result = serializer.Deserialize(successfulResponseXml) as ShippingResult;
            Assert.AreEqual("Shipped", result.Detail.OrderStatus);
        }

        [Fact]
        public void Deserialize_DetailContainsShipment_WhenDeserializingSuccessfulResponseXml_Test()
        {
            ShippingResult result = serializer.Deserialize(successfulResponseXml) as ShippingResult;
            Assert.IsNotNull(result.Detail.Shipment);
        }

        [Fact]
        public void Deserialize_ShipmentContainsPackages_WhenDeserializingSuccessfulResponseXml_Test()
        {
            ShippingResult result = serializer.Deserialize(successfulResponseXml) as ShippingResult;

            Assert.IsNotNull(result.Detail.Shipment.Packages);
            Assert.AreEqual(1, result.Detail.Shipment.Packages.Count);
        }

        [Fact]
        public void Deserialize_PackageContainsTrackingNumber_WhenDeserializingSuccessfulResponseXml_Test()
        {
            ShippingResult result = serializer.Deserialize(successfulResponseXml) as ShippingResult;
            Assert.AreEqual("lztestA0060001", result.Detail.Shipment.Packages[0].TrackingNumber);
        }

        [Fact]
        public void Deserialize_PackageContainsShipDate_WhenDeserializingSuccessfulResponseXml_Test()
        {
            ShippingResult result = serializer.Deserialize(successfulResponseXml) as ShippingResult;
            Assert.AreEqual(DateTime.Parse("2012-02-10T15:30:01"), result.Detail.Shipment.Packages[0].ShipDateInPacificStandardTime);
        }

        [Fact]
        public void Deserialize_PackageIsSuccessfullyProcessed_WhenDeserializingSuccessfulResponseXml_Test()
        {
            ShippingResult result = serializer.Deserialize(successfulResponseXml) as ShippingResult;
            Assert.IsTrue(result.Detail.Shipment.Packages[0].IsSuccessfullyProcessed);
        }

        [Fact]
        public void Deserialize_PackageContainsProcessingDescription_WhenDeserializingSuccessfulResponseXml_Test()
        {
            ShippingResult result = serializer.Deserialize(successfulResponseXml) as ShippingResult;
            Assert.AreEqual("Success", result.Detail.Shipment.Packages[0].ProcessingDescription);
        }

        [Fact]
        public void Deserialize_PackageContainsItems_WhenDeserializingSuccessfulResponseXml_Test()
        {
            ShippingResult result = serializer.Deserialize(successfulResponseXml) as ShippingResult;

            Assert.IsNotNull(result.Detail.Shipment.Packages[0].Items);
            Assert.AreEqual(1, result.Detail.Shipment.Packages[0].Items.Count);
        }

        [Fact]
        public void Deserialize_ItemContainsSellerPartNumber_WhenDeserializingSuccessfulResponseXml_Test()
        {
            ShippingResult result = serializer.Deserialize(successfulResponseXml) as ShippingResult;
            Assert.AreEqual("A006ZX-35833", result.Detail.Shipment.Packages[0].Items[0].SellerPartNumber);
        }

        [Fact]
        public void Deserialize_ItemContainsNeweggItemNumber_WhenDeserializingSuccessfulResponseXml_Test()
        {
            ShippingResult result = serializer.Deserialize(successfulResponseXml) as ShippingResult;
            Assert.AreEqual("9SIA0060845543", result.Detail.Shipment.Packages[0].Items[0].NeweggItemNumber);
        }

    }
}
