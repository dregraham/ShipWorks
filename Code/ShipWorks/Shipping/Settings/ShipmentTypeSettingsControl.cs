using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using Interapptive.Shared.Utility;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.Shipping.Carriers.Postal.Endicia;
using ShipWorks.UI.Utility;
using ShipWorks.Filters;
using ShipWorks.Filters.Controls;
using ShipWorks.Templates.Controls;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Templates.Printing;
using log4net;

namespace ShipWorks.Shipping.Settings
{
    /// <summary>
    /// User control for displaying the settings of a shipment type in the shipping settings window
    /// </summary>
    public partial class ShipmentTypeSettingsControl : UserControl, IPrintWithTemplates
    {
        static readonly ILog log = LogManager.GetLogger(typeof(ShipmentTypeSettingsControl));

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

            // Hide 
            Enum.GetValues(typeof(Page))
                .OfType<Page>()
                .Where(shipmentType.IsSettingsTabHidden)
                .ToList()
                .ForEach(x => tabControl.Controls.Remove(GetTabControlForPage(x)));
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
                tabControl.SelectedTab = GetTabControlForPage(value);

                // We can't just use the null coalescing operator here because setting the SelectedTab property
                // to a tab that doesn't exist in the controls collection will cause the property to set itself to null
                if (tabControl.SelectedTab == null)
                {
                    tabControl.SelectedTab = tabPageSettings;
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
            log.InfoFormat("Preparing to save settings to defaults, printing, and automation for {0}", EnumHelper.GetDescription(shipmentType.ShipmentTypeCode));

            defaultsControl.SaveSettings();
            printOutputControl.SaveSettings();
            automationControl.SaveSettings();

            log.InfoFormat("Default, printing, and automation settings saved for {0}", EnumHelper.GetDescription(shipmentType.ShipmentTypeCode));

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
        /// Gets whether any of the rules use disabled filters
        /// </summary>
        public virtual bool AreAnyPrintRuleFiltersDisabled
        {
            get
            {
                return printOutputControl.AreAnyRuleFiltersDisabled;
            }
        }

        /// <summary>
        /// Gets a value indicating whether there [are any ship rule filters changed].
        /// </summary>
        /// <value>
        /// <c>true</c> if there [are any ship rule filters changed]; otherwise, <c>false</c>.
        /// </value>
        public bool AreAnyPrintRuleFiltersChanged
        {
            get { return printOutputControl.AreAnyRuleFiltersChanged; }
        }

        /// <summary>
        /// Return all the template id's that the user has chosen to be used as templaets to print with
        /// </summary>
        IEnumerable<long> IPrintWithTemplates.TemplatesToPrintWith
        {
            get { return ((IPrintWithTemplates) printOutputControl).TemplatesToPrintWith; }
        }

        /// <summary>
        /// Gets a value indicating whether there [are any ship rule filters disabled].
        /// </summary>
        /// <value>
        /// <c>true</c> if there [are any ship rule filters disabled]; otherwise, <c>false</c>.
        /// </value>
        public bool AreAnyShipRuleFiltersDisabled
        {
            get
            {
                return defaultsControl.AreAnyRuleFiltersDisabled;
            }
        }

        /// <summary>
        /// Gets a value indicating whether there [are any ship rule filters changed].
        /// </summary>
        /// <value>
        /// <c>true</c> if there [are any ship rule filters changed]; otherwise, <c>false</c>.
        /// </value>
        public bool AreAnyShipRuleFiltersChanged
        {
            get { return defaultsControl.AreAnyRuleFiltersChanged; }
        }

        /// <summary>
        /// Gets a TabPage for the specified Page enum value
        /// </summary>
        /// <param name="page">Page for which to return a TabPage</param>
        /// <returns></returns>
        private TabPage GetTabControlForPage(Page page)
        {
            switch (page)
            {
                case Page.Settings: return tabPageSettings;
                case Page.Printing: return tabPagePrinting;
                case Page.Profiles: return tabPageProfiles;
                case Page.Actions: return tabPageActions;
            }

            throw new InvalidOperationException(string.Format("Could not find a page for {0}", page));
        }
    }
}
