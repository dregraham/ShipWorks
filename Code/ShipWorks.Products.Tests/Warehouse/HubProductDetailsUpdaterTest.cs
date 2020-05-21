using System;
using Autofac.Extras.Moq;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Products.Warehouse;
using ShipWorks.Products.Warehouse.DTO;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Products.Tests.Warehouse
{
    public class HubProductDetailsUpdaterTest
    {
        private readonly AutoMock mock;
        private readonly HubProductDetailsUpdater testObject;

        public HubProductDetailsUpdaterTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            testObject = mock.Create<HubProductDetailsUpdater>();
        }

        [Fact]
        public void UpdateProductVariant_CopiesAllDetails_FromHubData()
        {
            var createdDate = DateTime.UtcNow;
            var variant = new ProductVariantEntity { Product = new ProductEntity() };
            var hubData = new WarehouseProduct
            {
                Version = 9,
                Sequence = 212,
                ProductId = Guid.NewGuid().ToString(),
                Name = "TEST Name",
                Upc = "TEST Upc",
                Asin = "TEST Asin",
                Isbn = "TEST Isbn",
                Weight = 2,
                Length = 3,
                Width = 4,
                Height = 5,
                ImageUrl = "TEST ImageUrl",
                HarmonizedCode = "TEST HarmonizedCode",
                DeclaredValue = 6,
                CountryOfOrigin = "TEST CountryOfOrigin",
                Fnsku = "TEST Fnsku",
                Ean = "TEST Ean",
                Enabled = true,
                IsBundle = true,
                CreatedDate = createdDate,
            };

            testObject.UpdateProductVariant(variant, hubData);

            Assert.Equal(9, variant.HubVersion);
            Assert.Equal(212, variant.HubSequence);
            Assert.Equal(Guid.Parse(hubData.ProductId), variant.HubProductId);
            Assert.Equal("TEST Name", variant.Name);
            Assert.Equal("TEST Upc", variant.UPC);
            Assert.Equal("TEST Asin", variant.ASIN);
            Assert.Equal("TEST Isbn", variant.ISBN);
            Assert.Equal(2, variant.Weight);
            Assert.Equal(3, variant.Length);
            Assert.Equal(4, variant.Width);
            Assert.Equal(5, variant.Height);
            Assert.Equal("TEST ImageUrl", variant.ImageUrl);
            Assert.Equal("TEST HarmonizedCode", variant.HarmonizedCode);
            Assert.Equal(6, variant.DeclaredValue);
            Assert.Equal("TEST CountryOfOrigin", variant.CountryOfOrigin);
            Assert.Equal("TEST Fnsku", variant.FNSku);
            Assert.Equal("TEST Ean", variant.EAN);
            Assert.Equal(true, variant.Product.IsActive);
            Assert.Equal(true, variant.Product.IsBundle);
            Assert.Equal(createdDate, variant.CreatedDate);
            Assert.Equal(hubData.Enabled, variant.IsActive);
        }

        [Fact]
        public void UpdateProductVariant_SetsCreatedDateForSqlServer_FromHubData()
        {
            var createdDate = DateTime.UtcNow;
            var variant = new ProductVariantEntity { Product = new ProductEntity() };
            var hubData = new WarehouseProduct
            {
                Version = 9,
                Sequence = 212,
                ProductId = Guid.NewGuid().ToString(),
                Name = "TEST Name",
                Upc = "TEST Upc",
                Asin = "TEST Asin",
                Isbn = "TEST Isbn",
                Weight = 2,
                Length = 3,
                Width = 4,
                Height = 5,
                ImageUrl = "TEST ImageUrl",
                HarmonizedCode = "TEST HarmonizedCode",
                DeclaredValue = 6,
                CountryOfOrigin = "TEST CountryOfOrigin",
                Fnsku = "TEST Fnsku",
                Ean = "TEST Ean",
                Enabled = true,
                IsBundle = true,
                CreatedDate = DateTime.MinValue,
            };

            testObject.UpdateProductVariant(variant, hubData);

            Assert.Equal(9, variant.HubVersion);
            Assert.Equal(212, variant.HubSequence);
            Assert.Equal(Guid.Parse(hubData.ProductId), variant.HubProductId);
            Assert.Equal("TEST Name", variant.Name);
            Assert.Equal("TEST Upc", variant.UPC);
            Assert.Equal("TEST Asin", variant.ASIN);
            Assert.Equal("TEST Isbn", variant.ISBN);
            Assert.Equal(2, variant.Weight);
            Assert.Equal(3, variant.Length);
            Assert.Equal(4, variant.Width);
            Assert.Equal(5, variant.Height);
            Assert.Equal("TEST ImageUrl", variant.ImageUrl);
            Assert.Equal("TEST HarmonizedCode", variant.HarmonizedCode);
            Assert.Equal(6, variant.DeclaredValue);
            Assert.Equal("TEST CountryOfOrigin", variant.CountryOfOrigin);
            Assert.Equal("TEST Fnsku", variant.FNSku);
            Assert.Equal("TEST Ean", variant.EAN);
            Assert.Equal(true, variant.Product.IsActive);
            Assert.Equal(true, variant.Product.IsBundle);
            Assert.True(variant.CreatedDate >= createdDate);
        }
    }
}
