using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Postal.Other;
using System;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.Other
{
    public class OtherShipmentAdapterTest
    {
        readonly ShipmentEntity shipment;

        public OtherShipmentAdapterTest()
        {
            shipment = new ShipmentEntity();
        }

        [Fact]
        public void Constructor_ThrowsArgumentNullExcpetion_WhenShipmentIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new OtherShipmentAdapter(null));
        }

        [Fact]
        public void AccountId_ReturnsNull()
        {
            var testObject = new OtherShipmentAdapter(shipment);
            Assert.Null(testObject.AccountId);
        }

        [Fact]
        public void AccountId_DoesNotThrow_WhenValueIsValid()
        {
            var testObject = new OtherShipmentAdapter(shipment);
            testObject.AccountId = 6;
        }

        [Fact]
        public void AccountId_DoesNotThrow_WhenValueIsNull()
        {
            var testObject = new OtherShipmentAdapter(shipment);
            testObject.AccountId = null;
        }
    }
}
