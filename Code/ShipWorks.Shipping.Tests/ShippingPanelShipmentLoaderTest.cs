using Moq;
using ShipWorks.Data.Model.EntityClasses;
using Xunit;
using ShipWorks.Shipping;
using ShipWorks.Core.Common.Threading;

namespace ShipWorks.Tests.Shipping
{
    public class ShippingPanelShipmentLoaderTest
    {
        private ShippingPanelShipmentLoader testObject;
        private OrderEntity orderEntity;
        private ShipmentEntity shipmentEntity;
        private Mock<IShipmentLoader> shipmentLoader;
        private Mock<IValidator<ShipmentEntity>> validator;

        public ShippingPanelShipmentLoaderTest()
        {
            orderEntity = new OrderEntity(1006);
            shipmentEntity = new ShipmentEntity(1031);
            shipmentEntity.Order = orderEntity;
            
            ShippingPanelLoadedShipment shippingPanelLoadedShipment = new ShippingPanelLoadedShipment()
            {
                Shipment = shipmentEntity,
                Result = ShippingPanelLoadedShipmentResult.Success,
                Exception = null
            };

            shipmentLoader = new Mock<IShipmentLoader>();
            shipmentLoader.Setup(s => s.Load(It.IsAny<long>())).Returns(shippingPanelLoadedShipment);
            
            validator = new Mock<IValidator<ShipmentEntity>>();
            validator.Setup(s => s.ValidateAsync(It.IsAny<ShipmentEntity>())).Returns(TaskUtility.CompletedTask);

            testObject = new ShippingPanelShipmentLoader(shipmentLoader.Object, validator.Object);
        }

        [Fact]
        public async void ShipmentAndSuccess_WhenOrderHasOneShipment_ReturnsThatShipment_Test()
        {
            ShippingPanelLoadedShipment shipmentPanelLoadedShipment = await testObject.LoadAsync(orderEntity.OrderID);

            Assert.Equal(shipmentEntity.ShipmentID, shipmentPanelLoadedShipment.Shipment.ShipmentID);
            Assert.Equal(ShippingPanelLoadedShipmentResult.Success, shipmentPanelLoadedShipment.Result);
        }
        
    }
}
