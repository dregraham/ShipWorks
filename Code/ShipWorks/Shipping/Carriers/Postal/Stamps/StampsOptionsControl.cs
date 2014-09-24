using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Postal.Stamps.Express1;
using ShipWorks.Shipping.Profiles;
using ShipWorks.Shipping.Settings;
using System;
using ShipWorks.Common.IO.Hardware.Printers;

namespace ShipWorks.Shipping.Carriers.Postal.Stamps
{
    /// <summary>
    /// UserControl for editing options specific to the Stamps.com integration
    /// </summary>
    public partial class StampsOptionsControl : PostalOptionsControlBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public StampsOptionsControl()
        {
            InitializeComponent();

            EnumHelper.BindComboBox<ThermalLanguage>(labelFormat);
        }

        /// <summary>
        /// Whether the control is used for Express1.
        /// </summary>
        public bool IsExpress1 
        { 
            get; 
            set; 
        }

        /// <summary>
        /// Load the configured settings into the control
        /// </summary>
        public override void LoadSettings()
        {
            labelFormat.SelectedValue = IsExpress1 ? 
                ShippingProfileManager.GetLabelFormatFromDefaultProfile<Express1StampsShipmentType>() : 
                ShippingProfileManager.GetLabelFormatFromDefaultProfile<StampsShipmentType>();
        }

        /// <summary>
        /// Save the settings to the database
        /// </summary>
        public override void SaveSettings(ShippingSettingsEntity settings)
        {
            if (IsExpress1)
            {
                ShippingProfileManager.SaveLabelFormatToDefaultProfile<Express1StampsShipmentType>((ThermalLanguage)labelFormat.SelectedValue);
            }
            else
            {
                ShippingProfileManager.SaveLabelFormatToDefaultProfile<StampsShipmentType>((ThermalLanguage)labelFormat.SelectedValue);
            }
        }
    }
}
