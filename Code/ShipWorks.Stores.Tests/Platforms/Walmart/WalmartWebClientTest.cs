﻿using Autofac.Extras.Moq;
using ShipWorks.Stores.Platforms.Walmart;
using ShipWorks.Tests.Shared;
using System;
using System.Linq;
using Xunit;
using ShipWorks.Data.Model.EntityClasses;
using Interapptive.Shared.Net;
using Moq;
using System.Net;
using Interapptive.Shared.Utility;
using ShipWorks.Stores.Platforms.Walmart.DTO;

namespace ShipWorks.Stores.Tests.Platforms.Walmart
{
    public class WalmartWebClientTest : IDisposable
    {
        private readonly AutoMock mock;
        private readonly AutoMock acknowledgeMock;
        private const string OrdersResponse = "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\" ?><ns3:list xmlns:ns2=\"http://walmart.com/mp/orders\" xmlns:ns3=\"http://walmart.com/mp/v3/orders\" xmlns:ns4=\"http://walmart.com/\"><ns3:meta><ns3:totalCount>367</ns3:totalCount><ns3:limit>10</ns3:limit><ns3:nextCursor>123</ns3:nextCursor></ns3:meta><ns3:elements><ns3:order><ns3:purchaseOrderId>2575263094491</ns3:purchaseOrderId><ns3:customerOrderId>4091603648841</ns3:customerOrderId><ns3:customerEmailId>gsingh@walmartlabs.com</ns3:customerEmailId><ns3:orderDate>2016-05-18T16:53:14.000Z</ns3:orderDate><ns3:shippingInfo><ns3:phone>2342423234</ns3:phone><ns3:estimatedDeliveryDate>2016-06-22T06:00:00.000Z</ns3:estimatedDeliveryDate><ns3:estimatedShipDate>2016-06-15T06:00:00.000Z</ns3:estimatedShipDate><ns3:methodCode>Standard</ns3:methodCode><ns3:postalAddress><ns3:name>PGOMS Walmart</ns3:name><ns3:address1>850 Cherry Avenue</ns3:address1><ns3:address2>Floor 5</ns3:address2><ns3:city>San Bruno</ns3:city><ns3:state>CA</ns3:state><ns3:postalCode>94066</ns3:postalCode><ns3:country>USA</ns3:country><ns3:addressType>RESIDENTIAL</ns3:addressType></ns3:postalAddress></ns3:shippingInfo><ns3:orderLines><ns3:orderLine><ns3:lineNumber>4</ns3:lineNumber><ns3:item><ns3:productName>Kenmore CF1 or 2086883 Canister Secondary Filter Generic 2 Pack</ns3:productName><ns3:sku>RCA-OF-444gku444</ns3:sku></ns3:item><ns3:charges><ns3:charge><ns3:chargeType>PRODUCT</ns3:chargeType><ns3:chargeName>ItemPrice</ns3:chargeName><ns3:chargeAmount><ns3:currency>USD</ns3:currency><ns3:amount>25.00</ns3:amount></ns3:chargeAmount><ns3:tax><ns3:taxName>Tax1</ns3:taxName><ns3:taxAmount><ns3:currency>USD</ns3:currency><ns3:amount>1.87</ns3:amount></ns3:taxAmount></ns3:tax></ns3:charge></ns3:charges><ns3:orderLineQuantity><ns3:unitOfMeasurement>EACH</ns3:unitOfMeasurement><ns3:amount>1</ns3:amount></ns3:orderLineQuantity><ns3:statusDate>2016-05-18T17:01:26.000Z</ns3:statusDate><ns3:orderLineStatuses><ns3:orderLineStatus><ns3:status>Created</ns3:status><ns3:statusQuantity><ns3:unitOfMeasurement>EACH</ns3:unitOfMeasurement><ns3:amount>1</ns3:amount></ns3:statusQuantity></ns3:orderLineStatus></ns3:orderLineStatuses></ns3:orderLine></ns3:orderLines></ns3:order><ns3:order><ns3:purchaseOrderId>2575263094492</ns3:purchaseOrderId><ns3:customerOrderId>4091603648841</ns3:customerOrderId><ns3:customerEmailId>gsingh@walmartlabs.com</ns3:customerEmailId><ns3:orderDate>2016-05-18T16:53:14.000Z</ns3:orderDate><ns3:shippingInfo><ns3:phone>2342423234</ns3:phone><ns3:estimatedDeliveryDate>2016-06-22T06:00:00.000Z</ns3:estimatedDeliveryDate><ns3:estimatedShipDate>2016-06-15T06:00:00.000Z</ns3:estimatedShipDate><ns3:methodCode>Standard</ns3:methodCode><ns3:postalAddress><ns3:name>PGOMS Walmart</ns3:name><ns3:address1>850 Cherry Avenue</ns3:address1><ns3:address2>Floor 5</ns3:address2><ns3:city>San Bruno</ns3:city><ns3:state>CA</ns3:state><ns3:postalCode>94066</ns3:postalCode><ns3:country>USA</ns3:country><ns3:addressType>RESIDENTIAL</ns3:addressType></ns3:postalAddress></ns3:shippingInfo><ns3:orderLines><ns3:orderLine><ns3:lineNumber>1</ns3:lineNumber><ns3:item><ns3:productName>Kenmore CF1 or 2086883 Canister Secondary Filter Generic 2 Pack</ns3:productName><ns3:sku>RCA-OF-444gku444</ns3:sku></ns3:item><ns3:charges><ns3:charge><ns3:chargeType>PRODUCT</ns3:chargeType><ns3:chargeName>ItemPrice</ns3:chargeName><ns3:chargeAmount><ns3:currency>USD</ns3:currency><ns3:amount>25.00</ns3:amount></ns3:chargeAmount><ns3:tax><ns3:taxName>Tax1</ns3:taxName><ns3:taxAmount><ns3:currency>USD</ns3:currency><ns3:amount>1.89</ns3:amount></ns3:taxAmount></ns3:tax></ns3:charge></ns3:charges><ns3:orderLineQuantity><ns3:unitOfMeasurement>EACH</ns3:unitOfMeasurement><ns3:amount>1</ns3:amount></ns3:orderLineQuantity><ns3:statusDate>2016-05-18T17:01:26.000Z</ns3:statusDate><ns3:orderLineStatuses><ns3:orderLineStatus><ns3:status>Created</ns3:status><ns3:statusQuantity><ns3:unitOfMeasurement>EACH</ns3:unitOfMeasurement><ns3:amount>1</ns3:amount></ns3:statusQuantity></ns3:orderLineStatus></ns3:orderLineStatuses></ns3:orderLine></ns3:orderLines></ns3:order></ns3:elements></ns3:list>";
        private const string OrderFormat = "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?><ns3:order xmlns:ns2=\"http://walmart.com/mp/orders\" xmlns:ns3=\"http://walmart.com/mp/v3/orders\" xmlns:ns4=\"http://walmart.com/\"><ns3:purchaseOrderId>2575193093776</ns3:purchaseOrderId><ns3:customerOrderId>4021603941547</ns3:customerOrderId><ns3:customerEmailId>mgr@walmartlabs.com</ns3:customerEmailId><ns3:orderDate>2016-05-11T23:16:10.000Z</ns3:orderDate><ns3:shippingInfo><ns3:phone>6502248603</ns3:phone><ns3:estimatedDeliveryDate>2016-05-20T17:00:00.000Z</ns3:estimatedDeliveryDate><ns3:estimatedShipDate>2016-05-16T17:00:00.000Z</ns3:estimatedShipDate><ns3:methodCode>Standard</ns3:methodCode><ns3:postalAddress><ns3:name>Madhukara PGOMS</ns3:name><ns3:address1>860 W Cal Ave</ns3:address1><ns3:address2>Seat # 860C.2.176</ns3:address2><ns3:city>Sunnyvale</ns3:city><ns3:state>CA</ns3:state><ns3:postalCode>94086</ns3:postalCode><ns3:country>USA</ns3:country><ns3:addressType>RESIDENTIAL</ns3:addressType></ns3:postalAddress></ns3:shippingInfo><ns3:orderLines><ns3:orderLine><ns3:lineNumber>1</ns3:lineNumber><ns3:item><ns3:productName>Garmin Refurbished nuvi 2595LMT 5 GPS w Lifetime Maps and Traffic</ns3:productName><ns3:sku>GRMN100201</ns3:sku></ns3:item><ns3:charges><ns3:charge><ns3:chargeType>PRODUCT</ns3:chargeType><ns3:chargeName>ItemPrice</ns3:chargeName><ns3:chargeAmount><ns3:currency>USD</ns3:currency><ns3:amount>124.98</ns3:amount></ns3:chargeAmount><ns3:tax><ns3:taxName>Tax1</ns3:taxName><ns3:taxAmount><ns3:currency>USD</ns3:currency><ns3:amount>10.94</ns3:amount></ns3:taxAmount></ns3:tax></ns3:charge></ns3:charges><ns3:orderLineQuantity><ns3:unitOfMeasurement>EACH</ns3:unitOfMeasurement><ns3:amount>1</ns3:amount></ns3:orderLineQuantity><ns3:statusDate>2016-05-11T23:43:50.000Z</ns3:statusDate><ns3:orderLineStatuses><ns3:orderLineStatus><ns3:status>{0}</ns3:status><ns3:statusQuantity><ns3:unitOfMeasurement>EACH</ns3:unitOfMeasurement><ns3:amount>1</ns3:amount></ns3:statusQuantity></ns3:orderLineStatus></ns3:orderLineStatuses></ns3:orderLine></ns3:orderLines></ns3:order>";

