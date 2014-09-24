using System.Windows.Forms;
using Interapptive.Shared.Utility;
using ShipWorks.Common.IO.Hardware.Printers;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Profiles;

namespace ShipWorks.Shipping.Carriers.OnTrac
{
    /// <summary>
    /// UserControl for editing options specific to the OnTrac integration
    /// </summary>
    public partial class OnTracOptionsControl : UserControl
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public OnTracOptionsControl()
        {
            InitializeComponent();
            EnumHelper.BindComboBox<ThermalLanguage>(labelFormat);
        }
        
        /// <summary>
        /// Load the configured settings into the control
        /// </summary>
        public void LoadSettings()
        {
            labelFormat.SelectedValue = ShippingProfileManager.GetLabelFormatFromDefaultProfile<OnTracShipmentType>();
        }

        /// <summary>
        /// Save the settings to the database
        /// </summary>
        public void SaveSettings(ShippingSettingsEntity settings)
        {
            ShippingProfileManager.SaveLabelFormatToDefaultProfile<OnTracShipmentType>((ThermalLanguage)labelFormat.SelectedValue);
        }
    }
}