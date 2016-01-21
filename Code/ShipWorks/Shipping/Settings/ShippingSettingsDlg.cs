using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ShipWorks.Core.Messaging;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Filters.Management;
using ShipWorks.Shipping.Carriers.Postal.Usps;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Templates.Processing;
using ShipWorks.UI.Controls;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using ShipWorks.Templates.Printing;
using log4net;
using Autofac;
using ShipWorks.Messaging.Messages;
using System.Reactive.Linq;

namespace ShipWorks.Shipping.Settings
{
    /// <summary>
    /// Global settings window for all shipping settings
    /// </summary>
    public partial class ShippingSettingsDlg : Form
    {
        static readonly ILog log = LogManager.GetLogger(typeof(ShippingSettingsDlg));

        // So we only load the settings for each type once, no matter how many times its enabled\disabled
        Dictionary<ShipmentTypeCode, ShipmentTypeSettingsControl> settingsMap = new Dictionary<ShipmentTypeCode, ShipmentTypeSettingsControl>();

        // The tab page currently displayed in the settings.  So it looks like it remains the same when 
        // switching between service types.
        ShipmentTypeSettingsControl.Page settingsTabPage = ShipmentTypeSettingsControl.Page.Settings;
        private bool usedDisabledGeneralShipRule;
        private readonly IDisposable uspsAccountCreatedToken;
        private ILifetimeScope lifetimeScope;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShippingSettingsDlg(ILifetimeScope lifetimeScope)
        {
            InitializeComponent();

            this.lifetimeScope = lifetimeScope;
            WindowStateSaver.Manage(this);

            uspsAccountCreatedToken = Messenger.Current.OfType<UspsAccountCreatedMessage>().Subscribe(OnUspsAccountCreated);
        }

        /// <summary>
        /// Initialization
        /// </summary>
        private void OnLoad(object sender, EventArgs e)
        {
            ShippingSettingsEntity settings = ShippingSettings.Fetch();

            LoadProvidersPanel();
            providerRulesControl.LoadSettings(GetEnabledShipmentTypes());

            radioBlankPhoneUseShipper.Checked = (settings.BlankPhoneOption == (int) ShipmentBlankPhoneOption.ShipperPhone);
            radioBlankPhoneUseSpecified.Checked = !radioBlankPhoneUseShipper.Checked;
            blankPhone.Text = settings.BlankPhoneNumber;

            originControl.Initialize();
            usedDisabledGeneralShipRule = providerRulesControl.AreAnyRuleFiltersDisabled;

            LoadShipmentTypePages();
        }

        /// <summary>
        /// Load the providers panel with the checkboxes for selection
        /// </summary>
        private void LoadProvidersPanel()
        {
            panelProviders.LoadProviders(ShipmentTypeManager.ShipmentTypes.Where(st => st.ShipmentTypeCode != ShipmentTypeCode.None), ShippingManager.IsShipmentTypeEnabled);
            panelActiveProviders.Height = panelProviders.Bottom + 2;
        }

        /// <summary>
        /// Size of the provider rules control is changing
        /// </summary>
        private void OnProviderRulesSizeChanged(object sender, EventArgs e)
        {
            panelDefaultProvider.Height = providerRulesControl.Bottom + 2;
            panelActiveProviders.Top = panelDefaultProvider.Bottom + 2;
        }

        /// <summary>
        /// User has clicked a box to enable\disable the availability of a shipment type
        /// </summary>
        void OnChangeEnabledShipmentTypes(object sender, EventArgs e)
        {
            providerRulesControl.UpdateActiveProviders(GetEnabledShipmentTypes());
            LoadShipmentTypePages();
        }
        
