using System;
using System.IO;
using System.Linq;
using Autofac.Extras.Moq;
using ShipWorks.Products.Import;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Products.Tests.Import
{
    public class ProductExcelReaderTest : IDisposable
    {
        private readonly AutoMock mock;
        private readonly ProductExcelReader testObject;
        private readonly string filename;

        public ProductExcelReaderTest()
        {
            filename = Path.GetTempFileName();
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            testObject = mock.Create<ProductExcelReader>();
        }

        [Fact]
        public void LoadImportFile_ReturnsFailure_WhenInvalidFilename()
        {
            var results = testObject.LoadImportFile("asdf");

            Assert.True(results.Failure);
        }

        [Fact]
        public void LoadImportFile_ReturnsFailure_WhenFileIsEmpty()
        {
            var results = testObject.LoadImportFile(filename);

            Assert.True(results.Failure);
        }

        [Fact]
        public void LoadImportFile_ReturnsFailure_WhenFileHasBadData()
        {
            File.WriteAllText(filename, "This is some bad text for the file.");

            var results = testObject.LoadImportFile(filename);

            Assert.True(results.Failure);
        }

        [Fact]
        public void LoadImportFile_ReturnsSuccess_WhenFileHasCorrectColumns()
        {
            File.WriteAllLines(filename, new[]
            {
                "SKU,Alias SKUs,Bundled SKUs,Name,UPC ,ASIN ,ISBN ,Weight,Length,Width,Height,Image URL,Warehouse-Bin Location,Declared Value,Country of Origin,Harmonized Code,Active",
                ",SKU | SKU ,SKU: Qty | SKU: Qty ,,,,,lbs,inches,inches,inches,,,,,,"
            });

            var results = testObject.LoadImportFile(filename);

            Assert.True(results.Success);
        }

        [Fact]
        public void LoadImportFile_ReturnsFailure_WhenFileIsMissingSecondHeaderRow()
        {
            File.WriteAllLines(filename, new[]
            {
                "SKU,Alias SKUs,Bundled SKUs,Name,UPC ,ASIN ,ISBN ,Weight,Length,Width,Height,Image URL,Warehouse-Bin Location,Declared Value,Country of Origin,Harmonized Code,Active",
            });

            var results = testObject.LoadImportFile(filename);

            Assert.True(results.Failure);
        }

        [Fact]
        public void LoadImportFile_ReturnsFailure_WhenFileHasMissingColumn()
        {
            File.WriteAllLines(filename, new[]
            {
                "SK U,Alias SKUs,Bundled SKUs,Name,UPC ,ASIN ,ISBN ,Weight,Length,Width,Height,Image URL,Warehouse-Bin Location,Declared Value,Country of Origin,Harmonized Code,Active",
                ",SKU | SKU ,SKU: Qty | SKU: Qty ,,,,,lbs,inches,inches,inches,,,,,,",
                "2,1,1,Pink,111,222,333,1,2,3,2,https://www.com/1,bin 2,11,US,420212,active"
            });

            var results = testObject.LoadImportFile(filename);

            Assert.True(results.Failure);
            Assert.True(results.Exception.Message.Contains("SKU"));
        }

        [Fact]
        public void LoadImportFile_ReturnsSuccess_WhenHeaderColumnHasExtraSpacesAroundTheText()
        {
            File.WriteAllLines(filename, new[]
            {
                "SKU ,Alias SKUs,Bundled SKUs,Name, UPC , ASIN ,ISBN , Weight,Length,Width,Height,Image URL,Warehouse-Bin Location,Declared Value,Country of Origin,Harmonized Code,Active",
                ",SKU | SKU ,SKU: Qty | SKU: Qty ,,,,,lbs,inches,inches,inches,,,,,,",
                "2,1,1,Pink,111,222,333,xxx,xxxx,xxx,xxx,https://www.com/1,bin 2,xxxxx,US,420212,active"
            });

            var results = testObject.LoadImportFile(filename);

            Assert.True(results.Success);
        }

        [Fact]
        public void LoadImportFile_SetsProductToImportDtoAliasSkuAndName()
        {
            File.WriteAllLines(filename, new[]
            {
                "SKU ,Alias SKUs,Bundled SKUs,Name, UPC , ASIN ,ISBN , Weight,Length,Width,Height,Image URL,Warehouse-Bin Location,Declared Value,Country of Origin,Harmonized Code,Active",
                ",SKU : Name | SKU : Name ,SKU: Qty | SKU: Qty ,,,,,lbs,inches,inches,inches,,,,,,",
                "2,AliasSku:AliasName,1,Pink,111,222,333,xxx,xxxx,xxx,xxx,https://www.com/1,bin 2,xxxxx,US,420212,active"
            });

            var results = testObject.LoadImportFile(filename);

            Assert.True(results.Success);
            Assert.Equal("AliasName", results.Value.BundleRows.First().AliasSkuList.First().Name);
            Assert.Equal("AliasSku", results.Value.BundleRows.First().AliasSkuList.First().Sku);
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
