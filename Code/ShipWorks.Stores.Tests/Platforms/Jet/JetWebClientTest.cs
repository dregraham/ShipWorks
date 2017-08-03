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

        public JetWebClientTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
        }

        [Fact]
        public void GetOrderDetails_DelegatesToJetRequest()
        {
            var store = new JetStoreEntity();
            var testObject = mock.Create<JetWebClient>();

            testObject.GetOrderDetails("url", store);

            mock.Mock<IJetRequest>()
                .Verify(r => r.ProcessRequest<JetOrderDetailsResult>("GetOrderDetails", "url", HttpVerb.Get, store),
                    Times.Once);
        }

        [Fact]
        public void GetOrderDetails_ReturnsResultFromJetRequest()
        {
            var store = new JetStoreEntity();
            var testObject = mock.Create<JetWebClient>();

            var expectedResult = new GenericResult<JetOrderDetailsResult>();
            mock.Mock<IJetRequest>()
                .Setup(r => r.ProcessRequest<JetOrderDetailsResult>("GetOrderDetails", "url", HttpVerb.Get, store))
                .Returns(expectedResult);

            var actualResult = testObject.GetOrderDetails("url", store);
            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void GetOrders_DelegatesToJetRequest()
        {
            var store = new JetStoreEntity();
            var testObject = mock.Create<JetWebClient>();
            testObject.GetOrders(store);

            mock.Mock<IJetRequest>()
                .Verify(r => r.ProcessRequest<JetOrderResponse>("GetOrders", "/orders/ready", HttpVerb.Get, store),
                    Times.Once);
        }

        [Fact]
        public void GetOrders_ReturnsResultsFromJetRequest()
        {
            var store = new JetStoreEntity();
            var testObject = mock.Create<JetWebClient>();

            var expectedResult = new GenericResult<JetOrderResponse>();
            mock.Mock<IJetRequest>()
                .Setup(r => r.ProcessRequest<JetOrderResponse>("GetOrders", "/orders/ready", HttpVerb.Get, store))
                .Returns(expectedResult);

            var actualResult = testObject.GetOrders(store);
            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void GetProduct_DelegatesToJetRequest()
        {
            var store = new JetStoreEntity();

            var jetOrderItem = new JetOrderItem
            {
                MerchantSku = "123"
            };

            var testObject = mock.Create<JetWebClient>();
            testObject.GetProduct(jetOrderItem, store);

            mock.Mock<IJetRequest>()
                .Verify(r => r.ProcessRequest<JetProduct>("GetProduct", "/merchant-skus/123", HttpVerb.Get, store),
                    Times.Once);
        }

        [Fact]
        public void GetProduct_ReturnsResultsFromJetRequest()
        {
            var store = new JetStoreEntity();
            var jetOrderItem = new JetOrderItem
            {
                MerchantSku = "123"
            };

            var testObject = mock.Create<JetWebClient>();

            var expectedResult = new GenericResult<JetProduct>();
            mock.Mock<IJetRequest>()
                .Setup(r => r.ProcessRequest<JetProduct>("GetProduct", "/merchant-skus/123", HttpVerb.Get, store))
                .Returns(expectedResult);

            var actualResult = testObject.GetProduct(jetOrderItem, store);
            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void GetToken_Fails_WhenCredentialsAreIncorrect()
        {
            mock.Mock<IJetRequest>().Setup(r => r.GetToken(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(GenericResult.FromError<string>(new Exception("Whammy!")));

            var testObject = mock.Create<JetWebClient>();
            var result = testObject.GetToken("simulatedIncorrectUsername", "wrong password");

            Assert.False(result.Success);
        }

        [Fact]
        public void GetToken_IsSuccessful_WhenTokenReturnedSuccessfully()
        {
            mock.Mock<IJetRequest>().Setup(r => r.GetToken(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(GenericResult.FromSuccess("your username"));

            var testObject = mock.Create<JetWebClient>();
            var result = testObject.GetToken("valid username", "correct password");

            Assert.True(result.Success);
        }

        [Fact]
        public void Acknowledge_UsesCorrectEndpoint()
        {
            var request = mock.Mock<IJetRequest>();
            var order = new JetOrderEntity() {MerchantOrderId = "1"};
            var store = new JetStoreEntity();
            var testObject = mock.Create<JetWebClient>();

            testObject.Acknowledge(order, store);

            request.Verify(r => r.Acknowledge(order, store, "/orders/1/acknowledge"));
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}