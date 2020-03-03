using Autofac.Extras.Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Ups.ShipEngine;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.UPS.ShipEngine
{
    public class UpsShipEngineShipmentValidatorTest
    {
        private readonly AutoMock mock;
        private readonly UpsShipEngineShipmentValidator testObject;
        private readonly ShipmentEntity shipment;

        public UpsShipEngineShipmentValidatorTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            testObject = new UpsShipEngineShipmentValidator();

            shipment = new ShipmentEntity()
            {
                Ups = new UpsShipmentEntity()
            };

            shipment.Ups.Packages.Add(new UpsPackageEntity());
            shipment.Ups.Packages.Add(new UpsPackageEntity());
        }

        [Fact]
        public void ValidateShipment_ThrowsShippingException_WhenShipmentIsReturn()
        {
            shipment.ReturnShipment = true;

            Assert.Throws<ShippingException>(() => testObject.ValidateShipment(shipment));
        }

        [Fact]
        public void ValidateShipment_ThrowsShippingException_WhenShipmentHasEmailNotification()
        {
            shipment.Ups.EmailNotifySender = (int) UpsEmailNotificationType.Ship;

            Assert.Throws<ShippingException>(() => testObject.ValidateShipment(shipment));
        }
    }
}
