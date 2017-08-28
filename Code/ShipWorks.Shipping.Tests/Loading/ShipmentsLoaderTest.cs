using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using Interapptive.Shared.Threading;
using Moq;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Common.Threading;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Filters;
using ShipWorks.Shipping.Loading;
using ShipWorks.Stores.Content;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Shipping.Tests.Loading
{
    public class ShipmentsLoaderTest
    {
        private readonly AutoMock mock;
        private ShipmentsLoader testObject;
        private readonly OrderEntity orderEntity;
        private readonly ShipmentEntity shipmentEntity;

        public ShipmentsLoaderTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            orderEntity = new OrderEntity(1006)
            {
                StoreID = 1,
                Store = new StoreEntity(1)
            };

            shipmentEntity = new ShipmentEntity(1031)
            {
                Processed = false,
                Order = orderEntity
            };

            mock.Provide<ISchedulerProvider>(new ImmediateSchedulerProvider());

            mock.Mock<IOrderManager>()
                .Setup(x => x.LoadOrders(It.IsAny<IEnumerable<long>>(), It.IsAny<IPrefetchPath2>()))
                .Returns(new[] { orderEntity });
        }

        [Fact]
        public async Task ShipmentsReturned_Correct_WhenOrderHasMultipleShipments_Test()
        {
            var extraShipment = new ShipmentEntity();
            orderEntity.Shipments.Add(extraShipment);

            testObject = mock.Create<ShipmentsLoader>();

            IDictionary<long, ShipmentEntity> globalShipments = new Dictionary<long, ShipmentEntity>();

            await testObject.StartTask(new ProgressProvider(),
                new List<long> { orderEntity.OrderID }, globalShipments, new BlockingCollection<ShipmentEntity>(), false, 15);

            Assert.Equal(2, globalShipments.Count());
            Assert.Contains(extraShipment, globalShipments.Values);
            Assert.Contains(shipmentEntity, globalShipments.Values);
        }

        [Fact]
        public async Task ShipmentsReturned_WhenAutoCreateIsTrueAndHasPermission_Test()
        {
            orderEntity.Shipments.Clear();

            testObject = mock.Create<ShipmentsLoader>();

            await testObject.StartTask(new ProgressProvider(),
                new List<long> { orderEntity.OrderID }, new Dictionary<long, ShipmentEntity>(),
                new BlockingCollection<ShipmentEntity>(), true, 15);

            mock.Mock<IShipmentFactory>()
                .Verify(x => x.AutoCreateIfNecessary(orderEntity, true));
        }

        [Fact]
        public async Task EnsureFilterCountsUpdatedCalled_WhenTimeoutGreaterThanZero_Test()
        {
            orderEntity.Shipments.Clear();

            testObject = mock.Create<ShipmentsLoader>();

            await testObject.StartTask(new ProgressProvider(),
                new List<long> { orderEntity.OrderID }, new Dictionary<long, ShipmentEntity>(),
                new BlockingCollection<ShipmentEntity>(), true, 15);

            mock.Mock<IFilterHelper>()
                .Verify(x => x.EnsureFiltersUpToDate(TimeSpan.FromSeconds(15)), Times.Once);
        }

        [Fact]
        public async Task EnsureFilterCountsUpdatedNotCalled_WhenTimeoutIsZero_Test()
        {
            orderEntity.Shipments.Clear();

            testObject = mock.Create<ShipmentsLoader>();

            await testObject.StartTask(new ProgressProvider(),
                new List<long> { orderEntity.OrderID }, new Dictionary<long, ShipmentEntity>(),
                new BlockingCollection<ShipmentEntity>(), true, 0);

            mock.Mock<IFilterHelper>()
                .Verify(x => x.EnsureFiltersUpToDate(It.IsAny<TimeSpan>()), Times.Never);
        }
    }
}
