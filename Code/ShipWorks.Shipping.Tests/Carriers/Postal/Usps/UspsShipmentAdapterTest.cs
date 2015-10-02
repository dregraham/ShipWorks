using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Postal.Usps;
using System;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.Postal.Usps
{
    public class UspsShipmentAdapterTest
    {
        readonly ShipmentEntity shipment;

        public UspsShipmentAdapterTest()
        {
            shipment = new ShipmentEntity
            {
                Postal = new PostalShipmentEntity
                {
                    Usps = new UspsShipmentEntity()
                }
            };
        }

        [Fact]
        public void Constructor_ThrowsArgumentNullExcpetion_WhenShipmentIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new UspsShipmentAdapter(null));
        }

        [Fact]
        public void Constructor_ThrowsArgumentNullExcpetion_WhenPostalShipmentIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new UspsShipmentAdapter(new ShipmentEntity()));
        }

        [Fact]
        public void Constructor_ThrowsArgumentNullExcpetion_WhenUspsShipmentIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new UspsShipmentAdapter(new ShipmentEntity { Postal = new PostalShipmentEntity() }));
        }

        [Fact]
        public void AccountId_ReturnsShipmentValue()
        {
            shipment.Postal.Usps.UspsAccountID = 12;
            var testObject = new UspsShipmentAdapter(shipment);
            Assert.Equal(12, testObject.AccountId);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(10009238)]
        public void AccountId_StoresSpecifiedValue_WhenValueIsValid(long value)
        {
            var testObject = new UspsShipmentAdapter(shipment);
            testObject.AccountId = value;
            Assert.Equal(value, shipment.Postal.Usps.UspsAccountID);
        }

        [Fact]
        public void AccountId_StoresZero_WhenValueIsNull()
        {
            var testObject = new UspsShipmentAdapter(shipment);
            testObject.AccountId = null;
            Assert.Equal(0, shipment.Postal.Usps.UspsAccountID);
        }
    }
}
