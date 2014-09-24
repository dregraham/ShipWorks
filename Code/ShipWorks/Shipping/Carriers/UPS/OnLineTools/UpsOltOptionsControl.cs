using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Profiles;
using ShipWorks.Shipping.Settings;
using ShipWorks.Common.IO.Hardware.Printers;

namespace ShipWorks.Shipping.Carriers.UPS.OnLineTools
{
    /// <summary>
    /// User control for editing global ups options
    /// </summary>
    public partial class UpsOltOptionsControl : UserControl
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public UpsOltOptionsControl()
        {
            InitializeComponent();

            EnumHelper.BindComboBox<ThermalLanguage>(labelFormat);
        }

        /// <summary>
        /// Load the configured settings into the control
        /// </summary>
        public void LoadSettings()
        {
            labelFormat.SelectedValue = ShippingProfileManager.GetLabelFormatFromDefaultProfile<UpsOltShipmentType>();
        }

        /// <summary>
        /// Save the settings to the database
        /// </summary>
        public void SaveSettings(ShippingSettingsEntity settings)
        {
            ShippingProfileManager.SaveLabelFormatToDefaultProfile<UpsOltShipmentType>((ThermalLanguage)labelFormat.SelectedValue);
        }
    }
}
