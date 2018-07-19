using System;
using System.Collections.Generic;
using System.Linq;
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

            webClient.Setup(x => x.GetOrder(6))
                .Returns(new ShopifyOrder
                {
                    LineItems = new[]
                    {
                        new ShopifyLineItem { ID = 11, VariantID = 111 },
                        new ShopifyLineItem { ID = 22, VariantID = 222 },
                        new ShopifyLineItem { ID = 33, VariantID = 333 },
                        new ShopifyLineItem { ID = 44, VariantID = 444 },
                    }
                });

            webClient.Setup(x => x.GetShop()).Returns(new ShopifyShopResponse { Shop = new ShopifyShop { PrimaryLocationID = 0 } });
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
            webClient2.Setup(x => x.GetShop()).Returns(new ShopifyShopResponse { Shop = new ShopifyShop { PrimaryLocationID = 123 } });

            testObject.GetPrimaryLocationID(new ShopifyStoreEntity { StoreID = 1 }, webClient.Object);
            webClient.Verify(x => x.GetShop());

            testObject.GetPrimaryLocationID(new ShopifyStoreEntity { StoreID = 2 }, webClient2.Object);
            webClient2.Verify(x => x.GetShop());
        }

        [Fact]
        public void GetPrimaryLocationID_ReturnsValueFromWebClient_WhenLocationIsNotCached()
        {
            webClient.Setup(x => x.GetShop()).Returns(new ShopifyShopResponse { Shop = new ShopifyShop { PrimaryLocationID = 123 } });

            var location = testObject.GetPrimaryLocationID(new ShopifyStoreEntity { StoreID = 1 }, webClient.Object);

            Assert.Equal(123, location);
        }

        [Fact]
        public void GetPrimaryLocationID_ReturnsCachedValue_WhenLocationIsCached()
        {
            webClient.Setup(x => x.GetShop()).Returns(new ShopifyShopResponse { Shop = new ShopifyShop { PrimaryLocationID = 123 } });

            testObject.GetPrimaryLocationID(new ShopifyStoreEntity { StoreID = 1 }, webClient.Object);
            var location = testObject.GetPrimaryLocationID(new ShopifyStoreEntity { StoreID = 1 }, webClient.Object);

            Assert.Equal(123, location);
        }

        [Fact]
        public void GetPrimaryLocationID_Foo_OnFirstCall()
        {
            webClient.Setup(x => x.GetShop()).Returns(new ShopifyShopResponse());

            Assert.Throws<ShopifyException>(() => testObject.GetPrimaryLocationID(new ShopifyStoreEntity { StoreID = 1 }, webClient.Object));
        }

        [Fact]
        public void GetPrimaryLocationID_ReturnsCorrectValue_WhenMultipleStoresAreCached()
        {
            var webClient2 = mock.CreateMock<IShopifyWebClient>();

            webClient.Setup(x => x.GetShop()).Returns(new ShopifyShopResponse { Shop = new ShopifyShop { PrimaryLocationID = 123 } });
            webClient2.Setup(x => x.GetShop()).Returns(new ShopifyShopResponse { Shop = new ShopifyShop { PrimaryLocationID = 456 } });

            testObject.GetPrimaryLocationID(new ShopifyStoreEntity { StoreID = 1 }, webClient.Object);
            var location1 = testObject.GetPrimaryLocationID(new ShopifyStoreEntity { StoreID = 1 }, webClient.Object);

            testObject.GetPrimaryLocationID(new ShopifyStoreEntity { StoreID = 2 }, webClient2.Object);
            var location2 = testObject.GetPrimaryLocationID(new ShopifyStoreEntity { StoreID = 2 }, webClient2.Object);

            Assert.Equal(123, location1);
            Assert.Equal(456, location2);
        }

        [Fact]
        public void GetItemLocations_RequestsInventoryLevelsForAllItems_WhenItemsAreNotCached()
        {
            var item1 = new ShopifyOrderItemEntity { InventoryItemID = 1 };
            var item2 = new ShopifyOrderItemEntity { InventoryItemID = 2 };

            testObject.GetItemLocations(webClient.Object, 6, new[] { item1, item2 });

            webClient.Verify(x => x.GetInventoryLevelsForItems(new[] { 1L, 2L }));
        }

        [Fact]
        public void GetItemLocations_ReturnsEmpty_WhenNoInventoryLevelsAreFound()
        {
            var item1 = new ShopifyOrderItemEntity { InventoryItemID = 1 };
            var item2 = new ShopifyOrderItemEntity { InventoryItemID = 2 };

            var results = testObject.GetItemLocations(webClient.Object, 6, new[] { item1, item2 });

            Assert.Empty(results);
        }

        [Fact]
        public void GetItemLocations_ReturnsEntryPerItem_WhenEachItemHasDifferentLocation()
        {
            webClient.Setup(x => x.GetInventoryLevelsForItems(It.IsAny<IEnumerable<long>>()))
                .Returns(new ShopifyInventoryLevelsResponse
                {
                    InventoryLevels = new[] {
                    new ShopifyInventoryLevel { InventoryItemID = 1, LocationID = 11 },
                    new ShopifyInventoryLevel { InventoryItemID = 2, LocationID = 22 },
                    new ShopifyInventoryLevel { InventoryItemID = 3, LocationID = 33 }
                }
                });

            var item1 = new ShopifyOrderItemEntity { InventoryItemID = 1 };
            var item2 = new ShopifyOrderItemEntity { InventoryItemID = 2 };
            var item3 = new ShopifyOrderItemEntity { InventoryItemID = 3 };

            var result = testObject.GetItemLocations(webClient.Object, 6, new[] { item1, item2, item3 });

            Assert.Equal(item1, result.Single(x => x.locationID == 11).items.Single());
            Assert.Equal(item2, result.Single(x => x.locationID == 22).items.Single());
            Assert.Equal(item3, result.Single(x => x.locationID == 33).items.Single());
        }

        [Fact]
        public void GetItemLocations_ReturnsFirstLocation_WhenItemIsInTwoLocations()
        {
            webClient.Setup(x => x.GetInventoryLevelsForItems(It.IsAny<IEnumerable<long>>()))
                .Returns(new ShopifyInventoryLevelsResponse
                {
                    InventoryLevels = new[] {
                        new ShopifyInventoryLevel { InventoryItemID = 1, LocationID = 11 },
                        new ShopifyInventoryLevel { InventoryItemID = 1, LocationID = 22 },
                    }
                });

            var item1 = new ShopifyOrderItemEntity { InventoryItemID = 1 };

            var result = testObject.GetItemLocations(webClient.Object, 6, new[] { item1 });

            Assert.Equal(item1, result.Single(x => x.locationID == 11).items.Single());
        }

        [Fact]
        public void GetItemLocations_ReturnsLocationWithBothItems_WhenLocationsOverlap()
        {
            webClient.Setup(x => x.GetInventoryLevelsForItems(It.IsAny<IEnumerable<long>>()))
                .Returns(new ShopifyInventoryLevelsResponse
                {
                    InventoryLevels =
                    new[] {
                        new ShopifyInventoryLevel { InventoryItemID = 1, LocationID = 11 },
                        new ShopifyInventoryLevel { InventoryItemID = 1, LocationID = 22 },
                        new ShopifyInventoryLevel { InventoryItemID = 2, LocationID = 33 },
                        new ShopifyInventoryLevel { InventoryItemID = 2, LocationID = 11 }
                    }
                });

            var item1 = new ShopifyOrderItemEntity { InventoryItemID = 1 };
            var item2 = new ShopifyOrderItemEntity { InventoryItemID = 2 };

            var result = testObject.GetItemLocations(webClient.Object, 6, new[] { item1, item2 });

            Assert.Equal(1, result.Count());
            Assert.Contains(item1, result.Single(x => x.locationID == 11).items);
            Assert.Contains(item2, result.Single(x => x.locationID == 11).items);
        }

        [Fact]
        public void GetItemLocations_ReturnsFewestLocationsPossible_WhenLocationsOverlap()
        {
            webClient.Setup(x => x.GetInventoryLevelsForItems(It.IsAny<IEnumerable<long>>()))
                .Returns(new ShopifyInventoryLevelsResponse
                {
                    InventoryLevels = new[] {
                    new ShopifyInventoryLevel { InventoryItemID = 1, LocationID = 11 },
                    new ShopifyInventoryLevel { InventoryItemID = 1, LocationID = 22 },
                    new ShopifyInventoryLevel { InventoryItemID = 2, LocationID = 33 },
                    new ShopifyInventoryLevel { InventoryItemID = 2, LocationID = 22 }
                }
                });

            var item1 = new ShopifyOrderItemEntity { InventoryItemID = 1 };
            var item2 = new ShopifyOrderItemEntity { InventoryItemID = 2 };

            var result = testObject.GetItemLocations(webClient.Object, 6, new[] { item1, item2 });

            Assert.Equal(1, result.Count());
            Assert.Contains(item1, result.Single(x => x.locationID == 22).items);
            Assert.Contains(item2, result.Single(x => x.locationID == 22).items);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
