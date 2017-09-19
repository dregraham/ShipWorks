using System;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using log4net;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.BigCommerce;
using ShipWorks.Stores.Platforms.BigCommerce.OnlineUpdating;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.BigCommerceOnlineUpdating
{
    public class OrderStatusUpdaterTest : IDisposable
    {
        private readonly AutoMock mock;
        private readonly BigCommerceOrderStatusUpdater testObject;
        private readonly BigCommerceStoreEntity store;

        public OrderStatusUpdaterTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            store = new BigCommerceStoreEntity { StoreTypeCode = StoreTypeCode.BigCommerce };
            testObject = mock.Create<BigCommerceOrderStatusUpdater>();
        }

        [Fact]
        public async Task UpdateOrderStatus_DelegatesToDataAccess_ToGetUnitOfWork()
        {
            await testObject.UpdateOrderStatus(store, 1006, 1);

            mock.Mock<IBigCommerceDataAccess>()
                .Verify(x => x.GetUnitOfWork());
        }

        [Fact]
        public async Task UpdateOrderStatus_DelegatesToDataAccess_ToGetOrderDetails()
        {
            await testObject.UpdateOrderStatus(store, 1006, 1);

            mock.Mock<IBigCommerceDataAccess>()
                .Verify(x => x.GetOrderDetailsAsync(1006));
        }

        [Fact]
        public async Task UpdateOrderStatus_LogsWarning_WhenOrderDetailsCannotBeFound()
        {
            mock.Mock<IBigCommerceDataAccess>()
                .Setup(x => x.GetOrderDetailsAsync(It.IsAny<long>()))
                .ReturnsAsync((BigCommerceOnlineOrder) null);

            await testObject.UpdateOrderStatus(store, 1006, 1);

            mock.Mock<ILog>()
                .Verify(x => x.WarnFormat(It.Is<string>(m => m.Contains("cannot find order")), 1006L));
        }

        [Fact]
        public async Task UpdateOrderStatus_LogsInfo_WhenOrderIsManual()
        {
            mock.Mock<IBigCommerceDataAccess>()
                .Setup(x => x.GetOrderDetailsAsync(It.IsAny<long>()))
                .ReturnsAsync(CreateOnlineUpdateOrder(1006, true, 1, string.Empty));

            await testObject.UpdateOrderStatus(store, 1006, 1);

            mock.Mock<ILog>()
                .Verify(x => x.InfoFormat(It.Is<string>(m => m.Contains("is manual")), 1006L));
        }

        [Fact]
        public async Task UpdateOrderStatus_DelegatesToWebClient()
        {
            mock.Mock<IBigCommerceDataAccess>()
                .Setup(x => x.GetOrderDetailsAsync(It.IsAny<long>()))
                .ReturnsAsync(CreateOnlineUpdateOrder(1006, false, 123, string.Empty));

            var client = mock.FromFactory<IBigCommerceWebClientFactory>()
                .Mock(x => x.Create(store));

            await testObject.UpdateOrderStatus(store, 1006, 1);

            client.Verify(x => x.UpdateOrderStatus(123, 1));
        }

        [Fact]
        public async Task UpdateOrderStatus_AddsUpdatedOrderToUnitOfWork()
        {
            mock.Mock<IBigCommerceStatusCodeProvider>()
                .Setup(x => x.GetCodeName(1))
                .Returns("Bar");

            var unitOfWork = mock.FromFactory<IBigCommerceDataAccess>()
                .Mock(x => x.GetUnitOfWork());

            mock.Mock<IBigCommerceDataAccess>()
                .Setup(x => x.GetOrderDetailsAsync(It.IsAny<long>()))
                .ReturnsAsync(CreateOnlineUpdateOrder(1006, false, 1, string.Empty));

            await testObject.UpdateOrderStatus(store, 1006, 1);

            unitOfWork.Verify(x => x.AddForSave(It.Is<OrderEntity>(o => o.OrderID == 1006 && (int) o.OnlineStatusCode == 1 && o.OnlineStatus == "Bar")));
        }

        private BigCommerceOnlineOrder CreateOnlineUpdateOrder(int orderID, bool isManual, int orderNumber, string empty) =>
            new BigCommerceOnlineOrder(new BigCommerceOnlineOrderDetails(orderID, isManual, orderNumber, empty));

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
