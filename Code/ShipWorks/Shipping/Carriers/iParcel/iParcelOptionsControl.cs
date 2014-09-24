using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Interapptive.Shared.Utility;
using ShipWorks.Shipping.Profiles;
using ShipWorks.Shipping.Settings;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Common.IO.Hardware.Printers;

namespace ShipWorks.Shipping.Carriers.iParcel
{
    /// <summary>
    /// Control for choosing i-parcel label options.
    /// </summary>
    public partial class iParcelOptionsControl : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="iParcelOptionsControl" /> class.
        /// </summary>
        public iParcelOptionsControl()
        {
            InitializeComponent();

            EnumHelper.BindComboBox<ThermalLanguage>(labelFormat);
            labelFormat.Items.Remove(ThermalLanguage.ZPL);
        }

        /// <summary>
        /// Loads the settings.
        /// </summary>
        public void LoadSettings()
        {
            labelFormat.SelectedValue = ShippingProfileManager.GetLabelFormatFromDefaultProfile<iParcelShipmentType>();
        }

        /// <summary>
        /// Saves the settings.
        /// </summary>
        /// <param name="settings">The settings.</param>
        public void SaveSettings(ShippingSettingsEntity settings)
        {
            ShippingProfileManager.SaveLabelFormatToDefaultProfile<iParcelShipmentType>((ThermalLanguage)labelFormat.SelectedValue);
        }
    }
}
