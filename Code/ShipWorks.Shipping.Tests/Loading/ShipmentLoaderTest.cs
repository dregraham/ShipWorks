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
    public class ShipmentLoaderTest
    {
        private ShipmentLoader testObject;
        private OrderEntity orderEntity;
        private ShipmentEntity shipmentEntity;

        private Mock<IShippingConfiguration> shippingConfigurator;
        private Mock<IShippingManager> shippingManager;
        private Mock<IFilterHelper> filterHelper;
        private Mock<IValidator<ShipmentEntity>> addressValidator;

        public ShipmentLoaderTest()
        {
            orderEntity = new OrderEntity(1006);
            shipmentEntity = new ShipmentEntity(1031);
            shipmentEntity.Order = orderEntity;

            shippingConfigurator = new Mock<IShippingConfiguration>();
            shippingConfigurator.Setup(s => s.AutoCreateShipments).Returns(true);
            shippingConfigurator.Setup(s => s.UserHasPermission(It.IsAny<PermissionType>(), It.IsAny<long>())).Returns(true);
            shippingConfigurator.Setup(s => s.GetAddressValidation(It.IsAny<ShipmentEntity>())).Returns(true);

            shippingManager = new Mock<IShippingManager>();
            shippingManager.Setup(s => s.GetShipments(It.IsAny<long>(), true)).Returns(new List<ShipmentEntity>() { shipmentEntity });
            shippingManager.Setup(s => s.GetShipments(It.IsAny<long>(), false)).Returns(new List<ShipmentEntity>() { });

            filterHelper = new Mock<IFilterHelper>();
            filterHelper.Setup(s => s.EnsureFiltersUpToDate(It.IsAny<TimeSpan>())).Returns(true);

            addressValidator = new Mock<IValidator<ShipmentEntity>>();
            addressValidator.Setup(av => av.ValidateAsync(It.IsAny<ShipmentEntity>())).Returns(Task.FromResult(true));

            testObject = new ShipmentLoader(shippingConfigurator.Object, shippingManager.Object, filterHelper.Object, addressValidator.Object);
        }

        [Fact]
        public void Shipment_WhenOrderHasOneShipment_ReturnsThatShipment_Test()
        {
            OrderSelectionLoaded orderSelectionLoaded = testObject.Load(orderEntity.OrderID);

            Assert.Equal(shipmentEntity.ShipmentID, orderSelectionLoaded.Shipments.FirstOrDefault().ShipmentID);
            Assert.Equal(1, orderSelectionLoaded.Shipments.Count());
            Assert.Equal(orderEntity, orderSelectionLoaded.Order);
        }

        [Fact]
        public void ShipmentsReturned_Correct_WhenOrderHasMultipleShipments_Test()
        {
            shippingManager.Setup(s => s.GetShipments(It.IsAny<long>(), It.IsAny<bool>())).Returns(new List<ShipmentEntity>() { shipmentEntity, shipmentEntity });

            OrderSelectionLoaded orderSelectionLoaded = testObject.Load(orderEntity.OrderID);

            Assert.Equal(2, orderSelectionLoaded.Shipments.Count());
            Assert.Equal(orderEntity, orderSelectionLoaded.Order);
        }

        [Fact]
        public void NoShipmentsReturned_WhenAutoCreateIsFalse_Test()
        {
            shippingConfigurator.Setup(s => s.AutoCreateShipments).Returns(false);

            OrderSelectionLoaded orderSelectionLoaded = testObject.Load(orderEntity.OrderID);

            Assert.Equal(0, orderSelectionLoaded.Shipments.Count());
            Assert.Equal(null, orderSelectionLoaded.Order);
        }

        [Fact]
        public void ShipmentsReturned_WhenAutoCreateIsTrue_Test()
        {
            shippingConfigurator.Setup(s => s.AutoCreateShipments).Returns(true);

            OrderSelectionLoaded orderSelectionLoaded = testObject.Load(orderEntity.OrderID);

            Assert.Equal(1, orderSelectionLoaded.Shipments.Count());
            Assert.Equal(orderEntity, orderSelectionLoaded.Order);
        }

        [Fact]
        public void NoShipmentsReturned_WhenCreateShipmentPermissionNotAllowed_Test()
        {
            shippingConfigurator.Setup(s => s.UserHasPermission(It.IsAny<PermissionType>(), It.IsAny<long>())).Returns(false);

            OrderSelectionLoaded orderSelectionLoaded = testObject.Load(orderEntity.OrderID);

            Assert.Equal(0, orderSelectionLoaded.Shipments.Count());
            Assert.Equal(null, orderSelectionLoaded.Order);
        }

        [Fact]
        public void ShipmentsReturned_WhenCreateShipmentPermissionIsAllowed_Test()
        {
            shippingConfigurator.Setup(s => s.UserHasPermission(It.IsAny<PermissionType>(), It.IsAny<long>())).Returns(true);

            OrderSelectionLoaded orderSelectionLoaded = testObject.Load(orderEntity.OrderID);

            Assert.Equal(1, orderSelectionLoaded.Shipments.Count());
            Assert.Equal(orderEntity, orderSelectionLoaded.Order);
        }

        [Fact]
        public void AddressValidation_NotPerformed_WhenAddressValidationNotAllowed_Test()
        {
            shippingConfigurator.Setup(s => s.GetAddressValidation(It.IsAny<ShipmentEntity>())).Returns(false);

            OrderSelectionLoaded orderSelectionLoaded = testObject.Load(orderEntity.OrderID);

            addressValidator.Verify(av => av.ValidateAsync(It.IsAny<ShipmentEntity>()), Times.Never);
        }

        [Fact]
        public void AddressValidation_Performed_WhenAddressValidationAllowed_Test()
        {
            shippingConfigurator.Setup(s => s.GetAddressValidation(It.IsAny<ShipmentEntity>())).Returns(true);

            OrderSelectionLoaded orderSelectionLoaded = testObject.Load(orderEntity.OrderID);

            addressValidator.Verify(av => av.ValidateAsync(It.IsAny<ShipmentEntity>()), Times.Once);
        }

        [Fact]
        public void AddressValidation_NotPerformed_WhenNoShipmentsAndAddressValidationAllowed_Test()
        {
            shippingConfigurator.Setup(s => s.GetAddressValidation(It.IsAny<ShipmentEntity>())).Returns(true);
            shippingConfigurator.Setup(s => s.AutoCreateShipments).Returns(false);

            OrderSelectionLoaded orderSelectionLoaded = testObject.Load(orderEntity.OrderID);

            addressValidator.Verify(av => av.ValidateAsync(It.IsAny<ShipmentEntity>()), Times.Never);
        }

        [Fact]
        public void EnsureFiltersUpToDate_Performed_Test()
        {
            shippingConfigurator.Setup(s => s.GetAddressValidation(It.IsAny<ShipmentEntity>())).Returns(true);
            shippingConfigurator.Setup(s => s.AutoCreateShipments).Returns(false);

            OrderSelectionLoaded orderSelectionLoaded = testObject.Load(orderEntity.OrderID);

            filterHelper.Verify(s => s.EnsureFiltersUpToDate(It.IsAny<TimeSpan>()), Times.Once);
        }
    }
}
