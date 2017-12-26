using Autofac;
using Autofac.Extras.Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers;
using ShipWorks.Shipping.Carriers.Postal.Usps;
using ShipWorks.Tests.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.Postal.Usps
{
    public class UspsTermsAndConditionsTest
    {
        private readonly AutoMock mock;
        private readonly UspsShipmentType shipmentType;

        public UspsTermsAndConditionsTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            shipmentType = new UspsShipmentType();
        }

        [Fact]
        public void Validate_GetsUspsAccountFromRepoUsingAccountId()
        {
            var repo = mock.Mock<ICarrierAccountRepository<UspsAccountEntity, IUspsAccountEntity>>();

            var shipment = new ShipmentEntity()
            {
                Postal = new PostalShipmentEntity()
                {
                    Usps = new UspsShipmentEntity()
                    {
                        UspsAccountID = 123
                    }
                }
            };

            var testObject = mock.Create<UspsTermsAndConditions>(new TypedParameter(typeof(UspsShipmentType), shipmentType));

            testObject.Validate(shipment);

            repo.Verify(r => r.GetAccount(123));
        }
    }
}
