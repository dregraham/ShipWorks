﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.BestRate;

namespace ShipWorks.Tests.Shipping.Carriers.BestRate
{
    [TestClass]
    public class CounterRatesOriginAddressValidatorTest
    {

        [TestMethod]
        public void IsValid_ReturnsFalse_WhenOriginStreet1IsEmpty_Test()
        {
            ShipmentEntity shipment = CreateShipment();
            shipment.OriginStreet1 = string.Empty;

            Assert.IsFalse(CounterRatesOriginAddressValidator.IsValid(shipment));
        }

        [TestMethod]
        public void IsValid_ReturnsFalse_WhenOriginCityIsEmpty_Test()
        {
            ShipmentEntity shipment = CreateShipment();
            shipment.OriginCity = string.Empty;

            Assert.IsFalse(CounterRatesOriginAddressValidator.IsValid(shipment));
        }

        [TestMethod]
        public void IsValid_ReturnsFalse_WhenOriginStateIsEmpty_Test()
        {
            ShipmentEntity shipment = CreateShipment();
            shipment.OriginStateProvCode = string.Empty;

            Assert.IsFalse(CounterRatesOriginAddressValidator.IsValid(shipment));
        }

        [TestMethod]
        public void IsValid_ReturnsFalse_WhenOriginPostalCode1IsEmpty_Test()
        {
            ShipmentEntity shipment = CreateShipment();
            shipment.OriginPostalCode = string.Empty;

            Assert.IsFalse(CounterRatesOriginAddressValidator.IsValid(shipment));
        }

        [TestMethod]
        public void IsValid_ReturnsFalse_WhenOriginCountryCodeIsEmpty_Test()
        {
            ShipmentEntity shipment = CreateShipment();
            shipment.OriginCountryCode = string.Empty;

            Assert.IsFalse(CounterRatesOriginAddressValidator.IsValid(shipment));
        }

        [TestMethod]
        public void IsValid_ReturnsTrue_WhenOriginAddressIsComplete_Test()
        {
            ShipmentEntity shipment = CreateShipment();

            Assert.IsTrue(CounterRatesOriginAddressValidator.IsValid(shipment));
        }

        private ShipmentEntity CreateShipment()
        {
            ShipmentEntity shipment = new ShipmentEntity
            {
                OriginStreet1 = "1 Memorial Drive",
                OriginCity = "St. Louis",
                OriginStateProvCode = "MO",
                OriginPostalCode = "63102",
                OriginCountryCode = "US"
            };

            return shipment;
        }
    }
}
