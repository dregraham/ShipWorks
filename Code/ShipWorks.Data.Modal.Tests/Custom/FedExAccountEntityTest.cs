using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShipWorks.Data.Model.EntityClasses;
using Xunit;

namespace ShipWorks.Data.Modal.Tests.Custom
{
    
    public class FedExAccountEntityTest
    {
        private const long TestAccountId = 42;

        [Fact]
        public void ApplyTo_AccountIdIsSet()
        {
            ShipmentEntity shipment = new ShipmentEntity()
            {
                FedEx = new FedExShipmentEntity()
                {
                    FedExAccountID = 2
                }
            };

            FedExAccountEntity testObject = new FedExAccountEntity(TestAccountId);

            testObject.ApplyTo(shipment);

            Assert.Equal(TestAccountId, shipment.FedEx.FedExAccountID);
        }
    }
}
