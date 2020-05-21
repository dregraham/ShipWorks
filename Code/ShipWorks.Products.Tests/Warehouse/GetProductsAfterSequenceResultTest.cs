using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extras.Moq;
using Moq;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Products.Warehouse;
using ShipWorks.Products.Warehouse.DTO;
using ShipWorks.Tests.Shared;
using Xunit;
using static ShipWorks.Tests.Shared.ExtensionMethods.ParameterShorteners;

namespace ShipWorks.Products.Tests.Warehouse
{
    public class GetProductsAfterSequenceResultTest
    {
        private readonly AutoMock mock;

        public GetProductsAfterSequenceResultTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
        }

        [Fact]
        public async Task Apply_DelegatesToProductCatalog_ToGetExistingProducts()
        {
            var sqlAdapter = mock.Build<ISqlAdapter>();
            var data = new GetProductsAfterSequenceResponseData
            {
                Products = new[]
                {
                    new WarehouseProduct { ProductId = "fd8d83c3141d40a6915fa1342e5def13" },
                    new WarehouseProduct { ProductId = "fd8d83c3141d40a6915fa1342e5def14" }
                }
            };
            var testObject = mock.Create<GetProductsAfterSequenceResult>(TypedParameter.From(data));

            await testObject.Apply(sqlAdapter, CancellationToken.None);

            mock.Mock<IProductCatalog>()
                .Verify(x => x.GetProductsByHubIds(sqlAdapter, new[] {
                    Guid.Parse("fd8d83c3141d40a6915fa1342e5def13"),
                    Guid.Parse("fd8d83c3141d40a6915fa1342e5def14")
                }));
        }

        [Fact]
        public async Task Apply_MatchesProductsToData_WithExistingMatchedProducts()
        {
            var dataContainer1 = mock.CreateMock<IHubProductUpdater>();
            var dataContainer2 = mock.CreateMock<IHubProductUpdater>();
            var warehouseProduct1 = new WarehouseProduct { ProductId = "00000000000000000000000000000001" };
            var warehouseProduct2 = new WarehouseProduct { ProductId = "00000000000000000000000000000002" };
            var product1 = new ProductVariantEntity { HubProductId = Guid.Parse("00000000000000000000000000000001") };
            var product2 = new ProductVariantEntity { HubProductId = Guid.Parse("00000000000000000000000000000002") };

            var updaterFunc = mock.MockFunc<WarehouseProduct, IHubProductUpdater>();
            updaterFunc.Setup(x => x(warehouseProduct1)).Returns(dataContainer1.Object);
            updaterFunc.Setup(x => x(warehouseProduct2)).Returns(dataContainer2.Object);

            mock.Mock<IProductCatalog>()
                .Setup(x => x.GetProductsByHubIds(It.IsAny<ISqlAdapter>(), It.IsAny<IEnumerable<Guid>>()))
                .ReturnsAsync(new[] { product1, product2 });

            var data = new GetProductsAfterSequenceResponseData
            {
                Products = new[] { warehouseProduct1, warehouseProduct2 }
            };

            var testObject = mock.Create<GetProductsAfterSequenceResult>(TypedParameter.From(data));

            await testObject.Apply(mock.Build<ISqlAdapter>(), CancellationToken.None);

            dataContainer1.VerifySet(x => x.ProductVariant = product1);
            dataContainer2.VerifySet(x => x.ProductVariant = product2);
        }

        [Fact]
        public async Task Apply_DelegatesToProductCatalog_ToGetUnlinkedProductsBySku()
        {
            var sqlAdapter = mock.Build<ISqlAdapter>();

            var warehouseProduct1 = new WarehouseProduct { ProductId = "00000000000000000000000000000001", Sku = "ABC123" };
            var warehouseProduct2 = new WarehouseProduct { ProductId = "00000000000000000000000000000002" };
            var product2 = new ProductVariantEntity { HubProductId = Guid.Parse("00000000000000000000000000000002") };

            mock.MockFunc<WarehouseProduct, IHubProductUpdater>()
                .Setup(x => x(warehouseProduct1))
                .Returns((WarehouseProduct x) => new TestUpdater { ProductData = x });

            mock.Mock<IProductCatalog>()
                .Setup(x => x.GetProductsByHubIds(It.IsAny<ISqlAdapter>(), It.IsAny<IEnumerable<Guid>>()))
                .ReturnsAsync(new[] { product2 });

            var data = new GetProductsAfterSequenceResponseData
            {
                Products = new[] { warehouseProduct1, warehouseProduct2 }
            };

            var testObject = mock.Create<GetProductsAfterSequenceResult>(TypedParameter.From(data));

            await testObject.Apply(sqlAdapter, CancellationToken.None);

            mock.Mock<IProductCatalog>()
                .Verify(x => x.FetchProductVariantEntities(sqlAdapter, new[] { "ABC123" }, true));
        }

