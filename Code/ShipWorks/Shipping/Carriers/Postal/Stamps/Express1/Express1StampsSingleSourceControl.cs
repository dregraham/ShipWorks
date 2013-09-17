using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Postal.Express1;

namespace ShipWorks.Shipping.Carriers.Postal.Stamps.Express1
{
    /// <summary>
    /// Settings to use Express1 as a single-source for all postal shipping services
    /// </summary>
    public partial class Express1StampsSingleSourceControl : Express1SingleSourceControl
    {
        /// <summary>
        /// Load the settings
        /// </summary>
        public void LoadSettings(ShippingSettingsEntity settings)
        {
            singleSourceCheckBox.Checked = settings.Express1StampsSingleSource;
        }

        /// <summary>
        /// Save the UI settings to the entity
        /// </summary>
        public void SaveSettings(ShippingSettingsEntity settings)
        {
            settings.Express1StampsSingleSource = singleSourceCheckBox.Checked;
        }
    }
}
