using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using Interapptive.Shared.Threading;
using Interapptive.Shared.Utility;
using Moq;
using ShipWorks.Products.Import;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Products.Tests.Import
{
    public class ProductImporterTest : IDisposable
    {
        private readonly AutoMock mock;
        private readonly ProductImporter testObject;
        private readonly Mock<IProductExcelReader> productExcelReader;
        private readonly Mock<IProgressReporter> progressReporter;
        private readonly string filename;

        public ProductImporterTest()
        {
            filename = Path.GetTempFileName();
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            productExcelReader = mock.Mock<IProductExcelReader>();
            progressReporter = mock.Mock<IProgressReporter>();
            testObject = mock.Create<ProductImporter>();
        }

        [Fact]
        public async Task Import_ReturnsFailure_WhenImportHasNoProducts()
        {
            productExcelReader.Setup(r => r.LoadImportFile(It.IsAny<string>()))
                .Returns((
                    new List<ProductToImportDto> { },
                    new List<ProductToImportDto> { }
                ));

            var results = await testObject.ImportProducts(It.IsAny<string>(), progressReporter.Object);

            Assert.True(results.Failure);
            Assert.Contains("No records found", results.Exception.Message);
        }

        [Fact]
        public async Task LoadImportFile_ReturnsFailure_WhenWidthColumnDoesNotHaveANumberValue()
        {
            ProductToImportDto productToImport = new ProductToImportDto()
            {
                Sku = "1234",
                Active = "active",
                AliasSkus = "",
                BundleSkus = "",
                Asin = "",
                CountryOfOrigin = "",
                DeclaredValue = "",
                HarmonizedCode = "",
                Height = "",
                Width = "",
                Length = "",
                ImageUrl = "",
                Upc = "",
                Weight = "",
                Name = "",
                Isbn = "",
                WarehouseBin = ""
            };

            (List<ProductToImportDto>, List<ProductToImportDto>) result = (
                new List<ProductToImportDto>() { productToImport },
                new List<ProductToImportDto>() { }
            );

            productExcelReader.Setup(r => r.LoadImportFile(It.IsAny<string>()))
                .Returns(GenericResult.FromSuccess(result));

            var results = await testObject.ImportProducts(It.IsAny<string>(), progressReporter.Object);

            Assert.True(results.Success);
        }

        public void Dispose()
        {
            mock?.Dispose();

            if (File.Exists(filename))
            {
                File.Delete(filename);
            }
        }
    }
}
