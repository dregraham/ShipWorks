using System;
using System.Text;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using ShipWorks.Stores.Platforms.Newegg.Net.Orders.Download;
using ShipWorks.Stores.Platforms.Newegg.Enums;
using ShipWorks.Stores.Platforms.Newegg.Net.Orders.Download.Response;

namespace ShipWorks.Tests.Stores.Newegg
{
    public class DownloadResponseSerializerTest
    {
        private string successfulDownloadResponseXml;
        private string errorResponseXml;

        private DownloadResponseSerializer serializer;

        public DownloadResponseSerializerTest()
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

        [Fact]
        public void Deserialize_ThrowsInvalidOperationException_WhenDeserializingErrorResponseXml_Test()
        {
            Assert.Throws<InvalidOperationException>(() => serializer.Deserialize(errorResponseXml));
        }

        [Fact]
        public void Deserialize_ReturnsDownloadResult_WhenDeserializingSuccessfulResponseXml_Test()
        {
            object result = serializer.Deserialize(successfulDownloadResponseXml);

            Assert.NotNull(result);
            Assert.IsAssignableFrom<DownloadResult>(result);
        }

        [Fact]
        public void Deserialize_DownloadResultContainsSellerId_WhenDeserializingSuccessfulResponseXml_Test()
        {
            DownloadResult result = serializer.Deserialize(successfulDownloadResponseXml) as DownloadResult;
            Assert.Equal("A09V", result.SellerId);
        }

        [Fact]
        public void Deserialize_DownloadResultContainsResponseBody_WhenDeserializingSuccessfulResponseXml_Test()
        {
            DownloadResult result = serializer.Deserialize(successfulDownloadResponseXml) as DownloadResult;
            Assert.NotNull(result.Body);
        }

        [Fact]
        public void Deserialize_ResponseBodyContainsPageCount_WhenDeserializingSuccessfulResponseXml_Test()
        {
            DownloadResult result = serializer.Deserialize(successfulDownloadResponseXml) as DownloadResult;
            Assert.Equal(1, result.Body.PageInfo.PageCount);
        }

        [Fact]
        public void Deserialize_ResponseBodyContainsPageIndex_WhenDeserializingSuccessfulResponseXml_Test()
        {
            DownloadResult result = serializer.Deserialize(successfulDownloadResponseXml) as DownloadResult;
            Assert.Equal(1, result.Body.PageInfo.CurrentPageIndex);
        }

        [Fact]
        public void Deserialize_ResponseBodyContainsRecordCount_WhenDeserializingSuccessfulResponseXml_Test()
        {
            DownloadResult result = serializer.Deserialize(successfulDownloadResponseXml) as DownloadResult;
            Assert.Equal(42, result.Body.PageInfo.RecordCount);
        }

        [Fact]
        public void Deserialize_ResponseBodyContainsOrders_WhenDeserializingSuccessfulResponseXml_Test()
        {
            DownloadResult result = serializer.Deserialize(successfulDownloadResponseXml) as DownloadResult;
            Assert.NotNull(result.Body.Orders);
        }

        [Fact]
        public void Deserialize_ResponseBodyContainsAllOrderRecords_WhenDeserializingSuccessfulResponseXml_Test()
        {
            DownloadResult result = serializer.Deserialize(successfulDownloadResponseXml) as DownloadResult;
            Assert.Equal(7, result.Body.Orders.Count);
        }

        [Fact]
        public void Deserialize_OrderContainsOrderNumber_WhenDeserializingSuccessfulResponseXml_Test()
        {
            DownloadResult result = serializer.Deserialize(successfulDownloadResponseXml) as DownloadResult;
            Assert.Equal(150109083, result.Body.Orders[0].OrderNumber);
        }

        [Fact]
        public void Deserialize_OrderContainsOrderDate_WhenDeserializingSuccessfulResponseXml_Test()
        {
            DateTime expectedOrderDate = DateTime.Parse("06/20/2012 16:48:53");

            DownloadResult result = serializer.Deserialize(successfulDownloadResponseXml) as DownloadResult;

            Assert.Equal(expectedOrderDate, result.Body.Orders[0].OrderDateInPacificStandardTime);
        }

