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

        private Express1EndiciaSettingsFacade settings;

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

            string reseller = EndiciaAccountManager.GetResellerName(endiciaReseller);
            labelAccountType.Text = String.Format("{0} Accounts", reseller);

            express1PostageDiscountSettingsControl.Visible = (endiciaReseller == EndiciaReseller.None);
            express1Options.Visible = (endiciaReseller == EndiciaReseller.Express1);

            ShippingSettingsEntity settings = ShippingSettings.Fetch();
            LoadExpress1Settings(settings);

            if (endiciaReseller == EndiciaReseller.None)
            {
                // Showing the insurance control is dependent on if its allowed in tango
                insuranceProviderChooser.InsuranceProvider = (InsuranceProvider)settings.EndiciaInsuranceProvider;
                panelInsurance.Visible = (EditionManager.ActiveRestrictions.CheckRestriction(EditionFeature.EndiciaInsurance).Level == EditionRestrictionLevel.None);
            }
            else
            {
                // Doesn't make sense to show Endicia insurance choosing to Express1
                panelInsurance.Visible = false;
            }

            // Load up the service picker based on the excluded service types
            ShipmentType shipmentType = ShipmentTypeManager.GetType(ShipmentTypeCode);
            List<PostalServiceType> excludedServices = shipmentType.GetExcludedServiceTypes().Select(exclusion => (PostalServiceType)exclusion).ToList();

            List<PostalServiceType> postalServices = PostalUtility.GetDomesticServices(ShipmentTypeCode).Union(PostalUtility.GetInternationalServices(ShipmentTypeCode)).ToList();
            servicePicker.Initialize(postalServices, excludedServices);

            servicePicker.Top = panelBottom.Bottom + 6;
            panelInsurance.Top = servicePicker.Bottom;
        }

        /// <summary>
        /// Loads the Express1 settings.
        /// </summary>
        private void LoadExpress1Settings(ShippingSettingsEntity settings)
        {
            this.settings = new Express1EndiciaSettingsFacade(settings);

            if (endiciaReseller == EndiciaReseller.Express1)
            {
                express1Options.LoadSettings(settings);
                panelBottom.Top = express1Options.Bottom + 5;
            }
            else
            {
                if (ShipmentTypeManager.GetType(ShipmentTypeCode.Express1Endicia).IsShipmentTypeRestricted || ShipmentTypeManager.GetType(ShipmentTypeCode.Endicia).IsRateDiscountMessagingRestricted)
                {
                    // Hide the express1 settings if Express1 is restricted or discounted rate messaging is turned off
                    express1PostageDiscountSettingsControl.Hide();
                    express1Options.Hide();

                    panelBottom.Top = optionsControl.Bottom + 5;
                }
                else
                {
                    express1PostageDiscountSettingsControl.LoadSettings(this.settings);
                    express1PostageDiscountSettingsControl.Top = optionsControl.Bottom + 5;

                    panelBottom.Top = express1PostageDiscountSettingsControl.Bottom + 5;
                }
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
                this.settings.SaveSettings(settings);
                settings.EndiciaInsuranceProvider = (int)insuranceProviderChooser.InsuranceProvider;
            }
            else
            {
                express1Options.SaveSettings(settings);
            }
        }

        /// <summary>
        /// Gets the excluded services based on the items in the service picker.
        /// </summary>
        public override IEnumerable<int> GetExcludedServices()
        {
            return servicePicker.ExcludedServiceTypes.Cast<int>();
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

            LoadExpress1Settings(ShippingSettings.Fetch());
        }
    }
}
