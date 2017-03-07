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

namespace ShipWorks.Stores.Tests.Platforms.Walmart
{
    public class WalmartWebClientTest : IDisposable
    {
        private readonly AutoMock mock;
        private const string OrdersResponse = "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\" ?><ns3:list xmlns:ns2=\"http://walmart.com/mp/orders\" xmlns:ns3=\"http://walmart.com/mp/v3/orders\" xmlns:ns4=\"http://walmart.com/\"><ns3:meta><ns3:totalCount>367</ns3:totalCount><ns3:limit>10</ns3:limit><ns3:nextCursor>123</ns3:nextCursor></ns3:meta><ns3:elements><ns3:order><ns3:purchaseOrderId>2575263094491</ns3:purchaseOrderId><ns3:customerOrderId>4091603648841</ns3:customerOrderId><ns3:customerEmailId>gsingh@walmartlabs.com</ns3:customerEmailId><ns3:orderDate>2016-05-18T16:53:14.000Z</ns3:orderDate><ns3:shippingInfo><ns3:phone>2342423234</ns3:phone><ns3:estimatedDeliveryDate>2016-06-22T06:00:00.000Z</ns3:estimatedDeliveryDate><ns3:estimatedShipDate>2016-06-15T06:00:00.000Z</ns3:estimatedShipDate><ns3:methodCode>Standard</ns3:methodCode><ns3:postalAddress><ns3:name>PGOMS Walmart</ns3:name><ns3:address1>850 Cherry Avenue</ns3:address1><ns3:address2>Floor 5</ns3:address2><ns3:city>San Bruno</ns3:city><ns3:state>CA</ns3:state><ns3:postalCode>94066</ns3:postalCode><ns3:country>USA</ns3:country><ns3:addressType>RESIDENTIAL</ns3:addressType></ns3:postalAddress></ns3:shippingInfo><ns3:orderLines><ns3:orderLine><ns3:lineNumber>4</ns3:lineNumber><ns3:item><ns3:productName>Kenmore CF1 or 2086883 Canister Secondary Filter Generic 2 Pack</ns3:productName><ns3:sku>RCA-OF-444gku444</ns3:sku></ns3:item><ns3:charges><ns3:charge><ns3:chargeType>PRODUCT</ns3:chargeType><ns3:chargeName>ItemPrice</ns3:chargeName><ns3:chargeAmount><ns3:currency>USD</ns3:currency><ns3:amount>25.00</ns3:amount></ns3:chargeAmount><ns3:tax><ns3:taxName>Tax1</ns3:taxName><ns3:taxAmount><ns3:currency>USD</ns3:currency><ns3:amount>1.87</ns3:amount></ns3:taxAmount></ns3:tax></ns3:charge></ns3:charges><ns3:orderLineQuantity><ns3:unitOfMeasurement>EACH</ns3:unitOfMeasurement><ns3:amount>1</ns3:amount></ns3:orderLineQuantity><ns3:statusDate>2016-05-18T17:01:26.000Z</ns3:statusDate><ns3:orderLineStatuses><ns3:orderLineStatus><ns3:status>Created</ns3:status><ns3:statusQuantity><ns3:unitOfMeasurement>EACH</ns3:unitOfMeasurement><ns3:amount>1</ns3:amount></ns3:statusQuantity></ns3:orderLineStatus></ns3:orderLineStatuses></ns3:orderLine></ns3:orderLines></ns3:order><ns3:order><ns3:purchaseOrderId>2575263094492</ns3:purchaseOrderId><ns3:customerOrderId>4091603648841</ns3:customerOrderId><ns3:customerEmailId>gsingh@walmartlabs.com</ns3:customerEmailId><ns3:orderDate>2016-05-18T16:53:14.000Z</ns3:orderDate><ns3:shippingInfo><ns3:phone>2342423234</ns3:phone><ns3:estimatedDeliveryDate>2016-06-22T06:00:00.000Z</ns3:estimatedDeliveryDate><ns3:estimatedShipDate>2016-06-15T06:00:00.000Z</ns3:estimatedShipDate><ns3:methodCode>Standard</ns3:methodCode><ns3:postalAddress><ns3:name>PGOMS Walmart</ns3:name><ns3:address1>850 Cherry Avenue</ns3:address1><ns3:address2>Floor 5</ns3:address2><ns3:city>San Bruno</ns3:city><ns3:state>CA</ns3:state><ns3:postalCode>94066</ns3:postalCode><ns3:country>USA</ns3:country><ns3:addressType>RESIDENTIAL</ns3:addressType></ns3:postalAddress></ns3:shippingInfo><ns3:orderLines><ns3:orderLine><ns3:lineNumber>1</ns3:lineNumber><ns3:item><ns3:productName>Kenmore CF1 or 2086883 Canister Secondary Filter Generic 2 Pack</ns3:productName><ns3:sku>RCA-OF-444gku444</ns3:sku></ns3:item><ns3:charges><ns3:charge><ns3:chargeType>PRODUCT</ns3:chargeType><ns3:chargeName>ItemPrice</ns3:chargeName><ns3:chargeAmount><ns3:currency>USD</ns3:currency><ns3:amount>25.00</ns3:amount></ns3:chargeAmount><ns3:tax><ns3:taxName>Tax1</ns3:taxName><ns3:taxAmount><ns3:currency>USD</ns3:currency><ns3:amount>1.89</ns3:amount></ns3:taxAmount></ns3:tax></ns3:charge></ns3:charges><ns3:orderLineQuantity><ns3:unitOfMeasurement>EACH</ns3:unitOfMeasurement><ns3:amount>1</ns3:amount></ns3:orderLineQuantity><ns3:statusDate>2016-05-18T17:01:26.000Z</ns3:statusDate><ns3:orderLineStatuses><ns3:orderLineStatus><ns3:status>Created</ns3:status><ns3:statusQuantity><ns3:unitOfMeasurement>EACH</ns3:unitOfMeasurement><ns3:amount>1</ns3:amount></ns3:statusQuantity></ns3:orderLineStatus></ns3:orderLineStatuses></ns3:orderLine></ns3:orderLines></ns3:order></ns3:elements></ns3:list>";
        private const string Order = "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?><ns3:order xmlns:ns2=\"http://walmart.com/mp/orders\" xmlns:ns3=\"http://walmart.com/mp/v3/orders\" xmlns:ns4=\"http://walmart.com/\"><ns3:purchaseOrderId>2575193093776</ns3:purchaseOrderId><ns3:customerOrderId>4021603941547</ns3:customerOrderId><ns3:customerEmailId>mgr@walmartlabs.com</ns3:customerEmailId><ns3:orderDate>2016-05-11T23:16:10.000Z</ns3:orderDate><ns3:shippingInfo><ns3:phone>6502248603</ns3:phone><ns3:estimatedDeliveryDate>2016-05-20T17:00:00.000Z</ns3:estimatedDeliveryDate><ns3:estimatedShipDate>2016-05-16T17:00:00.000Z</ns3:estimatedShipDate><ns3:methodCode>Standard</ns3:methodCode><ns3:postalAddress><ns3:name>Madhukara PGOMS</ns3:name><ns3:address1>860 W Cal Ave</ns3:address1><ns3:address2>Seat # 860C.2.176</ns3:address2><ns3:city>Sunnyvale</ns3:city><ns3:state>CA</ns3:state><ns3:postalCode>94086</ns3:postalCode><ns3:country>USA</ns3:country><ns3:addressType>RESIDENTIAL</ns3:addressType></ns3:postalAddress></ns3:shippingInfo><ns3:orderLines><ns3:orderLine><ns3:lineNumber>1</ns3:lineNumber><ns3:item><ns3:productName>Garmin Refurbished nuvi 2595LMT 5 GPS w Lifetime Maps and Traffic</ns3:productName><ns3:sku>GRMN100201</ns3:sku></ns3:item><ns3:charges><ns3:charge><ns3:chargeType>PRODUCT</ns3:chargeType><ns3:chargeName>ItemPrice</ns3:chargeName><ns3:chargeAmount><ns3:currency>USD</ns3:currency><ns3:amount>124.98</ns3:amount></ns3:chargeAmount><ns3:tax><ns3:taxName>Tax1</ns3:taxName><ns3:taxAmount><ns3:currency>USD</ns3:currency><ns3:amount>10.94</ns3:amount></ns3:taxAmount></ns3:tax></ns3:charge></ns3:charges><ns3:orderLineQuantity><ns3:unitOfMeasurement>EACH</ns3:unitOfMeasurement><ns3:amount>1</ns3:amount></ns3:orderLineQuantity><ns3:statusDate>2016-05-11T23:43:50.000Z</ns3:statusDate><ns3:orderLineStatuses><ns3:orderLineStatus><ns3:status>Created</ns3:status><ns3:statusQuantity><ns3:unitOfMeasurement>EACH</ns3:unitOfMeasurement><ns3:amount>1</ns3:amount></ns3:statusQuantity></ns3:orderLineStatus></ns3:orderLineStatuses></ns3:orderLine></ns3:orderLines></ns3:order>";