        [Fact]
        public async Task Apply_MatchesProductsToData_WithExistingUnmatchedProducts()
        {
            var dataContainer1 = new TestUpdater();
            var dataContainer2 = new TestUpdater();
            var warehouseProduct1 = new WarehouseProduct { ProductId = "00000000000000000000000000000001", Sku = "ABC123" };
            var warehouseProduct2 = new WarehouseProduct { ProductId = "00000000000000000000000000000002", Sku = "DEF456" };
            var product1 = new ProductVariantEntity { Product = new ProductEntity() };
            product1.Aliases.Add(new ProductVariantAliasEntity { IsDefault = true, Sku = "ABC123" });
            var product2 = new ProductVariantEntity { Product = new ProductEntity() };
            product2.Aliases.Add(new ProductVariantAliasEntity { IsDefault = true, Sku = "DEF456" });

            var updaterFunc = mock.MockFunc<WarehouseProduct, IHubProductUpdater>();
            updaterFunc.Setup(x => x(warehouseProduct1))
                .Returns((WarehouseProduct x) =>
                {
                    dataContainer1.ProductData = x;
                    return dataContainer1;
                });
            updaterFunc.Setup(x => x(warehouseProduct2))
                .Returns((WarehouseProduct x) =>
                {
                    dataContainer2.ProductData = x;
                    return dataContainer2;
                });

            mock.Mock<IProductCatalog>()
                .Setup(x => x.FetchProductVariantEntities(It.IsAny<ISqlAdapter>(), It.IsAny<IEnumerable<string>>(), AnyBool))
                .ReturnsAsync(new[] { product1, product2 });

            var data = new GetProductsAfterSequenceResponseData
            {
                Products = new[] { warehouseProduct1, warehouseProduct2 }
            };

            var testObject = mock.Create<GetProductsAfterSequenceResult>(TypedParameter.From(data));

            await testObject.Apply(mock.Build<ISqlAdapter>(), CancellationToken.None);

            Assert.Equal(product1, dataContainer1.ProductVariant);
            Assert.Equal(product2, dataContainer2.ProductVariant);
        }

        [Fact]
        public async Task Apply_MatchesProductsToData_WithUnknownProducts()
        {
            var dataContainer1 = new TestUpdater();
            var dataContainer2 = new TestUpdater();
            var warehouseProduct1 = new WarehouseProduct { ProductId = "00000000000000000000000000000001", Sku = "ABC123" };
            var warehouseProduct2 = new WarehouseProduct { ProductId = "00000000000000000000000000000002", Sku = "DEF456" };

            var updaterFunc = mock.MockFunc<WarehouseProduct, IHubProductUpdater>();
            updaterFunc.Setup(x => x(warehouseProduct1))
                .Returns((WarehouseProduct x) =>
                {
                    dataContainer1.ProductData = x;
                    return dataContainer1;
                });
            updaterFunc.Setup(x => x(warehouseProduct2))
                .Returns((WarehouseProduct x) =>
                {
                    dataContainer2.ProductData = x;
                    return dataContainer2;
                });

            var data = new GetProductsAfterSequenceResponseData
            {
                Products = new[] { warehouseProduct1, warehouseProduct2 }
            };

            var testObject = mock.Create<GetProductsAfterSequenceResult>(TypedParameter.From(data));

            await testObject.Apply(mock.Build<ISqlAdapter>(), CancellationToken.None);

            Assert.Equal("ABC123", dataContainer1.ProductVariant.DefaultSku);
            Assert.Equal(Guid.Parse("00000000000000000000000000000001"), dataContainer1.ProductVariant.HubProductId);
            Assert.Equal("DEF456", dataContainer2.ProductVariant.DefaultSku);
            Assert.Equal(Guid.Parse("00000000000000000000000000000002"), dataContainer2.ProductVariant.HubProductId);
        }

        [Fact]
        public async Task Apply_CallsUpdate_OnAllUpdaters()
        {
            var dataContainer1 = mock.CreateMock<IHubProductUpdater>();
            var dataContainer2 = mock.CreateMock<IHubProductUpdater>();
            var warehouseProduct1 = new WarehouseProduct { ProductId = "00000000000000000000000000000001", Sku = "ABC123" };
            var warehouseProduct2 = new WarehouseProduct { ProductId = "00000000000000000000000000000002", Sku = "DEF456" };

            var updaterFunc = mock.MockFunc<WarehouseProduct, IHubProductUpdater>();
            updaterFunc.Setup(x => x(warehouseProduct1)).Returns(dataContainer1.Object);
            updaterFunc.Setup(x => x(warehouseProduct2)).Returns(dataContainer2.Object);

            var data = new GetProductsAfterSequenceResponseData
            {
                Products = new[] { warehouseProduct1, warehouseProduct2 }
            };

            var testObject = mock.Create<GetProductsAfterSequenceResult>(TypedParameter.From(data));

            await testObject.Apply(mock.Build<ISqlAdapter>(), CancellationToken.None);

            dataContainer1.Verify(x => x.UpdateProductVariant());
            dataContainer2.Verify(x => x.UpdateProductVariant());
        }

