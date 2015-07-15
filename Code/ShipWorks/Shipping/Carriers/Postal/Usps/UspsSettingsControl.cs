﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using log4net;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Postal.Usps.Express1;
using ShipWorks.Shipping.Insurance;
using ShipWorks.Shipping.Settings;

namespace ShipWorks.Shipping.Carriers.Postal.Usps
{
    /// <summary>
    /// User control for USPS settings
    /// </summary>
    public partial class UspsSettingsControl : SettingsControlBase
    {
        static readonly ILog log = LogManager.GetLogger(typeof(UspsSettingsControl));

        bool loadedAccounts = false;
        Express1UspsSettingsFacade express1Settings;
        UspsResellerType uspsResellerType;

        private CarrierServicePickerControl<PostalServiceType> servicePicker;

        /// <summary>
        /// Constructor
        /// </summary>
        public UspsSettingsControl()
        {
            InitializeComponent();

        }

        public override void Initialize(ShipmentTypeCode shipmentTypeCode)
        {
            base.Initialize(shipmentTypeCode);

            uspsResellerType = PostalUtility.GetUspsResellerType(shipmentTypeCode);

            optionsControl.ShipmentTypeCode = shipmentTypeCode;
            accountControl.UspsResellerType = uspsResellerType;

            InitializeServicePicker();

            // Update the Express1 controls now in addition to on visible changed because we were seeing crashes
            // where express1settings was null because save was being called before the controls got loaded.
            UpdateExpress1ControlDisplay();
            VisibleChanged += (sender, args) => UpdateExpress1ControlDisplay();
        }

        /// <summary>
        /// Initializes the service picker with Postal service types for the USPS carrier.
        /// </summary>
        private void InitializeServicePicker()
        {
            // Add carrier service picker control to the exclusions panel
            servicePicker = new CarrierServicePickerControl<PostalServiceType>();
            servicePicker.Dock = DockStyle.Fill;
            servicePicker.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            
            panelExclusionConfiguration.Controls.Add(servicePicker);
            panelExclusionConfiguration.Height = servicePicker.Height + 10;
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

            panelExclusionConfiguration.Top = panelBottom.Bottom;
            panelInsurance.Top = panelExclusionConfiguration.Bottom;
        }

        /// <summary>
        /// Load the settings
        /// </summary>
        public override void LoadSettings()
        {
            optionsControl.LoadSettings();

            string reseller = UspsAccountManager.GetResellerName(uspsResellerType);
            labelAccountType.Text = String.Format("{0} Accounts", reseller);

            ShippingSettingsEntity settings = ShippingSettings.Fetch();

            if (uspsResellerType == UspsResellerType.None)
            {
                // Showing the insurance control is dependent on if its allowed in tango
                insuranceProviderChooser.InsuranceProvider = (InsuranceProvider)settings.UspsInsuranceProvider;

                // Specify the provider name here so we can use a constant
                insuranceProviderChooser.CarrierProviderName = UspsUtility.StampsInsuranceDisplayName;
                panelInsurance.Visible = UspsUtility.IsStampsInsuranceAllowed;
            }
            else
            {
                // Doesn't make sense to show Stamps.com insurance choosing to Express1
                panelInsurance.Visible = false;
            }

            ShipmentType shipmentType = ShipmentTypeManager.GetType(ShipmentTypeCode);
            List<PostalServiceType> excludedServices = shipmentType.GetExcludedServiceTypes().Select(exclusion => (PostalServiceType)exclusion).ToList();

            List<PostalServiceType> postalServices = PostalUtility.GetDomesticServices(ShipmentTypeCode).Union(PostalUtility.GetInternationalServices(ShipmentTypeCode)).ToList();
            servicePicker.Initialize(postalServices, excludedServices);
        }

        /// <summary>
        /// Updates whether the Express1 controls are displayed
        /// </summary>
        private void UpdateExpress1ControlDisplay()
        {
            express1Options.Visible = ShipmentTypeCode == ShipmentTypeCode.Express1Usps;
            express1SettingsControl.Visible = ShipmentTypeCode == ShipmentTypeCode.Usps;

            LoadExpress1Settings();
            PositionControls();
        }

        /// <summary>
        /// Loads the Express1 settings.
        /// </summary>
        private void LoadExpress1Settings()
        {
            ShippingSettingsEntity settings = ShippingSettings.Fetch();
            express1Settings = new Express1UspsSettingsFacade(settings);

            if (ShipmentTypeCode == ShipmentTypeCode.Express1Usps)
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
            log.Info("Checking whether to show Express1");
            
            bool shouldHide = !UspsAccountManager.GetAccounts(UspsResellerType.Express1).Any() ||
                    ShipmentTypeManager.GetType(ShipmentTypeCode.Express1Usps).IsShipmentTypeRestricted;
            
            log.InfoFormat("{0} Express1 controls", shouldHide ? "Hiding" : "Showing");

            return shouldHide;
        }

        /// <summary>
        /// Save the settings 
        /// </summary>
        public override void SaveSettings(ShippingSettingsEntity settings)
        {
            if (optionsControl == null)
            {
                throw new NullReferenceException("The USPS options control was null when trying to save USPS shipping settings.");
            }

            log.Info("Saving settings to UspsOptionsControl");
            optionsControl.SaveSettings(settings);
            log.Info("Saved settings to UspsOptionsControl");

            if (ShipmentTypeCode == ShipmentTypeCode.Express1Usps)
            {
                log.InfoFormat("Preparing to save Express1 options {0}", express1Options == null);
                express1Options.SaveSettings(settings);
                log.Info("Finished saving Express1 options");
            }
            else
            {
                log.InfoFormat("Preparing to save Express1 settings {0}", express1Settings == null);
                express1Settings.SaveSettings(settings);
                log.Info("Finished saving Express1 settings");
            }

            if (uspsResellerType == UspsResellerType.None)
            {
                log.InfoFormat("Saving insurance provider {0} {1} {2}", settings == null, insuranceProviderChooser == null, insuranceProviderChooser.InsuranceProvider);
                settings.UspsInsuranceProvider = (int)insuranceProviderChooser.InsuranceProvider;
                log.Info("Finished saving insurance provider");
            }

        }

        public override List<ExcludedServiceTypeEntity> GetExcludededServices()
        {
            List<int> servicesToExclude = servicePicker.ExcludedServiceTypes.Select(type => (int)type).ToList();

            List<ExcludedServiceTypeEntity> excludedServiceTypes = servicesToExclude
                .Select(serviceToExclude => new ExcludedServiceTypeEntity { ShipmentType = (int)this.ShipmentTypeCode, ServiceType = serviceToExclude })
                .ToList();
            
            return excludedServiceTypes;
        }

        /// <summary>
        /// Refresh the content of the control
        /// </summary>
        public override void RefreshContent()
        {
            base.RefreshContent();

            // We do it this way b\c it takes so long.  If we did it in LoadSettings, or each time refresh was called,
            // we'd be constantly waiting on USPS.
            if (!loadedAccounts)
            {
                accountControl.Initialize();
                loadedAccounts = true;
            }
        }
    }
}