        public WalmartWebClientTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            acknowledgeMock = AutoMockExtensions.GetLooseThatReturnsMocks();
            SetupAcknowledgeOrderResponse();
        }

        [Fact]
        public void GetOrders_SetsUri()
        {
            DateTime start = DateTime.UtcNow.AddDays(-3);
            Mock<IHttpVariableRequestSubmitter> requestSubmitter = SetupHttpVariableRequestSubmitter(OrdersResponse);

            WalmartWebClient testObject = mock.Create<WalmartWebClient>();

            testObject.GetOrders(new WalmartStoreEntity(), start);

            requestSubmitter.VerifySet(r => r.Uri = new Uri("https://marketplace.walmartapis.com/v3/orders"));
        }

        private Mock<IHttpVariableRequestSubmitter> SetupHttpVariableRequestSubmitter(string response)
        {
            Mock<IHttpResponseReader> responseReader = mock.Mock<IHttpResponseReader>();
            responseReader.Setup(r => r.ReadResult()).Returns(response);

            Mock<IHttpVariableRequestSubmitter> requestSubmitter = mock.Mock<IHttpVariableRequestSubmitter>();
            requestSubmitter.Setup(r => r.GetResponse()).Returns(responseReader);
            mock.Mock<IHttpRequestSubmitterFactory>()
                .Setup(f => f.GetHttpVariableRequestSubmitter())
                .Returns(requestSubmitter.Object);

            return requestSubmitter;
        }

