using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.OnTrac;
using System;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.OnTrac
{
    public class OnTracShipmentAdapterTest
    {
        readonly ShipmentEntity shipment;

        public OnTracShipmentAdapterTest()
        {
            shipment = new ShipmentEntity
            {
                OnTrac = new OnTracShipmentEntity()
            };
        }

        [Fact]
        public void Constructor_ThrowsArgumentNullExcpetion_WhenShipmentIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new OnTracShipmentAdapter(null));
        }

        [Fact]
        public void Constructor_ThrowsArgumentNullExcpetion_WhenOnTracShipmentIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new OnTracShipmentAdapter(new ShipmentEntity()));
        }

        [Fact]
        public void AccountId_ReturnsShipmentValue()
        {
            shipment.OnTrac.OnTracAccountID = 12;
            var testObject = new OnTracShipmentAdapter(shipment);
            Assert.Equal(12, testObject.AccountId);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(10009238)]
        public void AccountId_StoresSpecifiedValue_WhenValueIsValid(long value)
        {
            var testObject = new OnTracShipmentAdapter(shipment);
            testObject.AccountId = value;
            Assert.Equal(value, shipment.OnTrac.OnTracAccountID);
        }

        [Fact]
        public void AccountId_StoresZero_WhenValueIsNull()
        {
            var testObject = new OnTracShipmentAdapter(shipment);
            testObject.AccountId = null;
            Assert.Equal(0, shipment.OnTrac.OnTracAccountID);
        }
    }
}
