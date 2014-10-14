using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Shipping.Settings;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Carriers.EquaShip
{
    /// <summary>
    /// Usercontrol for configuring EquaShip shipments
    /// </summary>
    public partial class EquaShipSettingsControl : SettingsControlBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public EquaShipSettingsControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Loads the UI
        /// </summary>
        public override void LoadSettings()
        {
            base.LoadSettings();

            optionsControl.LoadSettings();
            accountsControl.Initialize();
        }

        /// <summary>
        /// Saves settings to the database
        /// </summary>
        public override void SaveSettings(ShippingSettingsEntity settings)
        {
            base.SaveSettings(settings);

            optionsControl.SaveSettings();
        }
    }
}
