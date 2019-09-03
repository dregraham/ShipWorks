using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using Interapptive.Shared.Utility;
using Moq;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Editions;
using ShipWorks.Stores.Warehouse;
using ShipWorks.Tests.Shared;
using ShipWorks.Warehouse.DTO.Orders;
using Xunit;

namespace ShipWorks.Stores.Tests.Warehouse
{
    public class WarehouseOrderClientTest : IDisposable
    {
        private readonly AutoMock mock;
        private readonly Mock<ILicenseService> licenseService;
        private EditionRestrictionLevel warehouseRestriction;
        private readonly Mock<IStoreEntity> store;
        private readonly List<OrderEntity> orders;
        private readonly WarehouseOrderClient testObject;
        private readonly Mock<IUploadOrdersRequest> uploadOrdersRequest;

        public WarehouseOrderClientTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            licenseService = mock.Mock<ILicenseService>();

            warehouseRestriction = EditionRestrictionLevel.None;
            licenseService.Setup(s => s.CheckRestriction(EditionFeature.Warehouse, null))
                .Returns(() => warehouseRestriction);

            store = mock.CreateMock<IStoreEntity>();
            orders = new List<OrderEntity>();

            uploadOrdersRequest = mock.Mock<IUploadOrdersRequest>();
            testObject = mock.Create<WarehouseOrderClient>();
        }

        [Fact]
        public async Task UploadOrders_SubmitsOrders_WhenWarehouseNotRestricted()
        {
            await testObject.UploadOrders(orders, store.Object, true);

            uploadOrdersRequest.Verify(r => r.Submit(orders, store.Object, true), Times.Once);
        }

        [Fact]
        public async Task UploadOrders_ReturnsResponseFromUploadOrdersRequest()
        {
            uploadOrdersRequest
                .Setup(r => r.Submit(orders, store.Object, true))
                .ReturnsAsync(GenericResult.FromSuccess<IEnumerable<WarehouseUploadOrderResponse>>(new List<WarehouseUploadOrderResponse>(), "expected message"));

            var actualResult = await testObject.UploadOrders(orders, store.Object, true);

            Assert.Equal("expected message", actualResult.Message);
        }

        [Fact]
        public async Task UploadOrders_DoesNotSubmitOrders_WhenWarehouseRestricted()
        {
            warehouseRestriction = EditionRestrictionLevel.Forbidden;

            await testObject.UploadOrders(orders, store.Object, true);

            uploadOrdersRequest.Verify(r => r.Submit(It.IsAny<IEnumerable<OrderEntity>>(), It.IsAny<IStoreEntity>(), true), Times.Never);
        }

        [Fact]
        public async Task UploadOrders_ReturnsRestrictedMessage_WhenWarehouseRestricted()
        {
            warehouseRestriction = EditionRestrictionLevel.Forbidden;

            var result = await testObject.UploadOrders(orders, store.Object, true);

            Assert.False(result.Success);
            Assert.Equal(WarehouseOrderClient.RestrictedErrorMessage, result.Message);
        }

        [Fact]
        public async Task UploadOrders_ReturnsFailure_WhenUploadOrderREquestCreatorThrows()
        {
            string message = "exception message";
            uploadOrdersRequest
                .Setup(r => r.Submit(It.IsAny<IEnumerable<OrderEntity>>(), It.IsAny<IStoreEntity>(), false))
                .Throws(new Exception(message));

            var result = await testObject.UploadOrders(orders, store.Object, false);

            Assert.False(result.Success);
            Assert.Equal(message, result.Exception.Message);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