        /// <summary>
        /// Create an option page for each loaded shipment type
        /// </summary>
        private void LoadShipmentTypePages()
        {
            while (optionControl.OptionPages.Count > 1)
            {
                optionControl.OptionPages.RemoveAt(1);
            }

            foreach (ShipmentType shipmentType in GetEnabledShipmentTypes().Where(t => t.ShipmentTypeCode != ShipmentTypeCode.None))
            {
                OptionPage page = new OptionPage(shipmentType.ShipmentTypeName);
                page.Tag = shipmentType.ShipmentTypeCode;

                optionControl.OptionPages.Add(page);

                Control control;

                if (ShippingManager.IsShipmentTypeConfigured(shipmentType.ShipmentTypeCode))
                {
                    ShipmentTypeSettingsControl settingsControl;
                    if (!settingsMap.TryGetValue(shipmentType.ShipmentTypeCode, out settingsControl))
                    {
                        settingsControl = new ShipmentTypeSettingsControl(shipmentType, lifetimeScope);

                        // Force creation
                        IntPtr handle = settingsControl.Handle;
                        settingsControl.LoadSettings();

                        settingsControl.BackColor = Color.Transparent;
                        settingsControl.Dock = DockStyle.Fill;

                        log.InfoFormat("Adding settings control for {0}.", EnumHelper.GetDescription(shipmentType.ShipmentTypeCode));
                        settingsMap[shipmentType.ShipmentTypeCode] = settingsControl;
                    }

                    control = settingsControl;
                }
                else
                {
                    ShipmentTypeSetupControl setupControl = new ShipmentTypeSetupControl(shipmentType);
                    setupControl.SetupComplete += new EventHandler(OnShipmentTypeSetupComplete);

                    setupControl.Dock = DockStyle.Fill;
                    setupControl.BackColor = Color.Transparent;

                    control = setupControl;
                }

                // Don't use the white background & border for settings where a TabControl takes up the whole thing.
                if (control.Controls.Count != 1 || !(control.Controls[0] is TabControl))
                {
                    page.BorderStyle = BorderStyle.Fixed3D;
                    page.BackColor = Color.White;
                }

                page.Controls.Add(control);
            }
        }

        /// <summary>
        /// Change of the selected blank phone number option
        /// </summary>
        private void OnChangeBlankPhoneOption(object sender, EventArgs e)
        {
            blankPhone.Enabled = radioBlankPhoneUseSpecified.Checked;
        }

        /// <summary>
        /// Called when the setup wizard for a shipment type is finished
        /// </summary>
        private void OnShipmentTypeSetupComplete(object sender, EventArgs e)
        {
            ShipmentTypeCode? selected = optionControl.SelectedPage.Tag != null ? (ShipmentTypeCode) optionControl.SelectedPage.Tag : (ShipmentTypeCode?) null;
            
            // Reload the providers panel in case a new entry for Express1 needs to be added (in the case
            // where ShipWorks now needs to show both Express1 for Endicia and Express1 for USPS
            LoadProvidersPanel();
            LoadShipmentTypePages();

            if (selected != null)
            {
                OptionPage pageToSelect = optionControl.OptionPages.OfType<OptionPage>().FirstOrDefault(p => p.Tag != null && (ShipmentTypeCode) p.Tag == selected.Value);
                if (pageToSelect != null)
                {
                    optionControl.SelectedPage = pageToSelect;
                }
            }
        }

        /// <summary>
        /// Get the enabled shipment types (enabled in our UI, not necessarily in the database)
        /// </summary>
        private List<ShipmentType> GetEnabledShipmentTypes()
        {
            return panelProviders.SelectedShipmentTypes
                                 .Concat(new[] { ShipmentTypeManager.GetType(ShipmentTypeCode.None) })
                                 .ToList();
        }

        /// <summary>
        /// User is moving away from an option page
        /// </summary>
        private void OnOptionPageDeselecting(object sender, OptionControlCancelEventArgs e)
        {
            if (e.OptionPage == null || 
                e.OptionPage.Controls.Count != 1)
            {
                return;
            }

            ShipmentTypeSettingsControl settingsControl = e.OptionPage.Controls[0] as ShipmentTypeSettingsControl;

            if (settingsControl != null)
            {
                if (!AllowDisabledPrintingFiltersToBeSaved(settingsControl))
                {
                    e.Cancel = true;
                    return;
                }

                if (!AllowDisabledFilterInCarrierRuleToBeSaved(settingsControl))
                {
                    e.Cancel = true;
                    return;
                }

                settingsTabPage = settingsControl.CurrentPage;

                if (settingsTabPage == ShipmentTypeSettingsControl.Page.Printing)
                {
                    // Ensure each template selected for use has been configured with the printer to use
                    if (!TemplatePrinterSelectionDlg.EnsureConfigured(this, new ShipmentTypeSettingsControl[] { settingsControl }))
                    {
                        e.Cancel = true;
                        return;
                    }
                }
            }
            else
            {
                // The settings control could be null if the shipment type has not been
                // configured, so we need to check the page's tag to determine if we're
                // coming from the general settings page. We don't want to display the 
                // confirmation if navigating from a shipment type that is not configured.
                if (e.OptionPage.Tag == null)
                {
                    // We're coming from the general settings page
                    if (!AllowDisabledShippingFiltersToBeSaved())
                    {
                        e.Cancel = true;
                    }
                }
            }
        }

