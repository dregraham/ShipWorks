using System;
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
    public class ScanPackItemFactoryTest : IDisposable
    {
        private readonly AutoMock mock;
        private readonly ScanPackItemFactory testObject;
        private readonly Mock<IProductCatalog> productCatalog;
        
        public ScanPackItemFactoryTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            productCatalog = mock.Mock<IProductCatalog>();
            
            testObject = mock.Create<ScanPackItemFactory>();
        }

        [Fact]
        public async Task Create_ReturnsScanPackItemsForOrder()
        {
            var order = new OrderEntity();
            var item = new OrderItemEntity { Name = "foo", Image = "bar", Quantity = 3.3 };

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
            var item = new OrderItemEntity { Name = "foo", Image = "bar", Quantity = 3.3, SKU = "TheSku" };
            var product = new ProductVariantEntity { Name = "newFoo", ImageUrl = "newBar", Product = new ProductEntity()};
            product.Aliases.Add(new ProductVariantAliasEntity { Sku = item.SKU });

            order.OrderItems.Add(item);

            productCatalog.Setup(c => c.FetchProductVariantEntities(It.IsAny<ISqlAdapter>(), new[] { item.SKU }))
                .ReturnsAsync(new[] { product });

            var result = await testObject.Create(order);

            Assert.Equal(result.First().Quantity, item.Quantity);
            Assert.Equal(result.First().Name, product.Name);
            Assert.Equal(result.First().ImageUrl, product.ImageUrl);
        }

        [Fact]
        public async Task Create_ReturnsScanPackItemsForOrderWithThumbnail_WhenProductImageAndItemImageAreBlank()
        {
            var order = new OrderEntity();
            var item = new OrderItemEntity { Name = "foo", Image = "", Quantity = 3.3, SKU = "TheSku", Thumbnail = "thumbnail" };
            var product = new ProductVariantEntity { Name = "newFoo", ImageUrl = "", Product = new ProductEntity()};
            product.Aliases.Add(new ProductVariantAliasEntity { Sku = item.SKU });
            order.OrderItems.Add(item);

            productCatalog.Setup(c => c.FetchProductVariantEntities(It.IsAny<ISqlAdapter>(), new[] { item.SKU }))
                .ReturnsAsync(new[] { product });

            var result = await testObject.Create(order);

            Assert.Equal(result.First().Quantity, item.Quantity);
            Assert.Equal(result.First().Name, product.Name);
            Assert.Equal(result.First().ImageUrl, item.Thumbnail);
        }

        [Fact]
        public async Task Create_ReturnsScanPackItemsForOrder_WhenThereAreMultipleItems()
        {
            var order = new OrderEntity();
            var itemOne = new OrderItemEntity { Name = "foo", Image = "1", Quantity = 3.3, SKU = "TheSku", Thumbnail = "thumbnail" };
            var itemTwo = new OrderItemEntity { Name = "foo", Image = "2", Quantity = 3.3, SKU = "TheSku", Thumbnail = "thumbnail" };
            var itemThree = new OrderItemEntity { Name = "foo", Image = "3", Quantity = 3.3, SKU = "TheSku", Thumbnail = "thumbnail" };
            var itemFour = new OrderItemEntity { Name = "foo", Image = "4", Quantity = 3.3, SKU = "TheSku", Thumbnail = "thumbnail" };
            var itemFive = new OrderItemEntity { Name = "foo", Image = "5", Quantity = 3.3, SKU = "TheSku", Thumbnail = "thumbnail" };

            order.OrderItems.AddRange(new[] { itemOne, itemTwo, itemThree, itemThree, itemFour, itemFive });

            var result = await testObject.Create(order);

            Assert.Equal(result.Count, order.OrderItems.Count);
        }

        [Fact]
        public async Task Create_SplitsBundlesIntoMultipleItems()
        {
            var order = new OrderEntity();
            var item = new OrderItemEntity {Name = "foo", Image = "bar", Quantity = 3.3, SKU = "TheSku"};
            var product = new ProductVariantEntity
            {
                Name = "newFoo", ImageUrl = "newBar",
                Product = new ProductEntity
                {
                    IsBundle = true
                }
            };
            product.Aliases.Add(new ProductVariantAliasEntity {Sku = item.SKU});


            order.OrderItems.Add(item);

            productCatalog.Setup(c => c.FetchProductVariantEntities(It.IsAny<ISqlAdapter>(), new[] {item.SKU}))
                .ReturnsAsync(new[] {product});

            var result = await testObject.Create(order);

            Assert.Equal(4, result.Count);
            // since qty is 3.3, there should be 3 lines with qty of 1 and one line with qty of .3
            Assert.Equal(3, result.Count(r => Math.Abs(r.Quantity - 1) < .0001));
            Assert.Equal(1, result.Count(r => Math.Abs(r.Quantity - .3) < .0001));
        }
        
        [Fact]
        public async Task Create_PullsInContentsOfBundles_FromProductCatalog()
        {
            var order = new OrderEntity();
            var item = new OrderItemEntity {Name = "foo", Image = "bar", Quantity = 1, SKU = "TheSku"};
            var product = new ProductVariantEntity
            {
                Name = "newFoo", ImageUrl = "newBar",
                Product = new ProductEntity
                {
                    IsBundle = true
                }
            };
            product.Aliases.Add(new ProductVariantAliasEntity {Sku = item.SKU});
            product.Product.Bundles.Add(new ProductBundleEntity
            {
                Quantity = 2, ChildVariant = new ProductVariantEntity
                {
                    Name = "bundleFoo",
                    UPC = "bundleUPC",
                    Product = new ProductEntity{IsBundle = false}
                }
            });


            order.OrderItems.Add(item);

            productCatalog.Setup(c => c.FetchProductVariantEntities(It.IsAny<ISqlAdapter>(), new[] {item.SKU}))
                .ReturnsAsync(new[] {product});

            var result = await testObject.Create(order);

            Assert.Equal(2, result.Count);
            var parent = result.First();
            var child = result.Last();
            
            Assert.Equal("newFoo", parent.Name);
            Assert.Equal("newBar", parent.ImageUrl);
            Assert.Equal(0, parent.SortIdentifier);
            Assert.Null(parent.ParentSortIdentifier);
            Assert.True(parent.IsBundle);
            Assert.True(parent.IsBundleComplete);
            
            Assert.Equal("bundleFoo", child.Name);
            Assert.Contains("bundleUPC", child.Barcodes);
            Assert.False(child.IsBundle);
            Assert.Equal(0, child.ParentSortIdentifier);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
