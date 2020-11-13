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
        private readonly ICarrierAccountRepository<DhlExpressAccountEntity, IDhlExpressAccountEntity> dhlExpressAccountRepository;
        private readonly ILog log;

        /// <summary>
        /// Constructor
        /// </summary>
        public HubCarrierConfigurator(
            IIndex<ShipmentTypeCode, ICarrierSetup> carrierSetupFactory,
            ICarrierAccountRepository<UspsAccountEntity, IUspsAccountEntity> uspsAccountRepository,
            ICarrierAccountRepository<DhlExpressAccountEntity, IDhlExpressAccountEntity> dhlExpressAccountRepository)
        {
            this.carrierSetupFactory = carrierSetupFactory;
            this.uspsAccountRepository = uspsAccountRepository;
            this.dhlExpressAccountRepository = dhlExpressAccountRepository;
            this.log = LogManager.GetLogger(typeof(HubCarrierConfigurator));
        }

        private bool SkipOneBalanceSetup(List<CarrierConfiguration> configs)
        {
            var uspsOneBalanceConfig = configs.SingleOrDefault(c => c.CarrierType == ShipmentTypeCode.Usps && c.IsOneBalance);
            // USPS is setup for one balance, but it is a different account than the one in the hub
            if (uspsOneBalanceConfig != null &&
                uspsAccountRepository.AccountsReadOnly
                   .Where(a => !string.IsNullOrWhiteSpace(a.ShipEngineCarrierId))
                   .Any(a => a.HubCarrierId != uspsOneBalanceConfig.HubCarrierID))
            {
                return true;
            }

            var dhlOneBalanceConfig = configs.SingleOrDefault(c => c.CarrierType == ShipmentTypeCode.DhlExpress && c.IsOneBalance);
            // DHLx is setup for one balance, but it is a different account than the one in the hub
            if (dhlOneBalanceConfig != null &&
                dhlExpressAccountRepository.AccountsReadOnly.Where(a => a.UspsAccountId.HasValue)
                .Any(a => a.HubCarrierId != dhlOneBalanceConfig.HubCarrierID))
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
            IUspsAccountEntity oneBalanceUspsAccount = null;

            var uspsOneBalanceConfig = configs.SingleOrDefault(c => c.CarrierType == ShipmentTypeCode.Usps && c.IsOneBalance);

            if (!skipOneBalanceSetup)
            {
                await carrierSetupFactory[ShipmentTypeCode.Usps].Setup(uspsOneBalanceConfig, null).ConfigureAwait(false);
                oneBalanceUspsAccount = uspsAccountRepository.AccountsReadOnly.Single(a => a.HubCarrierId == uspsOneBalanceConfig.HubCarrierID);
            }

            foreach (var config in configs
                .Where(c => uspsOneBalanceConfig == null || c.HubCarrierID != uspsOneBalanceConfig.HubCarrierID)
                .Where(c=> !(skipOneBalanceSetup && c.IsOneBalance)))
            {
                try
                {
                    await carrierSetupFactory[config.CarrierType]?.Setup(config, oneBalanceUspsAccount);
                }
                catch (Exception ex)
                {
                    log.Error($"Failed to import configuration for {EnumHelper.GetDescription((ShipmentTypeCode) config.CarrierType)}: {ex.Message}", ex);
                }
            }
        }
    }
}
