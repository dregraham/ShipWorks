using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using Moq;
using RestSharp;
using ShipWorks.ApplicationCore.Licensing.Warehouse;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Products.Warehouse;
using ShipWorks.Products.Warehouse.DTO;
using ShipWorks.Tests.Shared;
using Xunit;
using static ShipWorks.Tests.Shared.ExtensionMethods.ParameterShorteners;

namespace ShipWorks.Products.Tests.Warehouse
{
    public class WarehouseProductClientTest
    {
        private readonly AutoMock mock;
        private readonly WarehouseProductClient testObject;

        public WarehouseProductClientTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            testObject = mock.Create<WarehouseProductClient>();
        }

        [Fact]
        public async Task AddProduct_DelegatesToWarehouseProductRequestFactory()
        {
            var product = new ProductVariantEntity
            {
                Product = new ProductEntity()
            };
            var payload = mock.Build<IWarehouseProductRequestData>();
            mock.Mock<IWarehouseProductDataFactory>()
                .Setup(x => x.CreateAddProductRequest(product))
                .Returns(payload);

            await testObject.AddProduct(product);

            mock.Mock<IWarehouseProductRequestFactory>()
                .Verify(x => x.Create("api/products", Method.PUT, payload));
        }

        [Fact]
        public async Task AddProduct_DelegatesToWarehouseRequestClient()
        {
            var request = mock.Build<IRestRequest>();
            mock.Mock<IWarehouseProductRequestFactory>()
                .Setup(x => x.Create(AnyString, It.IsAny<Method>(), It.IsAny<IWarehouseProductRequestData>()))
                .Returns(request);

            await testObject.AddProduct(It.IsAny<ProductVariantEntity>());

            mock.Mock<IWarehouseRequestClient>()
                .Verify(x => x.MakeRequest<AddProductResponseData>(request, "Add Product"));
        }

        [Fact]
        public async Task AddProduct_DelegatesToDataFactory_ToCreateResult()
        {
            var response = new AddProductResponseData();
            mock.Mock<IWarehouseRequestClient>()
                .Setup(x => x.MakeRequest<AddProductResponseData>(It.IsAny<IRestRequest>(), AnyString))
                .ReturnsAsync(response);

            await testObject.AddProduct(It.IsAny<ProductVariantEntity>());

            mock.Mock<IWarehouseProductDataFactory>()
                .Verify(x => x.CreateAddProductResult(response));
        }

        [Fact]
        public async Task AddProduct_ReturnsResult()
        {
            var result = mock.Build<IProductChangeResult>();
            mock.Mock<IWarehouseProductDataFactory>()
                .Setup(x => x.CreateAddProductResult(It.IsAny<AddProductResponseData>()))
                .Returns(result);

            var response = await testObject.AddProduct(It.IsAny<ProductVariantEntity>());

            Assert.Equal(result, response);
        }

        [Fact]
        public async Task ChangeProduct_DelegatesToWarehouseProductRequestFactory()
        {
            var product = new ProductVariantEntity
            {
                Product = new ProductEntity(),
                HubProductId = Guid.Parse("738227A1-613E-4128-ABE9-D525CE843F8A")
            };

            var payload = mock.Build<IWarehouseProductRequestData>();
            mock.Mock<IWarehouseProductDataFactory>()
                .Setup(x => x.CreateChangeProductRequest(product))
                .Returns(payload);

            await testObject.ChangeProduct(product);

            mock.Mock<IWarehouseProductRequestFactory>()
                .Verify(x => x.Create("api/product/738227a1-613e-4128-abe9-d525ce843f8a", Method.POST, payload));
        }

        [Fact]
        public async Task ChangeProduct_DelegatesToWarehouseRequestClient()
        {
            var request = mock.Build<IRestRequest>();
            mock.Mock<IWarehouseProductRequestFactory>()
                .Setup(x => x.Create(AnyString, It.IsAny<Method>(), It.IsAny<IWarehouseProductRequestData>()))
                .Returns(request);

            await testObject.ChangeProduct(new ProductVariantEntity { HubProductId = new Guid() });

            mock.Mock<IWarehouseRequestClient>()
                .Verify(x => x.MakeRequest<ChangeProductResponseData>(request, "Change Product"));
        }

