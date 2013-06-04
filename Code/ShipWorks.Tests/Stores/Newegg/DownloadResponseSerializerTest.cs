using System;
using System.Text;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShipWorks.Stores.Platforms.Newegg.Net.Orders.Download;
using ShipWorks.Stores.Platforms.Newegg.Enums;
using ShipWorks.Stores.Platforms.Newegg.Net.Orders.Download.Response;

namespace ShipWorks.Tests.Stores.Newegg
{
    [TestClass]
    public class DownloadResponseSerializerTest
    {
        private string successfulDownloadResponseXml;
        private string errorResponseXml; 

        private DownloadResponseSerializer serializer;

        [TestInitialize]
        public void Initialize()
        {
            serializer = new DownloadResponseSerializer();
            
            successfulDownloadResponseXml = GetSuccessfulOrderDownloadXml();

            errorResponseXml = @"<?xml version=""1.0"" encoding=""utf-8""?>
<Errors xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"">
  <Error>
    <Code>CE010</Code>
    <Message>Invalid Date From.</Message>
  </Error>
</Errors>";
            
        }

        private string GetSuccessfulOrderDownloadXml()
        {
            string xml = string.Empty;
            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("ShipWorks.Tests.Stores.Newegg.Artifacts.DownloadedOrders.xml"))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    xml = reader.ReadToEnd();
                }
            }

            return xml;
        }

        private string GetOrderDownloadXmlWithInvalidHyphen()
        {
            string xml = string.Empty;
            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("ShipWorks.Tests.Stores.Newegg.Artifacts.OrderDownloadWithInvalidHyphen.xml"))
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
            object result = serializer.Deserialize(errorResponseXml);
        }

        [TestMethod]
        public void Deserialize_ReturnsDownloadResult_WhenDeserializingSuccessfulResponseXml_Test()
        {
            object result = serializer.Deserialize(successfulDownloadResponseXml);
            
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(DownloadResult));
        }

        [TestMethod]
        public void Deserialize_DownloadResultContainsSellerId_WhenDeserializingSuccessfulResponseXml_Test()
        {
            DownloadResult result = serializer.Deserialize(successfulDownloadResponseXml) as DownloadResult;
            Assert.AreEqual("A09V", result.SellerId);
        }

        [TestMethod]
        public void Deserialize_DownloadResultContainsResponseBody_WhenDeserializingSuccessfulResponseXml_Test()
        {
            DownloadResult result = serializer.Deserialize(successfulDownloadResponseXml) as DownloadResult;
            Assert.IsNotNull(result.Body);
        }

        [TestMethod]
        public void Deserialize_ResponseBodyContainsPageCount_WhenDeserializingSuccessfulResponseXml_Test()
        {
            DownloadResult result = serializer.Deserialize(successfulDownloadResponseXml) as DownloadResult;
            Assert.AreEqual(1, result.Body.PageInfo.PageCount);
        }
        
        [TestMethod]
        public void Deserialize_ResponseBodyContainsPageIndex_WhenDeserializingSuccessfulResponseXml_Test()
        {
            DownloadResult result = serializer.Deserialize(successfulDownloadResponseXml) as DownloadResult;
            Assert.AreEqual(1, result.Body.PageInfo.CurrentPageIndex);
        }

        [TestMethod]
        public void Deserialize_ResponseBodyContainsRecordCount_WhenDeserializingSuccessfulResponseXml_Test()
        {
            DownloadResult result = serializer.Deserialize(successfulDownloadResponseXml) as DownloadResult;
            Assert.AreEqual(42, result.Body.PageInfo.RecordCount);
        }

        [TestMethod]
        public void Deserialize_ResponseBodyContainsOrders_WhenDeserializingSuccessfulResponseXml_Test()
        {
            DownloadResult result = serializer.Deserialize(successfulDownloadResponseXml) as DownloadResult;
            Assert.IsNotNull(result.Body.Orders);
        }

        [TestMethod]
        public void Deserialize_ResponseBodyContainsAllOrderRecords_WhenDeserializingSuccessfulResponseXml_Test()
        {
            DownloadResult result = serializer.Deserialize(successfulDownloadResponseXml) as DownloadResult;
            Assert.AreEqual(7, result.Body.Orders.Count);
        }

        [TestMethod]
        public void Deserialize_OrderContainsOrderNumber_WhenDeserializingSuccessfulResponseXml_Test()
        {
            DownloadResult result = serializer.Deserialize(successfulDownloadResponseXml) as DownloadResult;
            Assert.AreEqual(150109083, result.Body.Orders[0].OrderNumber);
        }

        [TestMethod]
        public void Deserialize_OrderContainsOrderDate_WhenDeserializingSuccessfulResponseXml_Test()
        {
            DateTime expectedOrderDate = DateTime.Parse("06/20/2012 16:48:53");

            DownloadResult result = serializer.Deserialize(successfulDownloadResponseXml) as DownloadResult;
            
            Assert.AreEqual(expectedOrderDate, result.Body.Orders[0].OrderDateInPacificStandardTime);
        }

        [TestMethod]
        public void Deserialize_OrderContainsOrderStatusDescription_WhenDeserializingSuccessfulResponseXml_Test()
        {
            DownloadResult result = serializer.Deserialize(successfulDownloadResponseXml) as DownloadResult;
            Assert.AreEqual("Unshipped", result.Body.Orders[0].OrderStatusDescription);
        }

        [TestMethod]
        public void Deserialize_OrderContainsOrderStatus_WhenDeserializingSuccessfulResponseXml_Test()
        {
            DownloadResult result = serializer.Deserialize(successfulDownloadResponseXml) as DownloadResult;
            Assert.AreEqual(NeweggOrderStatus.Unshipped, result.Body.Orders[0].OrderStatus);
        }

        [TestMethod]
        public void Deserialize_OrderContainsCustomerName_WhenDeserializingSuccessfulResponseXml_Test()
        {
            DownloadResult result = serializer.Deserialize(successfulDownloadResponseXml) as DownloadResult;
            Assert.AreEqual("Estella Pan", result.Body.Orders[0].CustomerName);
        }

        [TestMethod]
        public void Deserialize_OrderContainsCustomerEmailAddress_WhenDeserializingSuccessfulResponseXml_Test()
        {
            DownloadResult result = serializer.Deserialize(successfulDownloadResponseXml) as DownloadResult;
            Assert.AreEqual("cpin0l4tdojyl956z@marketplace.newegg.com", result.Body.Orders[0].CustomerEmailAddress);
        }

        [TestMethod]
        public void Deserialize_OrderContainsCustomerPhoneNumber_WhenDeserializingSuccessfulResponseXml_Test()
        {
            DownloadResult result = serializer.Deserialize(successfulDownloadResponseXml) as DownloadResult;
            Assert.AreEqual("626-271-9700EXT2225", result.Body.Orders[0].CustomerPhoneNumber);
        }

        [TestMethod]
        public void Deserialize_OrderContainsShipToAddress1_WhenDeserializingSuccessfulResponseXml_Test()
        {
            DownloadResult result = serializer.Deserialize(successfulDownloadResponseXml) as DownloadResult;
            Assert.AreEqual("17708 Rowland St", result.Body.Orders[0].ShipToAddress1);
        }

        [TestMethod]
        public void Deserialize_OrderContainsShipToAddress2_WhenDeserializingSuccessfulResponseXml_Test()
        {
            DownloadResult result = serializer.Deserialize(successfulDownloadResponseXml) as DownloadResult;
            Assert.AreEqual("Suite 100", result.Body.Orders[0].ShipToAddress2);
        }

        [TestMethod]
        public void Deserialize_OrderContainsShipToCity_WhenDeserializingSuccessfulResponseXml_Test()
        {
            DownloadResult result = serializer.Deserialize(successfulDownloadResponseXml) as DownloadResult;
            Assert.AreEqual("Rowland Heights", result.Body.Orders[0].ShipToCity);
        }

        [TestMethod]
        public void Deserialize_OrderContainsShipToState_WhenDeserializingSuccessfulResponseXml_Test()
        {
            DownloadResult result = serializer.Deserialize(successfulDownloadResponseXml) as DownloadResult;
            Assert.AreEqual("CA", result.Body.Orders[0].ShipToState);
        }

        [TestMethod]
        public void Deserialize_OrderContainsShipToZipCode_WhenDeserializingSuccessfulResponseXml_Test()
        {
            DownloadResult result = serializer.Deserialize(successfulDownloadResponseXml) as DownloadResult;
            Assert.AreEqual("91748-1119", result.Body.Orders[0].ShipToZipCode);
        }

        [TestMethod]
        public void Deserialize_OrderContainsShipToCountryCode_WhenDeserializingSuccessfulResponseXml_Test()
        {
            DownloadResult result = serializer.Deserialize(successfulDownloadResponseXml) as DownloadResult;
            Assert.AreEqual("USA", result.Body.Orders[0].ShipToCountryCode);
        }

        [TestMethod]
        public void Deserialize_OrderContainsShipService_WhenDeserializingSuccessfulResponseXml_Test()
        {
            DownloadResult result = serializer.Deserialize(successfulDownloadResponseXml) as DownloadResult;
            Assert.AreEqual("Standard Shipping (5-15 business days)", result.Body.Orders[0].ShippingService);
        }

        [TestMethod]
        public void Deserialize_OrderContainsShipToFirstName_WhenDeserializingSuccessfulResponseXml_Test()
        {
            DownloadResult result = serializer.Deserialize(successfulDownloadResponseXml) as DownloadResult;
            Assert.AreEqual("Estella", result.Body.Orders[0].ShipToFirstName);
        }

        [TestMethod]
        public void Deserialize_OrderContainsShipToLastName_WhenDeserializingSuccessfulResponseXml_Test()
        {
            DownloadResult result = serializer.Deserialize(successfulDownloadResponseXml) as DownloadResult;
            Assert.AreEqual("Pan", result.Body.Orders[0].ShipToLastName);
        }

        [TestMethod]
        public void Deserialize_OrderContainsShipToCompany_WhenDeserializingSuccessfulResponseXml_Test()
        {
            DownloadResult result = serializer.Deserialize(successfulDownloadResponseXml) as DownloadResult;
            Assert.AreEqual("Newegg - Test", result.Body.Orders[0].ShipToCompany);
        }

        [TestMethod]
        public void Deserialize_OrderContainsShippingAmount_WhenDeserializingSuccessfulResponseXml_Test()
        {
            DownloadResult result = serializer.Deserialize(successfulDownloadResponseXml) as DownloadResult;
            Assert.AreEqual((decimal)1.00, result.Body.Orders[0].ShippingAmount);
        }

        [TestMethod]
        public void Deserialize_OrderContainsOrderItemAmount_WhenDeserializingSuccessfulResponseXml_Test()
        {
            DownloadResult result = serializer.Deserialize(successfulDownloadResponseXml) as DownloadResult;
            Assert.AreEqual((decimal)24.90, result.Body.Orders[0].OrderItemAmount);
        }

        [TestMethod]
        public void Deserialize_OrderContainsDiscountAmount_WhenDeserializingSuccessfulResponseXml_Test()
        {
            DownloadResult result = serializer.Deserialize(successfulDownloadResponseXml) as DownloadResult;
            Assert.AreEqual((decimal)2.20, result.Body.Orders[0].DiscountAmount);
        }

        [TestMethod]
        public void Deserialize_OrderContainsRefundAmount_WhenDeserializingSuccessfulResponseXml_Test()
        {
            DownloadResult result = serializer.Deserialize(successfulDownloadResponseXml) as DownloadResult;
            Assert.AreEqual((decimal)7.00, result.Body.Orders[0].RefundAmount);
        }

        [TestMethod]
        public void Deserialize_OrderContainsOrderTotalAmount_WhenDeserializingSuccessfulResponseXml_Test()
        {
            DownloadResult result = serializer.Deserialize(successfulDownloadResponseXml) as DownloadResult;
            Assert.AreEqual((decimal)23.70, result.Body.Orders[0].OrderTotalAmount);
        }

        [TestMethod]
        public void Deserialize_OrderContainsOrderQuantity_WhenDeserializingSuccessfulResponseXml_Test()
        {
            DownloadResult result = serializer.Deserialize(successfulDownloadResponseXml) as DownloadResult;
            Assert.AreEqual(10, result.Body.Orders[0].OrderQuantity);
        }

        [TestMethod]
        public void Deserialize_OrderContainsItems_WhenDeserializingSuccessfulResponseXml_Test()
        {
            DownloadResult result = serializer.Deserialize(successfulDownloadResponseXml) as DownloadResult;
            
            Assert.IsNotNull(result.Body.Orders[0].Items);
            Assert.AreEqual(2, result.Body.Orders[0].Items.Count);
        }

        [TestMethod]
        public void Deserialize_ItemContainsSellerPartNumber_WhenDeserializingSuccessfulResponseXml_Test()
        {
            DownloadResult result = serializer.Deserialize(successfulDownloadResponseXml) as DownloadResult;
            Assert.AreEqual("A09V-CA-001", result.Body.Orders[0].Items[0].SellerPartNumber);
        }

        [TestMethod]
        public void Deserialize_ItemContainsNeweggItemNumber_WhenDeserializingSuccessfulResponseXml_Test()
        {
            DownloadResult result = serializer.Deserialize(successfulDownloadResponseXml) as DownloadResult;
            Assert.AreEqual("9SIA09V0876393", result.Body.Orders[0].Items[0].NeweggItemNumber);
        }

        [TestMethod]
        public void Deserialize_ItemContainsManufacturerPartNumber_WhenDeserializingSuccessfulResponseXml_Test()
        {
            DownloadResult result = serializer.Deserialize(successfulDownloadResponseXml) as DownloadResult;
            Assert.AreEqual("A069-CA-001", result.Body.Orders[0].Items[0].ManufacturerPartNumber);
        }

        [TestMethod]
        public void Deserialize_ItemContainsUpcCode_WhenDeserializingSuccessfulResponseXml_Test()
        {
            DownloadResult result = serializer.Deserialize(successfulDownloadResponseXml) as DownloadResult;
            Assert.AreEqual("123456780000", result.Body.Orders[0].Items[0].UpcCode);
        }

        [TestMethod]
        public void Deserialize_ItemContainsDescription_WhenDeserializingSuccessfulResponseXml_Test()
        {
            DownloadResult result = serializer.Deserialize(successfulDownloadResponseXml) as DownloadResult;
            Assert.AreEqual("Testing items only, please do not make purchase", result.Body.Orders[0].Items[0].Description);
        }

        [TestMethod]
        public void Deserialize_ItemContainsQuantityOrdered_WhenDeserializingSuccessfulResponseXml_Test()
        {
            DownloadResult result = serializer.Deserialize(successfulDownloadResponseXml) as DownloadResult;
            Assert.AreEqual(5, result.Body.Orders[0].Items[0].QuantityOrdered);
        }

        [TestMethod]
        public void Deserialize_ItemContainsQuantityShipped_WhenDeserializingSuccessfulResponseXml_Test()
        {
            DownloadResult result = serializer.Deserialize(successfulDownloadResponseXml) as DownloadResult;
            Assert.AreEqual(0, result.Body.Orders[0].Items[0].QuantityShipped);
        }

        [TestMethod]
        public void Deserialize_ItemContainsUnitPrice_WhenDeserializingSuccessfulResponseXml_Test()
        {
            DownloadResult result = serializer.Deserialize(successfulDownloadResponseXml) as DownloadResult;
            Assert.AreEqual((decimal)1.99, result.Body.Orders[0].Items[0].UnitPrice);
        }

        [TestMethod]
        public void Deserialize_ItemContainsStatus_WhenDeserializingSuccessfulResponseXml_Test()
        {
            DownloadResult result = serializer.Deserialize(successfulDownloadResponseXml) as DownloadResult;
            Assert.AreEqual(NeweggItemShippingStatus.Unshipped, result.Body.Orders[0].Items[0].ShippingStatus);
        }

        [TestMethod]
        public void Deserialize_ItemContainsStatusDescription_WhenDeserializingSuccessfulResponseXml_Test()
        {
            DownloadResult result = serializer.Deserialize(successfulDownloadResponseXml) as DownloadResult;
            Assert.AreEqual("Unshipped", result.Body.Orders[0].Items[0].ShippingStatusDescription);
        }

        [TestMethod]
        public void Deserialize_OrderContainsPackages_WhenDeserializingSuccessfulResponseXml_Test()
        {
            DownloadResult result = serializer.Deserialize(successfulDownloadResponseXml) as DownloadResult;
            int lastOrder = result.Body.Orders.Count - 1;

            Assert.IsNotNull(result.Body.Orders[lastOrder].Packages);
            Assert.AreEqual(3, result.Body.Orders[lastOrder].Packages.Count);
        }

        [TestMethod]
        public void Deserialize_PackageContainsType_WhenDeserializingSuccessfulResponseXml_Test()
        {
            DownloadResult result = serializer.Deserialize(successfulDownloadResponseXml) as DownloadResult;
            int lastOrder = result.Body.Orders.Count - 1;

            Assert.AreEqual("Shipped", result.Body.Orders[lastOrder].Packages[0].PackageType);
        }

        [TestMethod]
        public void Deserialize_PackageContainsShipCarrier_WhenDeserializingSuccessfulResponseXml_Test()
        {
            DownloadResult result = serializer.Deserialize(successfulDownloadResponseXml) as DownloadResult;
            int lastOrder = result.Body.Orders.Count - 1;

            Assert.AreEqual("FedEx", result.Body.Orders[lastOrder].Packages[0].ShipCarrier);
        }

        [TestMethod]
        public void Deserialize_PackageContainsShipService_WhenDeserializingSuccessfulResponseXml_Test()
        {
            DownloadResult result = serializer.Deserialize(successfulDownloadResponseXml) as DownloadResult;
            int lastOrder = result.Body.Orders.Count - 1;

            Assert.AreEqual("Ground - Smartpost", result.Body.Orders[lastOrder].Packages[0].ShipService);
        }

        [TestMethod]
        public void Deserialize_PackageContainsTrackingNumber_WhenDeserializingSuccessfulResponseXml_Test()
        {
            DownloadResult result = serializer.Deserialize(successfulDownloadResponseXml) as DownloadResult;
            int lastOrder = result.Body.Orders.Count - 1;

            Assert.AreEqual("02931504039050398192", result.Body.Orders[lastOrder].Packages[0].TrackingNumber);
        }

        [TestMethod]
        public void Deserialize_PackageContainsShipDate_WhenDeserializingSuccessfulResponseXml_Test()
        {
            DownloadResult result = serializer.Deserialize(successfulDownloadResponseXml) as DownloadResult;
            int lastOrder = result.Body.Orders.Count - 1;

            Assert.AreEqual(DateTime.Parse("03/26/2012 08:49:25"), result.Body.Orders[lastOrder].Packages[0].ShipDateInPacificStandardTime);
        }

        [TestMethod]
        public void Deserialize_PackageContainsShipFromAddress_WhenDeserializingSuccessfulResponseXml_Test()
        {
            DownloadResult result = serializer.Deserialize(successfulDownloadResponseXml) as DownloadResult;
            int lastOrder = result.Body.Orders.Count - 1;

            Assert.AreEqual("111", result.Body.Orders[lastOrder].Packages[0].ShipFromAddress1);
        }

        [TestMethod]
        public void Deserialize_PackageContainsShipFromAddress2_WhenDeserializingSuccessfulResponseXml_Test()
        {
            DownloadResult result = serializer.Deserialize(successfulDownloadResponseXml) as DownloadResult;
            int lastOrder = result.Body.Orders.Count - 1;

            Assert.AreEqual("11", result.Body.Orders[lastOrder].Packages[0].ShipFromAddress2);
        }

        [TestMethod]
        public void Deserialize_PackageContainsShipFromCity_WhenDeserializingSuccessfulResponseXml_Test()
        {
            DownloadResult result = serializer.Deserialize(successfulDownloadResponseXml) as DownloadResult;
            int lastOrder = result.Body.Orders.Count - 1;

            Assert.AreEqual("11", result.Body.Orders[lastOrder].Packages[0].ShipFromCity);
        }

        [TestMethod]
        public void Deserialize_PackageContainsShipFromState_WhenDeserializingSuccessfulResponseXml_Test()
        {
            DownloadResult result = serializer.Deserialize(successfulDownloadResponseXml) as DownloadResult;
            int lastOrder = result.Body.Orders.Count - 1;

            Assert.AreEqual("NY", result.Body.Orders[lastOrder].Packages[0].ShipFromState);
        }

        [TestMethod]
        public void Deserialize_PackageContainsShipFromZipCode_WhenDeserializingSuccessfulResponseXml_Test()
        {
            DownloadResult result = serializer.Deserialize(successfulDownloadResponseXml) as DownloadResult;
            int lastOrder = result.Body.Orders.Count - 1;

            Assert.AreEqual("12344", result.Body.Orders[lastOrder].Packages[0].ShipFromZipCode);
        }

        [TestMethod]
        public void Deserialize_PackageContainsShipFromName_WhenDeserializingSuccessfulResponseXml_Test()
        {
            DownloadResult result = serializer.Deserialize(successfulDownloadResponseXml) as DownloadResult;
            int lastOrder = result.Body.Orders.Count - 1;

            Assert.AreEqual("Joe Blow", result.Body.Orders[lastOrder].Packages[0].ShipFromName);
        }

        [TestMethod]
        public void Deserialize_PackageContainsItems_WhenDeserializingSuccessfulResponseXml_Test()
        {
            DownloadResult result = serializer.Deserialize(successfulDownloadResponseXml) as DownloadResult;
            int lastOrder = result.Body.Orders.Count - 1;

            Assert.IsNotNull(result.Body.Orders[lastOrder].Packages[0].Items);
            Assert.AreEqual(1, result.Body.Orders[lastOrder].Packages[0].Items.Count);
        }

        [TestMethod]
        public void Deserialize_PackageItemContainsSellerPartNumber_WhenDeserializingSuccessfulResponseXml_Test()
        {
            DownloadResult result = serializer.Deserialize(successfulDownloadResponseXml) as DownloadResult;
            int lastOrder = result.Body.Orders.Count - 1;

            Assert.AreEqual("A09V-CA-001", result.Body.Orders[lastOrder].Packages[0].Items[0].SellerPartNumber);
        }

        [TestMethod]
        public void Deserialize_PackageItemContainsManufacturerPartNumber_WhenDeserializingSuccessfulResponseXml_Test()
        {
            DownloadResult result = serializer.Deserialize(successfulDownloadResponseXml) as DownloadResult;
            int lastOrder = result.Body.Orders.Count - 1;

            Assert.AreEqual("A069-CA-001", result.Body.Orders[lastOrder].Packages[0].Items[0].ManufacturerPartNumber);
        }

        [TestMethod]
        public void Deserialize_PackageItemContainsQuantityShipped_WhenDeserializingSuccessfulResponseXml_Test()
        {
            DownloadResult result = serializer.Deserialize(successfulDownloadResponseXml) as DownloadResult;
            int lastOrder = result.Body.Orders.Count - 1;

            Assert.AreEqual(5, result.Body.Orders[lastOrder].Packages[0].Items[0].QuantityShipped);
        }

        [TestMethod]
        public void Deserialize_XmlContainingElongatedHyphen_DoesNotThrowException_Test()
        {
            // Test created as a result of a customer had order data containing an elongated 
            // hyphen/dash (represented by â€“ characters) that was causing a crash. Test that
            // XML containing this character set is deserialized without throwing an exception
            string badXml = GetOrderDownloadXmlWithInvalidHyphen();
            DownloadResult result = serializer.Deserialize(badXml) as DownloadResult;

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Deserialize_XmlContainingUnicodeCharacters_DoesNotThrowException_Test()
        {
            string badResponseXml = string.Empty;
            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("ShipWorks.Tests.Stores.Newegg.Artifacts.OrderDownloadWithUnicodeCharacter.xml"))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    badResponseXml = reader.ReadToEnd();
                }
            }

            // Test created as a result of a customer had order data containing the é Unicode
            // character that was causing a crash. Test that XML containing this character 
            // set is deserialized without throwing an exception
            DownloadResult result = serializer.Deserialize(badResponseXml) as DownloadResult;

            Assert.IsNotNull(result);
        }

    }
}
