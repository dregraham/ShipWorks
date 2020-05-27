using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using Interapptive.Shared.Utility;
using Moq;
using SD.LLBLGen.Pro.QuerySpec;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.Custom;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Products.Warehouse;
using ShipWorks.Tests.Shared;
using Xunit;
using static ShipWorks.Tests.Shared.ExtensionMethods.ParameterShorteners;

namespace ShipWorks.Products.Tests
{
    public class ProductCatalogTest
    {
        private static readonly Guid? variant1HubId = Guid.Parse("{00000000-0000-0000-0000-000000000001}");
        private static readonly Guid? variant2HubId = Guid.Parse("{00000000-0000-0000-0000-000000000002}");
        private readonly AutoMock mock;
        private readonly ProductCatalog testObject;
        private readonly ProductVariantEntity variant1 = new ProductVariantEntity
        {
            HubProductId = variant1HubId,
            Product = new ProductEntity()
        };
        private readonly ProductVariantEntity variant2 = new ProductVariantEntity
        {
            HubProductId = variant2HubId,
            Product = new ProductEntity()
        };

        public ProductCatalogTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            mock.FromFactory<ISqlAdapterFactory>()
                .Mock(x => x.Create(It.IsAny<DbConnection>()))
                .Setup(x => x.FetchQueryAsync(It.IsAny<EntityQuery<ProductVariantEntity>>()))
                .ReturnsAsync(new[] { variant1, variant2 }.ToEntityCollection());

            mock.Mock<IWarehouseProductClient>()
                .Setup(x => x.SetActivation(It.IsAny<IEnumerable<Guid?>>(), AnyBool))
                .ReturnsAsync(mock.Mock<IProductsChangeResult>().Object);

            mock.Mock<IProductValidator>()
                .Setup(x => x.Validate(It.IsAny<ProductVariantEntity>(), It.IsAny<IProductCatalog>()))
                .ReturnsAsync(Result.FromSuccess());

            testObject = mock.Create<ProductCatalog>();
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task SetActivation_DelegatesToWarehouseClient(bool activated)
        {
            await testObject.SetActivation(new[] { 1L, 2L }, activated);

            mock.Mock<IWarehouseProductClient>()
                .Verify(x => x.SetActivation(new[] { variant1HubId, variant2HubId }, activated));
        }

        [Fact]
        public async Task SetActivation_AppliesWarehouseResult_ToVariants()
        {
            await testObject.SetActivation(new[] { 1L, 2L }, true);

            mock.Mock<IProductsChangeResult>()
                .Verify(x => x.ApplyTo(new[] { variant1, variant2 }));
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task SetActivation_SetsActivation_OnVariants(bool activated)
        {
            await testObject.SetActivation(new[] { 1L, 2L }, activated);

            Assert.Equal(activated, variant1.IsActive);
            Assert.Equal(activated, variant2.IsActive);
        }

        [Fact]
        public async Task Save_DelegatesToProductValidator()
        {
            await testObject.Save(variant1);

            mock.Mock<IProductValidator>()
                .Verify(x => x.Validate(variant1, testObject));
        }

        [Fact]
        public async Task Save_ReturnsValidationErrors_WhenValidationFails()
        {
            var validationResult = Result.FromError("Nope");
            mock.Mock<IProductValidator>()
                .Setup(x => x.Validate(It.IsAny<ProductVariantEntity>(), It.IsAny<IProductCatalog>()))
                .ReturnsAsync(validationResult);

            var result = await testObject.Save(variant1);

            Assert.Equal(validationResult, result);
        }

        [Fact]
        public async Task Save_DelegatesToWarehouseProductClient_WhenVariantIsNew()
        {
            variant1.IsNew = true;

            await testObject.Save(variant1);

            mock.Mock<IWarehouseProductClient>()
                .Verify(x => x.AddProduct(variant1));
        }

        [Fact]
        public async Task Save_DelegatesToWarehouseProductClient_WhenVariantIsNotNewButDoesNotHaveHubId()
        {
            variant1.IsNew = false;
            variant1.HubProductId = null;

            await testObject.Save(variant1);

            mock.Mock<IWarehouseProductClient>()
                .Verify(x => x.AddProduct(variant1));
        }

        [Fact]
        public async Task Save_DelegatesToWarehouseProductClient_WhenVariantExistsAndHasHubId()
        {
            variant1.IsNew = false;
            variant1.HubProductId = variant1HubId;

            await testObject.Save(variant1);

            mock.Mock<IWarehouseProductClient>()
                .Verify(x => x.ChangeProduct(variant1));
        }

        [Fact]
        public async Task Save_AppliesWarehouseResult_ToVariant()
        {
            variant1.IsNew = true;
            mock.Mock<IWarehouseProductClient>()
                .Setup(x => x.AddProduct(It.IsAny<ProductVariantEntity>()))
                .ReturnsAsync(mock.Mock<IProductChangeResult>().Object);

            await testObject.Save(variant1);

            mock.Mock<IProductChangeResult>()
                .Verify(x => x.ApplyTo(variant1));
        }
    }
}
