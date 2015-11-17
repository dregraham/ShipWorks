using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using ShipWorks.Core.Messaging;
using ShipWorks.Core.Messaging.Messages.Shipping;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Filters;
using ShipWorks.Messaging.Messages;
using ShipWorks.Shipping.Configuration;
using ShipWorks.Shipping.Loading;
using ShipWorks.Shipping.Services;
using Xunit;
using System.Reactive.Linq;

namespace ShipWorks.Shipping.Tests.Services
{
    public class ShipmentLoaderServiceTest
    {
        private readonly ShipmentLoaderService testObject;
        private OrderSelectionLoaded orderSelectionLoaded;

        private readonly OrderEntity orderEntity;
        private readonly ShipmentEntity shipmentEntity;

        private readonly Mock<IShipmentLoader> shipmentLoader;
        private readonly IMessenger messenger;

        private Mock<IShippingConfiguration> shippingConfigurator;
        private Mock<IShippingManager> shippingManager;
        private Mock<IFilterHelper> filterHelper;
        private Mock<IValidator<ShipmentEntity>> addressValidator;

        public ShipmentLoaderServiceTest()
        {
            orderEntity = new OrderEntity(1006);
            shipmentEntity = new ShipmentEntity(1031);
            shipmentEntity.Order = orderEntity;

            orderSelectionLoaded = new OrderSelectionLoaded(orderEntity, new List<ShipmentEntity>() { shipmentEntity });

            shipmentLoader = new Mock<IShipmentLoader>();
            shipmentLoader.Setup(s => s.Load(orderEntity.OrderID)).Returns(orderSelectionLoaded);

            messenger = new Messenger();
            messenger.OfType<OrderSelectionChangedMessage>()
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
            orderSelectionLoaded = new OrderSelectionLoaded(orderEntity, new List<ShipmentEntity>() { shipmentEntity, secondShipmentEntity });
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