        /// <summary>
        /// User is moving into a new option page
        /// </summary>
        private void OnOptionPageSelecting(object sender, OptionControlCancelEventArgs e)
        {
            SaveSettings();

            ShipmentTypeSettingsControl settingsControl = null;

            if (e.OptionPage != null && e.OptionPage.Controls.Count == 1)
            {
                settingsControl = e.OptionPage.Controls[0] as ShipmentTypeSettingsControl;
            }

            if (settingsControl != null)
            {
                settingsControl.RefreshContent();
                settingsControl.CurrentPage = settingsTabPage;
            }

            if (e.OptionPage == optionPageGeneral)
            {
                originControl.Initialize();
                usedDisabledGeneralShipRule = providerRulesControl.AreAnyRuleFiltersDisabled;
            }
        }

        /// <summary>
        /// Closing the window - save the settings
        /// </summary>
        private void OnClosing(object sender, FormClosingEventArgs e)
        {
            if (!AllowDisabledFiltersToBeSaved())
            {
                e.Cancel = true;
                return;
            }

            SaveSettings();

            // Clear the rate cache since it may now be out of date due to 
            // settings be altered, accounts being deleted, etc.
            RateCache.Instance.Clear();
        }

        /// <summary>
        /// Saves the shipping settings
        /// </summary>
        private void SaveSettings()
        {
            using (SqlAdapter adapter = new SqlAdapter(true))
            {
                ShippingSettingsEntity settings = ShippingSettings.Fetch();
                List<int> existingExcludedTypes = settings.ExcludedTypes.ToList();
                settings.ExcludedTypes = panelProviders.UnselectedShipmentTypes.Select(x => (int)x.ShipmentTypeCode).ToArray();

                List<int> typesAdded = existingExcludedTypes.Except(settings.ExcludedTypes).ToList();
                List<int> typesRemoved = settings.ExcludedTypes.Except(existingExcludedTypes).ToList();

                if (typesRemoved.Any() || typesAdded.Any())
                {
                    Messenger.Current.Send(new EnabledCarriersChangedMessage(this, typesAdded, typesRemoved));
                }

                settings.BlankPhoneOption =
                    (int)
                    (radioBlankPhoneUseShipper.Checked
                         ? ShipmentBlankPhoneOption.ShipperPhone
                         : ShipmentBlankPhoneOption.SpecifiedPhone);
                settings.BlankPhoneNumber = blankPhone.Text;

                providerRulesControl.SaveSettings(settings);

                List<ExcludedServiceTypeEntity> excludedServices = new List<ExcludedServiceTypeEntity>();
                List<ExcludedPackageTypeEntity> excludedPackages = new List<ExcludedPackageTypeEntity>();

                // Save all the settings
                foreach (ShipmentTypeSettingsControl settingsControl in settingsMap.Values)
                {
                    settingsControl.SaveSettings(settings);
                    excludedServices.AddRange(settingsControl.GetExcludedServices());
                    excludedPackages.AddRange(settingsControl.GetExcludedPackages());
                }

                ExcludedServiceTypeRepository excludedServiceTypeRepository = new ExcludedServiceTypeRepository();
                excludedServiceTypeRepository.Save(excludedServices);

                ExcludedPackageTypeRepository excludedPackageTypeRepository = new ExcludedPackageTypeRepository();
                excludedPackageTypeRepository.Save(excludedPackages);

                bool wasDirty = settings.IsDirty;
                ShippingSettings.Save(settings);

                adapter.Commit();

                if (wasDirty)
                {
                    Messenger.Current.Send(new ShippingSettingsChangedMessage(this, settings));
                }
            }
        }

