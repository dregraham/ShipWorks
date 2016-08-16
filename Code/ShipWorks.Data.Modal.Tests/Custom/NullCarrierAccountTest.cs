using System;
using ShipWorks.Data.Model.Custom.EntityClasses;
using ShipWorks.Data.Model.EntityClasses;
using Xunit;

namespace ShipWorks.Data.Modal.Tests.Custom
{
    public class NullCarrierAccountTest
    {
        [Fact]
        public void ApplyTo_AccountIdIsSet()
        {
            ShipmentEntity shipment = new ShipmentEntity();

            NullCarrierAccount testObject = new NullCarrierAccount();

            Assert.Throws<NotImplementedException>(() => testObject.ApplyTo(shipment));
        }
    }
}
