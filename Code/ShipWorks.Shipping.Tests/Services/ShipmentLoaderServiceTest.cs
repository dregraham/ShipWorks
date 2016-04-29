using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Moq;
using ShipWorks.Core.Messaging;
using ShipWorks.Core.Messaging.Messages.Shipping;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Loading;
using ShipWorks.Shipping.Services;
using ShipWorks.Tests.Shared;
using Xunit;

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
        private LoadedOrderSelection orderSelectionLoaded;

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

            orderSelectionLoaded = new LoadedOrderSelection(orderEntity,
                new List<ICarrierShipmentAdapter>() { shipmentAdapterFactory.Object.Get(shipmentEntity) },
                ShippingAddressEditStateType.Editable
                );

            shipmentLoader = new Mock<IShipmentLoader>();
            shipmentLoader.Setup(s => s.Load(orderEntity.OrderID)).ReturnsAsync(orderSelectionLoaded);

            messenger = new Messenger();

            testObject = new ShipmentLoaderService(shipmentLoader.Object, messenger, new ImmediateSchedulerProvider());
        }

        [Fact]
        public async Task MessageCorrect_WhenOrderHasOneShipment_ReturnsThatShipment_Test()
        {
            LoadedOrderSelection handlededOrderSelectionLoaded = (await testObject.LoadAndNotify(new List<long> { orderEntity.OrderID })).OfType<LoadedOrderSelection>().Single();

            Assert.Equal(shipmentEntity.ShipmentID, handlededOrderSelectionLoaded.ShipmentAdapters.FirstOrDefault().Shipment.ShipmentID);
            Assert.Equal(1, handlededOrderSelectionLoaded.ShipmentAdapters.Count());
            Assert.Equal(orderEntity, handlededOrderSelectionLoaded.Order);
        }

        [Fact]
        public async Task MessageCorrect_WhenOrderHasMoreThanOneShipment_ReturnsFirstShipment_Test()
        {
            ShipmentEntity secondShipmentEntity = new ShipmentEntity(2031);
            orderSelectionLoaded = new LoadedOrderSelection(orderEntity, new List<ICarrierShipmentAdapter>()
                {
                    shipmentAdapterFactory.Object.Get(shipmentEntity),
                    shipmentAdapterFactory.Object.Get(secondShipmentEntity)
                },
                ShippingAddressEditStateType.Editable);

            shipmentLoader.Setup(s => s.Load(It.IsAny<long>())).ReturnsAsync(orderSelectionLoaded);

            await testObject.LoadAndNotify(new List<long> { orderEntity.OrderID });

            Assert.Equal(shipmentEntity.ShipmentID, orderSelectionLoaded.ShipmentAdapters.FirstOrDefault().Shipment.ShipmentID);
            Assert.Equal(2, orderSelectionLoaded.ShipmentAdapters.Count());
            Assert.Equal(orderEntity, orderSelectionLoaded.Order);
        }
    }
}
