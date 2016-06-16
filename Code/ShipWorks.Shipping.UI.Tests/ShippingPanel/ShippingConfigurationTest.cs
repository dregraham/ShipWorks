using System;
using Autofac.Extras.Moq;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Settings;
using ShipWorks.Shipping.UI.ShippingPanel;
using ShipWorks.Tests.Shared;
using ShipWorks.Users.Security;
using Xunit;

namespace ShipWorks.Shipping.UI.Tests.ShippingPanel
{
    public class ShippingConfigurationTest : IDisposable
    {
        readonly AutoMock mock;
        readonly OrderEntity order;

        public ShippingConfigurationTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            order = new OrderEntity();
            mock.Mock<IShippingSettings>()
                .Setup(x => x.AutoCreateShipments).Returns(true);
            mock.Mock<ISecurityContext>()
                .Setup(x => x.HasPermission(It.IsAny<PermissionType>(), It.IsAny<long>())).Returns(true);
        }

        [Fact]
        public void ShouldAutoCreateShipment_ReturnsTrue_WhenAllConditionsAreMet()
        {
            var testObject = mock.Create<ShippingConfiguration>();
            var result = testObject.ShouldAutoCreateShipment(order);
            Assert.True(result);
        }

        [Fact]
        public void ShouldAutoCreateShipment_ReturnsFalse_WhenOrderAlreadyHasAShipment()
        {
            order.Shipments.Add(new ShipmentEntity());

            var testObject = mock.Create<ShippingConfiguration>();
            var result = testObject.ShouldAutoCreateShipment(order);
            Assert.False(result);
        }

        [Fact]
        public void ShouldAutoCreateShipment_ReturnsFalse_WhenAutoCreateShipmentsIsFalse()
        {
            mock.Mock<IShippingSettings>()
                .Setup(x => x.AutoCreateShipments).Returns(false);

            var testObject = mock.Create<ShippingConfiguration>();
            var result = testObject.ShouldAutoCreateShipment(order);
            Assert.False(result);
        }

        [Fact]
        public void ShouldAutoCreateShipment_ReturnsFalse_WhenUserDoesNotHavePermission()
        {
            order.OrderID = 123;

            mock.Mock<ISecurityContext>()
                .Setup(x => x.HasPermission(PermissionType.ShipmentsCreateEditProcess, 123))
                .Returns(false);

            var testObject = mock.Create<ShippingConfiguration>();
            var result = testObject.ShouldAutoCreateShipment(order);
            Assert.False(result);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
