using System;
using System.Collections.Generic;
using Autofac.Features.Indexed;
using Common.Logging;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using ShipWorks.Shipping.CarrierSetup;
using ShipWorks.Warehouse.Configuration.DTO.ShippingSettings;

namespace ShipWorks.Shipping.Carriers.CarrierSetup
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
        public void Configure(List<CarrierConfiguration> configs)
        {
            foreach (var config in configs)
            {
                try
                {
                    carrierSetupFactory[config.CarrierType]?.Setup(config);
                }
                catch (Exception ex)
                {
                    log.Error($"Failed to import configuration for {EnumHelper.GetDescription((ShipmentTypeCode) config.CarrierType)}: {ex.Message}", ex);
                }
            }
        }
    }
}