        [Fact]
        public void GetOrders_SignsTheRequest()
        {
            Mock<IHttpVariableRequestSubmitter> requestSubmitter = SetupHttpVariableRequestSubmitter(OrdersResponse);

            WalmartWebClient testObject = mock.Create<WalmartWebClient>();
            WalmartStoreEntity store = new WalmartStoreEntity();

            testObject.GetOrders(store, "nextCursorValue");

            mock.Mock<IWalmartRequestSigner>().Verify(s => s.Sign(requestSubmitter.Object, store));
        }

        [Fact]
        public void GetOrders_AddsWalmartCredentialsHeaders()
        {
            DateTime start = DateTime.UtcNow.AddDays(-3);
            WebHeaderCollection webHeaderCollection = new WebHeaderCollection();

            Mock<IHttpVariableRequestSubmitter> requestSubmitter = SetupHttpVariableRequestSubmitter(OrdersResponse);
            requestSubmitter.SetupGet(r => r.Headers).Returns(webHeaderCollection);

            WalmartWebClient testObject = mock.Create<WalmartWebClient>();

            WalmartStoreEntity store = new WalmartStoreEntity();
            store.ConsumerID = "blah blah consumer id";

            testObject.GetOrders(store, start);

            Assert.Equal("Walmart Marketplace", webHeaderCollection.GetValues("WM_SVC.NAME").First());
            Assert.Equal(store.ConsumerID, webHeaderCollection.GetValues("WM_CONSUMER.ID").First());
            Assert.NotEmpty(webHeaderCollection.GetValues("WM_QOS.CORRELATION_ID"));
        }

