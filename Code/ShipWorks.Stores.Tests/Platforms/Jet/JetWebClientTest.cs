using System;
using Autofac.Extras.Moq;
using Interapptive.Shared.Net;
using Interapptive.Shared.Utility;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Jet;
using ShipWorks.Stores.Platforms.Jet.DTO;
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
        private readonly JetStoreEntity store;

        public JetWebClientTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            store = new JetStoreEntity();

            variableRequestSubmitter = mock.Mock<IHttpVariableRequestSubmitter>();
            requestSubmitter = mock.Mock<IHttpRequestSubmitter>();

            requestSubmitterFactory = mock.Mock<IHttpRequestSubmitterFactory>();

            requestSubmitterFactory.Setup(h => h.GetHttpVariableRequestSubmitter()).Returns(variableRequestSubmitter);
            requestSubmitterFactory.Setup(h => h.GetHttpTextPostRequestSubmitter(It.IsAny<string>(), "application/json")).Returns(requestSubmitter);
        }

        [Fact]
        public void GetOrderDetails_DelegatesToJetRequest()
        {
            mock.Create<JetWebClient>().GetOrderDetails("url", store);
            
            mock.Mock<IJetAuthenticatedRequest>()
                .Verify(r => r.ProcessRequest<JetOrderDetailsResult>("GetOrderDetails", variableRequestSubmitter.Object, store));
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
            JetStoreEntity store = new JetStoreEntity();
            GenericResult<JetOrderDetailsResult> expectedResult = new GenericResult<JetOrderDetailsResult>();
            mock.Mock<IJetAuthenticatedRequest>()
                .Setup(r => r.ProcessRequest<JetOrderDetailsResult>("GetOrderDetails", requestSubmitter.Object, store))
                .Returns(expectedResult);

            GenericResult<JetOrderDetailsResult> actualResult = mock.Create<JetWebClient>().GetOrderDetails("url", store);
            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void GetOrders_DelegatesToJetRequest()
        {
            mock.Create<JetWebClient>().GetOrders(store);
            
            mock.Mock<IJetAuthenticatedRequest>()
                .Verify(r => r.ProcessRequest<JetOrderResponse>("GetOrders", variableRequestSubmitter.Object, store));
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
                .Setup(r => r.ProcessRequest<JetOrderResponse>("GetOrders", requestSubmitter.Object, store))
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
                .Verify(r => r.ProcessRequest<JetProduct>("GetProduct", variableRequestSubmitter.Object, store));
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
                .Setup(r => r.ProcessRequest<JetProduct>("GetProduct", requestSubmitter.Object, store))
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

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}