using System;
using System.Collections.Generic;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Filters;
using ShipWorks.Shipping.UI.ShippingPanel;
using ShipWorks.Shipping.UI.ShippingPanel.Loading;
using ShipWorks.Users.Security;
using Xunit;

namespace ShipWorks.Shipping.UI.Tests.ShippingPanel.Loading
{
    public class ShipmentOrderLoaderTest
    {
        private ShipmentLoader testObject;
        private OrderEntity orderEntity;
        private ShipmentEntity shipmentEntity;

        private Mock<IShippingPanelConfiguration> shippingPanelConfigurator;
        private Mock<IShippingManager> shippingManager;
        private Mock<IFilterHelper> filterHelper;

        public ShipmentOrderLoaderTest()
        {
            orderEntity = new OrderEntity(1006);
            shipmentEntity = new ShipmentEntity(1031);
            shipmentEntity.Order = orderEntity;

            shippingPanelConfigurator = new Mock<IShippingPanelConfiguration>();
            shippingPanelConfigurator.Setup(s => s.AutoCreateShipments).Returns(true);
            shippingPanelConfigurator.Setup(s => s.UserHasPermission(It.IsAny<PermissionType>(), It.IsAny<long>())).Returns(true);
            shippingPanelConfigurator.Setup(s => s.GetAddressValidation(It.IsAny<ShipmentEntity>())).Returns(true);

            shippingManager = new Mock<IShippingManager>();
            shippingManager.Setup(s => s.GetShipments(It.IsAny<long>(), It.IsAny<bool>())).Returns(new List<ShipmentEntity>() { shipmentEntity });

            filterHelper = new Mock<IFilterHelper>();
            filterHelper.Setup(s => s.EnsureFiltersUpToDate(It.IsAny<TimeSpan>())).Returns(true);

            testObject = new ShipmentLoader(shippingPanelConfigurator.Object, shippingManager.Object, filterHelper.Object);
        }

        [Fact]
        public void ShipmentAndSuccess_WhenOrderHasOneShipment_ReturnsThatShipment_Test()
        {
            ShippingPanelLoadedShipment shipmentPanelLoadedShipment = testObject.Load(orderEntity.OrderID);

            Assert.Equal(shipmentEntity.ShipmentID, shipmentPanelLoadedShipment.Shipment.ShipmentID);
            Assert.Equal(ShippingPanelLoadedShipmentResult.Success, shipmentPanelLoadedShipment.Result);
        }
        
    }
}
