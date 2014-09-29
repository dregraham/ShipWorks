using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Shipping.Carriers.Postal.Stamps.Express1;
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
        private Express1StampsSettingsFacade express1Settings;
        readonly StampsResellerType stampsResellerType;

        /// <summary>
        /// Constructor
        /// </summary>
        public StampsSettingsControl(StampsResellerType stampsResellerType)
        {
            InitializeComponent();

            this.stampsResellerType = stampsResellerType;
            isExpress1 = stampsResellerType == StampsResellerType.Express1;

            optionsControl.IsExpress1 = isExpress1;
            accountControl.StampsResellerType = stampsResellerType;
        }

        /// <summary>
        /// Load the settings
        /// </summary>
        public override void LoadSettings()
        {
            optionsControl.LoadSettings();

            string reseller = StampsAccountManager.GetResellerName(stampsResellerType);
            labelAccountType.Text = String.Format("{0} Accounts", reseller);

            express1Options.Visible = isExpress1;
            express1SettingsControl.Visible = !isExpress1;

            LoadExpress1Settings();
        }

        /// <summary>
        /// Loads the Express1 settings.
        /// </summary>
        private void LoadExpress1Settings()
        {
            ShippingSettingsEntity settings = ShippingSettings.Fetch();
            express1Settings = new Express1StampsSettingsFacade(settings);

            if (isExpress1)
            {
                express1Options.LoadSettings(settings);
                panelBottom.Top = express1Options.Bottom + 5;
            }
            else
            {
                if (ShipmentTypeManager.GetType(ShipmentTypeCode.Express1Stamps).IsShipmentTypeRestricted)
                {
                    // Express1 is restricted - hide the express1 settings
                    express1SettingsControl.Hide();
                    express1Options.Hide();

                    panelBottom.Top = optionsControl.Bottom + 5;
                }
                else
                {
                    express1SettingsControl.LoadSettings(express1Settings);
                    express1SettingsControl.Top = optionsControl.Bottom + 5;

                    panelBottom.Top = express1SettingsControl.Bottom + 5;
                }
            }
        }

        /// <summary>
        /// Save the settings 
        /// </summary>
        public override void SaveSettings(ShippingSettingsEntity settings)
        {
            optionsControl.SaveSettings(settings);

            if (isExpress1)
            {
                express1Options.SaveSettings(settings);
            }
            else
            {
                express1Settings.SaveSettings(settings);
            }
        }

        /// <summary>
        /// Refresh the content of the control
        /// </summary>
        public override void RefreshContent()
        {
            base.RefreshContent();

            // We do it this way b\c it takes so long.  If we did it in LoadSettings, or each time refresh was called,
            // we'd be constantly waiting on stamps.com.
            if (!loadedAccounts)
            {
                accountControl.Initialize();
                loadedAccounts = true;
            }

            // Reload the Express1 settings in the event that an Express1 account was deleted to accurately
            // show whether to sign-up, remove the deleted account from the list, etc.
            LoadExpress1Settings();
        }
    }
}