        [Fact]
        public async Task ChangeProduct_DelegatesToDataFactory_ToCreateResult()
        {
            var response = new ChangeProductResponseData();
            mock.Mock<IWarehouseRequestClient>()
                .Setup(x => x.MakeRequest<ChangeProductResponseData>(It.IsAny<IRestRequest>(), AnyString))
                .ReturnsAsync(response);

            await testObject.ChangeProduct(new ProductVariantEntity { HubProductId = new Guid() });

            mock.Mock<IWarehouseProductDataFactory>()
                .Verify(x => x.CreateChangeProductResult(response));
        }

        [Fact]
        public async Task ChangeProduct_ReturnsResult()
        {
            var result = mock.Build<IProductChangeResult>();
            mock.Mock<IWarehouseProductDataFactory>()
                .Setup(x => x.CreateChangeProductResult(It.IsAny<ChangeProductResponseData>()))
                .Returns(result);

            var response = await testObject.ChangeProduct(new ProductVariantEntity { HubProductId = new Guid() });

            Assert.Equal(result, response);
        }

        [Fact]
        public async Task SetActivation_ThrowsException_WhenAtLeastOneGuidIsNull()
        {
            await Assert.ThrowsAsync<WarehouseProductException>(() => 
                testObject.SetActivation(new Guid?[] { Guid.NewGuid(), null }, true));
        }

        [Fact]
        public async Task SetActivation_DelegatesToWarehouseProductRequestFactory()
        {
            var guid1 = Guid.NewGuid();
            var guid2 = Guid.NewGuid();

            var payload = mock.Build<IWarehouseProductRequestData>();
            mock.Mock<IWarehouseProductDataFactory>()
                .Setup(x => x.CreateSetActivationRequest(It.Is<IEnumerable<Guid>>(g => g.SequenceEqual(new Guid[] { guid1, guid2 })), true))
                .Returns(payload);

            await testObject.SetActivation(new Guid?[] { guid1, guid2 }, true);

            mock.Mock<IWarehouseProductRequestFactory>()
                .Verify(x => x.Create("api/products/activation", Method.POST, payload));
        }

        [Fact]
        public async Task SetActivation_DelegatesToWarehouseRequestClient()
        {
            var request = mock.Build<IRestRequest>();
            mock.Mock<IWarehouseProductRequestFactory>()
                .Setup(x => x.Create(AnyString, It.IsAny<Method>(), It.IsAny<IWarehouseProductRequestData>()))
                .Returns(request);

            await testObject.SetActivation(new Guid?[] { new Guid() }, true);

            mock.Mock<IWarehouseRequestClient>()
                .Verify(x => x.MakeRequest<SetActivationBulkResponseData>(request, "Set Activation"));
        }

        [Fact]
        public async Task SetActivation_DelegatesToDataFactory_ToCreateResult()
        {
            var response = new SetActivationBulkResponseData();
            mock.Mock<IWarehouseRequestClient>()
                .Setup(x => x.MakeRequest<SetActivationBulkResponseData>(It.IsAny<IRestRequest>(), AnyString))
                .ReturnsAsync(response);

            await testObject.SetActivation(new Guid?[] { new Guid() }, true);

            mock.Mock<IWarehouseProductDataFactory>()
                .Verify(x => x.CreateSetActivationResult(response));
        }

        [Fact]
        public async Task SetActivation_ReturnsResult()
        {
            var result = mock.Build<IProductsChangeResult>();
            mock.Mock<IWarehouseProductDataFactory>()
                .Setup(x => x.CreateSetActivationResult(It.IsAny<SetActivationBulkResponseData>()))
                .Returns(result);

            var response = await testObject.SetActivation(new Guid?[] { new Guid() }, true);

            Assert.Equal(result, response);
        }

        [Fact]
        public async Task GetProduct_ReturnsResult()
        {
            var payload = mock.Build<IWarehouseProductRequestData>();
            mock.Mock<IWarehouseProductDataFactory>()
                .Setup(x => x.CreateGetProductRequest(It.IsAny<string>()))
                .Returns(payload);

            mock.Mock<IWarehouseRequestClient>()
                .Setup(x => x.MakeRequest<WarehouseProduct>(It.IsAny<IRestRequest>(), AnyString))
                .ReturnsAsync(new WarehouseProduct() { ProductId = "3"});

            var response = await testObject.GetProduct(It.IsAny<string>(), It.IsAny<CancellationToken>());

            Assert.NotNull(response);
            Assert.Equal("3", response.ProductId);
        }
    }
}
