using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using Interapptive.Shared.Threading;
using Moq;
using ShipWorks.Core.Messaging;
using ShipWorks.Core.Messaging.Messages.Shipping;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Messaging.Messages;
using ShipWorks.Shipping.Services;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Shipping.Tests.Services
{
    public class ShipmentLoaderServiceTest
    {
        private readonly AutoMock mock;
        private readonly ShipmentLoaderService testObject;
        private readonly TestMessenger messenger;

        ShipmentEntity shipment = new ShipmentEntity
        {
            Order = new OrderEntity
            {
                Store = new StoreEntity()
            }
        };

        public ShipmentLoaderServiceTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            mock.Provide<ISchedulerProvider>(new ImmediateSchedulerProvider());

            messenger = new TestMessenger();
            mock.Provide<IMessenger>(messenger);

            mock.Mock<IShipmentsLoader>()
                .Setup(x => x.LoadAsync(It.IsAny<IEnumerable<long>>()))
                .ReturnsAsync(new ShipmentsLoadedEventArgs(null, false, null, new[] { shipment }.ToList()));

            testObject = mock.Create<ShipmentLoaderService>();
        }

        [Fact]
        public async Task Load_DelegatesToShipmentAdapterFactory_WhenShipmentsLoaded()
        {
            LoadedOrderSelection handlededOrderSelectionLoaded = (await testObject.Load(new[] { 1L }))
                .OfType<LoadedOrderSelection>().Single();

            mock.Mock<ICarrierShipmentAdapterFactory>()
                .Verify(x => x.Get(shipment));
        }

        [Fact]
        public async Task Load_ReturnsOrderAndCarrierAdapter_WhenShipmentsLoaded()
        {
            var adapter = mock.Create<ICarrierShipmentAdapter>();

            mock.Mock<ICarrierShipmentAdapterFactory>()
                .Setup(x => x.Get(It.IsAny<ShipmentEntity>()))
                .Returns(adapter);

            LoadedOrderSelection result = (await testObject.Load(new[] { 1L }))
                .OfType<LoadedOrderSelection>().Single();

            Assert.Equal(shipment.Order, result.Order);
            Assert.Contains(adapter, result.ShipmentAdapters);
        }

        [Fact]
        public void Initialize_SendsMessage_WhenSelectionIsChanging()
        {
            bool wasCalled = false;

            testObject.InitializeForCurrentSession();
            messenger.OfType<OrderSelectionChangedMessage>()
                .Subscribe(x => wasCalled = true);

            messenger.Send(new OrderSelectionChangingMessage(this, new[] { 3L }));

            Assert.True(wasCalled);
        }
    }
}