        /// <summary>
        /// Called when the ShippingSettingsEventDispatcher.UspsAccountCreated event is raised to make
        /// sure the option pages and exluded shipment types are updated to reflect any changes since
        /// shipment types could be exluded.
        /// </summary>
        /// <param name="message">The <see cref="UspsAccountCreatedMessage"/> instance containing the event data.</param>
        private void OnUspsAccountCreated(UspsAccountCreatedMessage message)
        {
            if (message.ShipmentTypeCode != ShipmentTypeCode.Usps)
            {
                return;
            }

            // Make sure the active providers reflect any shipment types that may have been disabled.
            LoadProvidersPanel();

            // Clear out the settings map to force a reload (to pick up the new USPSs account)
            settingsMap.Clear();
            LoadShipmentTypePages();

            OptionPage pageToSelect = optionControl.OptionPages.OfType<OptionPage>().FirstOrDefault(p => p.Tag != null && (ShipmentTypeCode)p.Tag == ShipmentTypeCode.Usps);
            if (pageToSelect != null)
            {
                optionControl.SelectedPage = pageToSelect;
            }
        }

        /// <summary>
        /// Should disabled filters stop saving?
        /// </summary>
        private bool AllowDisabledFiltersToBeSaved()
        {
            if (optionControl.SelectedPage == optionPageGeneral)
            {
                return AllowDisabledShippingFiltersToBeSaved();
            }
            else
            {
                ShipmentTypeSettingsControl currentControl = optionControl.SelectedPage.Controls.OfType<ShipmentTypeSettingsControl>().FirstOrDefault();

                return AllowDisabledPrintingFiltersToBeSaved(currentControl) && AllowDisabledFilterInCarrierRuleToBeSaved(currentControl);
            }
        }

        /// <summary>
        /// Should disabled filters in carrier rules stop saving?
        /// </summary>
        private bool AllowDisabledFilterInCarrierRuleToBeSaved(ShipmentTypeSettingsControl settingsControl)
        {
            return settingsControl == null ||
                   !settingsControl.AreAnyShipRuleFiltersDisabled || 
                   !settingsControl.AreAnyShipRuleFiltersChanged ||
                   DoesUserWantToSaveDisabledFilters("shipping");
        }

        /// <summary>
        /// Should disabled printing filters stop saving?
        /// </summary>
        private bool AllowDisabledPrintingFiltersToBeSaved(ShipmentTypeSettingsControl settingsControl)
        {
            return settingsControl == null ||
                   !settingsControl.AreAnyPrintRuleFiltersDisabled ||
                   !settingsControl.AreAnyPrintRuleFiltersChanged ||
                   DoesUserWantToSaveDisabledFilters("printing");
        }

        /// <summary>
        /// Should disabled shipping filters stop saving?
        /// </summary>
        private bool AllowDisabledShippingFiltersToBeSaved()
        {
            return !providerRulesControl.AreAnyRuleFiltersDisabled ||
                usedDisabledGeneralShipRule ||
                DoesUserWantToSaveDisabledFilters("shipping");
        }

        /// <summary>
        /// Prompt the user about whether they want to save since they've selected a disabled filter
        /// </summary>
        private bool DoesUserWantToSaveDisabledFilters(string filterTypeDescription)
        {
            DialogResult result = MessageHelper.ShowQuestion(this, MessageBoxIcon.Warning, MessageBoxButtons.YesNo,
                string.Format("At least one {0} rule uses a disabled filter, and will not match any shipment.\n\nSave anyway?", filterTypeDescription));
            return result == DialogResult.Yes;
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }

                uspsAccountCreatedToken.Dispose();

                // Dispose all the controls we created
                foreach (ShipmentTypeSettingsControl settingsControl in settingsMap.Values)
                {
                    settingsControl.Dispose();
                }
            }

            base.Dispose(disposing);
        }
    }
}