        [Fact]
        public void Deserialize_OrderContainsOrderStatusDescription_WhenDeserializingSuccessfulResponseXml_Test()
        {
            DownloadResult result = serializer.Deserialize(successfulDownloadResponseXml) as DownloadResult;
            Assert.Equal("Unshipped", result.Body.Orders[0].OrderStatusDescription);
        }

        [Fact]
        public void Deserialize_OrderContainsOrderStatus_WhenDeserializingSuccessfulResponseXml_Test()
        {
            DownloadResult result = serializer.Deserialize(successfulDownloadResponseXml) as DownloadResult;
            Assert.Equal(NeweggOrderStatus.Unshipped, result.Body.Orders[0].OrderStatus);
        }

        [Fact]
        public void Deserialize_OrderContainsCustomerName_WhenDeserializingSuccessfulResponseXml_Test()
        {
            DownloadResult result = serializer.Deserialize(successfulDownloadResponseXml) as DownloadResult;
            Assert.Equal("Estella Pan", result.Body.Orders[0].CustomerName);
        }

        [Fact]
        public void Deserialize_OrderContainsCustomerEmailAddress_WhenDeserializingSuccessfulResponseXml_Test()
        {
            DownloadResult result = serializer.Deserialize(successfulDownloadResponseXml) as DownloadResult;
            Assert.Equal("cpin0l4tdojyl956z@marketplace.newegg.com", result.Body.Orders[0].CustomerEmailAddress);
        }

        [Fact]
        public void Deserialize_OrderContainsCustomerPhoneNumber_WhenDeserializingSuccessfulResponseXml_Test()
        {
            DownloadResult result = serializer.Deserialize(successfulDownloadResponseXml) as DownloadResult;
            Assert.Equal("626-271-9700EXT2225", result.Body.Orders[0].CustomerPhoneNumber);
        }

        [Fact]
        public void Deserialize_OrderContainsShipToAddress1_WhenDeserializingSuccessfulResponseXml_Test()
        {
            DownloadResult result = serializer.Deserialize(successfulDownloadResponseXml) as DownloadResult;
            Assert.Equal("17708 Rowland St", result.Body.Orders[0].ShipToAddress1);
        }

        [Fact]
        public void Deserialize_OrderContainsShipToAddress2_WhenDeserializingSuccessfulResponseXml_Test()
        {
            DownloadResult result = serializer.Deserialize(successfulDownloadResponseXml) as DownloadResult;
            Assert.Equal("Suite 100", result.Body.Orders[0].ShipToAddress2);
        }

        [Fact]
        public void Deserialize_OrderContainsShipToCity_WhenDeserializingSuccessfulResponseXml_Test()
        {
            DownloadResult result = serializer.Deserialize(successfulDownloadResponseXml) as DownloadResult;
            Assert.Equal("Rowland Heights", result.Body.Orders[0].ShipToCity);
        }

        [Fact]
        public void Deserialize_OrderContainsShipToState_WhenDeserializingSuccessfulResponseXml_Test()
        {
            DownloadResult result = serializer.Deserialize(successfulDownloadResponseXml) as DownloadResult;
            Assert.Equal("CA", result.Body.Orders[0].ShipToState);
        }

        [Fact]
        public void Deserialize_OrderContainsShipToZipCode_WhenDeserializingSuccessfulResponseXml_Test()
        {
            DownloadResult result = serializer.Deserialize(successfulDownloadResponseXml) as DownloadResult;
            Assert.Equal("91748-1119", result.Body.Orders[0].ShipToZipCode);
        }

        [Fact]
        public void Deserialize_OrderContainsShipToCountryCode_WhenDeserializingSuccessfulResponseXml_Test()
        {
            DownloadResult result = serializer.Deserialize(successfulDownloadResponseXml) as DownloadResult;
            Assert.Equal("USA", result.Body.Orders[0].ShipToCountryCode);
        }

