using System.Linq;
using Autofac.Extras.Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Products.Warehouse;
using ShipWorks.Products.Warehouse.DTO;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Products.Tests.Warehouse
{
    public class HubProductAttributeUpdaterTest
    {
        private readonly AutoMock mock;
        private readonly HubProductAttributeUpdater testObject;

        public HubProductAttributeUpdaterTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            testObject = mock.Create<HubProductAttributeUpdater>();
        }

        [Fact]
        public void UpdateProductVariant_RemovesAttributeValue_WhenItIsNotInHubData()
        {
            var attribute = new ProductAttributeEntity { AttributeName = "Color" };
            var attributeValue = new ProductVariantAttributeValueEntity { AttributeValue = "Red", ProductAttribute = attribute };
            var variant = new ProductVariantEntity { Product = new ProductEntity() };
            variant.Product.Attributes.Add(attribute);
            variant.AttributeValues.Add(attributeValue);

            var hubData = new WarehouseProduct { Attributes = Enumerable.Empty<ProductAttribute>() };

            testObject.UpdateProductVariant(variant, hubData);

            Assert.Contains(attributeValue, variant.AttributeValues.RemovedEntitiesTracker.OfType<ProductVariantAttributeValueEntity>());
            Assert.Empty(variant.AttributeValues);
            Assert.Contains(attribute, variant.Product.Attributes);
        }

        [Fact]
        public void UpdateProductVariant_UpdatesAttribute_WhenItAlreadyExists()
        {
            var attribute = new ProductAttributeEntity { AttributeName = "Color" };
            var attributeValue = new ProductVariantAttributeValueEntity { AttributeValue = "Red", ProductAttribute = attribute };
            var variant = new ProductVariantEntity { Product = new ProductEntity() };
            variant.Product.Attributes.Add(attribute);
            variant.AttributeValues.Add(attributeValue);

            var hubData = new WarehouseProduct
            {
                Attributes = new[] { new ProductAttribute { Name = "Color", Value = "Blue" } }
            };

            testObject.UpdateProductVariant(variant, hubData);

            Assert.Empty(variant.AttributeValues.RemovedEntitiesTracker.OfType<ProductVariantAttributeValueEntity>());
            Assert.Contains(attributeValue, variant.AttributeValues);
            Assert.Contains(attribute, variant.Product.Attributes);
            Assert.Equal("Blue", attributeValue.AttributeValue);
        }

        [Fact]
        public void UpdateProductVariant_AddsAttributeValue_WhenItDoesNotExist()
        {
            var attribute = new ProductAttributeEntity { AttributeName = "Color" };
            var variant = new ProductVariantEntity { Product = new ProductEntity() };
            variant.Product.Attributes.Add(attribute);

            var hubData = new WarehouseProduct
            {
                Attributes = new[] { new ProductAttribute { Name = "Color", Value = "Blue" } }
            };

            testObject.UpdateProductVariant(variant, hubData);

            Assert.Empty(variant.AttributeValues.RemovedEntitiesTracker.OfType<ProductVariantAttributeValueEntity>());
            Assert.Equal(1, variant.AttributeValues.Count);
            Assert.Contains(attribute, variant.Product.Attributes);
            Assert.Equal("Blue", variant.AttributeValues.ElementAt(0).AttributeValue);
            Assert.Equal(attribute, variant.AttributeValues.ElementAt(0).ProductAttribute);
        }

        [Fact]
        public void UpdateProductVariant_AddsAttribute_WhenItDoesNotExist()
        {
            var variant = new ProductVariantEntity { Product = new ProductEntity() };
            var hubData = new WarehouseProduct
            {
                Attributes = new[] { new ProductAttribute { Name = "Color", Value = "Blue" } }
            };

            testObject.UpdateProductVariant(variant, hubData);

            Assert.Empty(variant.AttributeValues.RemovedEntitiesTracker.OfType<ProductVariantAttributeValueEntity>());
            Assert.Equal(1, variant.AttributeValues.Count);
            Assert.Equal(1, variant.Product.Attributes.Count);
            Assert.Equal("Blue", variant.AttributeValues.ElementAt(0).AttributeValue);
            Assert.Equal("Color", variant.Product.Attributes.ElementAt(0).AttributeName);
            Assert.Equal(variant.Product.Attributes.ElementAt(0), variant.AttributeValues.ElementAt(0).ProductAttribute);
        }
    }
}
