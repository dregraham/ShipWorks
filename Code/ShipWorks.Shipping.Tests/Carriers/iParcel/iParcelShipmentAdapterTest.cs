using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.iParcel;
using System;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.iParcel
{
    public class iParcelShipmentAdapterTest
    {
        readonly ShipmentEntity shipment;

        public iParcelShipmentAdapterTest()
        {
            shipment = new ShipmentEntity
            {
                IParcel = new IParcelShipmentEntity()
            };
        }

        [Fact]
        public void Constructor_ThrowsArgumentNullExcpetion_WhenShipmentIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new iParcelShipmentAdapter(null));
        }

        [Fact]
        public void Constructor_ThrowsArgumentNullExcpetion_WhenIParcelShipmentIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new iParcelShipmentAdapter(new ShipmentEntity()));
        }

        [Fact]
        public void AccountId_ReturnsShipmentValue()
        {
            shipment.IParcel.IParcelAccountID = 12;
            var testObject = new iParcelShipmentAdapter(shipment);
            Assert.Equal(12, testObject.AccountId);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(10009238)]
        public void AccountId_StoresSpecifiedValue_WhenValueIsValid(long value)
        {
            var testObject = new iParcelShipmentAdapter(shipment);
            testObject.AccountId = value;
            Assert.Equal(value, shipment.IParcel.IParcelAccountID);
        }

        [Fact]
        public void AccountId_StoresZero_WhenValueIsNull()
        {
            var testObject = new iParcelShipmentAdapter(shipment);
            testObject.AccountId = null;
            Assert.Equal(0, shipment.IParcel.IParcelAccountID);
        }
    }
}
