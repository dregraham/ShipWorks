using System;
using Autofac.Extras.Moq;
using Interapptive.Shared.Net;
using Interapptive.Shared.Utility;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping;
using ShipWorks.Stores.Platforms.Jet;
using ShipWorks.Stores.Platforms.Jet.DTO;
using ShipWorks.Stores.Platforms.Jet.DTO.Requests;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.Jet
{
    public class JetWebClientTest : IDisposable
    {
        private readonly AutoMock mock;
        private readonly Mock<IHttpVariableRequestSubmitter> variableRequestSubmitter;
        private readonly Mock<IHttpRequestSubmitter> requestSubmitter;
        private readonly Mock<IHttpRequestSubmitterFactory> requestSubmitterFactory;
        private readonly Mock<IJetShipmentRequestFactory> shipmentRequestFactory;

        private readonly JetStoreEntity store;

        public JetWebClientTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            store = new JetStoreEntity();

            variableRequestSubmitter = mock.Mock<IHttpVariableRequestSubmitter>();
            requestSubmitter = mock.Mock<IHttpRequestSubmitter>();

            requestSubmitterFactory = mock.Mock<IHttpRequestSubmitterFactory>();

            shipmentRequestFactory = mock.Mock<IJetShipmentRequestFactory>();
            shipmentRequestFactory.Setup(f => f.Create(It.IsAny<ShipmentEntity>())).Returns(new JetShipmentRequest());

            requestSubmitterFactory.Setup(h => h.GetHttpVariableRequestSubmitter()).Returns(variableRequestSubmitter);
            requestSubmitterFactory.Setup(h => h.GetHttpTextPostRequestSubmitter(It.IsAny<string>(), "application/json")).Returns(requestSubmitter);

            mock.Mock<IJetAuthenticatedRequest>()
                .Setup(r => r.Submit<JetShipResponse>(It.IsAny<string>(), It.IsAny<IHttpRequestSubmitter>(),
                    It.IsAny<JetStoreEntity>()))
                .Returns(GenericResult.FromSuccess(new JetShipResponse()));

        }

        [Fact]
        public void GetOrderDetails_DelegatesToJetRequest()
        {
            mock.Create<JetWebClient>().GetOrderDetails("url", store);

            mock.Mock<IJetAuthenticatedRequest>()
                .Verify(r => r.Submit<JetOrderDetailsResult>("GetOrderDetails", variableRequestSubmitter.Object, store));
        }

        [Fact]
        public void GetOrderDetails_GetsHttpRequestSubmitterFromFactory()
        {
            mock.Create<JetWebClient>().GetOrderDetails("/url", store);

            requestSubmitterFactory.Verify(f => f.GetHttpVariableRequestSubmitter());
        }

        [Fact]
        public void GetOrderDetails_UsesOrderDetailsUri()
        {
            mock.Create<JetWebClient>().GetOrderDetails("/url", store);

            variableRequestSubmitter.VerifySet(r => r.Uri = new Uri("https://merchant-api.jet.com/api/url"));
        }

        [Fact]
        public void GetOrderDetails_ReturnsResultFromJetRequest()
        {
            GenericResult<JetOrderDetailsResult> expectedResult = new GenericResult<JetOrderDetailsResult>();
            mock.Mock<IJetAuthenticatedRequest>()
                .Setup(r => r.Submit<JetOrderDetailsResult>("GetOrderDetails", requestSubmitter.Object, store))
                .Returns(expectedResult);

            GenericResult<JetOrderDetailsResult> actualResult = mock.Create<JetWebClient>().GetOrderDetails("url", store);
            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void GetOrders_DelegatesToJetRequest()
        {
            mock.Create<JetWebClient>().GetOrders(store);

            mock.Mock<IJetAuthenticatedRequest>()
                .Verify(r => r.Submit<JetOrderResponse>("GetOrders", variableRequestSubmitter.Object, store));
        }

        [Fact]
        public void GetOrders_UsesOrdersUri()
        {
            mock.Create<JetWebClient>().GetOrders(store);

            variableRequestSubmitter.VerifySet(r => r.Uri = new Uri("https://merchant-api.jet.com/api/orders/ready"));
        }

        [Fact]
        public void GetOrders_ReturnsResultsFromJetRequest()
        {
            GenericResult<JetOrderResponse> expectedResult = new GenericResult<JetOrderResponse>();
            mock.Mock<IJetAuthenticatedRequest>()
                .Setup(r => r.Submit<JetOrderResponse>("GetOrders", requestSubmitter.Object, store))
                .Returns(expectedResult);

            GenericResult<JetOrderResponse> actualResult = mock.Create<JetWebClient>().GetOrders(store);
            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void GetProduct_DelegatesToJetRequest()
        {
            JetOrderItem jetOrderItem = new JetOrderItem
            {
                MerchantSku = "123"
            };
            mock.Create<JetWebClient>().GetProduct(jetOrderItem, store);

            mock.Mock<IJetAuthenticatedRequest>()
                .Verify(r => r.Submit<JetProduct>("GetProduct", variableRequestSubmitter.Object, store));
        }

        [Fact]
        public void GetProducts_UsesProductsUri()
        {
            JetOrderItem jetOrderItem = new JetOrderItem
            {
                MerchantSku = "123"
            };

            mock.Create<JetWebClient>().GetProduct(jetOrderItem, store);

            variableRequestSubmitter.VerifySet(r => r.Uri = new Uri("https://merchant-api.jet.com/api/merchant-skus/123"));
        }

        [Fact]
        public void GetProduct_ReturnsResultsFromJetRequest()
        {
            JetOrderItem jetOrderItem = new JetOrderItem
            {
                MerchantSku = "123"
            };
            GenericResult<JetProduct> expectedResult = new GenericResult<JetProduct>();
            mock.Mock<IJetAuthenticatedRequest>()
                .Setup(r => r.Submit<JetProduct>("GetProduct", requestSubmitter.Object, store))
                .Returns(expectedResult);

            var actualResult = mock.Create<JetWebClient>().GetProduct(jetOrderItem, store);
            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void Acknowledge_UsesCorrectEndpoint()
        {
            JetOrderEntity order = new JetOrderEntity {MerchantOrderId = "1"};

            mock.Create<JetWebClient>().Acknowledge(order, store);

            requestSubmitter.VerifySet(r => r.Uri = new Uri("https://merchant-api.jet.com/api/orders/1/acknowledge"));
        }

        [Fact]
        public void UpdateShipmentDetails_DelegatesToShipmentRequestFactory()
        {
            JetOrderEntity order = new JetOrderEntity()
            {
                MerchantOrderId = "1",
                Store = store
            };
            ShipmentEntity shipment = new ShipmentEntity
            {
                Order = order
            };

            mock.Create<JetWebClient>().UploadShipmentDetails(shipment, store);

            shipmentRequestFactory.Verify(s => s.Create(shipment));
        }

        [Fact]
        public void UpdateShipmentDetails_DelegatesToSubmitterFactory()
        {
            JetOrderEntity order = new JetOrderEntity()
            {
                MerchantOrderId = "1",
                Store = store
            };
            ShipmentEntity shipment = new ShipmentEntity
            {
                Order = order
            };

            mock.Create<JetWebClient>().UploadShipmentDetails(shipment, store);

            requestSubmitterFactory.Verify(s => s.GetHttpTextPostRequestSubmitter(It.IsAny<string>(), "application/json"));
        }

        [Fact]
        public void UpdateShipmentDetails_UsesCorrectEndpoint()
        {
            JetOrderEntity order = new JetOrderEntity()
            {
                MerchantOrderId = "1",
                Store = store
            };
            ShipmentEntity shipment = new ShipmentEntity
            {
                Order = order
            };

            mock.Create<JetWebClient>().UploadShipmentDetails(shipment, store);
            requestSubmitter.VerifySet(r => r.Uri = new Uri("https://merchant-api.jet.com/api/orders/1/shipped"));
        }

        [Fact]
        public void UpdateShipmentDetails_UsesCorrectVerb()
        {
            JetOrderEntity order = new JetOrderEntity()
            {
                MerchantOrderId = "1",
                Store = store
            };
            ShipmentEntity shipment = new ShipmentEntity
            {
                Order = order
            };

            mock.Create<JetWebClient>().UploadShipmentDetails(shipment, store);
            requestSubmitter.VerifySet(r => r.Verb = HttpVerb.Put);
        }

        [Fact]
        public void UpdateShipmentDetails_DelegatesToJetRequest()
        {
            JetOrderEntity order = new JetOrderEntity()
            {
                MerchantOrderId = "1",
                Store = store
            };
            ShipmentEntity shipment = new ShipmentEntity
            {
                Order = order
            };

            mock.Create<JetWebClient>().UploadShipmentDetails(shipment, store);

            mock.Mock<IJetAuthenticatedRequest>()
                .Verify(r => r.Submit<JetShipResponse>("UploadShipmentDetails", requestSubmitter.Object, store));
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}