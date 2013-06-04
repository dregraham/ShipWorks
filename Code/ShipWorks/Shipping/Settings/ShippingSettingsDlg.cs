using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.ApplicationCore;
using ShipWorks.UI.Controls;
using ShipWorks.Shipping.Editing;
using ShipWorks.UI;
using Interapptive.Shared.UI;
using ShipWorks.Templates.Printing;

namespace ShipWorks.Shipping.Settings
{
    /// <summary>
    /// Global settings window for all shipping settings
    /// </summary>
    public partial class ShippingSettingsDlg : Form
    {
        // So we only load the settings for each type once, no matter how many times its enabled\disabled
        Dictionary<ShipmentTypeCode, ShipmentTypeSettingsControl> settingsMap = new Dictionary<ShipmentTypeCode, ShipmentTypeSettingsControl>();

        // The tab page currently displayed in the settings.  So it looks like it remains the same when 
        // switching between service types.
        ShipmentTypeSettingsControl.Page settingsTabPage = ShipmentTypeSettingsControl.Page.Settings;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShippingSettingsDlg()
        {
            InitializeComponent();

            WindowStateSaver.Manage(this);
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

            LoadShipmentTypePages();
        }

        /// <summary>
        /// Load the providers panel with the checkboxes for selection
        /// </summary>
        private void LoadProvidersPanel()
        {
            int top = 3;
            int left = 3;

            // Add a checkbox for each shipment type
            foreach (ShipmentType shipmentType in ShipmentTypeManager.ShipmentTypes.Where(st => st.ShipmentTypeCode != ShipmentTypeCode.None))
            {
                CheckBox checkBox = new CheckBox();
                checkBox.AutoSize = true;
                checkBox.Location = new Point(left, top);

                checkBox.Tag = shipmentType.ShipmentTypeCode;
                checkBox.Text = shipmentType.ShipmentTypeName;
                checkBox.Checked = ShippingManager.IsShipmentTypeEnabled(shipmentType.ShipmentTypeCode);
                checkBox.CheckedChanged += new EventHandler(OnChangeEnabledShipmentTypes);

                panelProviders.Controls.Add(checkBox);

                top = checkBox.Bottom + 5;
            }

            panelProviders.Height = top;
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
                        settingsControl = new ShipmentTypeSettingsControl(shipmentType);

                        // Force creation
                        var handle = settingsControl.Handle;
                        settingsControl.LoadSettings();

                        settingsControl.BackColor = Color.Transparent;
                        settingsControl.Dock = DockStyle.Fill;

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
            List<ShipmentType> enabled = new List<ShipmentType>();

            foreach (ShipmentType shipmentType in ShipmentTypeManager.ShipmentTypes)
            {
                if (panelProviders.Controls.OfType<CheckBox>().Any(c => c.Checked && (ShipmentTypeCode) c.Tag == shipmentType.ShipmentTypeCode))
                {
                    enabled.Add(shipmentType);
                }
            }

            enabled.Add(ShipmentTypeManager.GetType(ShipmentTypeCode.None));

            return enabled;
        }

        /// <summary>
        /// User is moving away from an option page
        /// </summary>
        private void OnOptionPageDeselecting(object sender, OptionControlCancelEventArgs e)
        {
            ShipmentTypeSettingsControl settingsControl = null;

            if (e.OptionPage != null && e.OptionPage.Controls.Count == 1)
            {
                settingsControl = e.OptionPage.Controls[0] as ShipmentTypeSettingsControl;
            }

            if (settingsControl != null)
            {
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
        }

        /// <summary>
        /// User is moving into a new option page
        /// </summary>
        private void OnOptionPageSelecting(object sender, OptionControlCancelEventArgs e)
        {
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
            }
        }

        /// <summary>
        /// Closing the window - save the settings
        /// </summary>
        private void OnClosing(object sender, FormClosingEventArgs e)
        {
            using (SqlAdapter adapter = new SqlAdapter(true))
            {
                List<int> excludedTypes = new List<int>();

                foreach (CheckBox checkBox in panelProviders.Controls)
                {
                    if (!checkBox.Checked)
                    {
                        excludedTypes.Add((int) (ShipmentTypeCode) checkBox.Tag);
                    }
                }

                ShippingSettingsEntity settings = ShippingSettings.Fetch();
                settings.ExcludedTypes = excludedTypes.ToArray();

                settings.BlankPhoneOption = (int) (radioBlankPhoneUseShipper.Checked ? ShipmentBlankPhoneOption.ShipperPhone : ShipmentBlankPhoneOption.SpecifiedPhone);
                settings.BlankPhoneNumber = blankPhone.Text;

                providerRulesControl.SaveSettings(settings);

                // Save all the settings
                foreach (ShipmentTypeSettingsControl settingsControl in settingsMap.Values)
                {
                    settingsControl.SaveSettings(settings);
                }

                ShippingSettings.Save(settings);

                adapter.Commit();
            }
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
