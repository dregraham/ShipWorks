using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Shipping.Carriers.Postal.Endicia.Express1;
using ShipWorks.Shipping.Settings;
using ShipWorks.Data.Model.EntityClasses;
using Interapptive.Shared.UI;
using System.Collections;
using ShipWorks.Shipping.Insurance;
using ShipWorks.Editions;

namespace ShipWorks.Shipping.Carriers.Postal.Endicia
{
    /// <summary>
    /// Shipping settings control for endicia
    /// </summary>
    public partial class EndiciaSettingsControl : SettingsControlBase
    {
        bool loadedAccounts = false;

        // the reseller type being worked with
        EndiciaReseller endiciaReseller = EndiciaReseller.None;

        /// <summary>
        /// Constructor
        /// </summary>
        public EndiciaSettingsControl(EndiciaReseller endiciaReseller)
        {
            InitializeComponent();

            this.endiciaReseller = endiciaReseller;
        }

        /// <summary>
        /// Load the settings into the control
        /// </summary>
        public override void LoadSettings()
        {
            optionsControl.LoadSettings(endiciaReseller);

            ShippingSettingsEntity settings = ShippingSettings.Fetch();

            string reseller = EndiciaAccountManager.GetResellerName(endiciaReseller);
            labelAccountType.Text = String.Format("{0} Accounts", reseller);

            endiciaOptions.Top = express1Options.Top;
            endiciaOptions.Visible = (endiciaReseller == EndiciaReseller.None);
            express1Options.Visible = (endiciaReseller == EndiciaReseller.Express1);

            if (endiciaReseller == EndiciaReseller.None)
            {
                endiciaOptions.LoadSettings(new Express1EndiciaSettingsFacade());
                panelBottom.Top = endiciaOptions.Bottom + 5;

                insuranceProviderChooser.InsuranceProvider = (InsuranceProvider) settings.EndiciaInsuranceProvider;

                // Showing the insurance control is dependant on if its allowed in tango
                panelInsurance.Visible = (EditionManager.ActiveRestrictions.CheckRestriction(EditionFeature.EndiciaInsurance).Level == EditionRestrictionLevel.None);
            }
            else
            {
                express1Options.LoadSettings(settings);
                panelBottom.Top = express1Options.Bottom + 5;

                // Doesn't make sense to show Endicia insurance choosing to Express1
                panelInsurance.Visible = false;
            }
        }

        /// <summary>
        /// Save the settings 
        /// </summary>
        public override void SaveSettings(ShippingSettingsEntity settings)
        {
            optionsControl.SaveSettings(settings);

            if (endiciaReseller == EndiciaReseller.None)
            {
                endiciaOptions.SaveSettings(settings);

                settings.EndiciaInsuranceProvider = (int) insuranceProviderChooser.InsuranceProvider;
            }
            else
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

            // We do it this way b\c it takes so long.  If we did it in LoadSettings, or each time refresh was called,
            // we'd be constantly waiting on stamps.com.
            if (!loadedAccounts)
            {
                accountControl.Initialize(endiciaReseller);
                loadedAccounts = true;
            }
        }
    }
}