        [Fact]
        public void Deserialize_OrderContainsShipService_WhenDeserializingSuccessfulResponseXml_Test()
        {
            DownloadResult result = serializer.Deserialize(successfulDownloadResponseXml) as DownloadResult;
            Assert.Equal("Standard Shipping (5-15 business days)", result.Body.Orders[0].ShippingService);
        }

        [Fact]
        public void Deserialize_OrderContainsShipToFirstName_WhenDeserializingSuccessfulResponseXml_Test()
        {
            DownloadResult result = serializer.Deserialize(successfulDownloadResponseXml) as DownloadResult;
            Assert.Equal("Estella", result.Body.Orders[0].ShipToFirstName);
        }

        [Fact]
        public void Deserialize_OrderContainsShipToLastName_WhenDeserializingSuccessfulResponseXml_Test()
        {
            DownloadResult result = serializer.Deserialize(successfulDownloadResponseXml) as DownloadResult;
            Assert.Equal("Pan", result.Body.Orders[0].ShipToLastName);
        }

        [Fact]
        public void Deserialize_OrderContainsShipToCompany_WhenDeserializingSuccessfulResponseXml_Test()
        {
            DownloadResult result = serializer.Deserialize(successfulDownloadResponseXml) as DownloadResult;
            Assert.Equal("Newegg - Test", result.Body.Orders[0].ShipToCompany);
        }

        [Fact]
        public void Deserialize_OrderContainsShippingAmount_WhenDeserializingSuccessfulResponseXml_Test()
        {
            DownloadResult result = serializer.Deserialize(successfulDownloadResponseXml) as DownloadResult;
            Assert.Equal((decimal)1.00, result.Body.Orders[0].ShippingAmount);
        }

        [Fact]
        public void Deserialize_OrderContainsOrderItemAmount_WhenDeserializingSuccessfulResponseXml_Test()
        {
            DownloadResult result = serializer.Deserialize(successfulDownloadResponseXml) as DownloadResult;
            Assert.Equal((decimal)24.90, result.Body.Orders[0].OrderItemAmount);
        }

        [Fact]
        public void Deserialize_OrderContainsDiscountAmount_WhenDeserializingSuccessfulResponseXml_Test()
        {
            DownloadResult result = serializer.Deserialize(successfulDownloadResponseXml) as DownloadResult;
            Assert.Equal((decimal)2.20, result.Body.Orders[0].DiscountAmount);
        }

        [Fact]
        public void Deserialize_OrderContainsRefundAmount_WhenDeserializingSuccessfulResponseXml_Test()
        {
            DownloadResult result = serializer.Deserialize(successfulDownloadResponseXml) as DownloadResult;
            Assert.Equal((decimal)7.00, result.Body.Orders[0].RefundAmount);
        }

        [Fact]
        public void Deserialize_OrderContainsOrderTotalAmount_WhenDeserializingSuccessfulResponseXml_Test()
        {
            DownloadResult result = serializer.Deserialize(successfulDownloadResponseXml) as DownloadResult;
            Assert.Equal((decimal)23.70, result.Body.Orders[0].OrderTotalAmount);
        }

        [Fact]
        public void Deserialize_OrderContainsOrderQuantity_WhenDeserializingSuccessfulResponseXml_Test()
        {
            DownloadResult result = serializer.Deserialize(successfulDownloadResponseXml) as DownloadResult;
            Assert.Equal(10, result.Body.Orders[0].OrderQuantity);
        }

        [Fact]
        public void Deserialize_OrderContainsItems_WhenDeserializingSuccessfulResponseXml_Test()
        {
            DownloadResult result = serializer.Deserialize(successfulDownloadResponseXml) as DownloadResult;

            Assert.NotNull(result.Body.Orders[0].Items);
            Assert.Equal(2, result.Body.Orders[0].Items.Count);
        }

