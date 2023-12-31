﻿using Xunit;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping;

namespace ShipWorks.Tests.Shipping
{
    public class ShipmentTypeTest
    {
        [Fact]
        public void IsPuertoRicoAddress_ReturnsTrue_WhenCountryIsPR()
        {
            var shipment = new ShipmentEntity { OriginCountryCode = "PR" };
            var result = ShipmentType.IsPuertoRicoAddress(shipment, "Origin");
            Assert.True(result);
        }

        [Fact]
        public void IsPuertoRicoAddress_ReturnsTrue_WhenCountryIsUSButStateIsPR()
        {
            var shipment = new ShipmentEntity { OriginCountryCode = "US", OriginStateProvCode = "PR"};
            var result = ShipmentType.IsPuertoRicoAddress(shipment, "Origin");
            Assert.True(result);
        }

        [Fact]
        public void IsPuertoRicoAddress_ReturnsFalse_WhenCountryIsUSAndStateIsNotPR()
        {
            var shipment = new ShipmentEntity { OriginCountryCode = "US", OriginStateProvCode = "MO" };
            var result = ShipmentType.IsPuertoRicoAddress(shipment, "Origin");
            Assert.False(result);
        }

        [Fact]
        public void IsPuertoRicoAddress_ReturnsFalse_WhenCountryIsNotUSOrPR()
        {
            var shipment = new ShipmentEntity { OriginCountryCode = "FR" };
            var result = ShipmentType.IsPuertoRicoAddress(shipment, "Origin");
            Assert.False(result);
        }

        private void TestIsShipmentBetweenUnitedStatesAndPuertoRicoInBothDirections(string sourceCountry,
            string sourceState, string destinationCountry, string destinationState, bool expectedResults)
        {
            var shipment = new ShipmentEntity
            {
                OriginCountryCode = sourceCountry,
                OriginStateProvCode = sourceState,
                ShipCountryCode = destinationCountry,
                ShipStateProvCode = destinationState
            };
            var result = ShipmentType.IsShipmentBetweenUnitedStatesAndPuertoRico(shipment);
            Assert.Equal(expectedResults, result);

            shipment = new ShipmentEntity
            {
                OriginCountryCode = destinationCountry,
                OriginStateProvCode = destinationState,
                ShipCountryCode = sourceCountry,
                ShipStateProvCode = sourceState
            };
            result = ShipmentType.IsShipmentBetweenUnitedStatesAndPuertoRico(shipment);
            Assert.Equal(expectedResults, result);
        }

        [Fact]
        public void IsShipmentBetweenUnitedStatesAndPuertoRico_ReturnsTrue_WhenShipmentIsBetweenUSAndPR()
        {
            TestIsShipmentBetweenUnitedStatesAndPuertoRicoInBothDirections("US", "MO", "PR", "PR", true);
            TestIsShipmentBetweenUnitedStatesAndPuertoRicoInBothDirections("US", "MO", "US", "PR", true);
        }

        [Fact]
        public void IsShipmentBetweenUnitedStatesAndPuertoRico_ReturnsFalse_WhenShipmentIsNotBetweenUSAndPR()
        {
            TestIsShipmentBetweenUnitedStatesAndPuertoRicoInBothDirections("US", "MO", "US", "IL", false);
            TestIsShipmentBetweenUnitedStatesAndPuertoRicoInBothDirections("PR", "PR", "PR", "PR", false);
            TestIsShipmentBetweenUnitedStatesAndPuertoRicoInBothDirections("US", "PR", "PR", "PR", false);
            TestIsShipmentBetweenUnitedStatesAndPuertoRicoInBothDirections("US", "MO", "FR", string.Empty, false);
            TestIsShipmentBetweenUnitedStatesAndPuertoRicoInBothDirections("US", "PR", "FR", string.Empty, false);
        }
    }
}
