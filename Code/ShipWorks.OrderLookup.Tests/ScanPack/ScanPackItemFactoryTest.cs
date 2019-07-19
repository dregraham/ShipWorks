using System.Linq;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using Moq;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.OrderLookup.ScanPack;
using ShipWorks.Products;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.OrderLookup.Tests.ScanPack
{
    public class ScanPackItemFactoryTest
    {
        private readonly AutoMock mock;
        private readonly ScanPackItemFactory testObject;
        private readonly Mock<IProductCatalog> productCatalog;
        private readonly Mock<ISqlAdapterFactory> sqlAdapterFactory;

        public ScanPackItemFactoryTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            productCatalog = mock.Mock<IProductCatalog>();
            sqlAdapterFactory = mock.Mock<ISqlAdapterFactory>();

            testObject = mock.Create<ScanPackItemFactory>();
        }

        [Fact]
        public async Task Create_ReturnsScanPackItemsForOrder()
        {
            var order = new OrderEntity();
            var item = new OrderItemEntity() { Name = "foo", Image = "bar", Quantity = 3.3 };

            order.OrderItems.Add(item);

            var result = await testObject.Create(order);

            Assert.Equal(result.First().Quantity, item.Quantity);
            Assert.Equal(result.First().Name, item.Name);
            Assert.Equal(result.First().ImageUrl, item.Image);
        }

        [Fact]
        public async Task Create_ReturnsScanPackItemsForOrderWithProductInfo()
        {
            var order = new OrderEntity();
            var item = new OrderItemEntity() { Name = "foo", Image = "bar", Quantity = 3.3, SKU = "TheSku" };
            var product = new ProductVariantEntity() { Name = "newFoo", ImageUrl = "newBar"};
            order.OrderItems.Add(item);

            productCatalog.Setup(c => c.FetchProductVariantEntity(It.IsAny<ISqlAdapter>(), item.SKU))
                .Returns(product);

            var result = await testObject.Create(order);

            Assert.Equal(result.First().Quantity, item.Quantity);
            Assert.Equal(result.First().Name, product.Name);
            Assert.Equal(result.First().ImageUrl, product.ImageUrl);
        }

        [Fact]
        public async Task Create_ReturnsScanPackItemsForOrderWithThumbnail_WhenProductImageAndItemImageAreBlank()
        {
            var order = new OrderEntity();
            var item = new OrderItemEntity() { Name = "foo", Image = "", Quantity = 3.3, SKU = "TheSku", Thumbnail = "thumbnail" };
            var product = new ProductVariantEntity() { Name = "newFoo", ImageUrl = "" };
            order.OrderItems.Add(item);

            productCatalog.Setup(c => c.FetchProductVariantEntity(It.IsAny<ISqlAdapter>(), item.SKU))
                .Returns(product);

            var result = await testObject.Create(order);

            Assert.Equal(result.First().Quantity, item.Quantity);
            Assert.Equal(result.First().Name, product.Name);
            Assert.Equal(result.First().ImageUrl, item.Thumbnail);
        }

        [Fact]
        public async Task Create_ReturnsScanPackItemsForOrder_WhenthereAreMultipleItems()
        {
            var order = new OrderEntity();
            var itemOne = new OrderItemEntity() { Name = "foo", Image = "1", Quantity = 3.3, SKU = "TheSku", Thumbnail = "thumbnail" };
            var itemTwo = new OrderItemEntity() { Name = "foo", Image = "2", Quantity = 3.3, SKU = "TheSku", Thumbnail = "thumbnail" };
            var itemThree = new OrderItemEntity() { Name = "foo", Image = "3", Quantity = 3.3, SKU = "TheSku", Thumbnail = "thumbnail" };
            var itemFour = new OrderItemEntity() { Name = "foo", Image = "4", Quantity = 3.3, SKU = "TheSku", Thumbnail = "thumbnail" };
            var itemFive = new OrderItemEntity() { Name = "foo", Image = "5", Quantity = 3.3, SKU = "TheSku", Thumbnail = "thumbnail" };

            order.OrderItems.AddRange(new[] { itemOne, itemTwo, itemThree, itemThree, itemFour, itemFive });

            var result = await testObject.Create(order);

            Assert.Equal(result.Count, order.OrderItems.Count);
        }
    }
}