        [Fact]
        public void Deserialize_ItemContainsSellerPartNumber_WhenDeserializingSuccessfulResponseXml_Test()
        {
            DownloadResult result = serializer.Deserialize(successfulDownloadResponseXml) as DownloadResult;
            Assert.Equal("A09V-CA-001", result.Body.Orders[0].Items[0].SellerPartNumber);
        }

        [Fact]
        public void Deserialize_ItemContainsNeweggItemNumber_WhenDeserializingSuccessfulResponseXml_Test()
        {
            DownloadResult result = serializer.Deserialize(successfulDownloadResponseXml) as DownloadResult;
            Assert.Equal("9SIA09V0876393", result.Body.Orders[0].Items[0].NeweggItemNumber);
        }

        [Fact]
        public void Deserialize_ItemContainsManufacturerPartNumber_WhenDeserializingSuccessfulResponseXml_Test()
        {
            DownloadResult result = serializer.Deserialize(successfulDownloadResponseXml) as DownloadResult;
            Assert.Equal("A069-CA-001", result.Body.Orders[0].Items[0].ManufacturerPartNumber);
        }

        [Fact]
        public void Deserialize_ItemContainsUpcCode_WhenDeserializingSuccessfulResponseXml_Test()
        {
            DownloadResult result = serializer.Deserialize(successfulDownloadResponseXml) as DownloadResult;
            Assert.Equal("123456780000", result.Body.Orders[0].Items[0].UpcCode);
        }

        [Fact]
        public void Deserialize_ItemContainsDescription_WhenDeserializingSuccessfulResponseXml_Test()
        {
            DownloadResult result = serializer.Deserialize(successfulDownloadResponseXml) as DownloadResult;
            Assert.Equal("Testing items only, please do not make purchase", result.Body.Orders[0].Items[0].Description);
        }

        [Fact]
        public void Deserialize_ItemContainsQuantityOrdered_WhenDeserializingSuccessfulResponseXml_Test()
        {
            DownloadResult result = serializer.Deserialize(successfulDownloadResponseXml) as DownloadResult;
            Assert.Equal(5, result.Body.Orders[0].Items[0].QuantityOrdered);
        }

        [Fact]
        public void Deserialize_ItemContainsQuantityShipped_WhenDeserializingSuccessfulResponseXml_Test()
        {
            DownloadResult result = serializer.Deserialize(successfulDownloadResponseXml) as DownloadResult;
            Assert.Equal(0, result.Body.Orders[0].Items[0].QuantityShipped);
        }

        [Fact]
        public void Deserialize_ItemContainsUnitPrice_WhenDeserializingSuccessfulResponseXml_Test()
        {
            DownloadResult result = serializer.Deserialize(successfulDownloadResponseXml) as DownloadResult;
            Assert.Equal((decimal)1.99, result.Body.Orders[0].Items[0].UnitPrice);
        }

        [Fact]
        public void Deserialize_ItemContainsStatus_WhenDeserializingSuccessfulResponseXml_Test()
        {
            DownloadResult result = serializer.Deserialize(successfulDownloadResponseXml) as DownloadResult;
            Assert.Equal(NeweggItemShippingStatus.Unshipped, result.Body.Orders[0].Items[0].ShippingStatus);
        }

        [Fact]
        public void Deserialize_ItemContainsStatusDescription_WhenDeserializingSuccessfulResponseXml_Test()
        {
            DownloadResult result = serializer.Deserialize(successfulDownloadResponseXml) as DownloadResult;
            Assert.Equal("Unshipped", result.Body.Orders[0].Items[0].ShippingStatusDescription);
        }

        [Fact]
        public void Deserialize_OrderContainsPackages_WhenDeserializingSuccessfulResponseXml_Test()
        {
            DownloadResult result = serializer.Deserialize(successfulDownloadResponseXml) as DownloadResult;
            int lastOrder = result.Body.Orders.Count - 1;

            Assert.NotNull(result.Body.Orders[lastOrder].Packages);
            Assert.Equal(3, result.Body.Orders[lastOrder].Packages.Count);
        }

