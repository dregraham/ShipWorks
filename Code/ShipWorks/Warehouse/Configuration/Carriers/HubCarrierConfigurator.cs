using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Autofac.Features.Indexed;
using Common.Logging;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using ShipWorks.Shipping;
using ShipWorks.Warehouse.Configuration.Carriers.DTO;

namespace ShipWorks.Warehouse.Configuration.Carriers
{
    /// <summary>
    /// Configures carriers downloaded from the Hub
    /// </summary>
    [Component]
    public class HubCarrierConfigurator : IHubCarrierConfigurator
    {
        private readonly IIndex<ShipmentTypeCode, ICarrierSetup> carrierSetupFactory;
        private readonly ILog log;

        /// <summary>
        /// Constructor
        /// </summary>
        public HubCarrierConfigurator(IIndex<ShipmentTypeCode, ICarrierSetup> carrierSetupFactory)
        {
            this.carrierSetupFactory = carrierSetupFactory;
            this.log = LogManager.GetLogger(typeof(HubCarrierConfigurator));
        }

        /// <summary>
        /// Configure carriers
        /// </summary>
        public async Task Configure(IEnumerable<CarrierConfiguration> configs)
        {
            foreach (var config in configs)
            {
                try
                {
                    await carrierSetupFactory[config.CarrierType]?.Setup(config);
                }
                catch (Exception ex)
                {
                    log.Error($"Failed to import configuration for {EnumHelper.GetDescription((ShipmentTypeCode) config.CarrierType)}: {ex.Message}", ex);
                }
            }
        }
    }
}
