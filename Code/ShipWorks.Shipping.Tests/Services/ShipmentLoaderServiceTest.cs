using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using ShipWorks.Core.Messaging;
using ShipWorks.Core.Messaging.Messages.Shipping;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Filters;
using ShipWorks.Messaging.Messages;
using ShipWorks.Shipping.Configuration;
using ShipWorks.Shipping.Loading;
using ShipWorks.Shipping.Services;
using ShipWorks.Users.Security;
using Xunit;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using Autofac.Extras.Moq;
using ShipWorks.Tests.Shared;

namespace ShipWorks.Shipping.Tests.Services
{
    public class ShipmentLoaderServiceTest
    {
        private ShipmentLoaderService testObject;
        private OrderSelectionLoaded orderSelectionLoaded;

        private OrderEntity orderEntity;
        private ShipmentEntity shipmentEntity;

        private Mock<IShipmentLoader> shipmentLoader;
        private IMessenger messenger;

        private Mock<IShippingConfiguration> shippingConfigurator;
        private Mock<IShippingManager> shippingManager;
        private Mock<IFilterHelper> filterHelper;
        private Mock<IValidator<ShipmentEntity>> addressValidator;

        public ShipmentLoaderServiceTest()
        {
            orderEntity = new OrderEntity(1006);
            shipmentEntity = new ShipmentEntity(1031);
            shipmentEntity.Order = orderEntity;

            orderSelectionLoaded = new OrderSelectionLoaded(orderEntity, new List<ShipmentEntity>() { shipmentEntity }, true);

            shipmentLoader = new Mock<IShipmentLoader>();
            shipmentLoader.Setup(s => s.Load(orderEntity.OrderID)).Returns(orderSelectionLoaded);

            messenger = new Messenger();
            messenger.AsObservable<OrderSelectionChangedMessage>()
                .Subscribe(HandleOrderSelectionChangedMessage);

            testObject = new ShipmentLoaderService(shipmentLoader.Object, messenger);
        }

        [Fact]
        public async void MessageCorrect_WhenOrderHasOneShipment_ReturnsThatShipment_Test()
        {
            await testObject.LoadAndNotify(new List<long> { orderEntity.OrderID } );

            Assert.Equal(shipmentEntity.ShipmentID, orderSelectionLoaded.Shipments.FirstOrDefault().ShipmentID);
            Assert.Equal(1, orderSelectionLoaded.Shipments.Count());
            Assert.Equal(orderEntity, orderSelectionLoaded.Order);
        }

        [Fact]
        public async void MessageCorrect_WhenOrderHasMoreThanOneShipment_ReturnsFirstShipment_Test()
        {
            ShipmentEntity secondShipmentEntity = new ShipmentEntity(2031);
            orderSelectionLoaded = new OrderSelectionLoaded(orderEntity, new List<ShipmentEntity>() { shipmentEntity, secondShipmentEntity }, true);
            shipmentLoader.Setup(s => s.Load(It.IsAny<long>())).Returns(orderSelectionLoaded);

            await testObject.LoadAndNotify(new List<long> { orderEntity.OrderID });

            Assert.Equal(shipmentEntity.ShipmentID, orderSelectionLoaded.Shipments.FirstOrDefault().ShipmentID);
            Assert.Equal(2, orderSelectionLoaded.Shipments.Count());
            Assert.Equal(orderEntity, orderSelectionLoaded.Order);
        }

        public void HandleOrderSelectionChangedMessage(OrderSelectionChangedMessage orderSelectionChangedMessage)
        {
            orderSelectionLoaded = orderSelectionChangedMessage.LoadedOrderSelection.FirstOrDefault();
        }
    }
}
