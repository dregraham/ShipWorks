using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using log4net;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Stores.Platforms.BigCommerce;
using ShipWorks.Stores.Platforms.BigCommerce.OnlineUpdating;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.BigCommerce.OnlineUpdating
{
    public class ShipmentDetailsUpdaterTest : IDisposable
    {
        private readonly AutoMock mock;
        private readonly ShipmentDetailsUpdater testObject;
        private readonly BigCommerceStoreEntity store;
        private readonly ShipmentEntity shipment;
        private readonly IDictionary<long, IEnumerable<IBigCommerceOrderItemEntity>> allItems;

        public ShipmentDetailsUpdaterTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            store = new BigCommerceStoreEntity { StoreTypeCode = StoreTypeCode.BigCommerce };
            testObject = mock.Create<ShipmentDetailsUpdater>();

            shipment = new ShipmentEntity { ShipmentID = 1031 };
            mock.Mock<IDataAccess>()
                .Setup(x => x.GetLatestActiveShipmentAsync(It.IsAny<long>()))
                .ReturnsAsync(shipment);

            mock.Mock<IDataAccess>()
                .Setup(x => x.GetOrderDetailsAsync(It.IsAny<long>()))
                .ReturnsAsync(CreateOnlineUpdateOrder(1006, false, 123, "123"));

            allItems = CreateOrderDictionary(1006, new[] { new BigCommerceOrderItemEntity() });
            mock.Mock<IDataAccess>()
                .Setup(x => x.GetOrderItemsAsync(It.IsAny<long>()))
                .ReturnsAsync(allItems);
        }

        [Fact]
        public async Task UpdateShipmentDetailsForOrder_ThrowsArgumentNullException_WhenStoreIsNull()
        {
            await Assert.ThrowsAsync<ArgumentNullException>("store", () => testObject.UpdateShipmentDetailsForOrder(null, 1006));
        }

        [Fact]
        public async Task UpdateShipmentDetailsByShipmentID_ThrowsArgumentNullException_WhenStoreIsNull()
        {
            await Assert.ThrowsAsync<ArgumentNullException>("store", () => testObject.UpdateShipmentDetails(null, 1031));
        }

        [Fact]
        public async Task UpdateShipmentDetailsForOrder_DelegatesToDataAccess_ToGetLatestShipment()
        {
            await testObject.UpdateShipmentDetailsForOrder(store, 1006);

            mock.Mock<IDataAccess>()
                .Verify(x => x.GetLatestActiveShipmentAsync(1006L));
        }

        [Fact]
        public async Task UpdateShipmentDetailsForOrder_LogsDebug_WhenShipmentIsNull()
        {
            mock.Mock<IDataAccess>()
                .Setup(x => x.GetLatestActiveShipmentAsync(It.IsAny<long>()))
                .ReturnsAsync((ShipmentEntity) null);

            await testObject.UpdateShipmentDetailsForOrder(store, 1006);

            mock.Mock<ILog>()
                .Verify(x => x.DebugFormat(It.Is<string>(s => s.Contains("no shipment found")), 1006L));
        }

        [Fact]
        public async Task UpdateShipmentDetailsByShipmentID_DelegatesToDataAccess_ToGetShipment()
        {
            await testObject.UpdateShipmentDetails(store, 1031);

            mock.Mock<IDataAccess>()
                .Verify(x => x.GetShipmentAsync(1031L));
        }

        [Fact]
        public async Task UpdateShipmentDetailsByShipmentID_LogsDebug_WhenShipmentIsNull()
        {
            await testObject.UpdateShipmentDetails(store, 1031);

            mock.Mock<ILog>()
                .Verify(x => x.WarnFormat(It.Is<string>(s => s.Contains("it has gone away")), 1031L));
        }

        [Fact]
        public async Task UpdateShipmentDetailsByShipmentID_DelegatesToDataAccess_ToGetOrderDetails()
        {
            mock.Mock<IDataAccess>()
                .Setup(x => x.GetShipmentAsync(1031L))
                .ReturnsAsync(new ShipmentEntity { OrderID = 1006 });

            await testObject.UpdateShipmentDetails(store, 1031);

            mock.Mock<IDataAccess>()
                .Verify(x => x.GetOrderDetailsAsync(1006L));
        }

        [Fact]
        public async Task UpdateShipmentDetails_LogsWarning_WhenOrderDetailIsNull()
        {
            mock.Mock<IDataAccess>()
                .Setup(x => x.GetShipmentAsync(1031L))
                .ReturnsAsync(new ShipmentEntity { OrderID = 1006 });

            mock.Mock<IDataAccess>()
                .Setup(x => x.GetOrderDetailsAsync(1006))
                .ReturnsAsync((OnlineOrder) null);

            await testObject.UpdateShipmentDetails(store, 1031);

            mock.Mock<ILog>()
                .Verify(x => x.WarnFormat(It.Is<string>(s => s.Contains("it has gone away")), 1006L));
        }

        [Fact]
        public async Task UpdateShipmentDetailsForOrder_LogsWarning_WhenOrderIsManual()
        {
            mock.Mock<IDataAccess>()
                .Setup(x => x.GetOrderDetailsAsync(It.IsAny<long>()))
                .ReturnsAsync(CreateOnlineUpdateOrder(1006, true, 123, "123"));

            await testObject.UpdateShipmentDetailsForOrder(store, 1006);

            mock.Mock<ILog>()
                .Verify(x => x.WarnFormat(It.Is<string>(s => s.Contains("it is manual")), "123"));
        }

        [Fact]
        public async Task UpdateShipmentDetailsForOrder_ThrowsBigCommerceException_WhenNoItemsExist()
        {
            allItems.Clear();

            await Assert.ThrowsAsync<BigCommerceException>(() => testObject.UpdateShipmentDetailsForOrder(store, 1006));
        }

        [Fact]
        public async Task UpdateShipmentDetailsForOrder_DelegatesToDataAcces_ToGetOrderItems()
        {
            await testObject.UpdateShipmentDetailsForOrder(store, 1006);

            mock.Mock<IDataAccess>()
                .Verify(x => x.GetOrderItemsAsync(1006L));
        }

        [Fact]
        public async Task UpdateShipmentDetailsForOrder_DoesNotDelegateToUpdateClient_WhenItemsAreAllDigital()
        {
            mock.Mock<IShipmentDetailsUpdaterClient>()
                .Setup(x => x.IsOrderAllDigital(It.IsAny<IEnumerable<IBigCommerceOrderItemEntity>>()))
                .Returns(true);

            await testObject.UpdateShipmentDetailsForOrder(store, 1006);

            mock.Mock<IShipmentDetailsUpdaterClient>()
                .Verify(x => x.UpdateOnline(It.IsAny<IBigCommerceStoreEntity>(),
                        It.IsAny<OnlineOrderDetails>(),
                        It.IsAny<string>(),
                        It.IsAny<ShipmentEntity>(),
                        It.IsAny<IDictionary<long, IEnumerable<IBigCommerceOrderItemEntity>>>()), Times.Never);
        }

        [Fact]
        public async Task UpdateShipmentDetailsForOrder_DelegatesUpdateClient_WhenItemsAreNotDigital()
        {
            await testObject.UpdateShipmentDetailsForOrder(store, 1006);

            mock.Mock<IShipmentDetailsUpdaterClient>()
                .Verify(x => x.UpdateOnline(store, It.IsAny<OnlineOrderDetails>(), "123", shipment, allItems));
        }

        private OnlineOrder CreateOnlineUpdateOrder(int orderID, bool isManual, int orderNumber, string empty) =>
            new OnlineOrder(new OnlineOrderDetails(orderID, isManual, orderNumber, empty));

        private IDictionary<long, IEnumerable<IBigCommerceOrderItemEntity>> CreateOrderDictionary(long orderNumber, params BigCommerceOrderItemEntity[] orderItemEntity) =>
            new Dictionary<long, IEnumerable<IBigCommerceOrderItemEntity>> { { orderNumber, orderItemEntity } };

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
