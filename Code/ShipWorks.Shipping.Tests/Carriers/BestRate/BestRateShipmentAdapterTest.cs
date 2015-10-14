using ShipWorks.Data.Model.EntityClasses;
using System;
using ShipWorks.Shipping.Carriers.BestRate;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.BestRate
{
    public class BestRateShipmentAdapterTest
    {
        readonly ShipmentEntity shipment;

        public BestRateShipmentAdapterTest()
        {
            shipment = new ShipmentEntity()
            {
                BestRate = new BestRateShipmentEntity()
            };
        }

        [Fact]
        public void Constructor_ThrowsArgumentNullExcpetion_WhenShipmentIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new BestRateShipmentAdapter(null));
        }

        [Fact]
        public void AccountId_ReturnsNull()
        {
            var testObject = new BestRateShipmentAdapter(shipment);
            Assert.Null(testObject.AccountId);
        }

        [Fact]
        public void AccountId_DoesNotThrow_WhenValueIsValid()
        {
            var testObject = new BestRateShipmentAdapter(shipment);
            testObject.AccountId = 6;
        }

        [Fact]
        public void AccountId_DoesNotThrow_WhenValueIsNull()
        {
            var testObject = new BestRateShipmentAdapter(shipment);
            testObject.AccountId = null;
        }
    }
}