        public void GetOrders_RethrowsWalmartException_WhenWebRequestThrowsWebException()
        {
            SetupHttpVariableRequestSubmitter(OrdersResponse);

            WalmartWebClient testObject = mock.Create<WalmartWebClient>();

            Assert.Throws<WalmartException>(() => testObject.GetOrders(new WalmartStoreEntity(), "nextCursorValue"));
        }

        [Fact]
        public void GetOrders_SetsUriWithNextCursor()
        {
            var requestSubmitter = SetupHttpVariableRequestSubmitter(OrdersResponse);

            WalmartWebClient testObject = mock.Create<WalmartWebClient>();

            testObject.GetOrders(new WalmartStoreEntity(), "nextCursorValue");

            requestSubmitter.VerifySet(r => r.Uri = new Uri("https://marketplace.walmartapis.com/v3/ordersnextCursorValue"));
        }

        [Fact]
        public void GetOrders_SetsHttpVerb()
        {
            DateTime start = DateTime.UtcNow.AddDays(-3);
            var requestSubmitter = SetupHttpVariableRequestSubmitter(OrdersResponse);

            WalmartWebClient testObject = mock.Create<WalmartWebClient>();

            testObject.GetOrders(new WalmartStoreEntity(), start);

            requestSubmitter.VerifySet(r => r.Verb = HttpVerb.Get);
        }


        [Fact]
        public void GetOrders_AddsCreatedStartDateVariable()
        {
            DateTime start = DateTime.UtcNow.AddDays(-3);

            var requestSubmitter = SetupHttpVariableRequestSubmitter(OrdersResponse);
            HttpVariableCollection httpVariables = new HttpVariableCollection();
            requestSubmitter.SetupGet(r => r.Variables).Returns(httpVariables);

            WalmartWebClient testObject = mock.Create<WalmartWebClient>();

            testObject.GetOrders(new WalmartStoreEntity(), start);

            Assert.Contains(httpVariables, v => v.Name == "createdStartDate" && v.Value == start.ToString("s"));
        }

        [Fact]
        public void GetOrders_WithNextCursor_AcknowledgesEachOrder()
        {
            SetupHttpVariableRequestSubmitter(OrdersResponse);

            WalmartWebClient testObject = mock.Create<WalmartWebClient>();

            testObject.GetOrders(new WalmartStoreEntity(), "");

            // There are 2 orders to acknowledge
            acknowledgeMock.Mock<IHttpRequestSubmitter>().Verify(r => r.GetResponse(), Times.Exactly(2));
        }

