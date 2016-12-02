using System;
using System.Linq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping;
using Xunit;

namespace ShipWorks.Tests.Shipping
{
    public class IShipmentEntityExtensionsTest
    {
        [Fact]
        public void ProcessingCompletesExternally_WithWorldShipShipment_ReturnsTrue()
        {
            Assert.True(new ShipmentEntity { ShipmentTypeCode = ShipmentTypeCode.UpsWorldShip }.ProcessingCompletesExternally());
        }

        [Fact]
        public void ProcessingCompletesExternally_WithAllOtherShipmentTypes_ReturnsFalse()
        {
            var otherShipmentTypes = Enum.GetValues(typeof(ShipmentTypeCode))
                .OfType<ShipmentTypeCode>()
                .Except(new[] { ShipmentTypeCode.UpsWorldShip });

            foreach (var type in otherShipmentTypes)
            {
                Assert.False(new ShipmentEntity { ShipmentTypeCode = type }.ProcessingCompletesExternally());
            }
        }

        [Fact]
        public void ProcessingCompletesExternally_WithNullShipment_ReturnsFalse()
        {
            Assert.False(((IShipmentEntity) null).ProcessingCompletesExternally());
        }
    }
}
