using System.Collections.Generic;
using System.Windows.Forms;
using Autofac.Features.Indexed;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Shipping.CarrierSetup;
using ShipWorks.Warehouse.DTO.Configuration.ShippingSettings;

namespace ShipWorks.Shipping.Carriers.CarrierSetup
{
    /// <summary>
    /// Configures carriers downloaded from the Hub
    /// </summary>
    [Component]
    public class HubCarrierConfigurator : IHubCarrierConfigurator
    {
        private readonly IIndex<ShipmentTypeCode, ICarrierSetup> carrierSetupFactory;
        private readonly IShipmentPrintHelper printHelper;
        private readonly IWin32Window owner;

        /// <summary>
        /// Constructor
        /// </summary>
        public HubCarrierConfigurator(IIndex<ShipmentTypeCode, ICarrierSetup> carrierSetupFactory,
            IShipmentPrintHelper printHelper,
            IWin32Window owner)
        {
            this.carrierSetupFactory = carrierSetupFactory;
            this.printHelper = printHelper;
            this.owner = owner;
        }

        /// <summary>
        /// Configure carriers
        /// </summary>
        public void Configure(List<CarrierConfiguration> configs)
        {
            foreach (var config in configs)
            {
                carrierSetupFactory[config.TypeCode]?.Setup(config.Payload);
                printHelper.InstallDefaultRules(config.TypeCode, true, owner);
            }
        }
    }
}
