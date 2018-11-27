using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extras.Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Tests.Shared;
using Xunit;
using System.Reflection;

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

        [Fact]
        public void CanWriteXml_IsTrue_WhenVariantIsNotNull()
        {
            var testObject = mock.Create<ProductVariant>(
                TypedParameter.From("sku"));

            Assert.True(testObject.CanWriteXml);
        }


        [Obfuscation(Exclude = true)]
        public static IEnumerable<object[]> applyDimData =>
           new List<object[]>
           {
                new object[] { (decimal?) 42, 42 },
                new object[] { (decimal?) 0, 12 },
                new object[] { null, 12 }
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

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