        [Fact]
        public void Deserialize_PackageContainsType_WhenDeserializingSuccessfulResponseXml_Test()
        {
            DownloadResult result = serializer.Deserialize(successfulDownloadResponseXml) as DownloadResult;
            int lastOrder = result.Body.Orders.Count - 1;

            Assert.Equal("Shipped", result.Body.Orders[lastOrder].Packages[0].PackageType);
        }

        [Fact]
        public void Deserialize_PackageContainsShipCarrier_WhenDeserializingSuccessfulResponseXml_Test()
        {
            DownloadResult result = serializer.Deserialize(successfulDownloadResponseXml) as DownloadResult;
            int lastOrder = result.Body.Orders.Count - 1;

            Assert.Equal("FedEx", result.Body.Orders[lastOrder].Packages[0].ShipCarrier);
        }

        [Fact]
        public void Deserialize_PackageContainsShipService_WhenDeserializingSuccessfulResponseXml_Test()
        {
            DownloadResult result = serializer.Deserialize(successfulDownloadResponseXml) as DownloadResult;
            int lastOrder = result.Body.Orders.Count - 1;

            Assert.Equal("Ground - Smartpost", result.Body.Orders[lastOrder].Packages[0].ShipService);
        }

        [Fact]
        public void Deserialize_PackageContainsTrackingNumber_WhenDeserializingSuccessfulResponseXml_Test()
        {
            DownloadResult result = serializer.Deserialize(successfulDownloadResponseXml) as DownloadResult;
            int lastOrder = result.Body.Orders.Count - 1;

            Assert.Equal("02931504039050398192", result.Body.Orders[lastOrder].Packages[0].TrackingNumber);
        }

        [Fact]
        public void Deserialize_PackageContainsShipDate_WhenDeserializingSuccessfulResponseXml_Test()
        {
            DownloadResult result = serializer.Deserialize(successfulDownloadResponseXml) as DownloadResult;
            int lastOrder = result.Body.Orders.Count - 1;

            Assert.Equal(DateTime.Parse("03/26/2012 08:49:25"), result.Body.Orders[lastOrder].Packages[0].ShipDateInPacificStandardTime);
        }

        [Fact]
        public void Deserialize_PackageContainsShipFromAddress_WhenDeserializingSuccessfulResponseXml_Test()
        {
            DownloadResult result = serializer.Deserialize(successfulDownloadResponseXml) as DownloadResult;
            int lastOrder = result.Body.Orders.Count - 1;

            Assert.Equal("111", result.Body.Orders[lastOrder].Packages[0].ShipFromAddress1);
        }

        [Fact]
        public void Deserialize_PackageContainsShipFromAddress2_WhenDeserializingSuccessfulResponseXml_Test()
        {
            DownloadResult result = serializer.Deserialize(successfulDownloadResponseXml) as DownloadResult;
            int lastOrder = result.Body.Orders.Count - 1;

            Assert.Equal("11", result.Body.Orders[lastOrder].Packages[0].ShipFromAddress2);
        }

        [Fact]
        public void Deserialize_PackageContainsShipFromCity_WhenDeserializingSuccessfulResponseXml_Test()
        {
            DownloadResult result = serializer.Deserialize(successfulDownloadResponseXml) as DownloadResult;
            int lastOrder = result.Body.Orders.Count - 1;

            Assert.Equal("11", result.Body.Orders[lastOrder].Packages[0].ShipFromCity);
        }

        [Fact]
        public void Deserialize_PackageContainsShipFromState_WhenDeserializingSuccessfulResponseXml_Test()
        {
            DownloadResult result = serializer.Deserialize(successfulDownloadResponseXml) as DownloadResult;
            int lastOrder = result.Body.Orders.Count - 1;

            Assert.Equal("NY", result.Body.Orders[lastOrder].Packages[0].ShipFromState);
        }

