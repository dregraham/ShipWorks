using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Autofac.Features.Indexed;
using Common.Logging;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using ShipWorks.Shipping.CarrierSetup;
using ShipWorks.Warehouse.Configuration.DTO.ShippingSettings;
using System.Linq;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Carriers.CarrierSetup
{
    /// <summary>
    /// Configures carriers downloaded from the Hub
    /// </summary>
    [Component]
    public class HubCarrierConfigurator : IHubCarrierConfigurator
    {
        private readonly IIndex<ShipmentTypeCode, ICarrierSetup> carrierSetupFactory;
        private readonly ICarrierAccountRepository<UspsAccountEntity, IUspsAccountEntity> uspsAccountRepository;
        private readonly ILog log;

        /// <summary>
        /// Constructor
        /// </summary>
        public HubCarrierConfigurator(
            IIndex<ShipmentTypeCode, ICarrierSetup> carrierSetupFactory,
            ICarrierAccountRepository<UspsAccountEntity, IUspsAccountEntity> uspsAccountRepository)
        {
            this.carrierSetupFactory = carrierSetupFactory;
            this.uspsAccountRepository = uspsAccountRepository;
            this.log = LogManager.GetLogger(typeof(HubCarrierConfigurator));
        }

        private bool SkipOneBalanceSetup(List<CarrierConfiguration> configs)
        {
            var uspsOneBalanceConfig = configs.SingleOrDefault(c => c.CarrierType == ShipmentTypeCode.Usps && c.IsOneBalance);
            if(uspsOneBalanceConfig == null)
            {
                // one balance isn't set up in the hub
                return true;
            }

            // USPS is setup for one balance, but it is a different account than the one in the hub
            if (uspsAccountRepository.AccountsReadOnly
                   .Any(a => !string.IsNullOrWhiteSpace(a.ShipEngineCarrierId) &&
                            a.Username != uspsOneBalanceConfig.AdditionalData["usps"].ToObject<UspsAccountConfiguration>().Username))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Configure carriers
        /// </summary>
        public async Task Configure(List<CarrierConfiguration> configs)
        {
            bool skipOneBalanceSetup = SkipOneBalanceSetup(configs);
            UspsAccountEntity oneBalanceUspsAccount = null;

            var uspsOneBalanceConfig = configs.SingleOrDefault(c => c.CarrierType == ShipmentTypeCode.Usps && c.IsOneBalance);

            if (skipOneBalanceSetup)
            {
                log.Warn("Skipping One Balance accounts because One Balance already set up inside the client and conflicts with the hub");
            }
            else
            {
                await carrierSetupFactory[ShipmentTypeCode.Usps].Setup(uspsOneBalanceConfig, null).ConfigureAwait(false);
                oneBalanceUspsAccount = uspsAccountRepository.Accounts.Single(a => a.HubCarrierId == uspsOneBalanceConfig.HubCarrierID);
            }

            foreach (var config in configs
                .Where(c => (uspsOneBalanceConfig == null || c.HubCarrierID != uspsOneBalanceConfig.HubCarrierID) &&
                            !(skipOneBalanceSetup && c.IsOneBalance)))
            {
                try
                {
                    await carrierSetupFactory[config.CarrierType]?.Setup(config, oneBalanceUspsAccount);
                }
                catch (Exception ex)
                {
                    log.Error($"Failed to import configuration for {EnumHelper.GetDescription(config.CarrierType)}: {ex.Message}", ex);
                }
            }
        }
    }
}