        public WalmartWebClientTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
        }

        [Fact]
        public void GetOrders_SetsUri()
        {
            DateTime start = DateTime.UtcNow.AddDays(-3);
            Mock<IHttpResponseReader> responseReader = mock.Mock<IHttpResponseReader>();
            responseReader.Setup(r => r.ReadResult()).Returns(OrdersResponse);

            Mock<IHttpXmlVariableRequestSubmitter> requestSubmitter = mock.Mock<IHttpXmlVariableRequestSubmitter>();
            requestSubmitter.Setup(r => r.GetResponse()).Returns(responseReader);

            WalmartWebClient testObject = mock.Create<WalmartWebClient>();

            testObject.GetOrders(new WalmartStoreEntity(), start);

            requestSubmitter.VerifySet(r => r.Uri = new Uri("https://marketplace.walmartapis.com/v3/orders"));
        }

        [Fact]
        public void GetOrders_SignsTheRequest()
        {
            Mock<IHttpResponseReader> responseReader = mock.Mock<IHttpResponseReader>();
            responseReader.Setup(r => r.ReadResult()).Returns(OrdersResponse);

            Mock<IHttpXmlVariableRequestSubmitter> requestSubmitter = mock.Mock<IHttpXmlVariableRequestSubmitter>();
            requestSubmitter.Setup(r => r.GetResponse()).Returns(responseReader);

            WalmartWebClient testObject = mock.Create<WalmartWebClient>();
            WalmartStoreEntity store = new WalmartStoreEntity();

            testObject.GetOrders(store, "nextCursorValue");

            mock.Mock<IWalmartRequestSigner>().Verify(s => s.Sign(requestSubmitter.Object, store, It.IsAny<string>()));
        }