        [Fact]
        public void Deserialize_PackageContainsShipFromZipCode_WhenDeserializingSuccessfulResponseXml_Test()
        {
            DownloadResult result = serializer.Deserialize(successfulDownloadResponseXml) as DownloadResult;
            int lastOrder = result.Body.Orders.Count - 1;

            Assert.Equal("12344", result.Body.Orders[lastOrder].Packages[0].ShipFromZipCode);
        }

        [Fact]
        public void Deserialize_PackageContainsShipFromName_WhenDeserializingSuccessfulResponseXml_Test()
        {
            DownloadResult result = serializer.Deserialize(successfulDownloadResponseXml) as DownloadResult;
            int lastOrder = result.Body.Orders.Count - 1;

            Assert.Equal("Joe Blow", result.Body.Orders[lastOrder].Packages[0].ShipFromName);
        }

        [Fact]
        public void Deserialize_PackageContainsItems_WhenDeserializingSuccessfulResponseXml_Test()
        {
            DownloadResult result = serializer.Deserialize(successfulDownloadResponseXml) as DownloadResult;
            int lastOrder = result.Body.Orders.Count - 1;

            Assert.NotNull(result.Body.Orders[lastOrder].Packages[0].Items);
            Assert.Equal(1, result.Body.Orders[lastOrder].Packages[0].Items.Count);
        }

        [Fact]
        public void Deserialize_PackageItemContainsSellerPartNumber_WhenDeserializingSuccessfulResponseXml_Test()
        {
            DownloadResult result = serializer.Deserialize(successfulDownloadResponseXml) as DownloadResult;
            int lastOrder = result.Body.Orders.Count - 1;

            Assert.Equal("A09V-CA-001", result.Body.Orders[lastOrder].Packages[0].Items[0].SellerPartNumber);
        }

        [Fact]
        public void Deserialize_PackageItemContainsManufacturerPartNumber_WhenDeserializingSuccessfulResponseXml_Test()
        {
            DownloadResult result = serializer.Deserialize(successfulDownloadResponseXml) as DownloadResult;
            int lastOrder = result.Body.Orders.Count - 1;

            Assert.Equal("A069-CA-001", result.Body.Orders[lastOrder].Packages[0].Items[0].ManufacturerPartNumber);
        }

        [Fact]
        public void Deserialize_PackageItemContainsQuantityShipped_WhenDeserializingSuccessfulResponseXml_Test()
        {
            DownloadResult result = serializer.Deserialize(successfulDownloadResponseXml) as DownloadResult;
            int lastOrder = result.Body.Orders.Count - 1;

            Assert.Equal(5, result.Body.Orders[lastOrder].Packages[0].Items[0].QuantityShipped);
        }

        [Fact]
        public void Deserialize_XmlContainingElongatedHyphen_DoesNotThrowException_Test()
        {
            // Test created as a result of a customer had order data containing an elongated 
            // hyphen/dash (represented by â€“ characters) that was causing a crash. Test that
            // XML containing this character set is deserialized without throwing an exception
            string badXml = GetOrderDownloadXmlWithInvalidHyphen();
            DownloadResult result = serializer.Deserialize(badXml) as DownloadResult;

            Assert.NotNull(result);
        }

        [Fact]
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

            Assert.NotNull(result);
        }

        [Fact]
        public void Deserialize_XmlMissingInvoiceNumber_Test()
        {
            string xml = string.Empty;

            // Test created as a result of a customer having an order without an InvoiceNumber 
            // supplied from Newegg that was causing a crash. Test that XML with this scenario
            // is deserialized without throwing an exception
            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("ShipWorks.Tests.Stores.Newegg.Artifacts.OrderDownloadedWithEmptyInvoice.xml"))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    xml = reader.ReadToEnd();
                }
            }

            DownloadResult result = serializer.Deserialize(xml) as DownloadResult;

            Assert.NotNull(result);
        }

    }
}
