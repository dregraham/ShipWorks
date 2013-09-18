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
        readonly bool isExpress1 = false;
        bool loadedAccounts = false;

        /// <summary>
        /// Constructor
        /// </summary>
        public StampsSettingsControl(bool isExpress1)
        {
            InitializeComponent();

            this.isExpress1 = isExpress1;

            optionsControl.IsExpress1 = isExpress1;
            accountControl.IsExpress1 = isExpress1;
        }

        /// <summary>
        /// Load the settings
        /// </summary>
        public override void LoadSettings()
        {
            optionsControl.LoadSettings();

            ShippingSettingsEntity settings = ShippingSettings.Fetch();

            string reseller = StampsAccountManager.GetResellerName(isExpress1);
            labelAccountType.Text = String.Format("{0} Accounts", reseller);

            express1Options.Visible = isExpress1;

            if(isExpress1)
            {
                express1Options.LoadSettings(settings);
                panelBottom.Top = express1Options.Bottom + 5;
            }
            else
            {
                panelBottom.Top = optionsControl.Bottom + 5;
            }
        }

        /// <summary>
        /// Save the settings 
        /// </summary>
        public override void SaveSettings(ShippingSettingsEntity settings)
        {
            optionsControl.SaveSettings(settings);

            if(isExpress1)
            {
                express1Options.SaveSettings(settings);
            }
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
            if (!loadedAccounts)
            {
                accountControl.Initialize();
                loadedAccounts = true;
            }
        }
    }
}