        [Fact]
        public void GetOrders_AddsWalmartCredentialsHeaders()
        {
            DateTime start = DateTime.UtcNow.AddDays(-3);
            Mock<IHttpResponseReader> responseReader = mock.Mock<IHttpResponseReader>();
            responseReader.Setup(r => r.ReadResult()).Returns(OrdersResponse);

            WebHeaderCollection webHeaderCollection = new WebHeaderCollection();

            Mock<IHttpXmlVariableRequestSubmitter> requestSubmitter = mock.Mock<IHttpXmlVariableRequestSubmitter>();
            requestSubmitter.SetupGet(r => r.Headers).Returns(webHeaderCollection);
            requestSubmitter.Setup(r => r.GetResponse()).Returns(responseReader);

            WalmartWebClient testObject = mock.Create<WalmartWebClient>();

            WalmartStoreEntity store = new WalmartStoreEntity();
            store.ConsumerID = "blah blah consumer id";
            store.ChannelType = "channel type foo bar baz";

            testObject.GetOrders(store, start);

            Assert.Equal("Walmart Marketplace", webHeaderCollection.GetValues("WM_SVC.NAME").First());
            Assert.Equal(store.ConsumerID, webHeaderCollection.GetValues("WM_CONSUMER.ID").First());
            Assert.Equal(store.ChannelType, webHeaderCollection.GetValues("WM_CONSUMER.CHANNEL.TYPE").First());
            Assert.NotEmpty(webHeaderCollection.GetValues("WM_QOS.CORRELATION_ID"));
        }

        public void GetOrders_RethrowsWalmartException_WhenWebRequestThrowsWebException()
        {
            Mock<IHttpResponseReader> responseReader = mock.Mock<IHttpResponseReader>();
            responseReader.Setup(r => r.ReadResult()).Returns(OrdersResponse);

            Mock<IHttpXmlVariableRequestSubmitter> requestSubmitter = mock.Mock<IHttpXmlVariableRequestSubmitter>();
            requestSubmitter.Setup(r => r.GetResponse()).Throws(new WebException());

            WalmartWebClient testObject = mock.Create<WalmartWebClient>();

            Assert.Throws<WalmartException>(() => testObject.GetOrders(new WalmartStoreEntity(), "nextCursorValue"));
        }

