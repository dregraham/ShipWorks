using System;
using Autofac.Extras.Moq;
using Moq;
using ShipWorks.ApplicationCore.ExecutionMode;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Shopify;
using ShipWorks.Stores.Platforms.Shopify.DTOs;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.Shopify
{
    public class ShopifyLocationServiceTest : IDisposable
    {
        readonly AutoMock mock;
        private readonly ShopifyLocationService testObject;
        private readonly Mock<IShopifyWebClient> webClient;

        public ShopifyLocationServiceTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            testObject = mock.Create<ShopifyLocationService>();
            webClient = mock.CreateMock<IShopifyWebClient>();
        }

        [Fact]
        public void InitializeForCurrentDatabase_ClearsCache()
        {
            testObject.GetPrimaryLocationID(new ShopifyStoreEntity { StoreID = 1 }, webClient.Object);
            webClient.ResetCalls();

            testObject.InitializeForCurrentDatabase(mock.CreateMock<ExecutionMode>().Object);

            testObject.GetPrimaryLocationID(new ShopifyStoreEntity { StoreID = 1 }, webClient.Object);
            webClient.Verify(x => x.GetShop());
        }

        [Fact]
        public void GetPrimaryLocationID_CallsWebClient_OnFirstCall()
        {
            testObject.GetPrimaryLocationID(new ShopifyStoreEntity { StoreID = 1 }, webClient.Object);

            webClient.Verify(x => x.GetShop());
        }

        [Fact]
        public void GetPrimaryLocationID_DoesNotCallWebClient_OnSecondCall()
        {
            testObject.GetPrimaryLocationID(new ShopifyStoreEntity { StoreID = 1 }, webClient.Object);
            webClient.ResetCalls();

            testObject.GetPrimaryLocationID(new ShopifyStoreEntity { StoreID = 1 }, webClient.Object);

            webClient.Verify(x => x.GetShop(), Times.Never);
        }

        [Fact]
        public void GetPrimaryLocationID_CallsWebClientTwice_WhenDifferentStoresAreRequested()
        {
            var webClient2 = mock.Mock<IShopifyWebClient>();

            testObject.GetPrimaryLocationID(new ShopifyStoreEntity { StoreID = 1 }, webClient.Object);
            webClient.Verify(x => x.GetShop());

            testObject.GetPrimaryLocationID(new ShopifyStoreEntity { StoreID = 2 }, webClient2.Object);
            webClient2.Verify(x => x.GetShop());
        }

        [Fact]
        public void GetPrimaryLocationID_ReturnsValueFromWebClient_WhenLocationIsNotCached()
        {
            webClient.Setup(x => x.GetShop()).Returns(new ShopifyShop { PrimaryLocationID = 123 });

            var location = testObject.GetPrimaryLocationID(new ShopifyStoreEntity { StoreID = 1 }, webClient.Object);

            Assert.Equal(123, location);
        }

        [Fact]
        public void GetPrimaryLocationID_ReturnsCachedValue_WhenLocationIsCached()
        {
            webClient.Setup(x => x.GetShop()).Returns(new ShopifyShop { PrimaryLocationID = 123 });

            testObject.GetPrimaryLocationID(new ShopifyStoreEntity { StoreID = 1 }, webClient.Object);
            var location = testObject.GetPrimaryLocationID(new ShopifyStoreEntity { StoreID = 1 }, webClient.Object);

            Assert.Equal(123, location);
        }

        [Fact]
        public void GetPrimaryLocationID_ReturnsCorrectValue_WhenMultipleStoresAreCached()
        {
            var webClient2 = mock.CreateMock<IShopifyWebClient>();

            webClient.Setup(x => x.GetShop()).Returns(new ShopifyShop { PrimaryLocationID = 123 });
            webClient2.Setup(x => x.GetShop()).Returns(new ShopifyShop { PrimaryLocationID = 456 });

            testObject.GetPrimaryLocationID(new ShopifyStoreEntity { StoreID = 1 }, webClient.Object);
            var location1 = testObject.GetPrimaryLocationID(new ShopifyStoreEntity { StoreID = 1 }, webClient.Object);

            testObject.GetPrimaryLocationID(new ShopifyStoreEntity { StoreID = 2 }, webClient2.Object);
            var location2 = testObject.GetPrimaryLocationID(new ShopifyStoreEntity { StoreID = 2 }, webClient2.Object);

            Assert.Equal(123, location1);
            Assert.Equal(456, location2);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
