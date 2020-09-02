using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interapptive.Shared.Business.Geography;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Security;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.ApplicationCore.Licensing.Activation;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers;
using ShipWorks.Shipping.Carriers.Postal.Usps;
using ShipWorks.Shipping.Settings.Origin;
using ShipWorks.Warehouse.DTO.Configuration.ShippingSettings;

namespace ShipWorks.Shipping.CarrierSetup
{
    [KeyedComponent(typeof(ICarrierSetup), ShipmentTypeCode.Usps)]
    public class UspsCarrierSetup : ICarrierSetup
    {
        private readonly ICarrierAccountRepository<UspsAccountEntity, IUspsAccountEntity> uspsAccountRepository;
        private readonly IShipmentTypeSetupActivity shipmentTypeSetupActivity;
        public UspsCarrierSetup(IShipmentTypeSetupActivity shipmentTypeSetupActivity,
            ICarrierAccountRepository<UspsAccountEntity, IUspsAccountEntity> uspsAccountRepository)
        {
            this.uspsAccountRepository = uspsAccountRepository;
            this.shipmentTypeSetupActivity = shipmentTypeSetupActivity;
        }

        /// <summary>
        /// Creates a new usps account
        /// </summary>
        public void Setup(CarrierConfigurationPayload config)
        {
            
        }
    }
}
