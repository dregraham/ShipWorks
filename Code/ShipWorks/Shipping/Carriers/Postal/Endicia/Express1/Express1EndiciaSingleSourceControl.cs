using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers.Postal.Express1;

namespace ShipWorks.Shipping.Carriers.Postal.Endicia.Express1
{
    /// <summary>
    /// Settings to use Express1 as a single-source for all postal shipping services
    /// </summary>
    public class Express1EndiciaSingleSourceControl : Express1SingleSourceControl
    {
        /// <summary>
        /// Load the settings
        /// </summary>
        public void LoadSettings(IShippingSettingsEntity settings)
        {
            MethodConditions.EnsureArgumentIsNotNull(settings, nameof(settings));

            singleSourceCheckBox.Checked = settings.Express1EndiciaSingleSource;
        }

        /// <summary>
        /// Save the UI settings to the entity
        /// </summary>
        public void SaveSettings(ShippingSettingsEntity settings)
        {
            MethodConditions.EnsureArgumentIsNotNull(settings, nameof(settings));

            settings.Express1EndiciaSingleSource = singleSourceCheckBox.Checked;
        }
    }
}
