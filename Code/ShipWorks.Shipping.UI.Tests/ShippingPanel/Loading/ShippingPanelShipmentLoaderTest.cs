using Moq;
using ShipWorks.Core.Common.Threading;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.UI.ShippingPanel.Loading;
using Xunit;

namespace ShipWorks.Shipping.UI.Tests.ShippingPanel.Loading
{
    public class ShippingPanelShipmentLoaderTest
    {
        private readonly ShippingPanelShipmentLoader testObject;
        private readonly OrderEntity orderEntity;
        private readonly ShipmentEntity shipmentEntity;
        private readonly Mock<IShipmentLoader> shipmentLoader;
        private readonly Mock<IValidator<ShipmentEntity>> validator;

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
