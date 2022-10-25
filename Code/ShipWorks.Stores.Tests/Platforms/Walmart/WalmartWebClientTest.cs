using System;
using System.Linq;
using System.Net;
using Autofac.Extras.Moq;
using Interapptive.Shared.Net;
using Interapptive.Shared.Utility;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Walmart;
using ShipWorks.Stores.Platforms.Walmart.DTO;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.Walmart
{
    public class WalmartWebClientTest : IDisposable
    {
        private readonly AutoMock mock;
        private readonly AutoMock acknowledgeMock;
        private const string OrdersResponse = "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\" ?><ns3:list xmlns:ns2=\"http://walmart.com/mp/orders\" xmlns:ns3=\"http://walmart.com/mp/v3/orders\" xmlns:ns4=\"http://walmart.com/\"><ns3:meta><ns3:totalCount>367</ns3:totalCount><ns3:limit>10</ns3:limit><ns3:nextCursor>123</ns3:nextCursor></ns3:meta><ns3:elements><ns3:order><ns3:purchaseOrderId>2575263094491</ns3:purchaseOrderId><ns3:customerOrderId>4091603648841</ns3:customerOrderId><ns3:customerEmailId>gsingh@walmartlabs.com</ns3:customerEmailId><ns3:orderDate>2016-05-18T16:53:14.000Z</ns3:orderDate><ns3:shippingInfo><ns3:phone>2342423234</ns3:phone><ns3:estimatedDeliveryDate>2016-06-22T06:00:00.000Z</ns3:estimatedDeliveryDate><ns3:estimatedShipDate>2016-06-15T06:00:00.000Z</ns3:estimatedShipDate><ns3:methodCode>Standard</ns3:methodCode><ns3:postalAddress><ns3:name>PGOMS Walmart</ns3:name><ns3:address1>850 Cherry Avenue</ns3:address1><ns3:address2>Floor 5</ns3:address2><ns3:city>San Bruno</ns3:city><ns3:state>CA</ns3:state><ns3:postalCode>94066</ns3:postalCode><ns3:country>USA</ns3:country><ns3:addressType>RESIDENTIAL</ns3:addressType></ns3:postalAddress></ns3:shippingInfo><ns3:orderLines><ns3:orderLine><ns3:lineNumber>4</ns3:lineNumber><ns3:item><ns3:productName>Kenmore CF1 or 2086883 Canister Secondary Filter Generic 2 Pack</ns3:productName><ns3:sku>RCA-OF-444gku444</ns3:sku></ns3:item><ns3:charges><ns3:charge><ns3:chargeType>PRODUCT</ns3:chargeType><ns3:chargeName>ItemPrice</ns3:chargeName><ns3:chargeAmount><ns3:currency>USD</ns3:currency><ns3:amount>25.00</ns3:amount></ns3:chargeAmount><ns3:tax><ns3:taxName>Tax1</ns3:taxName><ns3:taxAmount><ns3:currency>USD</ns3:currency><ns3:amount>1.87</ns3:amount></ns3:taxAmount></ns3:tax></ns3:charge></ns3:charges><ns3:orderLineQuantity><ns3:unitOfMeasurement>EACH</ns3:unitOfMeasurement><ns3:amount>1</ns3:amount></ns3:orderLineQuantity><ns3:statusDate>2016-05-18T17:01:26.000Z</ns3:statusDate><ns3:orderLineStatuses><ns3:orderLineStatus><ns3:status>Created</ns3:status><ns3:statusQuantity><ns3:unitOfMeasurement>EACH</ns3:unitOfMeasurement><ns3:amount>1</ns3:amount></ns3:statusQuantity></ns3:orderLineStatus></ns3:orderLineStatuses></ns3:orderLine></ns3:orderLines></ns3:order><ns3:order><ns3:purchaseOrderId>2575263094492</ns3:purchaseOrderId><ns3:customerOrderId>4091603648841</ns3:customerOrderId><ns3:customerEmailId>gsingh@walmartlabs.com</ns3:customerEmailId><ns3:orderDate>2016-05-18T16:53:14.000Z</ns3:orderDate><ns3:shippingInfo><ns3:phone>2342423234</ns3:phone><ns3:estimatedDeliveryDate>2016-06-22T06:00:00.000Z</ns3:estimatedDeliveryDate><ns3:estimatedShipDate>2016-06-15T06:00:00.000Z</ns3:estimatedShipDate><ns3:methodCode>Standard</ns3:methodCode><ns3:postalAddress><ns3:name>PGOMS Walmart</ns3:name><ns3:address1>850 Cherry Avenue</ns3:address1><ns3:address2>Floor 5</ns3:address2><ns3:city>San Bruno</ns3:city><ns3:state>CA</ns3:state><ns3:postalCode>94066</ns3:postalCode><ns3:country>USA</ns3:country><ns3:addressType>RESIDENTIAL</ns3:addressType></ns3:postalAddress></ns3:shippingInfo><ns3:orderLines><ns3:orderLine><ns3:lineNumber>1</ns3:lineNumber><ns3:item><ns3:productName>Kenmore CF1 or 2086883 Canister Secondary Filter Generic 2 Pack</ns3:productName><ns3:sku>RCA-OF-444gku444</ns3:sku></ns3:item><ns3:charges><ns3:charge><ns3:chargeType>PRODUCT</ns3:chargeType><ns3:chargeName>ItemPrice</ns3:chargeName><ns3:chargeAmount><ns3:currency>USD</ns3:currency><ns3:amount>25.00</ns3:amount></ns3:chargeAmount><ns3:tax><ns3:taxName>Tax1</ns3:taxName><ns3:taxAmount><ns3:currency>USD</ns3:currency><ns3:amount>1.89</ns3:amount></ns3:taxAmount></ns3:tax></ns3:charge></ns3:charges><ns3:orderLineQuantity><ns3:unitOfMeasurement>EACH</ns3:unitOfMeasurement><ns3:amount>1</ns3:amount></ns3:orderLineQuantity><ns3:statusDate>2016-05-18T17:01:26.000Z</ns3:statusDate><ns3:orderLineStatuses><ns3:orderLineStatus><ns3:status>Created</ns3:status><ns3:statusQuantity><ns3:unitOfMeasurement>EACH</ns3:unitOfMeasurement><ns3:amount>1</ns3:amount></ns3:statusQuantity><ns3:trackingInfo><ns3:shipDateTime>2022-07-01T15:43:12.000Z</ns3:shipDateTime><ns3:carrierName><ns3:carrier>FOO</ns3:carrier></ns3:carrierName><ns3:methodCode>Standard</ns3:methodCode><ns3:trackingNumber>420054559374810912400494223439</ns3:trackingNumber><ns3:trackingURL>https://www.walmart.com/tracking?order_id=4871837592258&amp;tracking_id=420054559374810912400494223439</ns3:trackingURL></ns3:trackingInfo></ns3:orderLineStatus></ns3:orderLineStatuses></ns3:orderLine></ns3:orderLines></ns3:order></ns3:elements></ns3:list>";
        private const string OrderFormat = "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?><ns3:order xmlns:ns2=\"http://walmart.com/mp/orders\" xmlns:ns3=\"http://walmart.com/mp/v3/orders\" xmlns:ns4=\"http://walmart.com/\"><ns3:purchaseOrderId>2575193093776</ns3:purchaseOrderId><ns3:customerOrderId>4021603941547</ns3:customerOrderId><ns3:customerEmailId>mgr@walmartlabs.com</ns3:customerEmailId><ns3:orderDate>2016-05-11T23:16:10.000Z</ns3:orderDate><ns3:shippingInfo><ns3:phone>6502248603</ns3:phone><ns3:estimatedDeliveryDate>2016-05-20T17:00:00.000Z</ns3:estimatedDeliveryDate><ns3:estimatedShipDate>2016-05-16T17:00:00.000Z</ns3:estimatedShipDate><ns3:methodCode>Standard</ns3:methodCode><ns3:postalAddress><ns3:name>Madhukara PGOMS</ns3:name><ns3:address1>860 W Cal Ave</ns3:address1><ns3:address2>Seat # 860C.2.176</ns3:address2><ns3:city>Sunnyvale</ns3:city><ns3:state>CA</ns3:state><ns3:postalCode>94086</ns3:postalCode><ns3:country>USA</ns3:country><ns3:addressType>RESIDENTIAL</ns3:addressType></ns3:postalAddress></ns3:shippingInfo><ns3:orderLines><ns3:orderLine><ns3:lineNumber>1</ns3:lineNumber><ns3:item><ns3:productName>Garmin Refurbished nuvi 2595LMT 5 GPS w Lifetime Maps and Traffic</ns3:productName><ns3:sku>GRMN100201</ns3:sku></ns3:item><ns3:charges><ns3:charge><ns3:chargeType>PRODUCT</ns3:chargeType><ns3:chargeName>ItemPrice</ns3:chargeName><ns3:chargeAmount><ns3:currency>USD</ns3:currency><ns3:amount>124.98</ns3:amount></ns3:chargeAmount><ns3:tax><ns3:taxName>Tax1</ns3:taxName><ns3:taxAmount><ns3:currency>USD</ns3:currency><ns3:amount>10.94</ns3:amount></ns3:taxAmount></ns3:tax></ns3:charge></ns3:charges><ns3:orderLineQuantity><ns3:unitOfMeasurement>EACH</ns3:unitOfMeasurement><ns3:amount>1</ns3:amount></ns3:orderLineQuantity><ns3:statusDate>2016-05-11T23:43:50.000Z</ns3:statusDate><ns3:orderLineStatuses><ns3:orderLineStatus><ns3:status>{0}</ns3:status><ns3:statusQuantity><ns3:unitOfMeasurement>EACH</ns3:unitOfMeasurement><ns3:amount>1</ns3:amount></ns3:statusQuantity><ns3:trackingInfo><ns3:shipDateTime>2022-07-01T15:43:12.000Z</ns3:shipDateTime><ns3:carrierName><ns3:carrier>DHL Ecommerce - US</ns3:carrier></ns3:carrierName><ns3:methodCode>Standard</ns3:methodCode><ns3:trackingNumber>420054559374810912400494223439</ns3:trackingNumber><ns3:trackingURL>https://www.walmart.com/tracking?order_id=4871837592258&amp;tracking_id=420054559374810912400494223439</ns3:trackingURL></ns3:trackingInfo></ns3:orderLineStatus></ns3:orderLineStatuses></ns3:orderLine></ns3:orderLines></ns3:order>";
        private const string OutageErrorResponse = "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?><ns4:errors xmlns:ns2=\"http://walmart.com/mp/orders\" xmlns:ns3=\"http://walmart.com/mp/v3/orders\" xmlns:ns4=\"http://walmart.com/\"><ns4:error><ns4:code>INVALID_REQUEST.GMP_ORDER_API</ns4:code><ns4:field>data</ns4:field><ns4:description>Invalid Request</ns4:description><ns4:info>Request invalid.</ns4:info><ns4:severity>ERROR</ns4:severity><ns4:category>DATA</ns4:category><ns4:causes/><ns4:errorIdentifiers/></ns4:error></ns4:errors>";
        private const string ErrorResponse = "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?><ns4:errors xmlns:ns2=\"http://walmart.com/mp/orders\" xmlns:ns3=\"http://walmart.com/mp/v3/orders\" xmlns:ns4=\"http://walmart.com/\"><ns4:error><ns4:code>INVALID_REQUEST_HEADER.GMP_ORDER_API</ns4:code><ns4:field>WM_SVC.ENV</ns4:field><ns4:description>WM_SVC.ENV set blank or null</ns4:description><ns4:info>One or more request headers are invalid.</ns4:info><ns4:severity>ERROR</ns4:severity><ns4:category>DATA</ns4:category><ns4:causes/><ns4:errorIdentifiers/></ns4:error><ns4:error><ns4:code>INVALID_REQUEST_HEADER.GMP_ORDER_API</ns4:code><ns4:field>WM_CONSUMER.ID</ns4:field><ns4:description>WM_CONSUMER.ID set blank or null</ns4:description><ns4:info>One or more request headers are invalid.</ns4:info><ns4:severity>ERROR</ns4:severity><ns4:category>DATA</ns4:category><ns4:causes/><ns4:errorIdentifiers/></ns4:error></ns4:errors>";
        private const string OAuthTokenResponse = "<OAuthTokenDTO xmlns=\"\"><accessToken>eyJraWQiOiIzN2JmOWQ5MS04ZDRkLTQwYjEtODU4NS1mNzhlZDc3MjM4MDQiLCJlbmMiOiJBMjU2R0NNIiwiYWxnIjoiZGlyIn0..yI0d0EFgt3gpEMQj.APvV5upgh1nBRcZPG6d9a5PYNAwayZdIZc4s-J42Ol-Zk6V9liVWmgs4qyPdkghfLKWwAyOBoc5g1vxRsYQNOeDH_p7KM_dO3D80g8adtGDkcAoEimyhP0inGHOjT2PziwIWjtRbE5bm8WTZN7wwJPV5SFqBP-XvS03OdPcZ2V_f0mVln5EdzaRt0BmpvlVDgqn_9Pe_jIYadWX-qc1N_Lh6oe3Q6bUKTJIhb6N-v8dNDO6qyRbeWo0_0M1s7g_mBKIp2FeO8a_ezIkpjtRv81SwXxEDqFacT2O46GlILO0Nfh7lbI2HZK2eUPrC-XVCiLiGoKsQDxHxD_Po0QrH0OxD6jnQvUfiqpgsULsvtbsSuHpUNl-OlIFhxODRxyIILsjAoWaQi9yEEV5swLVTdCix7e8ZKJPWQKEi55e7WYZm8vJqIUaaKrgdbw8HDlOWiJcrzzKdg-Hk3QBxnuoT4wqiaJaKb3uIQUtPzo5Jn_58PIxHh1WawpJOOmTH5RFgGTHlMB-5nBKG0iW1Bzm_vlg0NU_ZlYjvTWWDGOldk18WbzBr88XUI_jyRylFp_gYxc2peAyPnhZtOJGC4-7Eudbjz7QtClntVenjcG9h0k-xUejD0fcvCQUCty8S0ZfvCMEhrOJXEYCfI9-ESFFzpRh0EDCyfllS2Ugor4ZtUBjztXsj5sz2tp1wOSG7QR22K8rBKBFSyaKdIrNoLoucCEFFqlK_WSxfgsvUhspr4ZztxzPCejv3pKu9XYOV5nM6_qRsQY9ub88kQQKf2ZwNUnuYM_JYcvr2P9R551Pqqssk2KmbU42P35a2t5xJo9h921tGxUNjS9k4LAaO9g.Em_yWzxnUwwvs90IRZzH-Q</accessToken><tokenType>Bearer</tokenType><expiresIn>900</expiresIn></OAuthTokenDTO>";
        private const string UnexpectedError = "Unexpected Error";
        private const string BaseErrorMessage = "ShipWorks encountered an error communicating with Walmart";

        public WalmartWebClientTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            acknowledgeMock = AutoMockExtensions.GetLooseThatReturnsMocks();

            mock.Mock<IWalmartWebClientSettings>().SetupGet(x => x.Endpoint).Returns("https://marketplace.walmartapis.com");

            SetupAcknowledgeOrderResponse();
        }

        [Fact]
        public void TestConnection_SetsUri()
        {
            Mock<IHttpVariableRequestSubmitter> requestSubmitter = SetupHttpVariableRequestSubmitter(OAuthTokenResponse, "TestConnectionResponse");

            WalmartWebClient testObject = mock.Create<WalmartWebClient>();
            WalmartStoreEntity store = new WalmartStoreEntity();

            store.ClientID = "ClientID";
            store.ClientSecret = "ClientSecret";

            testObject.TestConnection(store);

            requestSubmitter.VerifySet(r => r.Uri = new Uri("https://marketplace.walmartapis.com/v3/feeds"));
        }

        [Fact]
        public void GetOrders_SetsUri()
        {
            DateTime start = DateTime.UtcNow.AddDays(-3);
            Mock<IHttpVariableRequestSubmitter> requestSubmitter = SetupHttpVariableRequestSubmitter(OAuthTokenResponse, OrdersResponse);

            WalmartWebClient testObject = mock.Create<WalmartWebClient>();
            WalmartStoreEntity store = new WalmartStoreEntity();

            store.ClientID = "ClientID";
            store.ClientSecret = "ClientSecret";

            testObject.GetOrders(store, start);

            requestSubmitter.VerifySet(r => r.Uri = new Uri("https://marketplace.walmartapis.com/v3/orders"));
        }

        [Fact]
        public void GetOrders_AddsWalmartCredentialsHeaders()
        {
            DateTime start = DateTime.UtcNow.AddDays(-3);
            WebHeaderCollection webHeaderCollection = new WebHeaderCollection();

            Mock<IHttpVariableRequestSubmitter> requestSubmitter = SetupHttpVariableRequestSubmitter(OAuthTokenResponse, OrdersResponse);
            requestSubmitter.SetupGet(r => r.Headers).Returns(webHeaderCollection);

            WalmartWebClient testObject = mock.Create<WalmartWebClient>();

            WalmartStoreEntity store = new WalmartStoreEntity();

            store.ClientID = "ClientID";
            store.ClientSecret = "ClientSecret";

            testObject.GetOrders(store, start);

            Assert.Equal("Walmart Marketplace", webHeaderCollection.GetValues("WM_SVC.NAME").First());
            Assert.NotEmpty(webHeaderCollection.GetValues("Authorization"));
            Assert.NotEmpty(webHeaderCollection.GetValues("WM_QOS.CORRELATION_ID"));
        }

        [Fact]
        public void GetOrders_RethrowsWalmartException_WhenWebRequestThrowsWebException()
        {
            mock.Mock<IHttpVariableRequestSubmitter>()
                .Setup(r => r.GetResponse())
                .Throws(new WebException());

            WalmartWebClient testObject = mock.Create<WalmartWebClient>();

            Assert.Throws<WalmartException>(() => testObject.GetOrders(new WalmartStoreEntity(), "nextCursorValue"));
        }

        [Fact]
        public void GetOrders_SetsUriWithNextCursor()
        {
            var requestSubmitter = SetupHttpVariableRequestSubmitter(OAuthTokenResponse, OrdersResponse);

            WalmartWebClient testObject = mock.Create<WalmartWebClient>();
            WalmartStoreEntity store = new WalmartStoreEntity();

            store.ClientID = "ClientID";
            store.ClientSecret = "ClientSecret";

            testObject.GetOrders(store, "nextCursorValue");

            requestSubmitter.VerifySet(r => r.Uri = new Uri("https://marketplace.walmartapis.com/v3/ordersnextCursorValue"));
        }

        [Fact]
        public void GetOrders_SetsHttpVerb()
        {
            DateTime start = DateTime.UtcNow.AddDays(-3);
            var requestSubmitter = SetupHttpVariableRequestSubmitter(OAuthTokenResponse, OrdersResponse);

            WalmartWebClient testObject = mock.Create<WalmartWebClient>();
            WalmartStoreEntity store = new WalmartStoreEntity();

            store.ClientID = "ClientID";
            store.ClientSecret = "ClientSecret";

            testObject.GetOrders(store, start);

            requestSubmitter.VerifySet(r => r.Verb = HttpVerb.Get);
        }


        [Fact]
        public void GetOrders_AddsCreatedStartDateVariable()
        {
            DateTime start = DateTime.UtcNow.AddDays(-3);

            var requestSubmitter = SetupHttpVariableRequestSubmitter(OAuthTokenResponse, OrdersResponse);
            HttpVariableCollection httpVariables = new HttpVariableCollection();
            requestSubmitter.SetupGet(r => r.Variables).Returns(httpVariables);

            WalmartWebClient testObject = mock.Create<WalmartWebClient>();
            WalmartStoreEntity store = new WalmartStoreEntity();

            store.ClientID = "ClientID";
            store.ClientSecret = "ClientSecret";

            testObject.GetOrders(store, start);

            Assert.Contains(httpVariables, v => v.Name == "createdStartDate" && v.Value == start.ToString("s"));
        }

        [Fact]
        public void GetOrders_WithNextCursor_AcknowledgesEachOrder()
        {
            SetupHttpVariableRequestSubmitter(OAuthTokenResponse, OrdersResponse);

            WalmartWebClient testObject = mock.Create<WalmartWebClient>();
            WalmartStoreEntity store = new WalmartStoreEntity();

            store.ClientID = "ClientID";
            store.ClientSecret = "ClientSecret";

            testObject.GetOrders(store, "");

            // There are 2 orders to acknowledge
            acknowledgeMock.Mock<IHttpRequestSubmitter>().Verify(r => r.GetResponse(), Times.Exactly(2));
        }

        [Fact]
        public void GetOrders_WithStartDate_AcknowledgesEachOrder()
        {
            SetupHttpVariableRequestSubmitter(OAuthTokenResponse, OrdersResponse);

            WalmartWebClient testObject = mock.Create<WalmartWebClient>();
            WalmartStoreEntity store = new WalmartStoreEntity();

            store.ClientID = "ClientID";
            store.ClientSecret = "ClientSecret";

            testObject.GetOrders(store, new DateTime());

            // There are 2 orders to acknowledge
            acknowledgeMock.Mock<IHttpRequestSubmitter>().Verify(r => r.GetResponse(), Times.Exactly(2));
        }

        [Fact]
        public void GetOrders_ReturnsAcknowledgedOrders()
        {
            SetupHttpVariableRequestSubmitter(OAuthTokenResponse, OrdersResponse);

            WalmartWebClient testObject = mock.Create<WalmartWebClient>();
            WalmartStoreEntity store = new WalmartStoreEntity();

            store.ClientID = "ClientID";
            store.ClientSecret = "ClientSecret";

            ordersListType orders = testObject.GetOrders(store, new DateTime());

            Assert.Equal(2, orders.elements.Count(order => order.orderLines
                .All(line => line.orderLineStatuses[0].status == orderLineStatusValueType.Acknowledged)));
        }

        [Fact]
        public void GetOrders_ReturnsAcknowledgedOrders_WithCarrier()
        {
            SetupHttpVariableRequestSubmitter(OAuthTokenResponse, OrdersResponse);

            WalmartWebClient testObject = mock.Create<WalmartWebClient>();
            WalmartStoreEntity store = new WalmartStoreEntity();

            store.ClientID = "ClientID";
            store.ClientSecret = "ClientSecret";

            ordersListType orders = testObject.GetOrders(store, new DateTime());
            foreach (var order in orders.elements)
            {
                Assert.Equal(EnumHelper.GetApiValue(WalmartCarrierType.DHL_Ecommerce_US),
                    order.orderLines[0].orderLineStatuses[0].trackingInfo.carrierName.Item);
            }
        }

        [Fact]
        public void UpdateShipmentDetails_UsesCorrectUri()
        {
            SetupHttpVariableRequestSubmitter(OAuthTokenResponse);

            WalmartWebClient testObject = mock.Create<WalmartWebClient>();
            WalmartStoreEntity store = new WalmartStoreEntity();

            store.ClientID = "ClientID";
            store.ClientSecret = "ClientSecret";

            testObject.UpdateShipmentDetails(store, new orderShipment(), "123");

            acknowledgeMock.Mock<IHttpRequestSubmitter>()
                .VerifySet(submitter => submitter.Uri = new Uri("https://marketplace.walmartapis.com/v3/orders/123/shipping"));
        }

        [Fact]
        public void UpdateShipmentDetails_UsesTextPostSubmitterFromFactory()
        {
            SetupHttpVariableRequestSubmitter(OAuthTokenResponse);

            WalmartWebClient testObject = mock.Create<WalmartWebClient>();
            WalmartStoreEntity store = new WalmartStoreEntity();

            store.ClientID = "ClientID";
            store.ClientSecret = "ClientSecret";

            testObject.UpdateShipmentDetails(store, new orderShipment(), "123");

            mock.Mock<IHttpRequestSubmitterFactory>()
                .Verify(f => f.GetHttpTextPostRequestSubmitter(It.IsAny<string>(), @"application/xml"));
        }

        [Fact]
        public void UpdateShipmentDetails_ReturnsOrder()
        {
            SetupHttpVariableRequestSubmitter(OAuthTokenResponse);

            WalmartWebClient testObject = mock.Create<WalmartWebClient>();

            WalmartStoreEntity store = new WalmartStoreEntity();

            store.ClientID = "ClientID";
            store.ClientSecret = "ClientSecret";

            var order = testObject.UpdateShipmentDetails(store, new orderShipment(), "123");
            var expectedOrder =
                SerializationUtility.DeserializeFromXml<Order>(GetOrderText(orderLineStatusValueType.Acknowledged));

            Assert.Equal(SerializationUtility.SerializeToXml(expectedOrder), SerializationUtility.SerializeToXml(order));
        }

        [Fact]
        public void GetOrder_SetsUriWithPurchaseOrderId()
        {
            Mock<IHttpVariableRequestSubmitter> requestSubmitter = SetupHttpVariableRequestSubmitter(OAuthTokenResponse, GetOrderText(orderLineStatusValueType.Acknowledged));
            WalmartWebClient testObject = mock.Create<WalmartWebClient>();
            WalmartStoreEntity store = new WalmartStoreEntity();

            store.ClientID = "ClientID";
            store.ClientSecret = "ClientSecret";

            testObject.GetOrder(store, "12345678");

            requestSubmitter.VerifySet(r => r.Uri = new Uri("https://marketplace.walmartapis.com/v3/orders/12345678"));
        }

        [Fact]
        public void GetOrders_ThrowsWalmartExceptionWithErrorMessageFromErrorDTO()
        {
            SetupHttpVariableRequestSubmitterForBadRequest(OAuthTokenResponse, ErrorResponse);

            WalmartWebClient testObject = mock.Create<WalmartWebClient>();
            WalmartStoreEntity store = new WalmartStoreEntity();

            store.ClientID = "ClientID";
            store.ClientSecret = "ClientSecret";

            Exception ex = Assert.Throws<WalmartException>(() => testObject.GetOrders(store, "nextCursorValue"));

            Assert.Equal($"{BaseErrorMessage}:{Environment.NewLine}400 Bad Request{Environment.NewLine}WM_SVC.ENV set blank or null, WM_CONSUMER.ID set blank or null", ex.Message);
        }

        [Fact]
        public void GetOrders_ThrowsWalmartExceptionWithTryAgainLaterMessage_WhenErrorMatchesOutageError()
        {
            SetupHttpVariableRequestSubmitterForBadRequest(OAuthTokenResponse, OutageErrorResponse);

            WalmartWebClient testObject = mock.Create<WalmartWebClient>();
            WalmartStoreEntity store = new WalmartStoreEntity();

            store.ClientID = "ClientID";
            store.ClientSecret = "ClientSecret";

            Exception ex = Assert.Throws<WalmartException>(() => testObject.GetOrders(store, "nextCursorValue"));

            Assert.Equal($"{BaseErrorMessage}. Please try again later. 400 Bad Request", ex.Message);
        }

        [Fact]
        public void GetOrders_ThrowsWalmartExceptionWithDotNetExceptionMessage_WhenUnableToParseErrorDTO()
        {
            SetupHttpVariableRequestSubmitterForBadRequest(OAuthTokenResponse, UnexpectedError);

            WalmartWebClient testObject = mock.Create<WalmartWebClient>();
            WalmartStoreEntity store = new WalmartStoreEntity();

            store.ClientID = "ClientID";
            store.ClientSecret = "ClientSecret";

            Exception ex = Assert.Throws<WalmartException>(() => testObject.GetOrders(store, "nextCursorValue"));

            Assert.Equal($"ShipWorks encountered an error communicating with Walmart: {Environment.NewLine}400 Bad Request", ex.Message);
        }

        [Fact]
        public void GenerateAuthString_ThrowsWalmartException_WhenClientIDIsBlank()
        {
            DateTime start = DateTime.UtcNow.AddDays(-3);

            WalmartWebClient testObject = mock.Create<WalmartWebClient>();

            WalmartStoreEntity store = new WalmartStoreEntity();

            store.ClientID = "";
            store.ClientSecret = "ClientSecret";

            Exception ex = Assert.Throws<WalmartException>(() => testObject.GetOrders(store, start));

            Assert.Equal("ShipWorks requires updated credentials to connect to Walmart.\nFor more help, click: Manage > Stores > Edit Walmart store > Store Connection", ex.Message);
        }

        [Fact]
        public void GenerateAuthString_ThrowsWalmartException_WhenClientSecretIsBlank()
        {
            DateTime start = DateTime.UtcNow.AddDays(-3);

            WalmartWebClient testObject = mock.Create<WalmartWebClient>();

            WalmartStoreEntity store = new WalmartStoreEntity();

            store.ClientSecret = "";
            store.ClientID = "ClientID";

            Exception ex = Assert.Throws<WalmartException>(() => testObject.GetOrders(store, start));

            Assert.Equal("ShipWorks requires updated credentials to connect to Walmart.\nFor more help, click: Manage > Stores > Edit Walmart store > Store Connection", ex.Message);
        }

        private Mock<IHttpVariableRequestSubmitter> SetupHttpVariableRequestSubmitter(string response)
        {
            Mock<IHttpResponseReader> responseReader = mock.Mock<IHttpResponseReader>();
            responseReader.SetupSequence(r => r.ReadResult())
                .Returns(response)
                .CallBase();
            responseReader.Setup(r => r.HttpWebResponse.StatusCode).Returns(HttpStatusCode.OK);

            Mock<IHttpVariableRequestSubmitter> requestSubmitter = mock.Mock<IHttpVariableRequestSubmitter>();
            requestSubmitter.Setup(r => r.GetResponse()).Returns(responseReader);
            mock.Mock<IHttpRequestSubmitterFactory>()
                .Setup(f => f.GetHttpVariableRequestSubmitter())
                .Returns(requestSubmitter.Object);

            return requestSubmitter;
        }

        private Mock<IHttpVariableRequestSubmitter> SetupHttpVariableRequestSubmitter(string response1, string response2)
        {
            Mock<IHttpResponseReader> responseReader = mock.Mock<IHttpResponseReader>();
            responseReader.SetupSequence(r => r.ReadResult())
                .Returns(response1)
                .Returns(response2);
            responseReader.Setup(r => r.HttpWebResponse.StatusCode).Returns(HttpStatusCode.OK);

            Mock<IHttpVariableRequestSubmitter> requestSubmitter = mock.Mock<IHttpVariableRequestSubmitter>();
            requestSubmitter.Setup(r => r.GetResponse()).Returns(responseReader);
            mock.Mock<IHttpRequestSubmitterFactory>()
                .Setup(f => f.GetHttpVariableRequestSubmitter())
                .Returns(requestSubmitter.Object);

            return requestSubmitter;
        }

        private void SetupHttpVariableRequestSubmitterForBadRequest(string response1, string response2)
        {
            Mock<IHttpResponseReader> responseReader = mock.Mock<IHttpResponseReader>();
            int readResultCounter = 0;
            int statusCodeCounter = 0;
            responseReader.Setup(r => r.ReadResult())
                .Returns(() =>
                {
                    readResultCounter++;
                    if (readResultCounter < 2)
                    {
                        return response1;
                    }
                    return response2;
                });
            responseReader.Setup(r => r.HttpWebResponse.StatusCode)
                .Returns(() =>
                {
                    statusCodeCounter++;
                    if (statusCodeCounter < 2)
                    {
                        return HttpStatusCode.OK;
                    }
                    return HttpStatusCode.BadRequest;
                });
            responseReader.SetupSequence(r => r.HttpWebResponse.StatusDescription).Returns("Bad Request");

            Mock<IHttpVariableRequestSubmitter> requestSubmitter = mock.Mock<IHttpVariableRequestSubmitter>();
            requestSubmitter.Setup(r => r.GetResponse()).Returns(responseReader);
            mock.Mock<IHttpRequestSubmitterFactory>()
                .Setup(f => f.GetHttpVariableRequestSubmitter())
                .Returns(requestSubmitter.Object);
        }

        private void SetupAcknowledgeOrderResponse()
        {
            var responseReader = acknowledgeMock.Mock<IHttpResponseReader>();
            responseReader.Setup(r => r.ReadResult()).Returns(GetOrderText(orderLineStatusValueType.Acknowledged));
            responseReader.Setup(r => r.HttpWebResponse.StatusCode).Returns(HttpStatusCode.OK);

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
