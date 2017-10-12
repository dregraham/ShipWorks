using System;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Other;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.Other
{
    public class OtherShipmentDateManipulatorTest
    {
        private DateTime shipDate = DateTime.Now.AddDays(-1);

        [Fact]
        public void Manipulate_DoesNotChangeShipDate()
        {
            ShipmentEntity shipment = new ShipmentEntity()
            {
                ShipDate = shipDate
            };

            shipment.IsDirty = false;

            OtherShipmentDateManipulator testObject = new OtherShipmentDateManipulator();
            testObject.Manipulate(shipment);

            Assert.False(shipment.IsDirty);
            Assert.Equal(shipDate, shipment.ShipDate);
        }
    }
}
