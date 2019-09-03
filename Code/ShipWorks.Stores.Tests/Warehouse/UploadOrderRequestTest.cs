using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using Moq;
using RestSharp;
using ShipWorks.ApplicationCore.Licensing.Warehouse;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Odbc.Download;
using ShipWorks.Stores.Warehouse;
using ShipWorks.Tests.Shared;
using ShipWorks.Warehouse.DTO.Orders;
using Xunit;

namespace ShipWorks.Stores.Tests.Warehouse
{
    public class UploadOrderRequestTest
    {
        private readonly AutoMock mock;
        private readonly UploadOrdersRequest testObject;
        private readonly Mock<IWarehouseRequestClient> requestClient;
        private readonly Mock<IWarehouseOrderDtoFactory> dtoFactory;
        private readonly OdbcStoreEntity store;
        private readonly OrderEntity[] orders;

        public UploadOrderRequestTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            requestClient = mock.Mock<IWarehouseRequestClient>();
            dtoFactory = mock.Mock<IWarehouseOrderDtoFactory>();

            store = new OdbcStoreEntity();
            orders = new[] { new OrderEntity() };

            testObject = mock.Create<UploadOrdersRequest>();
        }

        [Fact]
        public async Task Submit_DelegatesToWarehouseRequestClient()
        {
            await testObject.Submit(orders, store, false);

            requestClient.Verify(r => r.MakeRequest(It.IsAny<IRestRequest>(), "Upload Order"));
        }

        [Fact]
        public async Task Submit_DelegatesToWarehouseOrderDtoFactory()
        {
            await testObject.Submit(orders, store, false);

            dtoFactory.Verify(d => d.Create(orders.First(), store));
        }

        [Fact]
        public async Task Submit_AddsSelfToRequest()
        {
            await testObject.Submit(orders, store, false);

            mock.Mock<IRestRequest>().Verify(r => r.AddJsonBody(testObject), Times.Once);
        }

        [Fact]
        public async Task Submit_BatchIsPopulated_WhenAssignBatchIsTrue()
        {
            Assert.Null(testObject.Batch);
            
            await testObject.Submit(orders, store, true);

            Assert.NotNull(testObject.Batch);
        }

        [Fact]
        public async Task Submit_BatchIsNull_WhenAssignBatchIsFalse()
        {
            Assert.Null(testObject.Batch);
            
            await testObject.Submit(orders, store, false);

            Assert.Null(testObject.Batch);
        }

        [Fact]
        public async Task Submit_OrdersArePopulated()
        {
            Assert.Null(testObject.Orders);

            var warehouseOrder = new WarehouseOrder();

            dtoFactory.Setup(f => f.Create(orders.Single(), store)).Returns(warehouseOrder);

            await testObject.Submit(orders, store, true);

            Assert.Same(warehouseOrder, testObject.Orders.Single());
        }
    }
}
