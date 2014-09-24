using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Interapptive.Shared.Utility;
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
        }

        /// <summary>
        /// Loads the settings.
        /// </summary>
        public void LoadSettings()
        {
            //ShippingSettingsEntity settings = ShippingSettings.Fetch();
            //thermalPrinter.Checked = settings.IParcelThermal;
        }

        /// <summary>
        /// Saves the settings.
        /// </summary>
        /// <param name="settings">The settings.</param>
        public void SaveSettings(ShippingSettingsEntity settings)
        {
            //settings.IParcelThermal = thermalPrinter.Checked;

            //// i-parcel only supports EPL
            //settings.IParcelThermalType = (int) ThermalLanguage.EPL;
        }
    }
}
