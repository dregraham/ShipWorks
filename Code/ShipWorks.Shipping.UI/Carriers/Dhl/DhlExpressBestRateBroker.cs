using ShipWorks.Shipping.Carriers.BestRate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.Insurance;
using ShipWorks.Shipping.Carriers.Dhl;
using SD.LLBLGen.Pro.ORMSupportClasses;

namespace ShipWorks.Shipping.UI.Carriers.Dhl
{
    public class DhlExpressBestRateBroker : PackageBasedBestRateBroker<DhlExpressAccountEntity, IDhlExpressAccountEntity, IDhlExpressPackageEntity>
    {
        public DhlExpressBestRateBroker(ShipmentType shipmentType, IDhlExpressAccountRepository accountRepository) :
            base(shipmentType, accountRepository, "DHL Express")
        {
        }

        public override InsuranceProvider GetInsuranceProvider(IShippingSettingsEntity settings)
        {
            return InsuranceProvider.Invalid;
        }

        protected override EntityState ChildShipmentEntityState(ShipmentEntity shipment)
        {
            return shipment.DhlExpress.Fields.State;
        }

        protected override void CreateShipmentChild(ShipmentEntity shipment)
        {
            shipment.DhlExpress
        }

        protected override bool IsExcludedServiceType(object tag)
        {
            throw new NotImplementedException();
        }

        protected override long PackageId(IDhlExpressPackageEntity package)
        {
            throw new NotImplementedException();
        }

        protected override IList<IDhlExpressPackageEntity> Packages(ShipmentEntity shipment)
        {
            throw new NotImplementedException();
        }

        protected override void SetPackageId(IDhlExpressPackageEntity package, long packageId)
        {
            throw new NotImplementedException();
        }

        protected override void SetServiceTypeFromTag(ShipmentEntity shipment, object tag)
        {
            throw new NotImplementedException();
        }
    }
}
