using System.Linq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Settings;

namespace ShipWorks.Shipping.Carriers.BestRate
{
    public partial class BestRateSettingsControl : SettingsControlBase
    {
        public BestRateSettingsControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Save the settings
        /// </summary>
        public override void SaveSettings(ShippingSettingsEntity settings)
        {
        }

        /// <summary>
        /// Reload the list of available shipment types when the control is activated
        /// </summary>
        public override void RefreshContent()
        {
            base.RefreshContent();

            panelProviders.LoadProviders(ShipmentTypeManager.ShipmentTypes
                .Where(t => ShippingManager.IsShipmentTypeEnabled(t.ShipmentTypeCode)), code => true);
        }
    }
}
