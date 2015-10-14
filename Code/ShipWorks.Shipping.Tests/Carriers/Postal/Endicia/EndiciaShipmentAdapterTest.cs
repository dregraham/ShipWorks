using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Postal.Endicia;
using System;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.Postal.Endicia
{
    public class EndiciaShipmentAdapterTest
    {
        readonly ShipmentEntity shipment;

        public EndiciaShipmentAdapterTest()
        {
            shipment = new ShipmentEntity
            {
                Postal = new PostalShipmentEntity
                {
                    Endicia = new EndiciaShipmentEntity()
                }
            };
        }

        [Fact]
        public void Constructor_ThrowsArgumentNullExcpetion_WhenShipmentIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new EndiciaShipmentAdapter(null));
        }

        [Fact]
        public void Constructor_ThrowsArgumentNullExcpetion_WhenPostalShipmentIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new EndiciaShipmentAdapter(new ShipmentEntity()));
        }

        [Fact]
        public void Constructor_ThrowsArgumentNullExcpetion_WhenEndiciaShipmentIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new EndiciaShipmentAdapter(new ShipmentEntity { Postal = new PostalShipmentEntity() }));
        }

        [Fact]
        public void AccountId_ReturnsShipmentValue()
        {
            shipment.Postal.Endicia.EndiciaAccountID = 12;
            var testObject = new EndiciaShipmentAdapter(shipment);
            Assert.Equal(12, testObject.AccountId);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(10009238)]
        public void AccountId_StoresSpecifiedValue_WhenValueIsValid(long value)
        {
            var testObject = new EndiciaShipmentAdapter(shipment);
            testObject.AccountId = value;
            Assert.Equal(value, shipment.Postal.Endicia.EndiciaAccountID);
        }

        [Fact]
        public void AccountId_StoresZero_WhenValueIsNull()
        {
            var testObject = new EndiciaShipmentAdapter(shipment);
            testObject.AccountId = null;
            Assert.Equal(0, shipment.Postal.Endicia.EndiciaAccountID);
        }
    }
}
