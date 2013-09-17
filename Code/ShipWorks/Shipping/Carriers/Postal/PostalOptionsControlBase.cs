using System.Windows.Forms;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Carriers.Postal
{
    /// <summary>
    /// Defines the interface for postal shipping options
    /// </summary>
    public abstract class PostalOptionsControlBase : UserControl
    {
        /// <summary>
        /// Load the configured settings into the control
        /// </summary>
        public abstract void LoadSettings();

        /// <summary>
        /// Save the settings to the database
        /// </summary>
        public abstract void SaveSettings(ShippingSettingsEntity settings);
    }
}
