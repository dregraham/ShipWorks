using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.UI.Utility;
using ShipWorks.Filters;
using ShipWorks.Filters.Controls;
using ShipWorks.Templates.Controls;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Templates.Printing;

namespace ShipWorks.Shipping.Settings
{
    /// <summary>
    /// User control for displaying the settings of a shipment type in the shipping settings window
    /// </summary>
    public partial class ShipmentTypeSettingsControl : UserControl, IPrintWithTemplates
    {
        ShipmentType shipmentType;

        /// <summary>
        /// The tabs the control supports
        /// </summary>
        public enum Page
        {
            Settings,
            Profiles,
            Printing,
            Actions,
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public ShipmentTypeSettingsControl(ShipmentType shipmentType)
        {
            InitializeComponent();

            this.shipmentType = shipmentType;
        }

        /// <summary>
        /// Gets \ sets the current tab page that is displayed
        /// </summary>
        public Page CurrentPage
        {
            get
            {
                if (tabControl.SelectedTab == tabPageSettings)
                {
                    return Page.Settings;
                }
                else if (tabControl.SelectedTab == tabPagePrinting)
                {
                    return Page.Printing;
                }
                else if (tabControl.SelectedTab == tabPageActions)
                {
                    return Page.Actions;
                }
                else
                {
                    return Page.Profiles;
                }
            }
            set
            {
                switch (value)
                {
                    case Page.Settings: tabControl.SelectedTab = tabPageSettings; break;
                    case Page.Printing: tabControl.SelectedTab = tabPagePrinting; break;
                    case Page.Profiles: tabControl.SelectedTab = tabPageProfiles; break;
                    case Page.Actions: tabControl.SelectedTab = tabPageActions; break;
                }
            }
        }

        /// <summary>
        /// Load the settings of the configured shipmetn type
        /// </summary>
        public void LoadSettings()
        {
            defaultsControl.LoadSettings(shipmentType);
            printOutputControl.LoadSettings(shipmentType);
            automationControl.EnsureInitialized(shipmentType.ShipmentTypeCode);

            LoadGeneralSettings();
        }

        /// <summary>
        /// Load the settings to go in the "General" tab
        /// </summary>
        private void LoadGeneralSettings()
        {
            SettingsControlBase settingsControl = shipmentType.CreateSettingsControl();
            if (settingsControl != null)
            {
                settingsControl.BackColor = Color.Transparent;
                settingsControl.Dock = DockStyle.Fill;

                settingsControl.LoadSettings();

                tabPageSettings.Controls.Add(settingsControl);
            }
            else
            {
                tabPageSettings.Controls.Add(new Label { Text = string.Format("There are no settings specific to {0}.", shipmentType.ShipmentTypeName), Location = new Point(8, 7), AutoSize = true });
            }
        }

        /// <summary>
        /// Return the control used for general settings, or null if there is not one
        /// </summary>
        private SettingsControlBase GeneralSettingsControl
        {
            get
            {
                return tabPageSettings.Controls.Count == 1 ? tabPageSettings.Controls[0] as SettingsControlBase : null;
            }
        }

        /// <summary>
        /// A profile in some setting has been edited
        /// </summary>
        private void OnProfileEdited(object sender, EventArgs e)
        {
            RefreshContent();
        }

        /// <summary>
        /// The tab page is changing
        /// </summary>
        private void OnTabPageDeselecting(object sender, TabControlCancelEventArgs e)
        {
            if (e.TabPage == tabPagePrinting)
            {
                // Ensure each template selected for use has been configured with the printer to use
                if (!TemplatePrinterSelectionDlg.EnsureConfigured(this, new IPrintWithTemplates[] { this }))
                {
                    e.Cancel = true;
                }
            }
        }

        /// <summary>
        /// Save the settings of the shipment type to the database.
        /// </summary>
        public void SaveSettings(ShippingSettingsEntity settings)
        {
            defaultsControl.SaveSettings();
            printOutputControl.SaveSettings();
            automationControl.SaveSettings();

            SettingsControlBase settingsControl = GeneralSettingsControl;
            if (settingsControl != null)
            {
                settingsControl.SaveSettings(settings);
            }
        }

        /// <summary>
        /// Called to notify the settings control to refresh itself due to an outside change
        /// </summary>
        public void RefreshContent()
        {
            if (GeneralSettingsControl != null)
            {
                GeneralSettingsControl.RefreshContent();
            }
        }

        /// <summary>
        /// Return all the template id's that the user has chosen to be used as templaets to print with
        /// </summary>
        IEnumerable<long> IPrintWithTemplates.TemplatesToPrintWith
        {
            get { return ((IPrintWithTemplates) printOutputControl).TemplatesToPrintWith; }
        }
    }
}
