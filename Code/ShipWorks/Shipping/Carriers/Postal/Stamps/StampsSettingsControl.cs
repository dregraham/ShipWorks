using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NDesk.Options;
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
        bool loadedAccounts = false;
        Express1StampsSettingsFacade express1Settings;
        readonly ShipmentTypeCode shipmentTypeCode;
        readonly StampsResellerType stampsResellerType;

        /// <summary>
        /// Constructor
        /// </summary>
        public StampsSettingsControl(ShipmentTypeCode shipmentTypeCode)
        {
            InitializeComponent();

            this.shipmentTypeCode = shipmentTypeCode;

            stampsResellerType = PostalUtility.GetStampsResellerType(shipmentTypeCode);

            optionsControl.ShipmentTypeCode = shipmentTypeCode;
            accountControl.StampsResellerType = stampsResellerType;

            PositionControls();
        }

        /// <summary>
        /// Positions the controls based on the visibility of other controls
        /// </summary>
        private void PositionControls()
        {
            express1SettingsControl.Top = optionsControl.Bottom + 5;

            if (express1SettingsControl.Visible)
            {
                panelBottom.Top = express1SettingsControl.Bottom;
            }
            else if (express1Options.Visible)
            {
                panelBottom.Top = express1Options.Bottom;
            }
            else
            {
                panelBottom.Top = optionsControl.Bottom;
            }
        }

        /// <summary>
        /// Load the settings
        /// </summary>
        public override void LoadSettings()
        {
            optionsControl.LoadSettings();

            string reseller = StampsAccountManager.GetResellerName(stampsResellerType);
            labelAccountType.Text = String.Format("{0} Accounts", reseller);

            express1Options.Visible = shipmentTypeCode == ShipmentTypeCode.Express1Stamps;
            express1SettingsControl.Visible = shipmentTypeCode == ShipmentTypeCode.Usps;

            LoadExpress1Settings();
            PositionControls();
        }

        /// <summary>
        /// Loads the Express1 settings.
        /// </summary>
        private void LoadExpress1Settings()
        {
            ShippingSettingsEntity settings = ShippingSettings.Fetch();
            express1Settings = new Express1StampsSettingsFacade(settings);

            if (shipmentTypeCode == ShipmentTypeCode.Express1Stamps)
            {
                express1Options.LoadSettings(settings);
            }
            else
            {
                if (ShouldHideExpress1Controls())
                {
                    // Express1 is restricted - hide the express1 settings
                    express1SettingsControl.Hide();
                    express1Options.Hide();
                }
                else
                {
                    express1SettingsControl.LoadSettings(express1Settings);
                    express1SettingsControl.Top = optionsControl.Bottom + 5;
                }
            }
        }

        /// <summary>
        /// Determines if the Express1 controls should be hidden
        /// </summary>
        private static bool ShouldHideExpress1Controls()
        {
            return !StampsAccountManager.GetAccounts(StampsResellerType.Express1).Any() ||
                    ShipmentTypeManager.GetType(ShipmentTypeCode.Express1Stamps).IsShipmentTypeRestricted; 
        }

        /// <summary>
        /// Save the settings 
        /// </summary>
        public override void SaveSettings(ShippingSettingsEntity settings)
        {
            optionsControl.SaveSettings(settings);

            if (shipmentTypeCode == ShipmentTypeCode.Express1Stamps)
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
