﻿using Autofac.Extras.Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Ups.ShipEngine;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.UPS.ShipEngine
{
    public class UpsShipEngineShipmentValidatorTest
    {
        private readonly UpsShipEngineShipmentValidator testObject;
        private readonly ShipmentEntity shipment;

        public UpsShipEngineShipmentValidatorTest()
        {
            testObject = new UpsShipEngineShipmentValidator();

            shipment = new ShipmentEntity()
            {
                Ups = new UpsShipmentEntity(),
                ShipCountryCode = "US"
            };

            shipment.Ups.Packages.Add(new UpsPackageEntity());
            shipment.Ups.Packages.Add(new UpsPackageEntity());
        }

        [Fact]
        public void ValidateShipment_ReturnsSuccess_WhenShipmentIsReturn()
        {
            shipment.ReturnShipment = true;

            Assert.True(testObject.ValidateShipment(shipment).Success);
        }

        [Fact]
        public void ValidateShipment_ThrowsShippingException_WhenShipmentHasEmailNotification()
        {
            shipment.Ups.EmailNotifySender = (int) UpsEmailNotificationType.Ship;

            Assert.True(testObject.ValidateShipment(shipment).Failure);
        }

        [Fact]
        public void ValidateShipment_ThrowsShippingException_WhenShipmentHasDryIce()
        {
            shipment.Ups.Packages[0].DryIceEnabled = true;
            shipment.Ups.Packages[0].UpsShipment.Service = 1;

            Assert.True(testObject.ValidateShipment(shipment).Failure);
        }

        [Fact]
        public void ValidateShipment_DoesntThrowShippingException_WhenShipmentHasDryIce_AndShipmentIsGround()
        {
            shipment.Ups.Packages[0].DryIceEnabled = true;

            Assert.True(testObject.ValidateShipment(shipment).Success);
        }

        [Fact]
        public void ValidateShipment_ThrowsShippingException_WhenShipmentHasCod()
        {
            shipment.Ups.CodEnabled = true;

            Assert.True(testObject.ValidateShipment(shipment).Failure);
        }

        [Fact]
        public void ValidateShipment_ThrowsShippingException_WhenShipmentHasShipperRelease()
        {
            shipment.Ups.ShipperRelease = true;

            Assert.True(testObject.ValidateShipment(shipment).Failure);
        }
        
        [Fact]
        public void ValidateShipment_ThrowsShippingException_WhenShipmentHasCarbonNeutral()
        {
            shipment.Ups.CarbonNeutral = true;

            Assert.True(testObject.ValidateShipment(shipment).Failure);
        }

        [Fact]
        public void ValidateShipment_ThrowsShippingException_WhenShipmentHasPaperless()
        {
            shipment.Ups.CommercialPaperlessInvoice = true;

            Assert.True(testObject.ValidateShipment(shipment).Failure);
        }

        [Fact]
        public void ValidateShipment_ThrowsShippingException_WhenShipmentHasVerbalConfirmation()
        {
            shipment.Ups.Packages[0].VerbalConfirmationEnabled = true;

            Assert.True(testObject.ValidateShipment(shipment).Failure);
        }
    }
}
