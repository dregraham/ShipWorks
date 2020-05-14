using System.Collections.Generic;
using System.Linq;
using Autofac.Extras.Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Products.Warehouse;
using ShipWorks.Products.Warehouse.DTO;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Products.Tests.Warehouse
{
    public class HubProductAliasUpdaterTest
    {
        private readonly AutoMock mock;
        private readonly HubProductAliasUpdater testObject;

        public HubProductAliasUpdaterTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            testObject = mock.Create<HubProductAliasUpdater>();
        }

        [Fact]
        public void UpdateProductVariant_DoesNotRemoveDefaultAlias()
        {
            var alias = new ProductVariantAliasEntity { IsDefault = true, Sku = "ABC123" };
            var variant = new ProductVariantEntity();
            variant.Aliases.Add(alias);

            var hubData = new WarehouseProduct { Aliases = new Dictionary<string, ProductAlias>() };

            testObject.UpdateProductVariant(variant, hubData);

            Assert.Empty(variant.Aliases.RemovedEntitiesTracker);
            Assert.Contains(alias, variant.Aliases);
        }

        [Fact]
        public void UpdateProductVariant_RemovesAlias_WhenItIsNotInHubData()
        {
            var alias = new ProductVariantAliasEntity { IsDefault = false, Sku = "ABC123" };
            var variant = new ProductVariantEntity();
            variant.Aliases.Add(alias);

            var hubData = new WarehouseProduct { Aliases = new Dictionary<string, ProductAlias>() };

            testObject.UpdateProductVariant(variant, hubData);

            Assert.Contains(alias, variant.Aliases.RemovedEntitiesTracker.OfType<ProductVariantAliasEntity>());
            Assert.Empty(variant.Aliases);
        }

        [Fact]
        public void UpdateProductVariant_UpdatesAlias_WhenItAlreadyExists()
        {
            var alias = new ProductVariantAliasEntity { IsDefault = false, Sku = "ABC123", AliasName = "Foo" };
            var variant = new ProductVariantEntity();
            variant.Aliases.Add(alias);

            var hubData = new WarehouseProduct
            {
                Aliases = new Dictionary<string, ProductAlias>
                {
                    { "ABC123", new ProductAlias { Sku = "ABC123", Name = "Bar" } }
                }
            };

            testObject.UpdateProductVariant(variant, hubData);

            Assert.Empty(variant.Aliases.RemovedEntitiesTracker.OfType<ProductVariantAliasEntity>());
            Assert.Contains(alias, variant.Aliases);
            Assert.Equal("Bar", alias.AliasName);
        }

        [Fact]
        public void UpdateProductVariant_AddsAlias_WhenItDoesNotExist()
        {
            var variant = new ProductVariantEntity();
            var hubData = new WarehouseProduct
            {
                Aliases = new Dictionary<string, ProductAlias>
                {
                    { "ABC123", new ProductAlias { Sku = "ABC123", Name = "Bar" } }
                }
            };

            testObject.UpdateProductVariant(variant, hubData);

            Assert.Empty(variant.Aliases.RemovedEntitiesTracker.OfType<ProductVariantAliasEntity>());
            Assert.Equal(1, variant.Aliases.Count);
            Assert.Equal("Bar", variant.Aliases.ElementAt(0).AliasName);
            Assert.Equal("ABC123", variant.Aliases.ElementAt(0).Sku);
        }
    }
}