        [Fact]
        public async Task Apply_CallsSaveEntityCollection_BeforeDealingWithBundles()
        {
            var warehouseProduct1 = new WarehouseProduct { ProductId = "00000000000000000000000000000001", Sku = "ABC123" };
            var data = new GetProductsAfterSequenceResponseData { Products = new[] { warehouseProduct1 } };
            var cancellationToken = new CancellationToken();

            var testObject = mock.Create<GetProductsAfterSequenceResult>(TypedParameter.From(data));

            await testObject.Apply(mock.Mock<ISqlAdapter>().Object, cancellationToken);

            mock.Mock<ISqlAdapter>()
                .Verify(x => x.SaveEntityCollectionAsync(It.IsAny<EntityCollection<ProductVariantEntity>>(), true, true, cancellationToken));
            mock.Mock<ISqlAdapter>()
                .Verify(x => x.StartTransaction(IsolationLevel.ReadCommitted, AnyString));
            mock.Mock<ISqlAdapter>()
                .Verify(x => x.Commit());
        }

        [Fact]
        public async Task Apply_DelegatesToBundleManager()
        {
            var dataContainer1 = mock.CreateMock<IHubProductUpdater>();
            var dataContainer2 = mock.CreateMock<IHubProductUpdater>();
            var warehouseProduct1 = new WarehouseProduct { ProductId = "00000000000000000000000000000001", Sku = "ABC123" };
            var warehouseProduct2 = new WarehouseProduct { ProductId = "00000000000000000000000000000002", Sku = "DEF456" };

            var updaterFunc = mock.MockFunc<WarehouseProduct, IHubProductUpdater>();
            updaterFunc.Setup(x => x(warehouseProduct1)).Returns(dataContainer1.Object);
            updaterFunc.Setup(x => x(warehouseProduct2)).Returns(dataContainer2.Object);
            var sqlAdapter = mock.Build<ISqlAdapter>();
            var cancellationToken = new CancellationToken();

            var data = new GetProductsAfterSequenceResponseData
            {
                Products = new[] { warehouseProduct1, warehouseProduct2 }
            };

            var testObject = mock.Create<GetProductsAfterSequenceResult>(TypedParameter.From(data));

            await testObject.Apply(sqlAdapter, CancellationToken.None);

            mock.Mock<IWarehouseProductBundleService>()
                .Verify(x => x.UpdateProductBundleDetails(sqlAdapter, It.IsAny<ProductVariantEntity>(), It.IsAny<WarehouseProduct>(), cancellationToken));
        }

        [Fact]
        public async Task Apply_CallsSaveEntityCollection_AfterDealingWithBundles()
        {
            var warehouseProduct1 = new WarehouseProduct { ProductId = "00000000000000000000000000000001", Sku = "ABC123" };
            var data = new GetProductsAfterSequenceResponseData { Products = new[] { warehouseProduct1 } };
            var cancellationToken = new CancellationToken();

            var testObject = mock.Create<GetProductsAfterSequenceResult>(TypedParameter.From(data));

            await testObject.Apply(mock.Mock<ISqlAdapter>().Object, cancellationToken);

            mock.Mock<ISqlAdapter>()
                .Verify(x => x.SaveEntityCollectionAsync(It.IsAny<EntityCollection<ProductVariantEntity>>(), false, true, cancellationToken));
        }

        [Fact]
        public async Task Apply_CallsRollback_WhenSaveEntityCollection_Throws()
        {
            var warehouseProduct1 = new WarehouseProduct { ProductId = "00000000000000000000000000000001", Sku = "ABC123" };
            var data = new GetProductsAfterSequenceResponseData { Products = new[] { warehouseProduct1 } };
            var cancellationToken = new CancellationToken();

            var sqlAdapter = mock.Mock<ISqlAdapter>();
            sqlAdapter.Setup(a => a.SaveEntityCollectionAsync(It.IsAny<IEntityCollection2>(), true, true, cancellationToken))
                .ThrowsAsync(new Exception());

            var testObject = mock.Create<GetProductsAfterSequenceResult>(TypedParameter.From(data));

            await Assert.ThrowsAsync<Exception>(() => testObject.Apply(mock.Mock<ISqlAdapter>().Object, cancellationToken));

            mock.Mock<ISqlAdapter>()
                .Verify(x => x.Rollback());
        }

        private class TestUpdater : IHubProductUpdater
        {
            public WarehouseProduct ProductData { get; set; }

            public ProductVariantEntity ProductVariant { get; set; }

            public void UpdateProductVariant()
            {

            }
        }
    }
}
