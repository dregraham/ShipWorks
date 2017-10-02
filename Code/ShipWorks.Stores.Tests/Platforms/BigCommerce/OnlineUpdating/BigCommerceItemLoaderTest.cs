using System;
using System.Linq;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.BigCommerce;
using ShipWorks.Stores.Platforms.BigCommerce.DTO;
using ShipWorks.Stores.Platforms.BigCommerce.OnlineUpdating;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.BigCommerce
{
    public class ItemLoaderTest : IDisposable
    {
        private readonly AutoMock mock;
        private readonly BigCommerceItemLoader testObject;
        private readonly Mock<IBigCommerceWebClient> webClient;

        public ItemLoaderTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            webClient = mock.Mock<IBigCommerceWebClient>();
            testObject = mock.Create<BigCommerceItemLoader>();
        }

        [Fact]
        public async Task LoadItems_ReturnsFailure_WhenItemsDontHaveOrderAddressIDAndHasDashInOrderNumberComplete()
        {
            var items = new[] { new BigCommerceOrderItemEntity() };

            var result = await testObject.LoadItems(items, "123-X", 123L, webClient.Object);

            Assert.True(result.Failure);
            Assert.Contains("downloaded prior", result.Message);
        }

        [Fact]
        public async Task LoadItems_DelegatesToWebClient_WhenItemsDontHaveOrderAddressIDAndDoesNotHaveDashInOrderNumberComplete()
        {
            var items = new[] { new BigCommerceOrderItemEntity() };

            await testObject.LoadItems(items, "123", 123L, webClient.Object);

            webClient.Verify(x => x.GetOrderProducts(123L));
        }

        [Fact]
        public async Task LoadItems_ReturnsProductsAsItems_WhenItemsDontHaveOrderAddressIDAndDoesNotHaveDashInOrderNumberComplete()
        {
            var items = new[] { new BigCommerceOrderItemEntity() };
            var products = new[]
            {
                new BigCommerceProduct { id = 6, quantity = 3, order_address_id = 99 },
                new BigCommerceProduct { id = 7, quantity = 4, order_address_id = 99 }
            };

            webClient.Setup(x => x.GetOrderProducts(It.IsAny<long>()))
                .ReturnsAsync(products);

            var result = await testObject.LoadItems(items, "123", 123L, webClient.Object);

            Assert.True(result.Success);
            Assert.Contains(result.Value.Items, x => x.order_product_id == 6 && x.quantity == 3);
            Assert.Contains(result.Value.Items, x => x.order_product_id == 7 && x.quantity == 4);
            Assert.Equal(99, result.Value.OrderAddressID);
        }

        [Fact]
        public async Task LoadItems_ReturnsFailure_WhenProductsWereNotRetured()
        {
            var items = new[] { new BigCommerceOrderItemEntity() };

            webClient.Setup(x => x.GetOrderProducts(It.IsAny<long>()))
                .ReturnsAsync((BigCommerceProduct[]) null);

            var result = await testObject.LoadItems(items, "123", 123L, webClient.Object);

            Assert.True(result.Failure);
            Assert.Contains("shipping order address", result.Message);
        }

        [Fact]
        public async Task LoadItems_ReturnsOrderItemsAsItems_WhenOrderItemsHaveAddressID()
        {
            var items = new[]
            {
                new BigCommerceOrderItemEntity { OrderAddressID = 99, OrderProductID = 6, Quantity = 3 },
                new BigCommerceOrderItemEntity { OrderAddressID = 99, OrderProductID = 7, Quantity = 4 }
            };

            var result = await testObject.LoadItems(items, "123", 123L, webClient.Object);

            Assert.True(result.Success);
            Assert.Contains(result.Value.Items, x => x.order_product_id == 6 && x.quantity == 3);
            Assert.Contains(result.Value.Items, x => x.order_product_id == 7 && x.quantity == 4);
            Assert.Equal(99, result.Value.OrderAddressID);
        }

        [Fact]
        public async Task LoadItems_DoesNotReturnNonBigCommerceOrders_WhenOrderItemsHaveAddressID()
        {
            var items = new[]
            {
                new OrderItemEntity(),
                new BigCommerceOrderItemEntity { OrderAddressID = 99, OrderProductID = 6, Quantity = 3 },
                new BigCommerceOrderItemEntity { OrderAddressID = 99, OrderProductID = 7, Quantity = 4 }
            };

            var result = await testObject.LoadItems(items, "123", 123L, webClient.Object);

            Assert.True(result.Success);
            Assert.Equal(2, result.Value.Items.Count());
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
