using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Postal.None;
using System;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.None
{
    public class NoneShipmentAdapterTest
    {
        readonly ShipmentEntity shipment;

        public NoneShipmentAdapterTest()
        {
            shipment = new ShipmentEntity();
        }

        [Fact]
        public void Constructor_ThrowsArgumentNullExcpetion_WhenShipmentIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new NoneShipmentAdapter(null));
        }

        [Fact]
        public void AccountId_ReturnsNull()
        {
            var testObject = new NoneShipmentAdapter(shipment);
            Assert.Null(testObject.AccountId);
        }

        [Fact]
        public void AccountId_DoesNotThrow_WhenValueIsValid()
        {
            var testObject = new NoneShipmentAdapter(shipment);
            testObject.AccountId = 6;
        }

        [Fact]
        public void AccountId_DoesNotThrow_WhenValueIsNull()
        {
            var testObject = new NoneShipmentAdapter(shipment);
            testObject.AccountId = null;
        }
    }
}
