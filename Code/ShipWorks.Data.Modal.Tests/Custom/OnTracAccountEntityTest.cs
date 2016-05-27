using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShipWorks.Data.Model.EntityClasses;
using Xunit;

namespace ShipWorks.Data.Modal.Tests.Custom
{
    public class OnTracAccountEntityTest
    {
        private const long TestAccountId = 42;

        [Fact]
        public void ApplyTo_AccountIdIsSet()
        {
            ShipmentEntity shipment = new ShipmentEntity()
            {
                OnTrac = new OnTracShipmentEntity()
                {
                    OnTracAccountID = 2
                }
            };

            OnTracAccountEntity testObject = new OnTracAccountEntity(TestAccountId);

            testObject.ApplyTo(shipment);

            Assert.Equal(TestAccountId, shipment.OnTrac.OnTracAccountID);
        }
    }
}
