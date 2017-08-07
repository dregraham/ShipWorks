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

        public JetWebClientTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            variableRequestSubmitter = mock.Mock<IHttpVariableRequestSubmitter>();
            requestSubmitter = mock.Mock<IHttpRequestSubmitter>();

            mock.Mock<IHttpRequestSubmitterFactory>().Setup(h => h.GetHttpVariableRequestSubmitter()).Returns(variableRequestSubmitter);
            mock.Mock<IHttpRequestSubmitterFactory>().Setup(h => h.GetHttpTextPostRequestSubmitter(It.IsAny<string>(), "application/json")).Returns(requestSubmitter);
        }

        [Fact]
        public void GetOrderDetails_DelegatesToJetRequest()
        {
            JetStoreEntity store = new JetStoreEntity();
            mock.Create<JetWebClient>().GetOrderDetails("url", store);
            
            mock.Mock<IJetAuthenticatedRequest>()
                .Verify(r => r.ProcessRequest<JetOrderDetailsResult>("GetOrderDetails", variableRequestSubmitter.Object, store, true));
        }

        [Fact]
        public void GetOrderDetails_ReturnsResultFromJetRequest()
        {
            JetStoreEntity store = new JetStoreEntity();
            GenericResult<JetOrderDetailsResult> expectedResult = new GenericResult<JetOrderDetailsResult>();
            mock.Mock<IJetAuthenticatedRequest>()
                .Setup(r => r.ProcessRequest<JetOrderDetailsResult>("GetOrderDetails", requestSubmitter.Object, store, true))
                .Returns(expectedResult);

            GenericResult<JetOrderDetailsResult> actualResult = mock.Create<JetWebClient>().GetOrderDetails("url", store);
            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void GetOrders_DelegatesToJetRequest()
        {
            JetStoreEntity store = new JetStoreEntity();
            mock.Create<JetWebClient>().GetOrders(store);
            
            mock.Mock<IJetAuthenticatedRequest>()
                .Verify(r => r.ProcessRequest<JetOrderResponse>("GetOrders", variableRequestSubmitter.Object, store, true));
        }

        [Fact]
        public void GetOrders_ReturnsResultsFromJetRequest()
        {
            JetStoreEntity store = new JetStoreEntity();
            GenericResult<JetOrderResponse> expectedResult = new GenericResult<JetOrderResponse>();
            mock.Mock<IJetAuthenticatedRequest>()
                .Setup(r => r.ProcessRequest<JetOrderResponse>("GetOrders", requestSubmitter.Object, store, true))
                .Returns(expectedResult);

            GenericResult<JetOrderResponse> actualResult = mock.Create<JetWebClient>().GetOrders(store);
            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void GetProduct_DelegatesToJetRequest()
        {
            JetStoreEntity store = new JetStoreEntity();
            JetOrderItem jetOrderItem = new JetOrderItem
            {
                MerchantSku = "123"
            };
            mock.Create<JetWebClient>().GetProduct(jetOrderItem, store);

            mock.Mock<IJetAuthenticatedRequest>()
                .Verify(r => r.ProcessRequest<JetProduct>("GetProduct", variableRequestSubmitter.Object, store, true));
        }

        [Fact]
        public void GetProduct_ReturnsResultsFromJetRequest()
        {
            JetStoreEntity store = new JetStoreEntity();
            JetOrderItem jetOrderItem = new JetOrderItem
            {
                MerchantSku = "123"
            };
            GenericResult<JetProduct> expectedResult = new GenericResult<JetProduct>();
            mock.Mock<IJetAuthenticatedRequest>()
                .Setup(r => r.ProcessRequest<JetProduct>("GetProduct", requestSubmitter.Object, store, false))
                .Returns(expectedResult);

            var actualResult = mock.Create<JetWebClient>().GetProduct(jetOrderItem, store);
            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void Acknowledge_UsesCorrectEndpoint()
        {
            JetOrderEntity order = new JetOrderEntity() { MerchantOrderId = "1" };
            JetStoreEntity store = new JetStoreEntity();

            


            
            mock.Create<JetWebClient>().Acknowledge(order, store);
            
            requestSubmitter.VerifySet(r => r.Uri = new Uri("https://merchant-api.jet.com/api/orders/1/acknowledge"));
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}