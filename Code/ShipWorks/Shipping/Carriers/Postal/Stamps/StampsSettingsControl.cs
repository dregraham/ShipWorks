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

namespace ShipWorks.Shipping.Carriers.Postal.Stamps
{
    /// <summary>
    /// User control for Stamps.com settings
    /// </summary>
    public partial class StampsSettingsControl : SettingsControlBase
    {
        bool loadedStampsAccounts = false;

        /// <summary>
        /// Constructor
        /// </summary>
        public StampsSettingsControl(bool isExpress1)
        {
            InitializeComponent();

            stampsAccountControl.IsExpress1 = isExpress1;
        }

        /// <summary>
        /// Load the settings
        /// </summary>
        public override void LoadSettings()
        {
            base.LoadSettings();

            originManagerControl.Initialize();
            optionsControl.LoadSettings();
        }

        /// <summary>
        /// Save the settings 
        /// </summary>
        public override void SaveSettings(ShippingSettingsEntity settings)
        {
            optionsControl.SaveSettings(settings);
        }

        /// <summary>
        /// Refresh the content of the control
        /// </summary>
        public override void RefreshContent()
        {
            base.RefreshContent();

            originManagerControl.Initialize();

            // We do it this way b\c it takes so long.  If we did it in LoadSettings, or each time refresh was called,
            // we'd be constantly waiting on stamps.com.
            if (!loadedStampsAccounts)
            {
                stampsAccountControl.Initialize();
                loadedStampsAccounts = true;
            }
        }
    }
}
