using System;
using System.Collections.Generic;
using Autofac;
using Autofac.Extras.Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Tests.Shared;
using Xunit;
using System.Reflection;
using log4net.Core;

namespace ShipWorks.Products.Tests
{
    public class ProductVariantTest : IDisposable
    {
        private readonly AutoMock mock;

        public ProductVariantTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
        }

        [Fact]
        public void CanWriteXml_IsFalse_WhenVariantIsNull()
        {
            var testObject = mock.Create<ProductVariant>(
                TypedParameter.From("sku"),
                TypedParameter.From<IProductVariantEntity>(null));

            Assert.False(testObject.CanWriteXml);
        }

        [Theory]
        [InlineData(true, true)]
        [InlineData(false, false)]
        public void CanWriteXml_IsTrue_WhenVariantIsNotNull(bool isActive, bool expectedValue)
        {
            var productVariantEntity = mock.Mock<IProductVariantEntity>();
            productVariantEntity.Setup(pve => pve.IsActive).Returns(isActive);

            var testObject = mock.Create<ProductVariant>(
                TypedParameter.From("sku"),
                TypedParameter.From(productVariantEntity.Object));

            Assert.Equal(expectedValue, testObject.CanWriteXml);
        }

        [Obfuscation(Exclude = true)]
        public static IEnumerable<object[]> applyDimData =>
           new List<object[]>
           {
                new object[] { (decimal?) 42, 42 },
                new object[] { (decimal?) 0, 12 },
                new object[] { null, 12 }
           };

        [Obfuscation(Exclude = true)]
        public static IEnumerable<object[]> applyDecimalData =>
           new List<object[]>
           {
                new object[] { (decimal?) 1, 2, 1 },
                new object[] { null, 2, 2 },
                new object[] { (decimal?) 0, 2, 2 }
           };

        [Theory]
        [MemberData(nameof(applyDimData))]
        public void Apply_AppliesWeight(decimal? productWeight, double expectedWeight)
        {
            var testObject = mock.Create<ProductVariant>(
            TypedParameter.From("sku"),
            TypedParameter.From<IProductVariantEntity>(new ProductVariantEntity() { IsActive = true, Weight = productWeight }));

            var item = new OrderItemEntity() { Weight = 12 };

            testObject.Apply(item);

            Assert.Equal(expectedWeight, item.Weight);
        }

        [Theory]
        [MemberData(nameof(applyDimData))]
        public void Apply_AppliesLength(decimal? productDim, decimal expectedDim)
        {
            var testObject = mock.Create<ProductVariant>(
            TypedParameter.From("sku"),
            TypedParameter.From<IProductVariantEntity>(new ProductVariantEntity() { IsActive = true, Length = productDim }));

            var item = new OrderItemEntity() { Length = 12 };

            testObject.Apply(item);

            Assert.Equal(expectedDim, item.Length);
        }

        [Theory]
        [MemberData(nameof(applyDimData))]
        public void Apply_AppliesWidth(decimal? productDim, decimal expectedDim)
        {
            var testObject = mock.Create<ProductVariant>(
            TypedParameter.From("sku"),
            TypedParameter.From<IProductVariantEntity>(new ProductVariantEntity() { IsActive = true, Width = productDim }));

            var item = new OrderItemEntity() { Width = 12 };

            testObject.Apply(item);

            Assert.Equal(expectedDim, item.Width);
        }

        [Theory]
        [MemberData(nameof(applyDimData))]
        public void Apply_AppliesHeight(decimal? productDim, decimal expectedDim)
        {
            var testObject = mock.Create<ProductVariant>(
            TypedParameter.From("sku"),
            TypedParameter.From<IProductVariantEntity>(new ProductVariantEntity() { IsActive = true, Height = productDim }));

            var item = new OrderItemEntity() { Height = 12 };

            testObject.Apply(item);

            Assert.Equal(expectedDim, item.Height);
        }

