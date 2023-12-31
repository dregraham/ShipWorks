﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
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
    [Trait("Category", "ProductImport")]
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
            progressReporter = new ProgressItem("Importing products");
            productCatalog = mock.Create<IProductCatalog>();
        }

        [Fact]
        public async Task LoadImportFile_ReturnsSuccess_WhenValidValues()
        {
            List<ProductToImportDto> skuProducts = new List<ProductToImportDto>() { GetFullProduct("1", "1-a | 1-b"), GetFullProduct("2", "2-a") };
            List<ProductToImportDto> bundleProducts = new List<ProductToImportDto>() { GetFullProduct("3", "3-a | 3-b | 3-c", "1 : 3 | 2 : 4 ") };

            var result = await RunTest(skuProducts, bundleProducts, true);

            Assert.True(result.Success);
            Assert.Equal(3, result.Value.SuccessCount);
        }

        [Fact]
        public async Task LoadImportFile_ReturnsFailure_WhenWeightIsInvalid()
        {
            List<ProductToImportDto> skuProducts = new List<ProductToImportDto>() { GetFullProduct("1") };
            List<ProductToImportDto> bundleProducts = new List<ProductToImportDto>();

            skuProducts.First().Weight = "asdf";

            var result = await RunTest(skuProducts, bundleProducts, true);

            Assert.True(result.Failure);
            Assert.Equal(0, result.Value.SuccessCount);
            Assert.Equal(1, result.Value.FailedCount);
        }

        [Fact]
        public async Task LoadImportFile_ReturnsFailure_WhenLengthIsInvalid()
        {
            List<ProductToImportDto> skuProducts = new List<ProductToImportDto>() { GetFullProduct("1") };
            List<ProductToImportDto> bundleProducts = new List<ProductToImportDto>();

            skuProducts.First().Length = "asdf";

            var result = await RunTest(skuProducts, bundleProducts, true);

            Assert.True(result.Failure);
            Assert.Equal(0, result.Value.SuccessCount);
            Assert.Equal(1, result.Value.FailedCount);
        }

        [Fact]
        public async Task LoadImportFile_ReturnsFailure_WhenWidthIsInvalid()
        {
            List<ProductToImportDto> skuProducts = new List<ProductToImportDto>() { GetFullProduct("1") };
            List<ProductToImportDto> bundleProducts = new List<ProductToImportDto>();

            skuProducts.First().Width = "asdf";

            var result = await RunTest(skuProducts, bundleProducts, true);

            Assert.True(result.Failure);
            Assert.Equal(0, result.Value.SuccessCount);
            Assert.Equal(1, result.Value.FailedCount);
        }

        [Fact]
        public async Task LoadImportFile_ReturnsFailure_WhenHeightIsInvalid()
        {
            List<ProductToImportDto> skuProducts = new List<ProductToImportDto>() { GetFullProduct("1") };
            List<ProductToImportDto> bundleProducts = new List<ProductToImportDto>();

            skuProducts.First().Height = "asdf";

            var result = await RunTest(skuProducts, bundleProducts, true);

            Assert.True(result.Failure);
            Assert.Equal(0, result.Value.SuccessCount);
            Assert.Equal(1, result.Value.FailedCount);
        }

        [Fact]
        public async Task LoadImportFile_ReturnsFailure_WhenDeclaredValueIsInvalid()
        {
            List<ProductToImportDto> skuProducts = new List<ProductToImportDto>() { GetFullProduct("1") };
            List<ProductToImportDto> bundleProducts = new List<ProductToImportDto>();

            skuProducts.First().DeclaredValue = "asdf";

            var result = await RunTest(skuProducts, bundleProducts, true);

            Assert.True(result.Failure);
            Assert.Equal(0, result.Value.SuccessCount);
            Assert.Equal(1, result.Value.FailedCount);
        }

        [Fact]
        public async Task LoadImportFile_ReturnsSuccess_WhenAllValuesDefault()
        {
            List<ProductToImportDto> skuProducts = new List<ProductToImportDto>() { GetFullProduct("1") };
            List<ProductToImportDto> bundleProducts = new List<ProductToImportDto>();

            var dto = skuProducts.First();
            dto.AliasSkus = string.Empty;
            dto.BundleSkus = string.Empty;
            dto.DeclaredValue = string.Empty;
            dto.Height = string.Empty;
            dto.Length = string.Empty;
            dto.Weight = string.Empty;
            dto.Width = string.Empty;
            dto.Active = string.Empty;
            dto.Asin = string.Empty;
            dto.CountryOfOrigin = string.Empty;
            dto.HarmonizedCode = string.Empty;
            dto.Upc = string.Empty;
            dto.Isbn = string.Empty;
            dto.ImageUrl = string.Empty;
            dto.Name = "A Name";
            dto.WarehouseBin = string.Empty;

            var result = await RunTest(skuProducts, bundleProducts, true);

            Assert.True(result.Success);
            Assert.Equal(1, result.Value.SuccessCount);
            Assert.Equal(0, result.Value.FailedCount);
        }

        [Fact]
        public async Task LoadImportFile_ReturnsFailure_WhenBundleReferencesItself()
        {
            List<ProductToImportDto> skuProducts = new List<ProductToImportDto>() { GetFullProduct("1", "1-a | 1-b"), GetFullProduct("2", "2-a") };
            List<ProductToImportDto> bundleProducts = new List<ProductToImportDto>() { GetFullProduct("3", "3-a", "1 : 3 | 3 : 4 ") };

            var result = await RunTest(skuProducts, bundleProducts, false);

            Assert.True(result.Failure);
            Assert.Equal(2, result.Value.SuccessCount);
            Assert.Equal(1, result.Value.FailedCount);
        }

        [Fact]
        public async Task LoadImportFile_ReturnsFailure_WhenBundleAliasReferencesItself()
        {
            List<ProductToImportDto> skuProducts = new List<ProductToImportDto>() { GetFullProduct("1", "1-a | 1-b"), GetFullProduct("2", "2-a") };
            List<ProductToImportDto> bundleProducts = new List<ProductToImportDto>() { GetFullProduct("3", "3-a", "1 : 3 | 3-a : 4 ") };

            var result = await RunTest(skuProducts, bundleProducts, false);

            Assert.True(result.Failure);
            Assert.Equal(2, result.Value.SuccessCount);
            Assert.Equal(1, result.Value.FailedCount);
        }

        [Fact]
        public async Task LoadImportFile_ReturnsTrueAndDoesNotCreateDuplicateAlias_WhenAliasReferencesItself()
        {
            List<ProductToImportDto> skuProducts = new List<ProductToImportDto>() { GetFullProduct("1", "1:1 | 1:1") };
            List<ProductToImportDto> bundleProducts = Enumerable.Empty<ProductToImportDto>().ToList();

            var result = await RunTest(skuProducts, bundleProducts, true);

            Assert.True(result.Failure);
            Assert.Equal(0, result.Value.SuccessCount);
            Assert.Equal(1, result.Value.FailedCount);
        }

        [Fact]
        public async Task LoadImportFile_ConvertsProductToBundle()
        {
            List<ProductToImportDto> skuProducts = new List<ProductToImportDto>() { GetFullProduct("1", "1-a:1 | 1-b:1"), GetFullProduct("2", "2-a:2") };
            List<ProductToImportDto> bundleProducts = Enumerable.Empty<ProductToImportDto>().ToList();

            var result = await RunTest(skuProducts, bundleProducts, true);
            Assert.True(result.Success);
            Assert.Equal(2, result.Value.SuccessCount);
            Assert.Equal(0, result.Value.FailedCount);

            skuProducts = new List<ProductToImportDto>() { GetFullProduct("3", "3-a:3 | 3-b:3"), GetFullProduct("1", "1-a:1 | 1-b:2", "2 : 3 | 3 : 4") };

            result = await RunTest(skuProducts, bundleProducts, true);

            Assert.True(result.Success);
            Assert.Equal(2, result.Value.SuccessCount);
            Assert.Equal(0, result.Value.FailedCount);
        }

        [Fact]
        public async Task LoadImportFile_ConvertsBundleToProduct()
        {
            List<ProductToImportDto> skuProducts = new List<ProductToImportDto>() { GetFullProduct("1", "1-a:1 | 1-b:2"), GetFullProduct("2", "2-a:1 | 2-b:2") };
            List<ProductToImportDto> bundleProducts = new List<ProductToImportDto>() { GetFullProduct("3", "3-a:3 | 3-b:4", "1 : 4 | 2 : 5") };

            var result = await RunTest(skuProducts, bundleProducts, true);
            Assert.True(result.Success);
            Assert.Equal(3, result.Value.SuccessCount);

            skuProducts = new List<ProductToImportDto>() { GetFullProduct("3", "3-a:a | 3-b:b") };
            bundleProducts = Enumerable.Empty<ProductToImportDto>().ToList();

            result = await RunTest(skuProducts, bundleProducts, true);

            Assert.True(result.Success);
            Assert.Equal(1, result.Value.SuccessCount);
        }

        [Fact]
        public async Task LoadImportFile_AddsItemToBundleToExistingBundle()
        {
            List<ProductToImportDto> skuProducts = new List<ProductToImportDto>()
            {
                GetFullProduct("1", "1-a | 1-b"),
                GetFullProduct("2", "2-a | 2-b"),
                GetFullProduct("4")
            };
            List<ProductToImportDto> bundleProducts = new List<ProductToImportDto>() { GetFullProduct("3", "3-a:a | 3-b:b", "1 : 4 | 2 : 5") };

            var result = await RunTest(skuProducts, bundleProducts, true);
            Assert.True(result.Success);
            Assert.Equal(4, result.Value.SuccessCount);

            skuProducts = new List<ProductToImportDto>() { GetFullProduct("3", "3-a:a | 3-b:b", "1 : 4 | 2 : 5 | 4 : 1") };
            bundleProducts = Enumerable.Empty<ProductToImportDto>().ToList();

            result = await RunTest(skuProducts, bundleProducts, true);

            Assert.True(result.Success);
            Assert.Equal(1, result.Value.SuccessCount);
        }

        [Fact]
        public async Task LoadImportFile_ReturnsSavesFirstAndThirdProduct_WhenSecondProductFailsToImport()
        {
            List<ProductToImportDto> skuProducts = new List<ProductToImportDto>()
            {
                GetFullProduct("1", "1-a:a"),
                GetFullProduct("2", "2-a:a", "2 : 1"), // This should fail as it is a bundle referencing itself.
                GetFullProduct("3", "3-a:a")
            };
            List<ProductToImportDto> bundleProducts = Enumerable.Empty<ProductToImportDto>().ToList();

            var result = await RunTest(skuProducts, bundleProducts, true);

            Assert.Equal(2, result.Value.SuccessCount);
            Assert.Equal(1, result.Value.FailedCount);

            Assert.True(result.Failure);
            Assert.Equal(2, result.Value.SuccessCount);
            Assert.Equal(1, result.Value.FailedCount);
        }

        [Fact]
        public async Task LoadImportFile_ReturnsFailure_WhenInvalidBundleHasInvalidQuantity()
        {
            List<string> badBundleTexts = new List<string>()
            {
                "1 :  | 2 :  ",
                "1 : 3 | 2 :  ",
                "1 :  | 2 : 4 ",
                "1 : a | 2 : 4 ",
                "1 : 3 | 2 : a ",
                "1 : 3 | 2 : 0 ",
                "1 : 3 | 2 : -1 "
            };

            foreach (string badBundleText in badBundleTexts)
            {
                List<ProductToImportDto> skuProducts = new List<ProductToImportDto>() { GetFullProduct("1", "1-a:a | 1-b:a"), GetFullProduct("2", "2-a:a") };
                List<ProductToImportDto> bundleProducts = new List<ProductToImportDto>() { GetFullProduct("3", "3-a:a | 3-b:b | 3-c:c", badBundleText) };

                var result = await RunTest(skuProducts, bundleProducts, false);

                Assert.True(result.Failure);
                Assert.Equal(2, result.Value.SuccessCount);
                Assert.Equal(1, result.Value.FailedCount);
            }
        }

        [Fact]
        public async Task LoadImportFile_ReturnsSuccess_WhenSpacesAroundBundleSeparators()
        {
            List<string> badBundleTexts = new List<string>()
            {
                "1 : 3|2 : 4 ",
                "1 : 3 |2 : 4 ",
                "1 : 3| 2 : 4 ",
                "1: 3 | 2 : 4 ",
                "1 :3 | 2 : 4 ",
                "1 : 3 | 2: 4 ",
                "1 : 3 | 2 :4 ",
                "1:3 | 2:4 "
            };

            foreach (string badBundleText in badBundleTexts)
            {
                List<ProductToImportDto> skuProducts = new List<ProductToImportDto>() { GetFullProduct("1", "1-a:a | 1-b:b"), GetFullProduct("2", "2-a:a") };
                List<ProductToImportDto> bundleProducts = new List<ProductToImportDto>() { GetFullProduct("3", "3-a:a | 3-b:b | 3-c:c", badBundleText) };

                var result = await RunTest(skuProducts, bundleProducts, false);

                Assert.True(result.Success);
                Assert.Equal(3, result.Value.SuccessCount);
                Assert.Equal(0, result.Value.FailedCount);
            }
        }

        [Fact]
        public async Task LoadImportFile_ReturnsSuccess_WhenSpacesAroundAliasSeparators()
        {
            List<string> badAliasTexts = new List<string>()
            {
                "1 : 3|9 : 4 ",
                "2 : 3 |10 : 4 ",
                "3 : 3| 11 : 4 ",
                "4: 3 | 12 : 4 ",
                "5 :3 | 13 : 4 ",
                "6 : 3 | 14: 4 ",
                "7 : 3 | 15 :4 ",
                "8:3 | 16:4 "
            };

            foreach (string badAliasText in badAliasTexts)
            {
                List<ProductToImportDto> skuProducts = new List<ProductToImportDto>() { GetFullProduct("99", badAliasText) };
                List<ProductToImportDto> bundleProducts = new List<ProductToImportDto>();

                var result = await RunTest(skuProducts, bundleProducts, false);

                Assert.True(result.Success);
            }
        }

        [Theory]
        [InlineData("42 :", @"1 \:: 3 |\| 2\:: 4 ")]
        [InlineData("42 :", @"1 \:: 3 |\| 2 \::4 ")]
        [InlineData("42:", @"1\::3 |\| 2\::4 ")]
        [InlineData("42 \\ :", @"1 \\ \::3 |\| 2\::4 ")]
        public async Task LoadImportFile_ReturnsSuccess_WhenAliasDelimiterIsEscaped(string sku1, string aliasText)
        {
            List<ProductToImportDto> skuProducts = new List<ProductToImportDto>() { GetFullProduct(sku1, aliasText)};
            List<ProductToImportDto> bundleProducts = new List<ProductToImportDto>();

            var result = await RunTest(skuProducts, bundleProducts, true);

            Assert.Equal(1, result.Value.SuccessCount);
        }

        [Theory]
        [InlineData("1 :", "| 2:", @"1 \:: 3 |\| 2\:: 4 ")]
        [InlineData("1 :", "| 2 :", @"1 \:: 3 |\| 2 \::4 ")]
        [InlineData("1:", "| 2:", @"1\::3 |\| 2\::4 ")]
        [InlineData("1 \\ :", "| 2:", @"1 \\ \::3 |\| 2\::4 ")]
        public async Task LoadImportFile_ReturnsSuccess_WhenBundleDelimiterIsEscaped(string sku1, string sku2, string bundleText)
        {
            List<ProductToImportDto> skuProducts = new List<ProductToImportDto>() { GetFullProduct(sku1, "1-a:A | 1-b:b"), GetFullProduct(sku2, "2-a:a") };
            List<ProductToImportDto> bundleProducts = new List<ProductToImportDto>() { GetFullProduct("3", "3-a:A | 3-b:b | 3-c:c", bundleText) };

            var result = await RunTest(skuProducts, bundleProducts, false);

            Assert.True(result.Success, $"{bundleText} => {result.Value.FailureResults.FirstOrDefault().Value} ");
            Assert.Equal(3, result.Value.SuccessCount);
            Assert.Equal(0, result.Value.FailedCount);
        }

        [Fact]
        public async Task LoadImportFile_ReturnsSuccess_WhenAliasContainsPipeButNotFullSeparator()
        {
            List<string> badAliasTexts = new List<string>()
            {
                "1a|1b ",
                "1a |1b ",
                "1a| 1b ",
            };

            foreach (string badAliasText in badAliasTexts)
            {
                List<ProductToImportDto> skuProducts = new List<ProductToImportDto>() { GetFullProduct("1", badAliasText), GetFullProduct("2") };
                List<ProductToImportDto> bundleProducts = new List<ProductToImportDto>() { GetFullProduct("3", "3-a:a | 3-b:b | 3-c:c", "1 : 3 | 2 : 4") };

                var result = await RunTest(skuProducts, bundleProducts, true);

                Assert.True(result.Success);
                Assert.Equal(3, result.Value.SuccessCount);
            }
        }

        public async Task<GenericResult<IImportProductsResult>> RunTest(List<ProductToImportDto> skuProducts, List<ProductToImportDto> bundleProducts, bool shouldSucceed)
        {
            var allProducts = skuProducts.Concat(bundleProducts);

            // Duplicate what the excel reader does for filtering out sku vs bundle rows.
            skuProducts = allProducts.Where(r => r.BundleSkus.IsNullOrWhiteSpace()).ToList();
            bundleProducts = allProducts.Where(r => !r.BundleSkus.IsNullOrWhiteSpace()).ToList();

            (List<ProductToImportDto>, List<ProductToImportDto>) result = (skuProducts, bundleProducts);

            context.Mock.Override<IProductExcelReader>().Setup(r => r.LoadImportFile(It.IsAny<string>()))
                .Returns(GenericResult.FromSuccess(result));

            testObject = mock.Create<ProductImporter>();

            var results = await testObject.ImportProducts(It.IsAny<string>(), progressReporter);

            if (shouldSucceed)
            {
                using (ISqlAdapter sqlAdapter = new SqlAdapter())
                {
                    skuProducts
                        .Concat(bundleProducts)
                        .ForEach(p =>
                        {
                            if (!DtoMatchesDbEntity(sqlAdapter, p))
                            {
                                if (results.Value.FailureResults.ContainsKey(p.Sku))
                                {
                                    results.Value.FailureResults[p.Sku] += $"{Environment.NewLine}DB entities did not match for test SKU '{p.Sku}'";
                                }
                                else
                                {
                                    results.Value.FailureResults.Add(p.Sku, $"DB entities did not match for test SKU '{p.Sku}'");
                                }

                            }
                        });
                }
            }
            else
            {
                if (results.Success)
                {
                    results.Value.FailureResults.Add("", "The product should have failed, but did not.");
                }
            }

            return results;
        }

        private bool DtoMatchesDbEntity(ISqlAdapter sqlAdapter, ProductToImportDto dto)
        {
            var productVariant = productCatalog.FetchProductVariantEntity(sqlAdapter, dto.Sku);
            if (productVariant == null)
            {
                return false;
            }

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
                productVariant.DeclaredValue == ProductToImportDto.GetValue<decimal?>(dto.DeclaredValue, "DeclaredValue", null) &&
                productVariant.Width == ProductToImportDto.GetValue<decimal?>(dto.Width, "Width", null) &&
                productVariant.Length == ProductToImportDto.GetValue<decimal?>(dto.Length, "Length", null) &&
                productVariant.Height == ProductToImportDto.GetValue<decimal?>(dto.Height, "Height", null) &&
                productVariant.Weight == ProductToImportDto.GetValue<decimal?>(dto.Weight, "Weight", null) &&
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
                                  .Any(asl => asl.Sku.Equals(aliasEntity.Sku, StringComparison.InvariantCultureIgnoreCase));
                }
            }

            return matches;
        }

        private ProductToImportDto GetFullProduct(string sku, string aliasSkus = "", string bundleSkus = "")
        {
            var dto = new ProductToImportDto()
            {
                Sku = sku,
                Active = "active",
                AliasSkus = aliasSkus,
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

            dto.AliasSkuList = new List<(string Sku, string Name)>();
            if (!aliasSkus.IsNullOrWhiteSpace())
            {
                dto.AliasSkuList = Regex.Split(dto.AliasSkus, ProductExcelReader.SkuSeparatorRegex, RegexOptions.IgnoreCase)
                    .Select(aliasNameAndSku => Regex.Split(aliasNameAndSku, ProductExcelReader.SkuNameSeparatorRegex, RegexOptions.IgnoreCase))
                    .Where(s => s.Length == 2 && !s[1].IsNullOrWhiteSpace() && !s[1].Equals(dto.Sku.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    .Distinct()
                    .Select(s => (Regex.Unescape(s[0]).Trim(), Regex.Unescape(s[1]).Trim()));
            }

            dto.BundleSkuList = GetBundleSkuList(dto.Sku, dto.BundleSkus, dto.AliasSkuList);

            return dto;
        }

        /// <summary>
        /// Build the list of product bundle sku and quantity
        /// </summary>
        private IEnumerable<(string Sku, int Quantity)> GetBundleSkuList(string sku, string bundleSkus, IEnumerable<(string Sku, string Name)> aliasSkuList)
        {
            if (bundleSkus.IsNullOrWhiteSpace())
            {
                return Enumerable.Empty<(string, int)>();
            }

            return Regex.Split(bundleSkus, ProductExcelReader.SkuSeparatorRegex, RegexOptions.IgnoreCase)
                .Select(skuAndQty => Regex.Split(skuAndQty, ProductExcelReader.SkuQuantitySeparatorRegex, RegexOptions.IgnoreCase))
                .Where(values => !(values.Length == 1 && values[0].IsNullOrWhiteSpace())) // Ignore extra beginning/ending delimiters
                .Select(values =>
                {
                    if (values.Length != 2)
                    {
                        throw new ProductImportException($"Quantity is required, but wasn't supplied for bundled item SKU {values[0]}");
                    }

                    string testSku = Regex.Unescape(values[0]);
                    string testQty = Regex.Unescape(values[1]);
                    if (testSku.Equals(sku, StringComparison.InvariantCultureIgnoreCase) ||
                        aliasSkuList.Any(a => a.Sku.Equals(testSku, StringComparison.CurrentCultureIgnoreCase)))
                    {
                        throw new ProductImportException($"Bundles may not be composed of its SKU or any of its alias SKUs.  Problem SKU: {testSku}");
                    }

                    int quantity = GetValue(testQty, "Bundle Quantity", 0);
                    if (quantity <= 0)
                    {
                        throw new ProductImportException($"Quantity must be greater than 0 for bundled item SKU {values[0]}");
                    }

                    return (testSku, quantity);
                });
        }

        /// <summary>
        /// Get T value of the given string.
        /// </summary>
        public static T GetValue<T>(string text, string propertyName, T defaultValue)
        {
            if (!text.IsNullOrWhiteSpace())
            {
                try
                {
                    var converter = TypeDescriptor.GetConverter(typeof(T));
                    if (converter != null)
                    {
                        return (T) converter.ConvertFromString(text);
                    }
                }
                catch
                {
                    // throwing below
                }

                throw new ProductImportException($"Unable to convert '{propertyName}' with value {text.Trim()} to a number.");
            }

            return defaultValue;
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