        [Fact]
        public void GetOrders_WithStartDate_AcknowledgesEachOrder()
        {
            SetupHttpVariableRequestSubmitter(OrdersResponse);

            WalmartWebClient testObject = mock.Create<WalmartWebClient>();

            testObject.GetOrders(new WalmartStoreEntity(), new DateTime());

            // There are 2 orders to acknowledge
            acknowledgeMock.Mock<IHttpRequestSubmitter>().Verify(r => r.GetResponse(), Times.Exactly(2));
        }

        [Fact]
        public void GetOrders_ReturnsAcknowledgedOrders()
        {
            SetupHttpVariableRequestSubmitter(OrdersResponse);

            WalmartWebClient testObject = mock.Create<WalmartWebClient>();

            ordersListType orders = testObject.GetOrders(new WalmartStoreEntity(), new DateTime());

            Assert.Equal(2, orders.elements.Count(order => order.orderLines
                .All(line => line.orderLineStatuses[0].status == orderLineStatusValueType.Acknowledged)));
        }

        [Fact]
        public void UpdateShipmentDetails_UsesCorrectUri()
        {
            WalmartWebClient testObject = mock.Create<WalmartWebClient>();

            testObject.UpdateShipmentDetails(new WalmartStoreEntity(), new orderShipment(), "123");

            acknowledgeMock.Mock<IHttpRequestSubmitter>()
                .VerifySet(submitter => submitter.Uri = new Uri("https://marketplace.walmartapis.com/v3/orders/123/shipping"));
        }

        [Fact]
        public void UpdateShipmentDetails_UsesTextPostSubmitterFromFactory()
        {
            WalmartWebClient testObject = mock.Create<WalmartWebClient>();

            testObject.UpdateShipmentDetails(new WalmartStoreEntity(), new orderShipment(), "123");

            mock.Mock<IHttpRequestSubmitterFactory>()
                .Verify(f=>f.GetHttpTextPostRequestSubmitter(It.IsAny<string>(), @"application/xml"));
        }

        [Fact]
        public void UpdateShipmentDetails_ReturnsOrder()
        {
            WalmartWebClient testObject = mock.Create<WalmartWebClient>();

            var order = testObject.UpdateShipmentDetails(new WalmartStoreEntity(), new orderShipment(), "123");
            var expectedOrder =
                SerializationUtility.DeserializeFromXml<Order>(GetOrderText(orderLineStatusValueType.Acknowledged));

            Assert.Equal(SerializationUtility.SerializeToXml(expectedOrder), SerializationUtility.SerializeToXml(order));
        }

        [Fact]
        public void GetOrder_SetsUriWithPurchaseOrderId()
        {
            Mock<IHttpVariableRequestSubmitter> requestSubmitter = SetupHttpVariableRequestSubmitter(GetOrderText(orderLineStatusValueType.Acknowledged));
            WalmartWebClient testObject = mock.Create<WalmartWebClient>();
            testObject.GetOrder(new WalmartStoreEntity(), "12345678");

            requestSubmitter.VerifySet(r => r.Uri = new Uri("https://marketplace.walmartapis.com/v3/orders/12345678"));
        }

        private void SetupAcknowledgeOrderResponse()
        {
            var responseReader = acknowledgeMock.Mock<IHttpResponseReader>();
            responseReader.Setup(r => r.ReadResult()).Returns(GetOrderText(orderLineStatusValueType.Acknowledged));

            var requestSubmitter = acknowledgeMock.Mock<IHttpRequestSubmitter>();
            requestSubmitter.Setup(r => r.GetResponse()).Returns(responseReader);

            var httpRequestSubmitterFactory = mock.Mock<IHttpRequestSubmitterFactory>();
            httpRequestSubmitterFactory
                .Setup(f => f.GetHttpTextPostRequestSubmitter(It.IsAny<string>(), "application/xml"))
                .Returns(requestSubmitter.Object);
        }

        private string GetOrderText(orderLineStatusValueType status)
        {
            return string.Format(OrderFormat, status);
        }

        public void Dispose()
        {
            acknowledgeMock.Dispose();
            mock.Dispose();
        }
    }
}