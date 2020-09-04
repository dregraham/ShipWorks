using System.Collections.Generic;
using Autofac.Features.Indexed;
using Interapptive.Shared.ComponentRegistration;
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

        /// <summary>
        /// Constructor
        /// </summary>
        public HubCarrierConfigurator(IIndex<ShipmentTypeCode, ICarrierSetup> carrierSetupFactory)
        {
            this.carrierSetupFactory = carrierSetupFactory;
        }

        /// <summary>
        /// Configure carriers
        /// </summary>
        public void Configure(List<CarrierConfiguration> configs)
        {
            foreach (var config in configs)
            {
                carrierSetupFactory[config.TypeCode]?.Setup(config.Payload);
            }
        }
    }
}