        [Fact]
        public void GetOrders_SetsUriWithNextCursor()
        {
            Mock<IHttpResponseReader> responseReader = mock.Mock<IHttpResponseReader>();
            responseReader.Setup(r => r.ReadResult()).Returns(OrdersResponse);

            Mock<IHttpXmlVariableRequestSubmitter> requestSubmitter = mock.Mock<IHttpXmlVariableRequestSubmitter>();
            requestSubmitter.Setup(r => r.GetResponse()).Returns(responseReader);

            WalmartWebClient testObject = mock.Create<WalmartWebClient>();

            testObject.GetOrders(new WalmartStoreEntity(), "nextCursorValue");

            requestSubmitter.VerifySet(r => r.Uri = new Uri("https://marketplace.walmartapis.com/v3/ordersnextCursorValue"));
        }

        [Fact]
        public void GetOrders_SetsHttpVerb()
        {
            DateTime start = DateTime.UtcNow.AddDays(-3);
            Mock<IHttpResponseReader> responseReader = mock.Mock<IHttpResponseReader>();
            responseReader.Setup(r => r.ReadResult()).Returns(OrdersResponse);

            Mock<IHttpXmlVariableRequestSubmitter> requestSubmitter = mock.Mock<IHttpXmlVariableRequestSubmitter>();
            requestSubmitter.Setup(r => r.GetResponse()).Returns(responseReader);

            WalmartWebClient testObject = mock.Create<WalmartWebClient>();

            testObject.GetOrders(new WalmartStoreEntity(), start);

            requestSubmitter.VerifySet(r => r.Verb = HttpVerb.Get);
        }


        [Fact]
        public void GetOrders_AddsCreatedStartDateVariable()
        {
            DateTime start = DateTime.UtcNow.AddDays(-3);
            Mock<IHttpResponseReader> responseReader = mock.Mock<IHttpResponseReader>();
            responseReader.Setup(r => r.ReadResult()).Returns(OrdersResponse);

            HttpVariableCollection httpVariables = new HttpVariableCollection();

            Mock<IHttpXmlVariableRequestSubmitter> requestSubmitter = mock.Mock<IHttpXmlVariableRequestSubmitter>();
            requestSubmitter.SetupGet(r => r.Variables).Returns(httpVariables);
            requestSubmitter.Setup(r => r.GetResponse()).Returns(responseReader);

            WalmartWebClient testObject = mock.Create<WalmartWebClient>();

            testObject.GetOrders(new WalmartStoreEntity(), start);

            Assert.Contains(httpVariables, v => v.Name == "createdStartDate" && v.Value == start.ToString("s"));
        }

        [Fact]
        public void GetOrders_AsknowledgesEachOrder()
        {
            Mock<IHttpResponseReader> responseReader = mock.Mock<IHttpResponseReader>();
            responseReader.Setup(r => r.ReadResult()).Returns(OrdersResponse);
            responseReader.SetupSequence(r => r.ReadResult()).Returns(OrdersResponse).Returns(Order);

            Mock<IHttpXmlVariableRequestSubmitter> getOrdersRequestSubmitter = mock.Mock<IHttpXmlVariableRequestSubmitter>();
            getOrdersRequestSubmitter.Setup(r => r.GetResponse()).Returns(responseReader);

            Mock<IHttpXmlVariableRequestSubmitter> acknowledgeRequestSubmitter = mock.Mock<IHttpXmlVariableRequestSubmitter>();
            acknowledgeRequestSubmitter.Setup(r => r.GetResponse()).Returns(responseReader);

            Mock<Func<IHttpXmlVariableRequestSubmitter>> repo = mock.MockRepository.Create<Func<IHttpXmlVariableRequestSubmitter>>();
            repo.SetupSequence(r => r()).Returns(getOrdersRequestSubmitter.Object).Returns(acknowledgeRequestSubmitter.Object);
            mock.Provide(repo);

            WalmartWebClient testObject = mock.Create<WalmartWebClient>();

            testObject.GetOrders(new WalmartStoreEntity(), "");

            // There are 3 orders to acknowledge
            acknowledgeRequestSubmitter.Verify(r => r.GetResponse(), Times.Exactly(3));
        }

        public void Dispose()
        {
            mock?.Dispose();
        }
    }
}
