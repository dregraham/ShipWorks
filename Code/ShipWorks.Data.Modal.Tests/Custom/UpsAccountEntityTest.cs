using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShipWorks.Data.Model.EntityClasses;
using Xunit;

namespace ShipWorks.Data.Modal.Tests.Custom
{
    public class UpsAccountEntityTest
    {
        private const long TestAccountId = 42;

        [Fact]
        public void ApplyTo_AccountIdIsSet()
        {
            ShipmentEntity shipment = new ShipmentEntity()
            {
                Ups = new UpsShipmentEntity()
                {
                    UpsAccountID = 2
                }
            };

            var testObject = new UpsAccountEntity(TestAccountId);

            testObject.ApplyTo(shipment);

            Assert.Equal(TestAccountId, shipment.Ups.UpsAccountID);
        }
    }
}
