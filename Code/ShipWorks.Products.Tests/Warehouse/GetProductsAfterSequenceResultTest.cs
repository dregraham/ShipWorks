using System;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extras.Moq;
using ShipWorks.Data.Connection;
using ShipWorks.Products.Warehouse;
using ShipWorks.Products.Warehouse.DTO;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Products.Tests.Warehouse
{
    public class GetProductsAfterSequenceResultTest
    {
        private readonly AutoMock mock;

        public GetProductsAfterSequenceResultTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
        }

        [Fact]
        public async Task Apply_DelegatesToProductCatalog_ToGetExistingProducts()
        {
            var sqlAdapter = mock.Build<ISqlAdapter>();
            var data = new GetProductsAfterSequenceResponseData
            {
                Products = new[]
                {
                    new WarehouseProduct { ProductId = "fd8d83c3141d40a6915fa1342e5def13" },
                    new WarehouseProduct { ProductId = "fd8d83c3141d40a6915fa1342e5def14" }
                }
            };
            var testObject = mock.Create<GetProductsAfterSequenceResult>(TypedParameter.From(data));

            await testObject.Apply(sqlAdapter, CancellationToken.None);

            mock.Mock<IProductCatalog>()
                .Verify(x => x.GetProductsByHubIds(sqlAdapter, new[] {
                    Guid.Parse("fd8d83c3141d40a6915fa1342e5def13"),
                    Guid.Parse("fd8d83c3141d40a6915fa1342e5def14")
                }));
        }
    }
}
