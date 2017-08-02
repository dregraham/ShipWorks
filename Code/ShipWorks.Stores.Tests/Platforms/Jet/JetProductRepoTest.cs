using Autofac.Extras.Moq;
using Interapptive.Shared.Utility;
using Moq;
using ShipWorks.Stores.Platforms.Jet;
using ShipWorks.Stores.Platforms.Jet.DTO;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.Jet
{
    public class JetProductRepositoryTest
    {
        private readonly AutoMock mock;

        public JetProductRepositoryTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
        }

        [Fact]
        public void GetProduct_DelegatesToWebClientForProduct_WhenNotCached()
        {
            JetProduct jetProduct = new JetProduct();
            JetOrderItem jetItem = new JetOrderItem() { MerchantSku = "testSku" };

            var webClient = mock.Mock<IJetWebClient>();
            webClient.Setup(w => w.GetProduct(jetItem)).Returns(GenericResult.FromSuccess(jetProduct));

            JetProductRepository testObject = mock.Create<JetProductRepository>();
            
            JetProduct result = testObject.GetProduct(jetItem);

            Assert.Equal(jetProduct, result);
            mock.Mock<IJetWebClient>().Verify(w => w.GetProduct(jetItem), Times.Once);
        }

        [Fact]
        public void GetProduct_ReturnsProductFromCache_WhenProductIsInCache()
        {
            JetProductRepository testObject = mock.Create<JetProductRepository>();
            JetProduct jetProduct = new JetProduct();
            JetOrderItem jetItem = new JetOrderItem() { MerchantSku = "testSku" };

            testObject.AddProduct("testSku", jetProduct);


            JetProduct result = testObject.GetProduct(jetItem);

            Assert.Equal(jetProduct, result);
            mock.Mock<IJetWebClient>().Verify(w => w.GetProduct(jetItem), Times.Never);
        }

        [Fact]
        public void GetProduct_ThrowsJetException_WhenWebClientFails()
        {
            JetOrderItem jetItem = new JetOrderItem() { MerchantSku = "testSku" };

            var webClient = mock.Mock<IJetWebClient>();
            webClient.Setup(w => w.GetProduct(jetItem)).Returns(GenericResult.FromError<JetProduct>("Something went wrong oh no!!"));

            JetProductRepository testObject = mock.Create<JetProductRepository>();

            JetException ex = Assert.Throws<JetException>(() => testObject.GetProduct(jetItem));
            Assert.Equal("Error retrieving product information for testSku, Something went wrong oh no!!.", ex.Message);
        }
    }
}