using ShipWorks.Data.Model.EntityClasses;
using Xunit;

namespace ShipWorks.Data.Modal.Tests.Custom
{
    public class EndicaAccountEntityTest
    {
        [Fact]
        public void ApplyTo_AccountIdIsSet()
        {
            ShipmentEntity shipment = new ShipmentEntity()
            {
                Postal = new PostalShipmentEntity()
                {
                    Endicia = new EndiciaShipmentEntity()
                    {
                        EndiciaAccountID = 7
                    }
                }
            };

            EndiciaAccountEntity testObject = new EndiciaAccountEntity(42);

            testObject.ApplyTo(shipment);

            Assert.Equal(42, shipment.Postal.Endicia.EndiciaAccountID);
        }
    }
}
