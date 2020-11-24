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
using ShipWorks.Shipping.Settings.OneBalance;

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
        private readonly IOneBalanceAccountHelper oneBalanceAccountHelper;
        private readonly ILog log;

        /// <summary>
        /// Constructor
        /// </summary>
        public HubCarrierConfigurator(
            IIndex<ShipmentTypeCode, ICarrierSetup> carrierSetupFactory,
            ICarrierAccountRepository<UspsAccountEntity, IUspsAccountEntity> uspsAccountRepository,
            IOneBalanceAccountHelper oneBalanceAccountHelper)
        {
            this.carrierSetupFactory = carrierSetupFactory;
            this.uspsAccountRepository = uspsAccountRepository;
            this.oneBalanceAccountHelper = oneBalanceAccountHelper;
            this.log = LogManager.GetLogger(typeof(HubCarrierConfigurator));
        }

        private bool SkipOneBalanceSetup(CarrierConfiguration uspsOneBalanceConfig, UspsAccountEntity existingUspsOneBalanceAccount)
        {
            if(uspsOneBalanceConfig == null)
            {
                // one balance isn't set up in the hub
                return true;
            }

            if (existingUspsOneBalanceAccount == null)
            {
                // if there isn't an existing one balance account, we don't want to skip
                return false;
            }

            var usernameFromConfig = uspsOneBalanceConfig.AdditionalData["usps"].ToObject<UspsAccountConfiguration>()
                .Username;

            // USPS is setup for one balance, but it is a different account than the one in the hub, so skip setup
            return !string.Equals(existingUspsOneBalanceAccount.Username, usernameFromConfig, StringComparison.InvariantCultureIgnoreCase);

        }

        /// <summary>
        /// Configure carriers
        /// </summary>
        public async Task Configure(List<CarrierConfiguration> configs)
        {
            var uspsOneBalanceConfig = configs.SingleOrDefault(c => c.CarrierType == ShipmentTypeCode.Usps && c.IsOneBalance);

            var oneBalanceUspsAccount = oneBalanceAccountHelper.GetUspsOneBalanceAccount();

            bool skipOneBalanceSetup = SkipOneBalanceSetup(uspsOneBalanceConfig, oneBalanceUspsAccount);

            if (skipOneBalanceSetup)
            {
                log.Warn("Skipping One Balance accounts because One Balance already set up inside the client and conflicts with the hub");
            }
            else
            {
                await carrierSetupFactory[ShipmentTypeCode.Usps].Setup(uspsOneBalanceConfig, null).ConfigureAwait(false);
                if (oneBalanceUspsAccount == null)
                {
                    oneBalanceUspsAccount = uspsAccountRepository.Accounts.Single(a => a.HubCarrierId == uspsOneBalanceConfig.HubCarrierID);
                }
            }

            foreach (var config in configs)
            {
                try
                {
                    var needsSetup = uspsOneBalanceConfig == null ||
                                       config.HubCarrierID != uspsOneBalanceConfig.HubCarrierID;

                    var skipOneBalance = skipOneBalanceSetup && config.IsOneBalance;

                    if (needsSetup && !skipOneBalance)
                    {
                        await carrierSetupFactory[config.CarrierType]?.Setup(config, oneBalanceUspsAccount);
                    }
                }
                catch (Exception ex)
                {
                    log.Error($"Failed to import configuration for {EnumHelper.GetDescription(config.CarrierType)}: {ex.Message}", ex);
                }
            }
        }
    }
}
