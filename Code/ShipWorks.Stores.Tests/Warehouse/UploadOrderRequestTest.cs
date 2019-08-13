using System.Linq;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using Moq;
using RestSharp;
using ShipWorks.ApplicationCore.Licensing.Warehouse;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Warehouse;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Stores.Tests.Warehouse
{
    public class UploadOrderRequestTest
    {
        private readonly AutoMock mock;
        private readonly UploadOrdersRequest testObject;
        private readonly Mock<IWarehouseRequestClient> requestClient;
        private readonly Mock<IWarehouseOrderDtoFactory> dtoFactory;

        public UploadOrderRequestTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            requestClient = mock.Mock<IWarehouseRequestClient>();
            dtoFactory = mock.Mock<IWarehouseOrderDtoFactory>();

            testObject = mock.Create<UploadOrdersRequest>();
        }

        [Fact]
        public async Task Submit_DelegatesToWarehouseRequestClient()
        {
            var store = new StoreEntity();
            var orders = new[] { new OrderEntity() };

            await testObject.Submit(orders, store);

            requestClient.Verify(r => r.MakeRequest(It.IsAny<IRestRequest>(), "Upload Order"));
        }

        [Fact]
        public async Task Submit_DelegatesToWarehouseOrderDtoFactory()
        {
            var store = new StoreEntity();
            var orders = new[] { new OrderEntity() };

            await testObject.Submit(orders, store);

            dtoFactory.Verify(d => d.Create(orders.First(), store));
        }
    }
}