        [Theory]
        [InlineData("", "abcd", "abcd")]
        [InlineData("abcd", "defg", "abcd")]
        [InlineData("abcd", "", "abcd")]
        [InlineData(null, "abcd", "abcd")]
        [InlineData(null, "", "")]
        public void Apply_AppliesName(string productName, string customsDescription, string expectedName)
        {
            var testObject = mock.Create<ProductVariant>(
                TypedParameter.From("sku"),
                TypedParameter.From<IProductVariantEntity>(new ProductVariantEntity() { IsActive = true, Name = productName }));

            var item = new ShipmentCustomsItemEntity()
            {
                Description = customsDescription
            };

            testObject.Apply(item);

            Assert.Equal(expectedName, item.Description);
        }

        [Theory]
        [InlineData("", "abcd", "abcd")]
        [InlineData("abcd", "defg", "abcd")]
        [InlineData("abcd", "", "abcd")]
        [InlineData(null, "abcd", "abcd")]
        [InlineData(null, "", "")]
        public void Apply_AppliesHarmonizedCode(string productHarmonizedCode, string customsHarmonizedCode, string expectedHarmonizedCode)
        {
            var testObject = mock.Create<ProductVariant>(
                TypedParameter.From("sku"),
                TypedParameter.From<IProductVariantEntity>(new ProductVariantEntity() { IsActive = true, HarmonizedCode = productHarmonizedCode }));

            var item = new ShipmentCustomsItemEntity()
            {
                HarmonizedCode = customsHarmonizedCode
            };

            testObject.Apply(item);

            Assert.Equal(expectedHarmonizedCode, item.HarmonizedCode);
        }

        [Theory]
        [InlineData("", "abcd", "abcd")]
        [InlineData("abcd", "defg", "abcd")]
        [InlineData("abcd", "", "abcd")]
        [InlineData(null, "abcd", "abcd")]
        [InlineData(null, "", "")]
        public void Apply_AppliesCountryOfOrigin(string productCountry, string customsCountry, string expectedCountry)
        {
            var testObject = mock.Create<ProductVariant>(
                TypedParameter.From("sku"),
                TypedParameter.From<IProductVariantEntity>(new ProductVariantEntity() { IsActive = true, CountryOfOrigin = productCountry }));

            var item = new ShipmentCustomsItemEntity()
            {
                CountryOfOrigin = customsCountry
            };

            testObject.Apply(item);

            Assert.Equal(expectedCountry, item.CountryOfOrigin);
        }

        [Theory]
        [MemberData(nameof(applyDecimalData))]
        public void Apply_AppliesCustomsWeight(decimal? productWeight, double customsWeight, double expectedWeight)
        {
            var testObject = mock.Create<ProductVariant>(
                TypedParameter.From("sku"),
                TypedParameter.From<IProductVariantEntity>(new ProductVariantEntity() { IsActive = true, Weight = productWeight}));

            var item = new ShipmentCustomsItemEntity()
            {
                Weight = customsWeight
            };

            testObject.Apply(item);

            Assert.Equal(expectedWeight, item.Weight);
        }

        [Theory]
        [MemberData(nameof(applyDecimalData))]
        public void Apply_AppliesCustomsUnitValue(decimal? productUnitValue, decimal customsUnitValue, decimal expectedUnitValue)
        {
            var testObject = mock.Create<ProductVariant>(
                TypedParameter.From("sku"),
                TypedParameter.From<IProductVariantEntity>(new ProductVariantEntity() { IsActive = true, DeclaredValue = productUnitValue }));

            var item = new ShipmentCustomsItemEntity()
            {
                UnitValue = customsUnitValue
            };

            testObject.Apply(item);

            Assert.Equal(expectedUnitValue, item.UnitValue);
        }


        [Theory]
        [MemberData(nameof(applyDecimalData))]
        public void Apply_AppliesCustomsUnitPriceAmount(decimal? productUnitPriceAmount, decimal customsUnitPriceAmount, decimal expectedUnitPriceAmount)
        {
            var testObject = mock.Create<ProductVariant>(
                TypedParameter.From("sku"),
                TypedParameter.From<IProductVariantEntity>(new ProductVariantEntity() { IsActive = true, DeclaredValue = productUnitPriceAmount }));

            var item = new ShipmentCustomsItemEntity()
            {
                UnitPriceAmount = customsUnitPriceAmount
            };

            testObject.Apply(item);

            Assert.Equal(expectedUnitPriceAmount, item.UnitPriceAmount);
        }
        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
