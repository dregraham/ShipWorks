using ShipWorks.Data.Model.EntityClasses;
using Xunit;

namespace ShipWorks.Data.Modal.Tests.Custom
{
    public class EndicaAccountEntityTest
    {
        private const long TestAccountId = 42;

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

            EndiciaAccountEntity testObject = new EndiciaAccountEntity(TestAccountId);

            testObject.ApplyTo(shipment);

            Assert.Equal(TestAccountId, shipment.Postal.Endicia.EndiciaAccountID);
        }
    }
}
