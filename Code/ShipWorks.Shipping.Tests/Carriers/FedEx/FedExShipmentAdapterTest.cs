using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.FedEx;
using System;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.FedEx
{
    public class FedExShipmentAdapterTest
    {
        readonly ShipmentEntity shipment;

        public FedExShipmentAdapterTest()
        {
            shipment = new ShipmentEntity
            {
                FedEx = new FedExShipmentEntity()
            };
        }

        [Fact]
        public void Constructor_ThrowsArgumentNullExcpetion_WhenShipmentIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new FedExShipmentAdapter(null));
        }

        [Fact]
        public void Constructor_ThrowsArgumentNullExcpetion_WhenFedExShipmentIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new FedExShipmentAdapter(new ShipmentEntity()));
        }

        [Fact]
        public void AccountId_ReturnsShipmentValue()
        {
            shipment.FedEx.FedExAccountID = 12;
            var testObject = new FedExShipmentAdapter(shipment);
            Assert.Equal(12, testObject.AccountId);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(10009238)]
        public void AccountId_StoresSpecifiedValue_WhenValueIsValid(long value)
        {
            var testObject = new FedExShipmentAdapter(shipment);
            testObject.AccountId = value;
            Assert.Equal(value, shipment.FedEx.FedExAccountID);
        }

        [Fact]
        public void AccountId_StoresZero_WhenValueIsNull()
        {
            var testObject = new FedExShipmentAdapter(shipment);
            testObject.AccountId = null;
            Assert.Equal(0, shipment.FedEx.FedExAccountID);
        }
    }
}
