using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using ShipWorks.Core.Messaging.Messages.Shipping;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Filters;
using ShipWorks.Shipping.Configuration;
using ShipWorks.Shipping.Loading;
using ShipWorks.Users.Security;
using Xunit;

namespace ShipWorks.Shipping.Tests.Loading
{
    public class ShipmentOrderLoaderTest
    {
        private ShipmentLoader testObject;
        private OrderEntity orderEntity;
        private ShipmentEntity shipmentEntity;

        private Mock<IShippingConfiguration> shippingConfigurator;
        private Mock<IShippingManager> shippingManager;
        private Mock<IFilterHelper> filterHelper;
        private Mock<IValidator<ShipmentEntity>> addressValidator;

        public ShipmentOrderLoaderTest()
        {
            orderEntity = new OrderEntity(1006);
            shipmentEntity = new ShipmentEntity(1031);
            shipmentEntity.Order = orderEntity;

            shippingConfigurator = new Mock<IShippingConfiguration>();
            shippingConfigurator.Setup(s => s.AutoCreateShipments).Returns(true);
            shippingConfigurator.Setup(s => s.UserHasPermission(It.IsAny<PermissionType>(), It.IsAny<long>())).Returns(true);
            shippingConfigurator.Setup(s => s.GetAddressValidation(It.IsAny<ShipmentEntity>())).Returns(true);

            shippingManager = new Mock<IShippingManager>();
            shippingManager.Setup(s => s.GetShipments(It.IsAny<long>(), It.IsAny<bool>())).Returns(new List<ShipmentEntity>() { shipmentEntity });

            filterHelper = new Mock<IFilterHelper>();
            filterHelper.Setup(s => s.EnsureFiltersUpToDate(It.IsAny<TimeSpan>())).Returns(true);

            addressValidator = new Mock<IValidator<ShipmentEntity>>();
            addressValidator.Setup(av => av.ValidateAsync(It.IsAny<ShipmentEntity>())).Returns(Task.FromResult(true));

            testObject = new ShipmentLoader(shippingConfigurator.Object, shippingManager.Object, filterHelper.Object, addressValidator.Object);
        }

        [Fact]
        public void ShipmentAndSuccess_WhenOrderHasOneShipment_ReturnsThatShipment_Test()
        {
            OrderSelectionLoaded orderSelectionLoaded = testObject.Load(orderEntity.OrderID);

            Assert.Equal(shipmentEntity.ShipmentID, orderSelectionLoaded.Shipments.FirstOrDefault().ShipmentID);
            //Assert.Equal(ShippingPanelLoadedShipmentResult.Success, orderSelectionLoaded.Result);
        }
    }
}
