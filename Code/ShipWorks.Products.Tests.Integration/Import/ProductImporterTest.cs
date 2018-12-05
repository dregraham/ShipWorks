using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using Interapptive.Shared.Collections;
using Interapptive.Shared.Threading;
using Interapptive.Shared.Utility;
using Moq;
using ShipWorks.Common.Threading;
using ShipWorks.Data.Connection;
using ShipWorks.Products.Import;
using ShipWorks.Startup;
using ShipWorks.Tests.Shared;
using ShipWorks.Tests.Shared.Database;
using Xunit;

namespace ShipWorks.Products.Tests.Integration.Import
{
    [Collection("Database collection")]
    [Trait("Category", "ContinuousIntegration")]
    public class ProductImporterTest : IDisposable
    {
        private readonly AutoMock mock;
        private readonly DataContext context;
        private readonly IProgressReporter progressReporter;
        private readonly IProductCatalog productCatalog;
        private readonly string filename;
        private ProductImporter testObject;

        public ProductImporterTest(DatabaseFixture db)
        {
            context = db.CreateDataContext(x => ContainerInitializer.Initialize(x));
            filename = Path.GetTempFileName();
            mock = context.Mock;
            progressReporter =  new ProgressItem("Importing products");
            productCatalog = mock.Create<IProductCatalog>();
        }

        [Fact]
        public async Task LoadImportFile_ReturnsSuccess_WhenValidValues()
        {
            List<ProductToImportDto> skuProducts = new List<ProductToImportDto>() {GetFullProduct("1"), GetFullProduct("2")};
            List<ProductToImportDto> bundleProducts = new List<ProductToImportDto>() {GetFullProduct("3", "1 : 3 | 2 : 4 ")};

            (List<ProductToImportDto>, List<ProductToImportDto>) result = (skuProducts, bundleProducts);

            context.Mock.Override<IProductExcelReader>().Setup(r => r.LoadImportFile(It.IsAny<string>()))
                .Returns(GenericResult.FromSuccess(result));

            testObject = mock.Create<ProductImporter>();

            var results = await testObject.ImportProducts(It.IsAny<string>(), progressReporter);

            Assert.True(results.Success);

            using (ISqlAdapter sqlAdapter = new SqlAdapter())
            {
                skuProducts
                    .Concat(bundleProducts)
                    .ForEach(p =>
                    {
                        Assert.True(DtoMatchesDbEntity(sqlAdapter, p), $"DB entities did not match SKU '{p.Sku}'");
                    });
            }
        }

        private bool DtoMatchesDbEntity(ISqlAdapter sqlAdapter, ProductToImportDto dto)
        {
            var productVariant = productCatalog.FetchProductVariantEntity(sqlAdapter, dto.Sku);
            var productVariantSku = productVariant.Aliases.First(a => a.IsDefault);

            bool matches = 
                productVariantSku.Sku == dto.Sku &&
                productVariant.IsActive == dto.IsActive &&
                productVariant.ASIN == dto.Asin &&
                productVariant.BinLocation == dto.WarehouseBin &&
                productVariant.CountryOfOrigin == dto.CountryOfOrigin &&
                productVariant.HarmonizedCode == dto.HarmonizedCode &&
                productVariant.ISBN == dto.Isbn &&
                productVariant.ImageUrl == dto.ImageUrl &&
                productVariant.Name == dto.Name &&
                productVariant.UPC == dto.Upc &&
                productVariant.DeclaredValue == ProductToImportDto.GetValue<decimal>(dto.DeclaredValue, "DeclaredValue") &&
                productVariant.Width == ProductToImportDto.GetValue<decimal>(dto.Width, "Width") &&
                productVariant.Length == ProductToImportDto.GetValue<decimal>(dto.Length, "Length") &&
                productVariant.Height == ProductToImportDto.GetValue<decimal>(dto.Height, "Height") &&
                productVariant.Weight == ProductToImportDto.GetValue<decimal>(dto.Weight, "Weight") &&
                productVariant.Product.IsActive == productVariant.IsActive &&
                productVariant.Product.IsBundle == !dto.BundleSkus.IsNullOrWhiteSpace() &&
                dto.AliasSkuList.Count() == productVariant.Aliases.Count - 1 && // Subtract out the main sku
                dto.BundleSkuList.Count() == productVariant.Product.Bundles.Count;

            if (matches)
            {
                foreach (var bundleEntity in productVariant.Product.Bundles)
                {
                    var childVariant = productCatalog.FetchProductVariantEntity(sqlAdapter, bundleEntity.ChildProductVariantID);
                    matches = matches &&
                              dto.BundleSkuList
                                  .Any(bsl => bsl.Sku.Equals(childVariant.Aliases.First(a => a.IsDefault).Sku, StringComparison.InvariantCultureIgnoreCase) &&
                                              bsl.Quantity == bundleEntity.Quantity);
                }
            }

            if (matches)
            {
                foreach (var aliasEntity in productVariant.Aliases.Where(a => !a.IsDefault))
                {
                    matches = matches &&
                              dto.AliasSkuList
                                  .Any(asl => asl.Equals(aliasEntity.Sku, StringComparison.InvariantCultureIgnoreCase));
                }
            }

            return matches;
        }

        private ProductToImportDto GetFullProduct(string sku, string bundleSkus = "")
        {
            return new ProductToImportDto()
            {
                Sku = sku,
                Active = "active",
                AliasSkus = $"{sku}-a | {sku}-b",
                BundleSkus = bundleSkus,
                Asin = $"{sku}-asin",
                CountryOfOrigin = "US",
                DeclaredValue = "0.1",
                HarmonizedCode = $"{sku}-HarmCode",
                Height = "6.1",
                Width = "5.1",
                Length = "4.1",
                Weight = "0.1",
                ImageUrl = "http://www.com",
                Upc = $"{sku}-upc",
                Name = $"{sku}-name",
                Isbn = $"{sku}-isbn",
                WarehouseBin = $"{sku}-warehouse/bin"
            };
        }

        public void Dispose()
        {
            context.Dispose();
            mock?.Dispose();

            if (File.Exists(filename))
            {
                File.Delete(filename);
            }
        }
    }
}
