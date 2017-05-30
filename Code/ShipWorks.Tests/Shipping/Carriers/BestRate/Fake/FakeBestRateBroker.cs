using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.Shipping.Insurance;

namespace ShipWorks.Tests.Shipping.Carriers.BestRate.Fake
{
    public class FakeBestRateBroker : BestRateBroker<UspsAccountEntity, IUspsAccountEntity>
    {
        public FakeBestRateBroker(ShipmentType shipmentType, ICarrierAccountRepository<UspsAccountEntity, IUspsAccountEntity> accountRepository, string carrierDescription) : 
            base(shipmentType, accountRepository, carrierDescription)
        {
        }

        public override InsuranceProvider GetInsuranceProvider(IShippingSettingsEntity settings)
        {
            throw new NotImplementedException();
        }

        protected override bool IsExcludedServiceType(object tag)
            => false;

        protected override void CreateShipmentChild(ShipmentEntity shipment)
        {
            // no-op
        }

        protected override void SetServiceTypeFromTag(ShipmentEntity shipment, object tag)
        {
           // no-op
        }
    }
}
