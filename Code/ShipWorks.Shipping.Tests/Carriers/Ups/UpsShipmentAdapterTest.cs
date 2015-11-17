using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.UPS;
using System;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.Ups
{
    public class UpsShipmentAdapterTest
    {
        readonly ShipmentEntity shipment;

        public UpsShipmentAdapterTest()
        {
            shipment = new ShipmentEntity
            {
                Ups = new UpsShipmentEntity()
            };
        }

        [Fact]
        public void Constructor_ThrowsArgumentNullExcpetion_WhenShipmentIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new UpsShipmentAdapter(null));
        }

        [Fact]
        public void Constructor_ThrowsArgumentNullExcpetion_WhenPostalShipmentIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new UpsShipmentAdapter(new ShipmentEntity()));
        }

        [Fact]
        public void AccountId_ReturnsShipmentValue()
        {
            shipment.Ups.UpsAccountID = 12;
            var testObject = new UpsShipmentAdapter(shipment);
            Assert.Equal(12, testObject.AccountId);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(10009238)]
        public void AccountId_StoresSpecifiedValue_WhenValueIsValid(long value)
        {
            var testObject = new UpsShipmentAdapter(shipment);
            testObject.AccountId = value;
            Assert.Equal(value, shipment.Ups.UpsAccountID);
        }

        [Fact]
        public void AccountId_StoresZero_WhenValueIsNull()
        {
            var testObject = new UpsShipmentAdapter(shipment);
            testObject.AccountId = null;
            Assert.Equal(0, shipment.Ups.UpsAccountID);
        }
    }
}
