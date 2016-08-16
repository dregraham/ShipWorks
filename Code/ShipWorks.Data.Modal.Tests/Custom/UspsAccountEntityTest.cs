using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShipWorks.Data.Model.EntityClasses;
using Xunit;

namespace ShipWorks.Data.Modal.Tests.Custom
{
    public class UspsAccountEntityTest
    {
        private const long TestAccountId = 42;

        [Fact]
        public void ApplyTo_AccountIdIsSet()
        {
            ShipmentEntity shipment = new ShipmentEntity()
            {
                Postal = new PostalShipmentEntity()
                {
                    Usps = new UspsShipmentEntity()
                    {
                        UspsAccountID = 7
                    }
                }
            };

            UspsAccountEntity testObject = new UspsAccountEntity(TestAccountId);

            testObject.ApplyTo(shipment);

            Assert.Equal(TestAccountId, shipment.Postal.Usps.UspsAccountID);
        }
    }
}
