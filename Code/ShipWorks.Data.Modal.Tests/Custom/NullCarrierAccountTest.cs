using ShipWorks.Data.Model.Custom.EntityClasses;
using ShipWorks.Data.Model.EntityClasses;
using Xunit;

namespace ShipWorks.Data.Modal.Tests.Custom
{
    public class NullCarrierAccountTest
    {
        [Fact]
        public void ApplyTo_AccountIdIsSet_DoesNotThrow()
        {
            ShipmentEntity shipment = new ShipmentEntity();

            NullCarrierAccount testObject = new NullCarrierAccount();

            testObject.ApplyTo(shipment);
        }
    }
}
