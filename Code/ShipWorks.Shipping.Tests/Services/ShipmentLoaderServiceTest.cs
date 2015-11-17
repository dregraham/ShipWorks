using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using ShipWorks.Core.Messaging;
using ShipWorks.Core.Messaging.Messages.Shipping;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Messaging.Messages;
using ShipWorks.Shipping.Loading;
using ShipWorks.Shipping.Services;
using Xunit;
using System.Reactive.Linq;

namespace ShipWorks.Shipping.Tests.Services
{
    public class ShipmentLoaderServiceTest
    {
        private readonly ShipmentLoaderService testObject;

        private readonly OrderEntity orderEntity;
        private readonly ShipmentEntity shipmentEntity;

        private readonly Mock<IShipmentLoader> shipmentLoader;
        private readonly IMessenger messenger;

        private readonly Mock<ICarrierShipmentAdapterFactory> shipmentAdapterFactory;
        private readonly Mock<ICarrierShipmentAdapter> shipmentAdapter;
        private OrderSelectionLoaded orderSelectionLoaded;

        public ShipmentLoaderServiceTest()
        {
            orderEntity = new OrderEntity(1006);
            shipmentEntity = new ShipmentEntity(1031)
            {
                ShipmentTypeCode = ShipmentTypeCode.FedEx,
                Order = orderEntity,
            };

            shipmentAdapter = new Mock<ICarrierShipmentAdapter>();
            shipmentAdapter.Setup(s => s.Shipment).Returns(shipmentEntity);

            shipmentAdapterFactory = new Mock<ICarrierShipmentAdapterFactory>();
            shipmentAdapterFactory.Setup(s => s.Get(It.IsAny<ShipmentEntity>())).Returns(shipmentAdapter.Object);

            orderSelectionLoaded = new OrderSelectionLoaded(orderEntity,
                new List<ICarrierShipmentAdapter>() { shipmentAdapterFactory.Object.Get(shipmentEntity) },
                ShippingAddressEditStateType.Editable
                );

            shipmentLoader = new Mock<IShipmentLoader>();
            shipmentLoader.Setup(s => s.Load(orderEntity.OrderID)).Returns(orderSelectionLoaded);

            messenger = new Messenger();
            //messenger.OfType<OrderSelectionChangedMessage>()
            //    .Subscribe(HandleOrderSelectionChangedMessage);

            testObject = new ShipmentLoaderService(shipmentLoader.Object, messenger);
        }

        [Fact]
        public async void MessageCorrect_WhenOrderHasOneShipment_ReturnsThatShipment_Test()
        {
            OrderSelectionLoaded handlededOrderSelectionLoaded = new OrderSelectionLoaded();

            messenger.OfType<OrderSelectionChangedMessage>()
                .Subscribe((OrderSelectionChangedMessage orderSelectionChangedMessage) => handlededOrderSelectionLoaded = orderSelectionChangedMessage.LoadedOrderSelection.FirstOrDefault());

            await testObject.LoadAndNotify(new List<long> { orderEntity.OrderID } );

            Assert.Equal(shipmentEntity.ShipmentID, handlededOrderSelectionLoaded.ShipmentAdapters.FirstOrDefault().Shipment.ShipmentID);
            Assert.Equal(1, handlededOrderSelectionLoaded.ShipmentAdapters.Count());
            Assert.Equal(orderEntity, handlededOrderSelectionLoaded.Order);
        }

        [Fact]
        public async void MessageCorrect_WhenOrderHasMoreThanOneShipment_ReturnsFirstShipment_Test()
        {
            ShipmentEntity secondShipmentEntity = new ShipmentEntity(2031);
            orderSelectionLoaded = new OrderSelectionLoaded(orderEntity, new List<ICarrierShipmentAdapter>()
                {
                    shipmentAdapterFactory.Object.Get(shipmentEntity),
                    shipmentAdapterFactory.Object.Get(secondShipmentEntity)
                },
                ShippingAddressEditStateType.Editable);

            shipmentLoader.Setup(s => s.Load(It.IsAny<long>())).Returns(orderSelectionLoaded);

            await testObject.LoadAndNotify(new List<long> { orderEntity.OrderID });

            Assert.Equal(shipmentEntity.ShipmentID, orderSelectionLoaded.ShipmentAdapters.FirstOrDefault().Shipment.ShipmentID);
            Assert.Equal(2, orderSelectionLoaded.ShipmentAdapters.Count());
            Assert.Equal(orderEntity, orderSelectionLoaded.Order);
        }
    }
}
