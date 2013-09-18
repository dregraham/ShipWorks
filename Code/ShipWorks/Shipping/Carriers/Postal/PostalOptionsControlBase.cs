using System.Windows.Forms;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Carriers.Postal
{
    /// <summary>
    /// Defines the interface for postal shipping options
    /// </summary>
    public class PostalOptionsControlBase : UserControl
    {
        /// <summary>
        /// Load the configured settings into the control
        /// </summary>
        public virtual void LoadSettings() { }

        /// <summary>
        /// Save the settings to the database
        /// </summary>
        public virtual void SaveSettings(ShippingSettingsEntity settings) { }
    }
}
