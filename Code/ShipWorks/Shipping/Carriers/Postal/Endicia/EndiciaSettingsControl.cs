﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Autofac;
using ShipWorks.ApplicationCore;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Editions;
using ShipWorks.Shipping.Carriers.Postal.Endicia.Express1;
using ShipWorks.Shipping.Insurance;
using ShipWorks.Shipping.Settings;

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
        /// Carrier supports services
        /// </summary>
        protected override bool SupportsServices => true;

        /// <summary>
        /// Carrier supports packages
        /// </summary>
        protected override bool SupportsPackages => true;

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

            IShippingSettingsEntity settings = ShippingSettings.FetchReadOnly();

            if (endiciaReseller == EndiciaReseller.None)
            {
                // Showing the insurance control is dependent on if its allowed in tango
                insuranceProviderChooser.InsuranceProvider = (InsuranceProvider) settings.EndiciaInsuranceProvider;

                using (ILifetimeScope scope = IoC.BeginLifetimeScope())
                {
                    ILicenseService licenseService = scope.Resolve<ILicenseService>();
                    EditionRestrictionLevel restrictionLevel = licenseService.CheckRestriction(EditionFeature.EndiciaInsurance, null);

                    panelInsurance.Visible = restrictionLevel == EditionRestrictionLevel.None;
                }

                optionsControl.ShowShippingCutoffDate = true;
            }
            else
            {
                // Doesn't make sense to show Endicia insurance choosing to Express1
                panelInsurance.Visible = false;
                optionsControl.ShowShippingCutoffDate = false;
            }

            express1Options.Top = optionsControl.Bottom;

            LoadExpress1Settings(settings);

            ShipmentType shipmentType = ShipmentTypeManager.GetType(ShipmentTypeCode);
            PostalUtility.InitializeServicePicker(servicePicker, shipmentType);
            PostalUtility.InitializePackagePicker(packagePicker, shipmentType);

            servicePicker.Top = panelBottom.Bottom + 6;
            packagePicker.Top = servicePicker.Bottom + 10;
            panelInsurance.Top = packagePicker.Bottom + 5;
        }

        /// <summary>
        /// Loads the Express1 settings.
        /// </summary>
        private void LoadExpress1Settings(IShippingSettingsEntity settings)
        {
            this.settings = new Express1EndiciaSettingsFacade(settings);

            if (endiciaReseller == EndiciaReseller.Express1)
            {
                express1Options.LoadSettings(settings);
                panelBottom.Top = express1Options.Bottom + 5;
            }
            else
            {
                if (ShipmentTypeManager.GetType(ShipmentTypeCode.Express1Endicia).IsShipmentTypeRestricted ||
                    ShipmentTypeManager.GetType(ShipmentTypeCode.Endicia).IsRateDiscountMessagingRestricted)
                {
                    // Hide the express1 settings if Express1 is restricted or discounted rate messaging is turned off
                    express1PostageDiscountSettingsControl.Hide();
                    express1Options.Hide();

                    panelBottom.Top = optionsControl.Bottom + 5;
                }
                else
                {
                    express1PostageDiscountSettingsControl.LoadSettings(this.settings);
                    express1PostageDiscountSettingsControl.Top = optionsControl.Bottom;

                    panelBottom.Top = express1PostageDiscountSettingsControl.Bottom + 5;
                }
            }
        }

        /// <summary>
        /// Save the settings
        /// </summary>
        protected override void SaveSettings(ShippingSettingsEntity settings)
        {
            optionsControl.SaveSettings(settings);

            if (endiciaReseller == EndiciaReseller.None)
            {
                this.settings.SaveSettings(settings);
                settings.EndiciaInsuranceProvider = (int) insuranceProviderChooser.InsuranceProvider;
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
            return servicePicker.ExcludedEnumValues.Cast<int>();
        }

        /// <summary>
        /// Gets the excluded packages based on the items in the package picker.
        /// </summary>
        public override IEnumerable<int> GetExcludedPackageTypes()
        {
            return packagePicker.ExcludedEnumValues.Cast<int>();
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
